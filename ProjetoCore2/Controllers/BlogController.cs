using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoCore2.Data;
using ProjetoCore2.Models;

namespace ProjetoCore2.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Postagem != null ?
                          View(await _context.Postagem.ToListAsync()) :
                          Problem("Não há nenhuma postagem para ser exibida no momento.");
        }

        public IActionResult CreatorPage(int id)
        {
            if (id != 0)
            {
                BlogEntry? existingEntry = _context.Postagem?.FirstOrDefault(x => x.Id == id);
                return View(model: existingEntry);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatorPage(BlogEntry entry)
        {
            if (entry.Id != 0)
            {
                BlogEntry? existingEntry = _context.Postagem?.FirstOrDefault(x => x.Id == entry.Id);
                existingEntry.Conteudo = entry.Conteudo;
                _context.Update(existingEntry);
                await _context.SaveChangesAsync();
            }
            else
            {
                BlogEntry newEntry = new()
                {
                    Conteudo = entry.Conteudo
                };
                _context.Add(newEntry);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
