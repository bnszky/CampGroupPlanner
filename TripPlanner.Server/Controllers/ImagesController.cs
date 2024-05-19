using Microsoft.AspNetCore.Mvc;
using TripPlanner.Server.Data;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly TripDbContext _dbContext;
        private IImageService _imageService;

        public ImagesController(TripDbContext dbContext, IImageService imageService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> Index()
        {
            return new List<string>(){"abc"};
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile image)
        {
            await _imageService.UploadImage(image);
            return Ok();
        }

        [HttpGet("{imageUrl}")]
        public async Task<ActionResult<IFormFile>> Get(string imageUrl)
        {
            IFormFile file = await _imageService.DownloadImage(imageUrl);
            return Ok(file);
        }

        [HttpPost("{imageUrl}")]
        public async Task<ActionResult> Reupload(string imageUrl)
        {
            IFormFile file = await _imageService.DownloadImage(imageUrl);
            await _imageService.UploadImage(file);
            return Ok();
        }
    }
}
