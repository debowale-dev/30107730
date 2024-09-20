using System.ComponentModel.DataAnnotations;

namespace FoodApi
{
    public class recipeDTO
    {
        [Key]
        public int recipe_id { get; set; }  // recipe_id in database
        public string name { get; set; }  // recipe_name in database
        public List<string> ingredients { get; set; }
        public List<string> steps { get; set; }
        public int n_ingredients { get; set; }
        public int n_steps { get; set; }

        public static implicit operator recipeDTO(RecipeDetailsDTO v)
        {
            throw new NotImplementedException();
        }
    }

}
