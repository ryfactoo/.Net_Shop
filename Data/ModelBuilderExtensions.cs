using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using DotNet_lab_lista_10.Models;
using System.Collections.Generic;

namespace DotNet_lab_lista_10.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Beverages"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Cosmetics"
                });

            var articles = new List<Article>();

            for (int i = 1; i <= 1000; i++)
            {
                articles.Add(new Article()
                {
                    Id = i,
                    Name = $"Article_{i}",
                    Price = i,
                    CategoryId = ((i % 2)+1)
                });
            }

            modelBuilder.Entity<Article>().HasData(articles);
        }
    }
}
