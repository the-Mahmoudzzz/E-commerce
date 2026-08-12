public interface IEmailChannel
{

     Task AddEmailTaskAsync(UserEmailEvent emailEvent, CancellationToken ct = default);


    IAsyncEnumerable<UserEmailEvent> ReadAllAsync(CancellationToken ct = default);
}