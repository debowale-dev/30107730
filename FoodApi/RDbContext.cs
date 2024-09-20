using Microsoft.EntityFrameworkCore;

namespace FoodApi
{
    public class RDbContext : DbContext
    {
        public RDbContext(DbContextOptions<RDbContext> options) : base(options) { }

        public DbSet<Recipe> recipes { get; set; }
          }
}
