using System.Text;
using System.Text.Json;

namespace Smartynov.RabbitMq.Rpc;

public class BasicJsonSerializationProvider<T> : ISerializationProvider<T>
{
    public ReadOnlyMemory<byte> Serialize(T value)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
    }
}