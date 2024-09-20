using FoodApi.Controllers;

namespace FoodApi
{
    public class RecipeDetailsAndSuggestions
    {
        public recipeDTO MostSimilarRecipe { get; set; }
        public List<RecipeSuggestionDTO> Suggestions { get; set; }
    }
}
