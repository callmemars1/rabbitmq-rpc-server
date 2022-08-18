using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;

namespace Smartynov.RabbitMq.Rpc;

public record RabbitMqConnectionProperties
{
    public RabbitMqConnectionProperties()
    {
    }

    public bool Secure { get; init; } = false;
    public string Hostname { get; init; } = "localhost";
    public string Username { get; init; } = "guest";
    public string Password { get; init; } = "guest";
    public string? VirtualHost { get; init; } = null!;
    public int? Port { get; init; } = 5672;

    public Uri ToUri()
    {
        var uriBuilder = new StringBuilder();
        uriBuilder.Append("amqp");
        if (Secure) uriBuilder.Append('s');
        uriBuilder.Append("://");
        uriBuilder.Append(Username);
        uriBuilder.Append(':');
        uriBuilder.Append(Password);
        uriBuilder.Append('@');
        uriBuilder.Append(Hostname);
        if (Port.HasValue) uriBuilder.Append(':').Append(Port.Value);
        if (VirtualHost != null) uriBuilder.Append('/').Append(VirtualHost);
        return new Uri(uriBuilder.ToString());
    }
}