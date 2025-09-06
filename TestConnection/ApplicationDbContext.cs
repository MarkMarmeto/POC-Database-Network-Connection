using Microsoft.EntityFrameworkCore;
using TestConnection.Entities;

namespace TestConnection
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public ApplicationDbContext(string connectionString) : base (GetOptions(connectionString)) { }
        private static DbContextOptions GetOptions(string connectionString)
            => SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;

        public virtual DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<UserEntity>().ToTable("Users");

            base.OnModelCreating(modelBuilder);
        }

        public async Task SeedAsync()
        {
            await Database.EnsureCreatedAsync();

            var users = await Users.AsNoTracking().ToListAsync();

            if (!users.Any())
            {
                var newUser = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Mark",
                    LastName = "Marmeto"
                };

                await Users.AddAsync(newUser);
                await SaveChangesAsync();
            }
        }
    }
}
