using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace personal_pages.Controllers
{
    public class DepartamentsController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Departaments
        public async Task<ActionResult> Index()
        {
            return View(await _db.Departaments.ToListAsync());
        }


        // GET: Departaments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departaments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Departament departament)
        {
            if (!ModelState.IsValid) return View(departament);
            departament.DepId = Guid.NewGuid();
            _db.Departaments.Add(departament);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Departaments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var departament = await _db.Departaments.FindAsync(id);
            if (departament == null)
            {
                return HttpNotFound();
            }
            return View(departament);
        }

        // POST: Departaments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Departament departament)
        {
            if (!ModelState.IsValid) return View(departament);
            _db.Entry(departament).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Departaments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var departament = await _db.Departaments.FindAsync(id);
            if (departament == null)
            {
                return HttpNotFound();
            }
            return View(departament);
        }

        // POST: Departaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var departament = await _db.Departaments.FindAsync(id);
            _db.Departaments.Remove(departament);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}