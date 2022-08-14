namespace Smartynov.RabbitMq.Rpc;

public interface IRemoteProcedure
{
    Task<ReadOnlyMemory<byte>?> ExecuteAsync(ReadOnlyMemory<byte> request);
}