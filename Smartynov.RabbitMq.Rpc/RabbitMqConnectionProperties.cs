using System.Threading.Channels;
using RabbitMQ.Client;

namespace Smartynov.RabbitMq.Rpc;

public record RabbitMqConnectionProperties
{
    public RabbitMqConnectionProperties()
    {
    }
    public string Hostname { get; init; } = "localhost";
    public string Username { get; init; } = "guest";
    public string Password { get; init; } = "guest";
    
    public string? VirtualHost { get; init; } = null!;
    
    public int Port { get; init; } = 5672;
}