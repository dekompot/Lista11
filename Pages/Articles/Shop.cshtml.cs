using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lista10.Data;
using Lista10.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Lista12.Pages.Articles
{
    public class ShopModel : PageModel
    {
        private readonly Lista10.Data.MyDbContext _context;

        public ShopModel(Lista10.Data.MyDbContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }

        public async Task OnGetAsync(string Category = "-1")
        {

            List<SelectListItem> categories = new List<SelectListItem>
            {
                new SelectListItem { Text = "All", Value = "-1" }
            };
            foreach (var category in _context.Category)
            {
                categories.Add(new SelectListItem { Text = category.Name, Value = category.CategoryId.ToString() });
            }

            categories.Find(c => c.Value == Category).Selected = true;

            ViewData["Category"] = categories;

            var articles = await _context.Article
                .Include(a => a.Category).ToListAsync();
            if (Category != "-1")
            {
                articles = articles.Where(a => a.CategoryId.ToString() == Category).ToList();
            }

            Article = articles;
        }

        public IActionResult OnPostAddToBasket(int id)
        {
            AddElement(id);
            return RedirectToPage();
        }
        public int GetPieces(int articleId)
        {
            string? pieces = Request.Cookies[articleId.ToString()];
            return pieces == null ? 0 : Int32.Parse(pieces);
        }

        public void AddElement(int id)
        {
            int pieces = GetPieces(id);
            if (pieces > 0)
            {
                SetCookie(id, pieces + 1);
            }
            else
            {
                SetCookie(id, 1);
            }
        }

        public void SetCookie(int key, int value)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append(key.ToString(), value.ToString(), options);
        }

 
    }
}
