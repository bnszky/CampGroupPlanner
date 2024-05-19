using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly TripDbContext _dbContext;
        private readonly IImageService _imageService;

        public ArticlesController(TripDbContext dbContext, IImageService imageService)
        {
            _dbContext = dbContext;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Article>>> Index()
        {
            var articles = await _dbContext.Articles.ToListAsync();

            if (articles == null) return new List<Article>();

            return articles;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> Get(int id)
        {
            try
            {
                var article = await _dbContext.Articles.FindAsync(id);

                if (article == null)
                {
                    return NotFound();
                }

                return article;
            }
            catch (Exception ex)
            {
                var error = new List<string> { "Couldn't find the article in the database" };
                return BadRequest(error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ArticleCreate articleFromBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = new List<string>();
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        errors.Add(error.ErrorMessage);
                    }
                    return BadRequest(errors);
                }

                Article article = new Article();
                if (articleFromBody.ImageFile != null)
                {
                    try
                    {
                        article.ImageURL = await _imageService.UploadImage(articleFromBody.ImageFile);
                    }
                    catch (Exception ex)
                    {
                        var uploadError = new List<string> { "Couldn't upload image" };
                        return BadRequest(uploadError);
                    }
                }

                article.Title = articleFromBody.Title;
                article.Description = articleFromBody.Description;
                article.CreatedAt = DateTime.Now;
                article.SourceLink = articleFromBody.SourceLink;
                article.RegionId = articleFromBody.RegionId;
                article.Region = articleFromBody.Region;

                // Check if an article with the same SourceLink already exists in the database
                if (_dbContext.Articles.Any(a => a.SourceLink == article.SourceLink))
                {
                    var error = new List<string> { "Link to this article exists in the database" };
                    return BadRequest(error);
                }

                article.CreatedAt = DateTime.Now;

                // Add the article to the database
                _dbContext.Articles.Add(article);
                await _dbContext.SaveChangesAsync();

                return Ok(article);
            }
            catch (Exception ex)
            {
                var error = new List<string> { "Couldn't add to the database" };
                return BadRequest(error);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var article = await _dbContext.Articles.FindAsync(id);

                if (article == null)
                {
                    return NotFound();
                }

                if(article.ImageURL != null) await _imageService.DeleteImage(article.ImageURL);

                _dbContext.Articles.Remove(article);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                var error = new List<string> { "Couldn't delete from the database" };
                return BadRequest(error);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] ArticleCreate editedArticle)
        {
            try
            {
                var article = await _dbContext.Articles.FindAsync(id);

                if (article == null)
                {
                    return NotFound();
                }

                if (editedArticle.ImageFile != null)
                {
                    try
                    {
                        article.ImageURL = await _imageService.UploadImage(editedArticle.ImageFile);
                    }
                    catch (Exception ex)
                    {
                        var uploadError = new List<string> { "Couldn't upload image" };
                        return BadRequest(uploadError);
                    }
                }

                // Update the properties of the existing article
                article.Title = editedArticle.Title;
                article.Description = editedArticle.Description;
                article.SourceLink = editedArticle.SourceLink;
                article.Region = editedArticle.Region;

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return Ok(article);
            }
            catch (Exception ex)
            {
                var error = new List<string> { "Couldn't edit the article in the database" };
                return BadRequest(error);
            }
        }
    }
}
