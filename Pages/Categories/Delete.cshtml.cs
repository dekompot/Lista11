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

namespace Lista12.Pages.Categories
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
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (Category == null)
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

            Category = await _context.Category.FindAsync(id);

            if (Category != null)
            {
                var articles = _context.Article.Where(art => art.CategoryId == id).ToList();
                for (int i = articles.Count - 1; i >= 0; i--)
                {
                    if (articles[i].ImageName is not null)
                    {
                        RemoveFile(articles[i].ImageName);
                    }
                    articles.Remove(articles[i]);
                }
                _context.Category.Remove(Category);
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
