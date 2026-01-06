using Application.Abstractions;
using MediatR;

namespace Application.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IAppDbContext _db;

    public TransactionBehavior(IAppDbContext db) => _db = db;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        // Only Commands are transactional (skip Queries)
        var isCommand = request.GetType().Name.EndsWith("Command", StringComparison.OrdinalIgnoreCase);
        if (!isCommand)
            return await next();

        await _db.BeginTransactionAsync(ct);

        try
        {
            var response = await next();

            await _db.SaveChangesAsync(ct);
            await _db.CommitTransactionAsync(ct);

            return response;
        }
        catch
        {
            await _db.RollbackTransactionAsync(ct);
            throw;
        }
    }
}
