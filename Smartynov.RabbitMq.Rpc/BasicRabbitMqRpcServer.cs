using System.Text;
using System.Text.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Smartynov.RabbitMq.Rpc;

public class BasicRabbitMqRpcServer :
    IRabbitMqRpcServer,
    IDisposable
{
    private readonly RabbitMqConnectionProperties _connectionProperties;
    private IConnection? _connection;
    private IModel? _model;

    public BasicRabbitMqRpcServer(RabbitMqConnectionProperties connectionProperties)
    {
        _connectionProperties = connectionProperties;
    }

    public void Start()
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = _connectionProperties.Hostname,
            UserName = _connectionProperties.Username,
            Password = _connectionProperties.Password,
            Port = _connectionProperties.Port,

            DispatchConsumersAsync = true
        };
        _connection = connectionFactory.CreateConnection();
        _model = _connection.CreateModel();
        _model.BasicQos(
            prefetchSize: 0, prefetchCount: 1, global: false);
    }

    public void AddRpc(string methodName, IRemoteProcedure procedure)
    {
        if (_connection == null || _model == null)
            throw new InvalidOperationException("Connection not established");

        _model.QueueDeclare(
            queue: methodName, durable: false, exclusive: false,
            autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_model);
        _model.BasicConsume(
            queue: methodName, autoAck: false, consumer: consumer);

        consumer.Received += async (_, eventArgs) =>
        {
            var replyProps = _model.CreateBasicProperties();
            replyProps.CorrelationId = eventArgs.BasicProperties.CorrelationId;
            try
            {
                var result = await procedure.ExecuteAsync(eventArgs.Body);

                _model.BasicPublish(
                    exchange: "", routingKey: eventArgs.BasicProperties.ReplyTo,
                    basicProperties: replyProps, body: result!.Value);
                _model.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (RemoteProcedureException ex)
            {
                _model.BasicPublish(
                    exchange: "", routingKey: eventArgs.BasicProperties.ReplyTo,
                    basicProperties: replyProps, body: Encoding.UTF8.GetBytes(ex.Message));
                _model.BasicNack(eventArgs.DeliveryTag, false, true);
            }
            catch (Exception ex)
            {
                _model.BasicNack(eventArgs.DeliveryTag, false, true);
            }
        };
    }

    public void Stop()
    {
        _model?.Close();
        _model?.Dispose();

        try
        {
            _connection?.Close();
        }
        catch (IOException)
        {
            // pass, because its anyway closed
        }

        _connection?.Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Stop();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}