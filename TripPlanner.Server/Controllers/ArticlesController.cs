using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly TripDbContext _dbContext;

        public ArticlesController(TripDbContext dbContext)
        {
            _dbContext = dbContext;
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
        public async Task<IActionResult> Create([FromBody]Article article)
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
        public async Task<IActionResult> Edit(int id, [FromBody] Article editedArticle)
        {
            try
            {
                var article = await _dbContext.Articles.FindAsync(id);

                if (article == null)
                {
                    return NotFound();
                }

                // Update the properties of the existing article
                article.Title = editedArticle.Title;
                article.Description = editedArticle.Description;
                article.ImageURL = editedArticle.ImageURL;
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
