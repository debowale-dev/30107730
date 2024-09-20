using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FoodApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly RecipeDbContext _context;
        private readonly EmbeddingService _embeddingService;

        public RecipesController(RecipeDbContext context, EmbeddingService embeddingService)
        {
            _context = context;
            _embeddingService = embeddingService;
        }

        [HttpGet("get-embedding")]
        public IActionResult GetEmbedding([FromQuery] string foodName)
        {
            try
            {
                // Call the embedding service to get the embedding for the food name
                var embedding = _embeddingService.GetEmbedding(foodName);

                // Check if the embedding is null (error case)
                if (embedding == null)
                {
                    return StatusCode(500, "Failed to generate embeddings.");
                }

                return Ok(embedding); // Return the embedding as JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("similar")]
        public IActionResult GetSimilarRecipes([FromQuery] string foodName)
        {
            try
            {
                // Get the input embedding for the given food name
                var inputEmbedding = _embeddingService.GetEmbedding(foodName);

                // Check if the embedding is null (error case)
                if (inputEmbedding == null)
                {
                    return StatusCode(500, "Failed to generate embeddings.");
                }

                // Convert float[] to double[]
                double[] inputEmbeddingDouble = inputEmbedding.Select(e => (double)e).ToArray();

                // Get all embeddings from the database and compute similarities
                var topSimilarRecipes = GetTopSimilarRecipes(inputEmbeddingDouble);

                // Return the top 5 similar recipes
                return Ok(topSimilarRecipes);
            }
            catch (Exception ex)
            {
                // Log and return a server error
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private List<SimilarRecipeDTO> GetTopSimilarRecipes(double[] inputEmbedding)
        {
            var similarities = new List<SimilarRecipeDTO>();

            try
            {
                using (var connection = new SqlConnection("Server=DESKTOP-P4V7441;Database=recipe;Trusted_Connection=True;User Id=Debo;Password=Newpass123;MultipleActiveResultSets=true"))
                {
                    connection.Open();
                    var query = "SELECT recipe_id, embeddings FROM new_recipe_embeddings";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int recipeId = reader.GetInt32(0);
                            double[] recipeEmbedding = reader.GetString(1)
                                .Split(',')
                                .Select(double.Parse)
                                .ToArray();

                            double similarity = RecipeSimilarity.CosineSimilarity(inputEmbedding, recipeEmbedding);

                            similarities.Add(new SimilarRecipeDTO
                            {
                                RecipeId = recipeId.ToString(),
                                Similarity = similarity
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting similar recipes: {ex.Message}");
            }

            // Return top N similar recipes
            return similarities.OrderByDescending(s => s.Similarity).Take(5).ToList();
        }
    }
}
