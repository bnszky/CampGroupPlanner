using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly TripDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReviewController(TripDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("region/{regionId}")]
        [AllowAnonymous]
        public ActionResult<List<Review>> GetReviewsByRegion(int regionId)
        {
            var reviews = _context.Reviews.Where(r => r.RegionId == regionId).ToList();
            return Ok(reviews);
        }

        [HttpGet("region/name/{regionName}")]
        [AllowAnonymous]
        public ActionResult<List<Review>> GetReviewsByRegionName(string regionName)
        {
            var region = _context.Regions.FirstOrDefault(r => r.Name == regionName);

            if (region == null)
            {
                return NotFound($"Region with name '{regionName}' not found.");
            }

            var reviews = _context.Reviews.Where(r => r.RegionId == region.Id).ToList();

            return Ok(reviews);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Review>> GetAllReviews()
        {
            var reviews = _context.Reviews.ToList();
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Review>> GetReviewsByUser(string userId)
        {
            var reviews = _context.Reviews.Where(r => r.AuthorId == userId).ToList();
            return Ok(reviews);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<List<Review>>> GetUserReviews()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var reviews = _context.Reviews.Where(r => r.AuthorId == user.Id).ToList();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromForm] ReviewCreate model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var review = new Review
                {
                    Title = model.Title,
                    Text = model.Text,
                    Rate = model.Rate,
                    CreatedAt = DateTime.Now,
                    RegionId = model.RegionId,
                    RegionName = model.RegionName,
                    Author = user,
                    AuthorId = user.Id,
                    AuthorUsername = user.UserName
                };

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }

}
