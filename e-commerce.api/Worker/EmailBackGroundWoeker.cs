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
                        string subject = "Welcome to our E-Commerce Platform!";
                        string body = $"<h1>Hello {userEvent.FullName}</h1><p>Your account ({userEvent.Role}) has been created successfully.</p><p>Please click the link below to confirm your email address:</p><p><a href='{userEvent.ConfirmationLink}' target='_blank'>Confirm Email</a></p>";

                        _logger.LogInformation($"Sending email to {userEvent.Email}...");

                        await emailService.SendEmailAsync(userEvent.Email, subject, body, stoppingToken);

                        _logger.LogInformation($" Email sent successfully to {userEvent.Email}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to send email to {userEvent.Email}");
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