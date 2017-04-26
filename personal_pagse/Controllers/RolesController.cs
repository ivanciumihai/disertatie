using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace personal_pages.Controllers
{
    [MyAuthorize]
    public class RolesController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Roles
        public async Task<ActionResult> Index()
        {
            return View(await _db.AspNetRoles.ToListAsync());
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                aspNetRole.Id = Guid.NewGuid().ToString();
                _db.AspNetRoles.Add(aspNetRole);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(aspNetRole);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspNetRole = await _db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(aspNetRole).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNetRole);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspNetRole = await _db.AspNetRoles.FindAsync(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var aspNetRole = await _db.AspNetRoles.FindAsync(id);
            _db.AspNetRoles.Remove(aspNetRole);
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