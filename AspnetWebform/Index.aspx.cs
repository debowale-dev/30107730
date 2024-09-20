using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestSharp;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;


namespace WebApplication2
{
    public partial class Index : System.Web.UI.Page
    {
        private List<Allergen> allergens;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.HttpMethod == "POST")
                {
                string ratingValue = Request.Form["ratingValue"];
                string predictionId = Request.Form["predictionId"];
                string recipeName = Request.Form["recipeName"];

                if (!string.IsNullOrEmpty(ratingValue) && !string.IsNullOrEmpty(predictionId))
                {
                    SaveRatingToDatabase(Convert.ToInt32(predictionId), Convert.ToInt32(ratingValue), recipeName);
                }
            }
           upload.Visible = false;
           searchresult.Visible = false;
            allergens = LoadAllergenListFromDatabase();
        }
        private void SaveRatingToDatabase(int predictionId, int rating, string recipeName)
        {
            string connectionString = "Server=DESKTOP-P4V7441;Database=recipe;Trusted_Connection=True;User Id=Debo;Password=Newpass123;MultipleActiveResultSets=true";  // Replace with your actual database connection string
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO UserRatings (PredictionId, Rating, RecipeName) VALUES (@PredictionId, @Rating, @recipeName)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PredictionId", predictionId);
                cmd.Parameters.AddWithValue("@Rating", rating);
                cmd.Parameters.AddWithValue("@recipeName", recipeName);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        private List<Allergen> LoadAllergenListFromDatabase()
        {
            var allergenList = new List<Allergen>();

            // Your connection string
            string connectionString = "Server=DESKTOP-P4V7441;Database=recipe;Trusted_Connection=True;User Id=Debo;Password=Newpass123;MultipleActiveResultSets=true";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT [Allergen], [Ingredients] FROM [recipe].[dbo].[Allergens]", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allergenList.Add(new Allergen
                        {
                            AllergenName = reader["Allergen"].ToString(),
                            FoodLabel = reader["Ingredients"].ToString()
                        });
                    }
                }
            }

            return allergenList;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    // Get the file from the FileUpload control
                    string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    var fileStream = FileUpload1.PostedFile.InputStream;

                    // Initialize RestSharp client
                    var client = new RestClient("http://localhost:8030/api/Dt/predict");
                    var request = new RestRequest(Method.POST);
                    request.AddFile("image", ReadFully(fileStream), fileName, FileUpload1.PostedFile.ContentType);
                    request.AlwaysMultipartFormData = true;

                    var response = client.Execute(request);

                    if (response.IsSuccessful)
                    {
                        string predictionResult = response.Content;
                        Session["Prediction"] = predictionResult; // Save prediction to session

                    }
                    else
                    {
                        Response.Write("<script>alert('Error: API request failed with status: " + response.StatusCode + "');</script>");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select an image to upload.');</script>");
            }
        }



        // Helper method to fully read the file stream into a byte array
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        } 
   

        private void GetSimilarRecipes(string foodName)
        {
            try
            {
                // Construct the full URL with query parameters
                var url = $"http://localhost:8030/api/Recipes/similar?foodName={Uri.EscapeDataString(foodName)}";

                // Initialize RestSharp client with a timeout
                var client = new RestClient(url)
                {
                    Timeout = 1000000 // Set timeout to 10 seconds
                };
                var request = new RestRequest(Method.GET);

                // Log the URL for debugging
                System.Diagnostics.Debug.WriteLine($"Request URL: {url}");

                // Execute the request
                var response = client.Execute(request);

                // Log the response details for debugging
                System.Diagnostics.Debug.WriteLine($"Response Status Code: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Response Content: {response.Content}");

                if (response.IsSuccessful)
                {
                    var similarRecipes = JsonConvert.DeserializeObject<List<SimilarRecipe>>(response.Content);
                    Session["SimilarRecipes"] = similarRecipes; // Save similar recipes to session
                   // DisplaySimilarRecipes(similarRecipes); // Use the similar recipes data as needed
                }
                else
                {
                    Response.Write("<script>alert('Error: API request failed with status: " + response.StatusCode + "');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }



        protected void SearchRecipeButton_Click(object sender, EventArgs e)
        {
            string foodName = SearchRecipeTextBox.Text;
            if (foodName.IsNullOrWhiteSpace())
            {
                Response.Write("<script>alert('Error: Search Field is null');</script>");

            }
            else
            {
                GetSimilarRecipes(foodName);

                var similarRecipes = Session["SimilarRecipes"] as List<SimilarRecipe>;

                if (similarRecipes != null && similarRecipes.Count >= 3)
                {
                    // Update each literal with the corresponding recipe details
                    DisplayRecipeDetails(similarRecipes[0].recipeId, FirstPrediction, FirstPredictionIngredients, FirstPredictionSteps, AllergenFirst);
                    DisplayRecipeDetails(similarRecipes[1].recipeId, SecondPrediction, SecondPredictionIngredients, SecondPredictionSteps, AllergenSecond);
                    DisplayRecipeDetails(similarRecipes[2].recipeId, ThirdPrediction, ThirdPredictionIngredients, ThirdPredictionSteps, AllergenThird);
                }
                else
                {
                    Response.Write("<script>alert('Not enough similar recipes found');</script>");
                }
            }
        }

        private void DisplayRecipeDetails(int recipeId, Label nameLabel, Literal ingredientsLiteral, Literal stepsLiteral, Label allergenLabel)
        {
            try
            {
                var url = $"http://localhost:8030/api/R/{recipeId}";
                var client = new RestClient(url) { Timeout = 10000 }; // 10 seconds timeout
                var request = new RestRequest(Method.GET);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var recipeDetails = JsonConvert.DeserializeObject<RecipeDetails>(response.Content);

                    // Update the name
                    nameLabel.Text = recipeDetails.Name;

                    // Format the ingredients and steps as HTML lists
                    ingredientsLiteral.Text = FormatWithIcons(recipeDetails.Ingredients);
                    stepsLiteral.Text = FormatWithIcons(recipeDetails.Steps);

                    // Check for allergens and update label
                    allergenLabel.Text = CheckForAllergies(recipeDetails.Ingredients);

                    

                    searchresult.Visible = true;
                }
                else
                {
                    nameLabel.Text = "Error fetching recipe details";
                }
            }
            catch (Exception ex)
            {
                nameLabel.Text = $"Error: {ex.Message}";
            }
        }

        private string FormatWithIcons(string items)
        {
            var itemList = items.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var formattedList = itemList.Select(item => $"<li><i class='bi bi-check-circle-fill'></i> {item}</li>");
            return string.Join("", formattedList);
        }

        // Check for allergens in the ingredients
        private string CheckForAllergies(string ingredients)
        {
            var ingredientList = ingredients.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var potentialAllergies = new List<string>();

            // Keep track of ingredients processed to avoid duplication
            var processedIngredients = new HashSet<string>();

            foreach (var ingredient in ingredientList)
            {
                var trimmedIngredient = ingredient.Trim();

                // Skip if ingredient has already been processed
                if (processedIngredients.Contains(trimmedIngredient))
                {
                    continue;
                }

                processedIngredients.Add(trimmedIngredient);

                foreach (var allergen in allergens)
                {
                    if (trimmedIngredient.IndexOf(allergen.FoodLabel, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Avoid adding duplicate allergen warnings for the same ingredient
                        if (!potentialAllergies.Any(a => a.Contains(trimmedIngredient) && a.Contains(allergen.AllergenName)))
                        {
                            // Clean the allergen text
                            var allergenText = $"Allergen: {allergen.AllergenName}<br>Ingredient: {trimmedIngredient}";
                            allergenText = allergenText.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "");

                            potentialAllergies.Add(allergenText);
                        }
                    }
                }
            }

            return potentialAllergies.Count > 0 ? string.Join("<br />", potentialAllergies) : "No allergens detected";
        }
       
    }
}
