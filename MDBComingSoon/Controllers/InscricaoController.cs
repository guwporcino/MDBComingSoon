using MDBComingSoon.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MDBComingSoon.Controllers
{
    public class InscricaoController : Controller
    {
        private InscricaoContext db = new InscricaoContext();

        // GET: Inscricao
        public async Task<ActionResult> Index()
        {
            var inscricaoViewModels = db.Inscricoes.Include(i => i.Pessoa);
            return View(await inscricaoViewModels.ToListAsync());
        }

        // GET: Inscricao/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InscricaoViewModel inscricaoViewModel = await db.Inscricoes.FindAsync(id);
            if (inscricaoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(inscricaoViewModel);
        }

        // GET: Inscricao/Create
        public ActionResult Create()
        {
            ViewBag.PessoaId = new SelectList(db.Pessoas, "Id", "Nome");
            return View();
        }

        // POST: Inscricao/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PessoaId")] InscricaoViewModel inscricaoViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Inscricoes.Add(inscricaoViewModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PessoaId = new SelectList(db.Pessoas, "Id", "Nome", inscricaoViewModel.PessoaId);
            return View(inscricaoViewModel);
        }

        // GET: Inscricao/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InscricaoViewModel inscricaoViewModel = await db.Inscricoes.FindAsync(id);
            if (inscricaoViewModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.PessoaId = new SelectList(db.Pessoas, "Id", "Nome", inscricaoViewModel.PessoaId);
            return View(inscricaoViewModel);
        }

        // POST: Inscricao/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PessoaId")] InscricaoViewModel inscricaoViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inscricaoViewModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PessoaId = new SelectList(db.Pessoas, "Id", "Nome", inscricaoViewModel.PessoaId);
            return View(inscricaoViewModel);
        }

        // GET: Inscricao/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InscricaoViewModel inscricaoViewModel = await db.Inscricoes.FindAsync(id);
            if (inscricaoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(inscricaoViewModel);
        }

        // POST: Inscricao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InscricaoViewModel inscricaoViewModel = await db.Inscricoes.FindAsync(id);
            db.Inscricoes.Remove(inscricaoViewModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
