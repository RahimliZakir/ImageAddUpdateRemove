using ImageAddUpdateRemoveWebApplicationUI.Models.DataContext;
using ImageAddUpdateRemoveWebApplicationUI.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageAddUpdateRemoveWebApplicationUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImageDbContext _context;
        readonly IWebHostEnvironment env;

        public HomeController(ImageDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImagePath,Id,CreatedDate,UpdatedDate,DeletedDate")] Image image, IFormFile file)
        {
            if (file == null)
            {
                ModelState.AddModelError("imageSelectError", "Şəkil seçməyibsiniz!");
            }
            else
            {
                var ext = Path.GetExtension(file.FileName);
                var fileName = $"image-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                var fullPath = Path.Combine(env.WebRootPath, "uploads", "images", fileName);

                using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fs);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        image.ImagePath = fileName;
                    }
                    catch (Exception)
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    _context.Add(image);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(image);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileTemp,Id,CreatedDate,UpdatedDate,DeletedDate")] Image image, IFormFile file)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var entity = _context.Images.AsNoTracking().FirstOrDefault(i=>i.Id == id);
                string fullPath = null;
                string currentPath = null;

                if (file == null && !string.IsNullOrWhiteSpace(image.FileTemp))
                {
                    image.ImagePath = entity.ImagePath;
                }
                else if (file == null)
                {
                    currentPath = Path.Combine(env.WebRootPath, "uploads", "images", entity.ImagePath);
                }
                else if (file != null)
                {
                    var ext = Path.GetExtension(file.FileName);
                    var fileName = $"image-{Guid.NewGuid().ToString().Replace("-", "")}{ext}";
                    fullPath = Path.Combine(env.WebRootPath, "uploads", "images", fileName);

                    using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fs);
                    }

                    image.ImagePath = fileName;

                    image.UpdatedDate = DateTime.UtcNow.AddHours(4);
                }

                try
                {
                    _context.Update(image);

                    await _context.SaveChangesAsync();

                    if (System.IO.File.Exists(currentPath) && !string.IsNullOrWhiteSpace(currentPath))
                    {
                        System.IO.File.Delete(currentPath);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {                          
                    if (System.IO.File.Exists(fullPath) && !string.IsNullOrWhiteSpace(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    if (!ImageExists(image.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
