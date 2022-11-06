using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;

namespace WebApi.Data.Repositories
{
    public interface IWorksRepository
    {
        Task CreateAsync(Work work);
        Task DeleteAsync(Work Work);
        Task<Work?> GetAsync(int? workId, int? orderId, int ?companyId);
        Task<IReadOnlyList<Work>> GetManyAsync(int? workId, int? orderId);
        Task UpdateAsync(Work Work);
    }
    public class WorksRepository : IWorksRepository
    {
        private readonly LaucekERPDbContext _context;
        public WorksRepository(LaucekERPDbContext _context)
        {
            this._context = _context;
        }

        public async Task<Work?> GetAsync(int? workId, int? orderId, int? companyId)
        {
            return await _context.Works
                .Include(e => e.ProductionOrder)
                    .ThenInclude(e => e.Company)
                .FirstOrDefaultAsync(e => e.ProductionOrder.Company.Id == companyId && e.ProductionOrder.Id == orderId && e.Id == workId);
        }

        public async Task<IReadOnlyList<Work>> GetManyAsync(int? companyId, int? orderId)
        {
            return await _context.Works
                .AsNoTracking()
                .Include(e => e.ProductionOrder)
                    .ThenInclude(e => e.Company)
                .Where(e => e.ProductionOrder.Id == orderId && e.ProductionOrder.Company.Id == companyId)
                .ToListAsync();
        }
        public async Task CreateAsync(Work work)
        {
            _context.Works.Add(work);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Work work)
        {
            _context.Works.Remove(work);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Work work)
        {
            _context.Works.Update(work);
            await _context.SaveChangesAsync();
        }
    }
}
