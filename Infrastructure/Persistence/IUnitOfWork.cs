namespace Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();

    IStudentRepository StudentRepository { get; }
}
