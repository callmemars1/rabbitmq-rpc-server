namespace Smartynov.RabbitMq.Rpc;

public interface IRemoteProcedureArgument
{
    void Parse(ReadOnlyMemory<byte> data);
}