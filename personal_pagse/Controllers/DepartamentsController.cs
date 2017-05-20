using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Elmah;
using Microsoft.AspNet.Identity;
using personal_pages.Helpers;

namespace personal_pages.Controllers
{
    [Authorize(Roles = "Admin, Secretary")]
    public class DepartamentsController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Departaments
        public async Task<ActionResult> Index()
        {
            var strCurrentUserId = User.Identity.GetUserId();
            var userDetails = await _db.Users.FindAsync(strCurrentUserId);
            return User.IsInRole("Secretary") ? View(await _db.Departaments.Where(x => x.FacultyId == userDetails.FacultyId).ToListAsync()) : View(await _db.Departaments.ToListAsync());

        }


        // GET: Departaments/Create
        public async Task<ViewResult> Create()
        {
            var strCurrentUserId = User.Identity.GetUserId();
            var userDetails = await _db.Users.FindAsync(strCurrentUserId);
            if (User.IsInRole("Admin"))
            {
                ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
            }
            else
            {
                ViewBag.FacultyId = new SelectList(_db.Departaments.Where(x => x.FacultyId == userDetails.FacultyId), "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities.Where(x => x.UniversityId == userDetails.UniversityId), "UniversityId", "Name");
            }
            return View();
        }

        // POST: Departaments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Departament departament)
        {
            if (ModelState.IsValid)
            {
                departament.DepId = Guid.NewGuid();
                departament.Name = StringHelper.CutWhiteSpace(departament.Name.ToTitleCase(TitleCase.All));

                _db.Departaments.Add(departament);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var strCurrentUserId = User.Identity.GetUserId();
            var userDetails = await _db.Users.FindAsync(strCurrentUserId);
            if (User.IsInRole("Admin"))
            {
                ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
            }
            else
            {
                ViewBag.FacultyId = new SelectList(_db.Departaments.Where(x => x.FacultyId == userDetails.FacultyId),
                    "FacultyId", "Name");
                ViewBag.UniversityId =
                    new SelectList(_db.Universities.Where(x => x.UniversityId == userDetails.UniversityId),
                        "UniversityId", "Name");
            }
            return View(departament);
        }

        // GET: Departaments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var strCurrentUserId = User.Identity.GetUserId();
            var userDetails = await _db.Users.FindAsync(strCurrentUserId);
            if (User.IsInRole("Admin"))
            {
                ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
            }
            else
            {
                ViewBag.FacultyId = new SelectList(_db.Departaments.Where(x => x.FacultyId == userDetails.FacultyId),
                    "FacultyId", "Name");
                ViewBag.UniversityId =
                    new SelectList(_db.Universities.Where(x => x.UniversityId == userDetails.UniversityId),
                        "UniversityId", "Name");
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
            if (!string.IsNullOrEmpty(departament.Name))
            {
                _db.Entry(departament).State = EntityState.Modified;
                departament.Name = StringHelper.CutWhiteSpace(departament.Name.ToTitleCase(TitleCase.All));
                var db2 = new personal_pageEntities();
                var departaments = await db2.Departaments.FindAsync(departament.DepId);

                if (departament.StartDate == null)
                {
                    departament.StartDate = departaments.StartDate;
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var strCurrentUserId = User.Identity.GetUserId();
            var userDetails = await _db.Users.FindAsync(strCurrentUserId);
            if (User.IsInRole("Admin"))
            {
                ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
            }
            else
            {
                ViewBag.FacultyId = new SelectList(_db.Departaments.Where(x => x.FacultyId == userDetails.FacultyId),
                    "FacultyId", "Name");
                ViewBag.UniversityId =
                    new SelectList(_db.Universities.Where(x => x.UniversityId == userDetails.UniversityId),
                        "UniversityId", "Name");
            }
            return View(departament);
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

            try
            {
                _db.Departaments.Remove(departament);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Process is used");
                ErrorSignal.FromCurrentContext().Raise(ex);
                return View(departament);
            }
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

        public JsonResult GetFaculty(Guid universityId)
        {
            try
            {
                var faculties = _db.Faculties.Where(a => a.UniversityId.Equals(universityId)).DefaultIfEmpty();
                return new JsonResult
                {
                    Data = faculties.Select(x => new { x.Name, x.FacultyId }).ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = null,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}