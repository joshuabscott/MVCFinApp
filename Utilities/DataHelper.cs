using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using MVCFinApp.Data;
using MVCFinApp.Services;
using MVCFinApp.Models;

namespace MVCFinApp.Utilities
{
    public class DataHelper
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        public static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };
            return builder.ToString();
        }

        public static async Task ManageData(IHost host)
        {
            try
            {
                using var svcScope = host.Services.CreateScope();
                var svcProvider = svcScope.ServiceProvider;

                var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
                await dbContextSvc.Database.MigrateAsync();

                var services = svcScope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<FAUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var fileService = services.GetRequiredService<IAvatarService>();
                await ContextSeed.SeedDataBaseAsync(context, userManager, roleManager, fileService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while running Manage Data => {ex}");
            }
        }
    }
}
