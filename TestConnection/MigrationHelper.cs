using Microsoft.EntityFrameworkCore;

namespace TestConnection
{
    public static class MigrationHelper
    {
        public static async Task MigrateAsync(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServerConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                var context = new ApplicationDbContext(connectionString);
                await context.Database.MigrateAsync();
                await context.SeedAsync();
            }
        }
    }
}
