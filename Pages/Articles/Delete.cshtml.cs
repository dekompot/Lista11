using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lista10.Data;
using Lista10.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Lista12.Pages.Articles
{
    public class DeleteModel : PageModel
    {
        private readonly Lista10.Data.MyDbContext _context;

        private readonly string uploadFolder;

        public DeleteModel(Lista10.Data.MyDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "Uploads");
        }

        [BindProperty]
        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article
                .Include(a => a.Category).FirstOrDefaultAsync(m => m.ArticleId == id);

            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article.FindAsync(id);

            if (Article != null)
            {
                if (Article.ImageName is not null) 
                    RemoveFile(Article.ImageName);
                _context.Article.Remove(Article);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private void RemoveFile(string fileName)
        {
            string path = Path.Combine(uploadFolder, fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

            }
        }
    }
}
