using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Sale sale, CancellationToken cancellationToken = default);
        Task<(List<Sale>, int)> GetPagedSalesAsync(int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, Guid? customerId, Guid? branchId, SaleStatus? status, CancellationToken cancellationToken = default);
    }
}
