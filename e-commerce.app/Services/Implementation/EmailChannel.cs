using System.Threading.Channels;

public class EmailChannel : IEmailChannel
{
    private readonly Channel<UserRegisteredEvent> _channel = Channel.CreateBounded<UserRegisteredEvent>(100);

    
    public async Task AddEmailTaskAsync(UserRegisteredEvent emailEvent, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(emailEvent, ct);
    }
    public IAsyncEnumerable<UserRegisteredEvent> ReadAllAsync(CancellationToken ct = default)
    {
        return _channel.Reader.ReadAllAsync(ct);
    }
}