using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace personal_pages.Controllers
{
    public class UniversitiesController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Universities
        public async Task<ActionResult> Index()
        {
            return View(await _db.Universities.ToListAsync());
        }

        // GET: Universities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Universities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UniversityId,Name")] University university)
        {
            if (ModelState.IsValid)
            {
                university.UniversityId = Guid.NewGuid();
                _db.Universities.Add(university);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(university);
        }

        // GET: Universities/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var university = await _db.Universities.FindAsync(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        // POST: Universities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UniversityId,Name")] University university)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(university).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(university);
        }

        // GET: Universities/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var university = await _db.Universities.FindAsync(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }

        // POST: Universities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var university = await _db.Universities.FindAsync(id);
            _db.Universities.Remove(university);
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