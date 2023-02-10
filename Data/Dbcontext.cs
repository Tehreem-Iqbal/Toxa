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
            optionsBuilder.UseSqlServer(@"Server=localhost; Database=toxa; User Id=SA; Password=Khokhar640; TrustServerCertificate=True");

        public int userId {get; set; }

        
        public override int SaveChanges()
        {
            var addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            var updatedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            var deletedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();

            foreach (var entity in addedEntries)
            {
                var data = entity.Entity;
                var temp = (FullAuditModel)data;

                temp.Id = userId;
                temp.IsActive = true;
                temp.CreatedDate = DateTime.Now;
                temp.CreatedByUserId = userId.ToString();
                temp.LastModifiedDate = DateTime.Now;
                temp.LastModifiedUserId = userId.ToString();

            }

            foreach (var entity in updatedEntries)
            {
                var temp = (FullAuditModel)entity.Entity;
                temp.LastModifiedUserId = userId.ToString();
                temp.LastModifiedDate = DateTime.Now;
            }

            foreach (var entity in deletedEntries)
            {
                try
                {
                    var temp = (FullAuditModel)entity.Entity;
                    temp.IsActive = false;
                    temp.LastModifiedUserId = userId.ToString();
                    entity.State = EntityState.Modified;
                }
                catch
                {
                    continue;
                }

            }
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
