using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNet_lab_lista_10.Data;
using DotNet_lab_lista_10.Models;
using Microsoft.AspNetCore.Cors;

namespace DotNet_lab_lista_10.ControllersAPI
{
    [Route("api/articles")]
    [ApiController]
    public class ArticlesAPIController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ArticlesAPIController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/ArticlesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticle()
        {
            return await _context.Article.ToListAsync();
        }

        // GET: api/ArticlesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Article.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/ArticlesAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            var orginArticle = _context.Article.Find(article.Id);

            if(orginArticle != null)
            {
                orginArticle.Name = article.Name;
                orginArticle.Price = article.Price;
                orginArticle.CategoryId = article.CategoryId;
            }


            _context.Entry(orginArticle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ArticlesAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            article.Image = null;
            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/ArticlesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            if(article.Image != null)
            {
                DeleteImage(article.Image);
            }

            _context.Article.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }


        // GET: api/ArticlesAPI
        [HttpGet("nextpage/percategory/{page_index},{number_of_items},{category_id}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetNArticleStartWithQ(int page_index, int number_of_items, int category_id)
        {
            if(number_of_items < 1 || page_index < 0)
            {
                return new List<Article>();
            }
            return await _context.GetArticlesPagePerCategoryAsync(page_index,number_of_items,category_id);
        }



        private void DeleteImage(string imagePath)
        {
            try
            {
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }
    }
}