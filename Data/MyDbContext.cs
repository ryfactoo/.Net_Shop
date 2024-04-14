using DotNet_lab_lista_10.Models;
using Microsoft.EntityFrameworkCore;
using DotNet_lab_lista_10.ViewModesl;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNet_lab_lista_10.Data
{
    public class MyDbContext : IdentityDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Article { get; set; }
        public DbSet<Category> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }

        public DbSet<DotNet_lab_lista_10.ViewModesl.ArticleCartItemViewModel> ArticleCartItemViewModel { get; set; }



        public async Task<List<Article>> GetArticlesPagePerCategoryAsync(int page_index, int number_of_items, int category_id)
        {
            var articles = await Article.Include(a => a.Category)
                .Where(a => a.CategoryId == category_id)
                .OrderBy(s => s.Id)
                .Skip(page_index * number_of_items)
                .Take(number_of_items)
                .ToListAsync();

            articles.ForEach(a => a.Category.Articles = null);

            return articles;
        }

    }
}
