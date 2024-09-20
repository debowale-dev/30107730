using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RController : ControllerBase
    {
        private readonly RDbContext _context;


        public RController(RDbContext context)
        {
            _context = context;
        }

        // GET: api/Recipe/{recipe_id}
        [HttpGet("{recipe_id}")]
        public async Task<IActionResult> GetRecipeById(int recipe_id)
        {
            // Raw SQL query
            var recipe = await _context.recipes
                .FromSqlRaw("SELECT [recipe_id], [name], [ingredients], [n_ingredients], [steps], [n_steps] FROM [recipe].[dbo].[recepies] WHERE recipe_id = {0}", recipe_id)
                .Select(r => new
                {
                    r.recipe_id,
                    r.name,
                    r.ingredients,
                    r.n_ingredients,
                    r.steps,
                    r.n_steps
                })
                .FirstOrDefaultAsync();

            // Check if the recipe was found
            if (recipe == null)
            {
                return NotFound(new { message = "Recipe not found" });
            }

            return Ok(recipe);
        }
    }
}
