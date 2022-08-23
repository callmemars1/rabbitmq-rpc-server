namespace Smartynov.RabbitMq.Rpc;

public interface IRemoteProcedureHandler
{
    Task<ReadOnlyMemory<byte>?> HandleAsync(ReadOnlyMemory<byte> request);
}