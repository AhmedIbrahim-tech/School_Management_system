namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDBContext _context;
    private IStudentRepository _studentRepository;
    private IDbContextTransaction _transaction;

    public UnitOfWork(
        ApplicationDBContext context,
        IStudentRepository studentRepository)
    {
        _context = context;
        _studentRepository = studentRepository;
    }

    public void BeginTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public void Commit()
    {
        if (_transaction != null)
        {
            _transaction.Commit();
        }
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
        }
    }

    public void Rollback()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(_context);


    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
