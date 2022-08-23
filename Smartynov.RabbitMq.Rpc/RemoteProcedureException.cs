namespace Smartynov.RabbitMq.Rpc;

public class RemoteProcedureException : Exception
{
    public RemoteProcedureException(string message, Exception exception)
        : base(message, exception)
    {
    }
}