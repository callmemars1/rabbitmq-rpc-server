using System.Runtime.ExceptionServices;

namespace Smartynov.RabbitMq.Rpc;

/// <summary>
/// Base class for processing remote procedure calls.
/// In basic usage, marks procedure as succeeded if no exception is thrown. Else marks procedure as failed.
/// </summary>
public abstract class RemoteProcedureBase<TArgument, TResult> : IRemoteProcedureHandler
    where TArgument : class
    where TResult : class
{
    private readonly IDeserializationProvider<TArgument> _deserializationProvider;
    private readonly ISerializationProvider<TResult> _serializationProvider;

    protected RemoteProcedureBase(IDeserializationProvider<TArgument> deserializationProvider, ISerializationProvider<TResult> serializationProvider)
    {
        _deserializationProvider = deserializationProvider;
        _serializationProvider = serializationProvider;
    }
    
    protected RemoteProcedureBase()
    {
        _deserializationProvider = new BasicJsonDeserializationProvider<TArgument>();
        _serializationProvider = new BasicJsonSerializationProvider<TResult>();
    }

    public async Task<ReadOnlyMemory<byte>?> HandleAsync(ReadOnlyMemory<byte> request)
    {
        var argument = _deserializationProvider.Deserialize(request);
        try
        {
            var result = await ProcessRequest(argument);
            return _serializationProvider.Serialize(result);
        }
        catch (Exception ex) when (ex is not RemoteProcedureException)
        {
            HandleException(ex);
            return null;
        }
    }

    /// <summary>
    /// Throw RemoteProcedureException if you want to send error response.
    /// In this case message will not be acknowledged
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected abstract Task<TResult> ProcessRequest(TArgument request);

    /// <summary>
    /// If you want to handle exception, override this method.
    /// </summary>
    protected virtual void HandleException(Exception exception)
        => ExceptionDispatchInfo.Capture(exception).Throw();
}