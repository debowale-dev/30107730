using System;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;

namespace FoodApi
{
    public class RecipeSimilarity
    {
        private static string connectionString = "Server=DESKTOP-P4V7441;Database=recipe;Trusted_Connection=True;User Id=Debo;Password=Newpass123;MultipleActiveResultSets=true";

        public static double[] GetRecipeEmbedding(int recipeId)
        {
            double[] embedding = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT embeddings FROM new_recipe_embeddings WHERE recipe_id = @recipeId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@recipeId", recipeId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string embeddingStr = reader.GetString(0);
                                embedding = embeddingStr.Split(',')
                                                        .Select(Double.Parse)
                                                        .ToArray();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error getting recipe embedding: {ex.Message}");
            }

            return embedding;
        }

        public static List<SimilarRecipeDTO> GetTopSimilarRecipes(int inputRecipeId, int topN = 5)
        {
            double[] inputEmbedding = GetRecipeEmbedding(inputRecipeId);
            if (inputEmbedding == null)
            {
                throw new Exception("Error retrieving input recipe embedding from the database.");
            }

            List<SimilarRecipeDTO> similarities = new List<SimilarRecipeDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT recipe_id, embeddings
                        FROM new_recipe_embeddings
                        WHERE recipe_id != @inputRecipeId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@inputRecipeId", inputRecipeId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int recipeId = reader.GetInt32(0);
                                double[] recipeEmbedding = reader.GetString(1)
                                                                .Split(',')
                                                                .Select(Double.Parse)
                                                                .ToArray();

                                double similarity = CosineSimilarity(inputEmbedding, recipeEmbedding);

                                similarities.Add(new SimilarRecipeDTO
                                {
                                    RecipeId = recipeId.ToString(),
                                    Similarity = similarity
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error getting similar recipes: {ex.Message}");
            }

            // Return top N similar recipes
            return similarities.OrderByDescending(s => s.Similarity).Take(topN).ToList();
        }

        public static double CosineSimilarity(double[] vectorA, double[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be of the same length");

            double dotProduct = vectorA.Zip(vectorB, (a, b) => a * b).Sum();
            double magnitudeA = Math.Sqrt(vectorA.Sum(a => a * a));
            double magnitudeB = Math.Sqrt(vectorB.Sum(b => b * b));

            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;

            return dotProduct / (magnitudeA * magnitudeB);
        }
    }
}
  
