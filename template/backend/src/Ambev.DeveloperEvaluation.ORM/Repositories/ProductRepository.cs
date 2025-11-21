using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly DefaultContext _context;

    public BranchRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Branches.FirstOrDefaultAsync(x=> x.Id == id, cancellationToken);
    }


}
