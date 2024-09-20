using System.ComponentModel.DataAnnotations;

namespace FoodApi
{
    public class RecipeEmbedding
    {
  // Marks RecipeId as the primary key
        public string recipe_id { get; set; }

        public double[] embeddings { get; set; }
    }


}
