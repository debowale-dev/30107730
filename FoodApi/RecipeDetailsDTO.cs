namespace FoodApi
{
    public class RecipeDetailsDTO
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Steps { get; set; }
    }
}
