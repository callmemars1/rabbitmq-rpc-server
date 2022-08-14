namespace Smartynov.RabbitMq.Rpc;

/// <summary>
/// Base class for processing remote procedure calls.
/// In basic usage, marks procedure as succeeded if no exception is thrown. Else marks procedure as failed.
/// </summary>
public abstract class RemoteProcedureBase<TArgument, TResult> : IRemoteProcedure
    where TArgument : class, IRemoteProcedureArgument, new()
    where TResult : class, IRemoteProcedureResult
{
    public async Task<ReadOnlyMemory<byte>?> ExecuteAsync(ReadOnlyMemory<byte> request)
    {
        var argument = new TArgument();
        argument.Parse(request);
        try
        {
            var result = await ProcessRequest(argument);
            return result.Serialize();
        }
        catch (Exception ex) when (ex is not RemoteProcedureException)
        {
            HandleException(ex);
            return null;
        }
    }

    /// <summary>
    /// Throw RemoteProcedureException if you want to send error response.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected abstract Task<TResult> ProcessRequest(TArgument request);

    /// <summary>
    /// If you want to handle exception, override this method.
    /// <code></code>
    /// Potential use: marking messages as acknowledged even when processing failed.
    /// </summary>
    protected virtual void HandleException(Exception exception)
        => throw new RemoteProcedureException("procedure failed", exception);
}

public class RemoteProcedureException : Exception
{
    public RemoteProcedureException(string message, Exception exception)
        : base(message, exception)
    {
    }
}