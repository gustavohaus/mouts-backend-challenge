using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {   
            _context = context;
        }

        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }


        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.SaleProducts)
                .ThenInclude(s => s.Product)
                .Include(s => s.Branch)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }


        /// <summary>
        /// Updates an existing sale in the database
        /// </summary>
        /// <param name="sale">The sale to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated sale</returns>
        public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        /// <summary>
        /// Deletes a sale from the database
        /// </summary>
        /// <param name="id">The unique identifier of the sale to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the sale was deleted, false if not found</returns>
        public async Task<bool> DeleteAsync(Sale sale, CancellationToken cancellationToken = default)
        {

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }


        public async Task<(List<Sale>, int)> GetPagedSalesAsync(
            int pageNumber,
            int pageSize,
            DateTime? startDate,
            DateTime? endDate,
            Guid? customerId,
            Guid? branchId,
            SaleStatus? status,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Sales.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(s => s.CreatedAt >= DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc));

            if (endDate.HasValue)
                query = query.Where(s => s.CreatedAt <= DateTime.SpecifyKind(endDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc));

            if (customerId.HasValue)
                query = query.Where(s => s.CustomerId == customerId.Value);

            if (branchId.HasValue)
                query = query.Where(s => s.BranchId == branchId.Value);

            if (status.HasValue)
                query = query.Where(s => s.Status == status.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var sales = await query
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .Include(s => s.Branch)
                .Include(s => s.Customer)
                .OrderBy(s => s.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (sales, totalCount);
        }
    }
}
