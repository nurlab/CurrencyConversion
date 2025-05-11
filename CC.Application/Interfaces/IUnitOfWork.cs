namespace CC.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
        ValueTask DisposeAsync();
    }
}