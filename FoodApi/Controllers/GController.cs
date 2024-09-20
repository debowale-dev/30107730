using Microsoft.AspNetCore.Mvc;

namespace FoodApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GController : ControllerBase
    {
        private readonly EmbeddingService _embeddingService;

        public GController(EmbeddingService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        [HttpGet("get-embedding")]
        public IActionResult GetEmbedding([FromQuery] string foodName)
        {
            try
            {
                var embedding = _embeddingService.GetEmbedding(foodName);
                return Ok(embedding);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
