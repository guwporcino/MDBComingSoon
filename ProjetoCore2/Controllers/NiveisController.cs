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
    public class NiveisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NiveisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Niveis
        public async Task<IActionResult> Index()
        {
              return _context.Nivel != null ? 
                          View(await _context.Nivel.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Nivel'  is null.");
        }

        // GET: Niveis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nivel == null)
            {
                return NotFound();
            }

            var nivel = await _context.Nivel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nivel == null)
            {
                return NotFound();
            }

            return View(nivel);
        }

        // GET: Niveis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Niveis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao")] Nivel nivel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nivel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nivel);
        }

        // GET: Niveis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nivel == null)
            {
                return NotFound();
            }

            var nivel = await _context.Nivel.FindAsync(id);
            if (nivel == null)
            {
                return NotFound();
            }
            return View(nivel);
        }

        // POST: Niveis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao")] Nivel nivel)
        {
            if (id != nivel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nivel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NivelExists(nivel.Id))
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
            return View(nivel);
        }

        // GET: Niveis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nivel == null)
            {
                return NotFound();
            }

            var nivel = await _context.Nivel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nivel == null)
            {
                return NotFound();
            }

            return View(nivel);
        }

        // POST: Niveis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nivel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Nivel'  is null.");
            }
            var nivel = await _context.Nivel.FindAsync(id);
            if (nivel != null)
            {
                _context.Nivel.Remove(nivel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NivelExists(int id)
        {
          return (_context.Nivel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
