using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Elmah;
using personal_pages.Helpers;

namespace personal_pages.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Secretary")]
    public class FacultiesController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Faculties
        public async Task<ActionResult> Index()
        {
            var faculties = _db.Faculties.Include(f => f.University);
            return View(await faculties.ToListAsync());
        }

        // GET: Faculties/Create
        public ActionResult Create()
        {
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
            return View();
        }

        // POST: Faculties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                faculty.FacultyId = Guid.NewGuid();
                faculty.Name = StringHelper.CutWhiteSpace(faculty.Name.ToTitleCase(TitleCase.All));
                _db.Faculties.Add(faculty);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name", faculty.UniversityId);
            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var faculty = await _db.Faculties.FindAsync(id);
            if (faculty == null)
                return HttpNotFound();
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name", faculty.UniversityId);
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(faculty).State = EntityState.Modified;
                faculty.Name = StringHelper.CutWhiteSpace(faculty.Name.ToTitleCase(TitleCase.All));
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name", faculty.UniversityId);
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var faculty = await _db.Faculties.FindAsync(id);
            if (faculty == null)
                return HttpNotFound();
            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var faculty = await _db.Faculties.FindAsync(id);

            try
            {
                _db.Faculties.Remove(faculty);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Process is used");
                ErrorSignal.FromCurrentContext().Raise(ex);
                return View(faculty);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}