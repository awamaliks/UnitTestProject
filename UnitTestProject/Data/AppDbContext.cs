using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
            var isConfigured = this.Database.GetDbConnection().ConnectionString;
            Console.WriteLine("Connection string: " + isConfigured);
        }

        public DbSet<Product> Products { get; set; }
    }
}