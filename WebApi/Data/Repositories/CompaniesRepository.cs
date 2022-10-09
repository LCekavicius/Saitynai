using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;

namespace WebApi.Data.Repositories
{
    public interface ICompaniesRepository
    {
        Task CreateAsync(Company company);
        Task DeleteAsync(Company company);
        Task<Company?> GetAsync(int companyId);
        Task<IReadOnlyList<Company>> GetManyAsync();
        Task<IReadOnlyList<Work>> GetAllCompanyWorks(int companyId);
        Task UpdateAsync(Company company);
    }

    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly LaucekERPDbContext _context;
        public CompaniesRepository(LaucekERPDbContext _context)
        {
            this._context = _context;
        }

        public async Task<IReadOnlyList<Work>> GetAllCompanyWorks(int companyId)
        {
            return await _context.Works
                .AsNoTracking()
                .Include(e => e.ProductionOrder)
                    .ThenInclude(e => e.Company)
                .Where(e => e.ProductionOrder.Company.Id == companyId)
                .ToListAsync();
        }

        public async Task<Company?> GetAsync(int companyId)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(e => e.Id == companyId);
        }

        public async Task<IReadOnlyList<Company>> GetManyAsync()
        {
            return await _context.Companies
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CreateAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company company)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }
    }
}
