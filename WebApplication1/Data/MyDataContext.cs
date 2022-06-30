using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApplication1.EntityModels;

namespace WebApplication1.Data
{
    public class MyDataContext : IdentityDbContext<ApplicationUser>
    {
        public MyDataContext(DbContextOptions<MyDataContext> dbContext)
            : base(dbContext)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole").HasNoKey();
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim").HasNoKey();
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin").HasNoKey();
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasNoKey();

        }
    }


}
