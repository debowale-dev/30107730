using FoodApi;
using Microsoft.EntityFrameworkCore;

namespace FoodApi
{
    public class RecipeDbContext : DbContext
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options) : base(options) { }

        public DbSet<Recipe> recipes { get; set; }
        public DbSet<RecipeEmbedding> recipeEmbeddings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignore the Embedding property when mapping to the database
            modelBuilder.Entity<RecipeEmbedding>().Ignore(e => e.embeddings);
        }
    }

}
