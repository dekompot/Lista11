using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lista10.Data;
using Lista10.Models;
using Microsoft.AspNetCore.Http;

namespace Lista12.Pages.Articles
{
    public class BasketModel : PageModel
    {
        private readonly Lista10.Data.MyDbContext _context;

        public BasketModel(Lista10.Data.MyDbContext context)
        {
            _context = context;
        }

        public IList<Tuple<Article,int>> Basket { get;set; }

        public async Task OnGetAsync()
        {
            ClearCookies();
            await UpdateBasketAsync();
            
        }

        public void ClearCookies()
        {
            foreach (var cookie in Request.Cookies)
            {
                int value;
                if (cookie.Value == "0" || (Int32.TryParse(cookie.Key, out value) && !_context.Article.Any(a => a.ArticleId == value)))
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }

        public int GetPieces(int articleId)
        {
            string? pieces = Request.Cookies[articleId.ToString()];
            return pieces == null ? 0 : Int32.Parse(pieces);
        }

        public async Task UpdateBasketAsync()
        {
            var articles = await _context.Article
                .Include(a => a.Category).ToListAsync();
            Basket = articles
                .Select(a => new Tuple<Article, int>(a, GetPieces(a.ArticleId)))
                .Where(t => t.Item2 > 0)
                .ToList();
            ViewData["Total"] = Math.Round((decimal)Basket.Select(i => i.Item1.Price * i.Item2).Sum()*100)/100;
        }

        public IActionResult OnPostAddToBasket(int id)
        {
            AddElement(id);
            return RedirectToPage();
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


        public IActionResult OnPostRemoveAllPiecesFromBasket(int id)
        {
            DeleteCookie(id);
            return RedirectToPage();
        }

        public IActionResult OnPostRemoveFromBasket(int id)
        {
            string? pieces = Request.Cookies[id.ToString()];
            if (pieces != null)
            {
                int piecesInt = Int32.Parse(pieces);
                if (piecesInt > 0)
                {
                    SetCookie(id, piecesInt - 1);
                }
                else
                {
                    DeleteCookie(id);
                }
            }
            return RedirectToPage();
        }

        public void SetCookie(int key, int value)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append(key.ToString(), value.ToString(), options);
        }

        public void DeleteCookie(int key)
        {
            Response.Cookies.Delete(key.ToString());
        }
    }
}
