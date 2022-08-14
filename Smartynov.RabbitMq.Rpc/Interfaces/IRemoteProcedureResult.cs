namespace Smartynov.RabbitMq.Rpc;

public interface IRemoteProcedureResult
{
    ReadOnlyMemory<byte> Serialize();
}