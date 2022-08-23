namespace Smartynov.RabbitMq.Rpc;

public interface ISerializationProvider<in T>
{
    ReadOnlyMemory<byte> Serialize(T value);
}