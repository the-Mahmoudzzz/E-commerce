using System.Threading.Channels;

public interface INotificationChannel
{
    Task SendNotificationAsync(NotificationEvent notificationEvent, CancellationToken ct = default);
    IAsyncEnumerable<NotificationEvent> ReadAllAsync(CancellationToken ct = default);
}

public class NotificationChannel : INotificationChannel
{
    private readonly Channel<NotificationEvent> _channel = Channel.CreateBounded<NotificationEvent>(100);

    public async Task SendNotificationAsync(NotificationEvent notificationEvent, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(notificationEvent, ct);
    }

    public IAsyncEnumerable<NotificationEvent> ReadAllAsync(CancellationToken ct = default)
    {
        return _channel.Reader.ReadAllAsync(ct);
    }
}