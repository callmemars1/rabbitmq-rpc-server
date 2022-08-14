namespace Smartynov.RabbitMq.Rpc;

public record RabbitMqConnectionProperties
{
    public string Hostname { get; init; } = "localhost";
    public string Username { get; init; } = "guest";
    public string Password { get; init; } = "guest";
    public int Port { get; init; } = 5672;
}