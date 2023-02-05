using Microsoft.EntityFrameworkCore;
using ProjectManagementApplication.Models;

namespace ProjectManagementApplication.Data
{
    public class Dbcontext : DbContext
    {
       
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<PurchasedServices> PurchasedServices { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=PMDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
  }
}
