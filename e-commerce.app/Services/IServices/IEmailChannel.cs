public interface IEmailChannel
{

     Task AddEmailTaskAsync(UserRegisteredEvent emailEvent, CancellationToken ct = default);


    IAsyncEnumerable<UserRegisteredEvent> ReadAllAsync(CancellationToken ct = default);
}