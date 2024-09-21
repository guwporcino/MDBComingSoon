using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoCoreDash.Data;
using ProjetoCoreDash.Models;

namespace ProjetoCoreDash.Controllers
{
    public class EscalasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EscalasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Escalas
        public async Task<IActionResult> Index()
        {
              return View(await _context.Escala.ToListAsync());
        }

        // GET: Escalas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Escala == null)
            {
                return NotFound();
            }

            var Escala = await _context.Escala
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Escala == null)
            {
                return NotFound();
            }

            return View(Escala);
        }

        // GET: Escalas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Escalas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao")] Escala Escala)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Escala);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Escala);
        }

        // GET: Escalas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Escala == null)
            {
                return NotFound();
            }

            var Escala = await _context.Escala.FindAsync(id);
            if (Escala == null)
            {
                return NotFound();
            }
            return View(Escala);
        }

        // POST: Escalas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao")] Escala Escala)
        {
            if (id != Escala.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Escala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EscalaExists(Escala.Id))
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
            return View(Escala);
        }

        // GET: Escalas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Escala == null)
            {
                return NotFound();
            }

            var Escala = await _context.Escala
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Escala == null)
            {
                return NotFound();
            }

            return View(Escala);
        }

        // POST: Escalas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Escala == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Escala'  is null.");
            }
            var Escala = await _context.Escala.FindAsync(id);
            if (Escala != null)
            {
                _context.Escala.Remove(Escala);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EscalaExists(int id)
        {
          return _context.Escala.Any(e => e.Id == id);
        }
    }
}
