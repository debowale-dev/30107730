using System;
using System.Data.SqlClient;

public partial class RatePrediction : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            string ratingValue = Request.Form["ratingValue"];
            string predictionId = Request.Form["predictionId"];

            if (!string.IsNullOrEmpty(ratingValue) && !string.IsNullOrEmpty(predictionId))
            {
                SaveRatingToDatabase(Convert.ToInt32(predictionId), Convert.ToInt32(ratingValue));
            }
        }
    }

    private void SaveRatingToDatabase(int predictionId, int rating)
    {
        string connectionString = "Server=DESKTOP-P4V7441;Database=recipe;Trusted_Connection=True;User Id=Debo;Password=Newpass123;MultipleActiveResultSets=true\";";  // Replace with your actual database connection string
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO UserRatings (PredictionId, Rating) VALUES (@PredictionId, @Rating)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PredictionId", predictionId);
            cmd.Parameters.AddWithValue("@Rating", rating);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
