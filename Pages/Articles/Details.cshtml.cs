using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lista10.Data;
using Lista10.Models;

namespace Lista12.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly Lista10.Data.MyDbContext _context;

        public DetailsModel(Lista10.Data.MyDbContext context)
        {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }
        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id == null)
            {
                return NotFound();
            }

            Article = await _context.Article
                .Include(a => a.Category).FirstOrDefaultAsync(m => m.ArticleId == Id);

            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
