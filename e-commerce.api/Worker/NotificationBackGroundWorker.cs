using e_commerce.app.Dto.NotificationDto;
using e_commerce.app.Services.IServices;

public class NotificationBackGroundWorker : BackgroundService
{
    private readonly INotificationChannel _notificationChannel;
    private readonly ILogger<NotificationBackGroundWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    public NotificationBackGroundWorker(INotificationChannel notificationChannel, ILogger<NotificationBackGroundWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _notificationChannel = notificationChannel;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        try
        {
            await foreach (var notificationEvent in _notificationChannel.ReadAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    _logger.LogInformation($"Sending notification to user {notificationEvent.UserId}...");

                    await notificationService.AddNotifiAsync(new CreateNotificationDto
                    {
                        UserId = notificationEvent.UserId,
                        Message = notificationEvent.Message,
                        Title = notificationEvent.Title
                    }, stoppingToken);

                    _logger.LogInformation($"Notification sent successfully to user {notificationEvent.UserId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send notification to user {notificationEvent.UserId}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Notification Worker is shutting down gracefully.");
        }
    }
}