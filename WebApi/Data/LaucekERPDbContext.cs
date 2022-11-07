using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Auth.Model;
using WebApi.Data.Entities;

namespace WebApi.Data
{
    public class LaucekERPDbContext : IdentityDbContext<ERPUser>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<Work> Works { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog= laucekErp");
            optionsBuilder.UseSqlServer("Server=tcp:saitynaidbktu.database.windows.net,1433;Initial Catalog=SaitynaiDb;Persist Security Info=False;User ID=saitynuadmin1;Password=Saitynai1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
