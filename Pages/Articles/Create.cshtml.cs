using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lista10.Data;
using Lista10.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Lista12.Pages.Articles
{
    public class CreateModel : PageModel
    {
        private readonly Lista10.Data.MyDbContext _context;

        private readonly string uploadFolder;

        public CreateModel(Lista10.Data.MyDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "Uploads");
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "Name");
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Article.ImageName = Image is null ? null : SaveFileAndGetFilename(Image);
            _context.Article.Add(Article);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private string SaveFileAndGetFilename(IFormFile file)
        {
            string name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            string path = Path.Combine(uploadFolder, name);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return name;
        }
    }
}
