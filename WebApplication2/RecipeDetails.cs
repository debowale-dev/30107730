namespace WebApplication2
{
    internal class RecipeDetails
    {
            public string Name { get; set; }
            public string Ingredients { get; set; }  // This will be a string in JSON format
            public int N_Ingredients { get; set; }
            public string Steps { get; set; }        // This will also be a string in JSON format
            public int N_Steps { get; set; }
        }

    }
