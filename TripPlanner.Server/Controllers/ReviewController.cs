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
        public IActionResult GetReviewsByRegion(int regionId)
        {
            var reviews = _context.Reviews.Where(r => r.RegionId == regionId).ToList();
            return Ok(reviews);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllReviews()
        {
            var reviews = _context.Reviews.ToList();
            return Ok(reviews);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetReviewsByUser(string userId)
        {
            var reviews = _context.Reviews.Where(r => r.AuthorId == userId).ToList();
            return Ok(reviews);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreate model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var review = new Review
            {
                Title = model.Title,
                Text = model.Text,
                Rate = model.Rate,
                CreatedAt = DateTime.UtcNow,
                RegionId = model.RegionId,
                Author = user,
                AuthorId = user.Id
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(review);
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
