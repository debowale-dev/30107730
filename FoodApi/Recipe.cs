using System.ComponentModel.DataAnnotations;

namespace FoodApi
{  public class Recipe
    {
        [Key]
        public long recipe_id { get; set; }  // recipe_id in database
        public string name { get; set; }  // recipe_name in database
        public string ingredients { get; set; }
        public string steps { get; set; }
        public int n_ingredients { get; set; }
        public int n_steps { get; set; }
    }

}
