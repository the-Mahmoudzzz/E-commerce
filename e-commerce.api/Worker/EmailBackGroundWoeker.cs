using e_commerce.app.Services.ExternalService;
// ... usings

namespace e_commerce.app.Workers
{
    public class EmailBackgroundWorker : BackgroundService 
    {
        private readonly IEmailChannel _emailChannel;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EmailBackgroundWorker> _logger;

        public EmailBackgroundWorker(
            IEmailChannel emailChannel, 
            IServiceScopeFactory scopeFactory, 
            ILogger<EmailBackgroundWorker> logger)
        {
            _emailChannel = emailChannel;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield(); 
            _logger.LogInformation("Email Background Worker is starting...");

            try
            {
            
                await foreach (var userEvent in _emailChannel.ReadAllAsync(stoppingToken))
                {
                    try
                    {
                        
                        using var scope = _scopeFactory.CreateScope();
                        
                        var emailService = scope.ServiceProvider.GetRequiredService<ISendEmailService>();
                        string subject = userEvent.Subject;
                        string body = userEvent.Body;

                        _logger.LogInformation($"Sending email to {userEvent.ToEmail}...");

                        await emailService.SendEmailAsync(userEvent.ToEmail, subject, body, stoppingToken);

                        _logger.LogInformation($" Email sent successfully to {userEvent.ToEmail}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to send email to {userEvent.ToEmail}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Email Worker is shutting down gracefully.");
            }
        }
    }
}