using DotNet_lab_lista_10.Data;
using DotNet_lab_lista_10.Models;
using DotNet_lab_lista_10.ViewModesl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNet_lab_lista_10.Controllers
{
    [Authorize(Policy = "!Admin")]

    public class ShopController : Controller
    {
        private readonly MyDbContext _context;

        public ShopController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Category.Include(c => c.Articles).ToList();
            return View(categories);
        }

        public async Task<IActionResult> Products(int categoryId)
        {
            var n = 10;

            var categories = _context.Category.Include(c => c.Articles).ToList();

            var categoryIndex = categories.FindIndex(c => c.Id == categoryId);

            ViewBag.selectedCategory = categories[categoryIndex].Name;
            ViewBag.selectedCategoryID = categoryId;
            ViewBag.categories = categories;
            ViewBag.nArticles = n;


            return View("ProductsB" ,await _context.GetArticlesPagePerCategoryAsync(0,n,categoryId));
        }

        /* public IActionResult Products(int categoryId)
         {

             var categories = _context.Category.Include(c => c.Articles).ToList();
             var articlesInCategory = _context.Article
            .Where(a => a.CategoryId == categoryId)
            .Include(a => a.Category)
             .ToList();

             var shop = new ShopProductViewModel(articlesInCategory, categories, categoryId);
             return View(shop);
         }*/


        public async Task<IActionResult> CartItems()
        {
            List<ArticleCartItemViewModel> cartItems = new List<ArticleCartItemViewModel>();

            foreach (var cookie in Request.Cookies)
            {
                // Przyjmujemy, że klucze ciasteczek są identyfikatorami artykułów
                int articleId;
                if (int.TryParse(cookie.Key, out articleId))
                {
                    int quantity = int.Parse(cookie.Value);

                    // Pobierz artykuł z identyfikatorem articleId z bazy danych
                    var article = await _context.Article
                    .Include(a => a.Category)
                    .FirstOrDefaultAsync(m => m.Id == articleId);



                    if (article != null)
                    {
                        cartItems.Add(new ArticleCartItemViewModel(article,quantity));
                    }
                }
            }

            cartItems.Sort((item1, item2) => item1.Id.CompareTo(item2.Id));

            return View(cartItems);
        }

        public IActionResult AddItemToCart(int articleId, int categoryId)
        {

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);

            var categories = _context.Category.Include(c => c.Articles).ToList();

            var categoryIndex = categories.FindIndex((c => c.Id == categoryId));


            var temp = categories[categoryIndex];
            categories[categoryIndex] = categories[0];
            categories[0] = temp;


            if (Request.Cookies.ContainsKey(articleId.ToString()))
            {
                Response.Cookies.Append(articleId.ToString(), (int.Parse(Request.Cookies[articleId.ToString()]) + 1).ToString(), option);
            }
            else
            {
                Response.Cookies.Append(articleId.ToString(), "1", option);
            }

            return View("Products", categories);
        }

        public IActionResult CartIncrement(int articleId)
        {

            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append(articleId.ToString(), (int.Parse(Request.Cookies[articleId.ToString()]) + 1).ToString(), option);
            return RedirectToAction("CartItems");
        }

        public IActionResult CartDecrement(int articleId)
        {
            CookieOptions option = new CookieOptions();

            if (Request.Cookies[articleId.ToString()] == "1")
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Delete(articleId.ToString(), option);
            }
            else
            {
                option.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append(articleId.ToString(), (int.Parse(Request.Cookies[articleId.ToString()]) - 1).ToString(), option);
            }
            return RedirectToAction("CartItems");
        }

        public IActionResult RemoveFromCart(int articleId)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Delete(articleId.ToString(), option);
            return RedirectToAction("CartItems");
        }



        [Authorize(Roles = null)]
        public async Task<IActionResult> Order()
        {
            var shippingData = new ShippingViewModel();
            await PrepareShippingData(shippingData);
            return View(shippingData);
        }

        [Authorize(Roles = null)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order([FromForm] ShippingViewModel shippingModel)
        {
            await PrepareShippingData(shippingModel);

            if (ModelState.IsValid)
            {
                return View("ConfirmOrder", shippingModel);
            }

            return View(shippingModel);
        }



        private async Task<ShippingViewModel> PrepareShippingData(ShippingViewModel shippingData)
        {
            shippingData.CartItems = new List<ArticleCartItemViewModel>();

            foreach (var cookie in Request.Cookies)
            {
                int articleId;
                if (int.TryParse(cookie.Key, out articleId))
                {
                    int quantity = int.Parse(cookie.Value);

                    var article = await _context.Article
                        .Include(a => a.Category)
                        .FirstOrDefaultAsync(m => m.Id == articleId);

                    if (article != null)
                    {
                        shippingData.CartItems.Add(new ArticleCartItemViewModel(article, quantity));
                    }
                }
            }

            ViewBag.AvailablePaymentMethods = Enum.GetValues(typeof(PaymentMethod))
                .Cast<PaymentMethod>()
                .Select(method => new SelectListItem
                {
                    Value = method.ToString(),
                    Text = method.ToString()
                })
                .ToList();

            shippingData.CartItems.Sort((item1, item2) => item1.Id.CompareTo(item2.Id));

            return shippingData;
        }



        public async Task<IActionResult> ConfirmOrder(ShippingViewModel shippingData)
        {


            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);

            var cookies = Request.Cookies.Keys;
            foreach (var cookie in cookies)
            {
                Response.Cookies.Delete(cookie, option);
            }


            return RedirectToAction("Index", "Shop");
        }
    }



   
}
