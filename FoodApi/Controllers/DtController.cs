using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FoodApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DtController : ControllerBase
    {
        private readonly Dt _dt;

        public DtController()
        {
            _dt = new Dt();
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image file uploaded.");

            using var stream = image.OpenReadStream();
            int label = _dt.Predict(stream);

            return Ok(label);
        }
    }
}
