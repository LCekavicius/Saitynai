using Microsoft.EntityFrameworkCore;
using WebApi.Data.Entities;

namespace WebApi.Data
{
    public class LaucekERPDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<Work> Works { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog= laucekErp");
        }
    }
}
