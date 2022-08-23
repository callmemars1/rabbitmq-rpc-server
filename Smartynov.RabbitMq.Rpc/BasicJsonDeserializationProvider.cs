using System.Text.Json;

namespace Smartynov.RabbitMq.Rpc;

public class BasicJsonDeserializationProvider<T> : IDeserializationProvider<T>
{
    public T Deserialize(ReadOnlyMemory<byte> request)
    {
        return JsonSerializer.Deserialize<T>(request.Span)!;
    }
}