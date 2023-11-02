using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoIdentty.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // cắt chuỗi AspNet trước tên của table
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tblName = entityType.GetTableName();
                if (tblName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tblName.Substring(6));
                }
            }
        }
    }
}