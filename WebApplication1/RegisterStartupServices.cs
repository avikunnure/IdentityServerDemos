using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.EntityModels;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1
{
    public static class RegisterStartupServices
    {
        public static void AddRegisterIdentityServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationsAssembly = typeof(RegisterStartupServices).Assembly.GetName().Name;
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            services.AddDbContext<MyDataContext>(options =>
            {
                options.UseSqlServer(connectionString, b =>
                {
                    b.MigrationsAssembly(assemblyName: migrationsAssembly);
                });
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyDataContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddTestUsers(TestUsers.Users)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                });
        }


        public static void InitializeDatabase(this IServiceCollection services)
        {
            try
            {
                using (var serviceScope = services.BuildServiceProvider())
                {
                    serviceScope.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                    var context = serviceScope.GetRequiredService<ConfigurationDbContext>();
                    context.Database.Migrate();
                    if (!context.Clients.Any())
                    {
                        foreach (var client in Config.Clients)
                        {
                            context.Clients.Add(client.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.IdentityResources.Any())
                    {
                        foreach (var resource in Config.IdentityResources)
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.ApiScopes.Any())
                    {
                        foreach (var resource in Config.ApiScopes)
                        {
                            context.ApiScopes.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
