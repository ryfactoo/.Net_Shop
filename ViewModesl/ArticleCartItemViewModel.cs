using DotNet_lab_lista_10.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DotNet_lab_lista_10.ViewModesl
{
    public class ArticleCartItemViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Too long name, do not exceed {1}")]
        public string Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00} $")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{1,2}){0,1}$", ErrorMessage = "The price must be a positive number accurate up to hundredths")]
        [Range(0.0d, double.MaxValue, ErrorMessage = "The price must be positive")]
        public double Price { get; set; }

        [AllowNull]
        public String Image { get; set; }

        [Required]
        [DisplayName(displayName: "Category")]
        public int CategoryId { get; set; }

        [AllowNull]
        public Category Category { get; set; }

        [Required]
        public int Quantity { get; set; }

        public ArticleCartItemViewModel()
        {
        }

        public ArticleCartItemViewModel(Article article, int quantity)
        {
            Id = article.Id;
            Name = article.Name;
            Price = article.Price;
            Image = article.Image;
            CategoryId = article.CategoryId;
            Category = article.Category;
            Quantity = quantity;
        }
    }
}
