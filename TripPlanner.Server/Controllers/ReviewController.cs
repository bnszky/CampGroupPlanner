using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.Server.Data;
using Microsoft.Extensions.Logging;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Services.Abstractions;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TripPlanner.Server.Models.Auth;
using TripPlanner.Server.Models.Database;
using AutoMapper;
using TripPlanner.Server.Models.DTOs.Outgoing;
using TripPlanner.Server.Models.DTOs.Incoming;
using Microsoft.EntityFrameworkCore;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly TripDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ReviewController> _logger;
        private readonly IErrorService _errorService;
        private readonly IMapper _mapper;

        public ReviewController(TripDbContext context, UserManager<User> userManager, ILogger<ReviewController> logger, IErrorService errorService, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _errorService = errorService;
            _mapper = mapper;
        }

        [HttpGet("region/{regionId}")]
        [AllowAnonymous]
        public ActionResult<List<ReviewGetDto>> GetReviewsByRegion(int regionId)
        {
            try
            {
                var reviews = _context.Reviews.Where(r => r.RegionId == regionId).Include(r => r.Author).ToList();
                var reviewDtos = _mapper.Map<IEnumerable<ReviewGetDto>>(reviews).ToList();
                _logger.LogInformation("{Message} RegionId: {RegionId}, ReviewsCount: {Count}", ResponseMessages.ReviewsFetched, regionId, reviewDtos.Count);
                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} RegionId: {RegionId}", ResponseMessages.CouldNotFetchReviews, regionId);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchReviews));
            }
        }

        [HttpGet("region/name/{regionName}")]
        [AllowAnonymous]
        public ActionResult<List<ReviewGetDto>> GetReviewsByRegionName(string regionName)
        {
            try
            {
                var region = _context.Regions.FirstOrDefault(r => r.Name == regionName);

                if (region == null)
                {
                    _logger.LogError("{Message} RegionName: {RegionName}", ResponseMessages.RegionNotFound, regionName);
                    return NotFound(_errorService.CreateError(ResponseMessages.RegionNotFound, StatusCodes.Status404NotFound));
                }

                var reviews = _context.Reviews.Where(r => r.RegionId == region.Id).Include(r => r.Author).ToList();
                var reviewDtos = _mapper.Map<IEnumerable<ReviewGetDto>>(reviews).ToList();
                _logger.LogInformation("{Message} RegionName: {RegionName}, ReviewsCount: {Count}", ResponseMessages.ReviewsFetched, regionName, reviewDtos.Count);
                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} RegionName: {RegionName}", ResponseMessages.CouldNotFetchReviews, regionName);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchReviews));
            }
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<ReviewGetDto>> GetAllReviews()
        {
            try
            {
                var reviews = _context.Reviews.Include(r => r.Author).ToList();
                var reviewDtos = _mapper.Map<IEnumerable<ReviewGetDto>>(reviews).ToList();
                _logger.LogInformation("{Message} ReviewsCount: {Count}", ResponseMessages.ReviewsFetched, reviewDtos.Count);
                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchReviews);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchReviews));
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<ReviewGetDto>> GetReviewsByUser(string userId)
        {
            try
            {
                var reviews = _context.Reviews.Where(r => r.AuthorId == userId).Include(r => r.Author).ToList();
                var reviewDtos = _mapper.Map<IEnumerable<ReviewGetDto>>(reviews).ToList();
                _logger.LogInformation("{Message} UserId: {UserId}, ReviewsCount: {Count}", ResponseMessages.ReviewsFetched, userId, reviewDtos.Count);
                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} UserId: {UserId}", ResponseMessages.CouldNotFetchReviews, userId);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchReviews));
            }
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<List<ReviewGetDto>>> GetUserReviews()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized(_errorService.CreateError(ResponseMessages.UserNotFound));

                var reviews = _context.Reviews.Where(r => r.AuthorId == user.Id).Include(r => r.Author).ToList();
                var reviewDtos = _mapper.Map<IEnumerable<ReviewGetDto>>(reviews).ToList();
                _logger.LogInformation("{Message} Username: {UserUsername}, ReviewsCount: {Count}", ResponseMessages.ReviewsFetched, user.UserName, reviewDtos.Count);
                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchReviews);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchReviews));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromForm] ReviewCreateDto model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized(_errorService.CreateError(ResponseMessages.UserNotFound));

                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var review = _mapper.Map<Review>(model);
                review.Author = user;
                var region = _context.Regions.FirstOrDefault(r => r.Name.ToLower().Equals(model.RegionName));
                if (region == null)
                {
                    _logger.LogError("{Message} RegionName: {RegionName}", ResponseMessages.RegionNotFound, model.RegionName);
                    return BadRequest(_errorService.CreateError(ResponseMessages.RegionNotFound, StatusCodes.Status400BadRequest));
                }
                review.Region = region;

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{Message} Review: {Review}, Username: {UserUserName}", ResponseMessages.ReviewCreated, review, user.UserName);
                return Ok(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.ReviewCreateError);
                return BadRequest(_errorService.CreateError(ResponseMessages.ReviewCreateError));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    _logger.LogError("{Message} ReviewId: {ReviewId}", ResponseMessages.ReviewNotFound, id);
                    return NotFound(_errorService.CreateError(ResponseMessages.ReviewNotFound, StatusCodes.Status404NotFound));
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{Message} ReviewId: {ReviewId}", ResponseMessages.ReviewDeleted, id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} ReviewId: {ReviewId}", ResponseMessages.ReviewDeleteError, id);
                return BadRequest(_errorService.CreateError(ResponseMessages.ReviewDeleteError));
            }
        }
    }
}
