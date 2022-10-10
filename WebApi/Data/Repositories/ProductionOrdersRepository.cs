using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;

namespace WebApi.Data.Repositories
{
    public interface IProductionOrdersRepository
    {
        Task CreateAsync(ProductionOrder productionOrder);
        Task DeleteAsync(ProductionOrder productionOrder);
        Task<ProductionOrder?> GetAsync(int orderId, int companyId);
        Task<IReadOnlyList<ProductionOrder>> GetManyAsync(int? companyId);
        Task UpdateAsync(ProductionOrder productionOrder);
    }
    public class ProductionOrdersRepository : IProductionOrdersRepository
    {
        private readonly LaucekERPDbContext _context;
        public ProductionOrdersRepository(LaucekERPDbContext _context)
        {
            this._context = _context;
        }

        public async Task<ProductionOrder?> GetAsync(int orderId, int companyId)
        {
            return await _context.ProductionOrders
                    .Include(e => e.Company)
                    .FirstOrDefaultAsync(e => e.Id == orderId && e.Company.Id == companyId);
        }

        public async Task<IReadOnlyList<ProductionOrder>> GetManyAsync(int? companyId)
        {
            return await _context.ProductionOrders
                .Include(e => e.Company)
                .AsNoTracking()
                .Where(e => companyId.HasValue ? e.Company.Id == companyId : true)
                .ToListAsync();
        }
        public async Task CreateAsync(ProductionOrder productionOrder)
        {
            _context.ProductionOrders.Add(productionOrder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductionOrder productionOrder)
        {
            _context.ProductionOrders.Remove(productionOrder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductionOrder productionOrder)
        {
            _context.ProductionOrders.Update(productionOrder);
            await _context.SaveChangesAsync();
        }
    }
}
