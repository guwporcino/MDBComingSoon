using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoCore2.Data;
using ProjetoCore2.Models;

namespace ProjetoCore2.Controllers
{
    public class SubCategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubCategorias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SubCategoria.Include(s => s.Categoria);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubCategorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SubCategoria == null)
            {
                return NotFound();
            }

            var subCategoria = await _context.SubCategoria
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategoria == null)
            {
                return NotFound();
            }

            return View(subCategoria);
        }

        // GET: SubCategorias/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Id");
            return View();
        }

        // POST: SubCategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,CategoriaId")] SubCategoria subCategoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subCategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Id", subCategoria.CategoriaId);
            return View(subCategoria);
        }

        // GET: SubCategorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SubCategoria == null)
            {
                return NotFound();
            }

            var subCategoria = await _context.SubCategoria.FindAsync(id);
            if (subCategoria == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Id", subCategoria.CategoriaId);
            return View(subCategoria);
        }

        // POST: SubCategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,CategoriaId")] SubCategoria subCategoria)
        {
            if (id != subCategoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoriaExists(subCategoria.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Id", subCategoria.CategoriaId);
            return View(subCategoria);
        }

        // GET: SubCategorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SubCategoria == null)
            {
                return NotFound();
            }

            var subCategoria = await _context.SubCategoria
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategoria == null)
            {
                return NotFound();
            }

            return View(subCategoria);
        }

        // POST: SubCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SubCategoria == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SubCategoria'  is null.");
            }
            var subCategoria = await _context.SubCategoria.FindAsync(id);
            if (subCategoria != null)
            {
                _context.SubCategoria.Remove(subCategoria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoriaExists(int id)
        {
          return _context.SubCategoria.Any(e => e.Id == id);
        }
    }
}
