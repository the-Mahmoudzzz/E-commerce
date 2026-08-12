using System.Threading.Channels;

public class EmailChannel : IEmailChannel
{
    private readonly Channel<UserEmailEvent> _channel = Channel.CreateBounded<UserEmailEvent>(100);

    
    public async Task AddEmailTaskAsync(UserEmailEvent emailEvent, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(emailEvent, ct);
    }
    public IAsyncEnumerable<UserEmailEvent> ReadAllAsync(CancellationToken ct = default)
    {
        return _channel.Reader.ReadAllAsync(ct);
    }
}