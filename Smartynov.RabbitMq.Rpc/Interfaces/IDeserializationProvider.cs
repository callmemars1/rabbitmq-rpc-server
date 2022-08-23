namespace Smartynov.RabbitMq.Rpc;

public interface IDeserializationProvider<out T>
{
    T Deserialize(ReadOnlyMemory<byte> request);
}