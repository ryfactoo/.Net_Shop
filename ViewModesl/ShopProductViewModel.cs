using DotNet_lab_lista_10.Models;
using System.Collections.Generic;

namespace DotNet_lab_lista_10.ViewModesl
{
    public class ShopProductViewModel
    {
        public ICollection<Article> Articles{ get; }
        public ICollection<Category> Categories { get; }

        public int SelectedCategoryId { get; }

        public ShopProductViewModel(ICollection<Article> articlesInCategory, ICollection<Category> categories, int selectedCategoryId)
        {
            this.Articles = articlesInCategory;
            this.Categories = categories;
            this.SelectedCategoryId = selectedCategoryId;
        }

        public ShopProductViewModel()
        {
        }
    }
}
