namespace Smartynov.RabbitMq.Rpc;

public interface IRabbitMqRpcServer
{
    void Start();
 
    bool Listening { get; }
    
    /// <param name="methodName">name of rpc queue</param>
    void AddRpc(string methodName, IRemoteProcedure procedure);
    
    void Stop();
}