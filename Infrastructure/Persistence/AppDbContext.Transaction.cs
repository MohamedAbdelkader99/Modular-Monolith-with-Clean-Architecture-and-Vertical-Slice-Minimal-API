using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;

public sealed partial class AppDbContext : IAppDbContext
{
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync(CancellationToken ct)
    {
        if (_currentTransaction is not null) return;

        _currentTransaction = await Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct)
    {
        try
        {
            if (_currentTransaction is not null)
                await _currentTransaction.CommitAsync(ct);
        }
        finally
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }


    public async Task RollbackTransactionAsync(CancellationToken ct)
    {
        try
        {
            if (_currentTransaction is not null)
                await _currentTransaction.RollbackAsync(ct);
        }
        finally
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
    Task<int> IAppDbContext.SaveChangesAsync(CancellationToken ct)
    => base.SaveChangesAsync(ct);

}
