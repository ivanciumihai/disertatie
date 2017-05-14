using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using personal_pages.Helpers;

namespace personal_pages.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Secretary")]
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
            ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");

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

            ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
            return View(departament);
        }

        // GET: Departaments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
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
                var departaments = await _db.Departaments.FindAsync(departament.DepId);

                if (departament.StartDate == null)
                {
                    departament.StartDate = departaments.StartDate;
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
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

        public JsonResult GetFaculty(Guid universityId)
        {
            try
            {
                var faculties = _db.Faculties.Where(a => a.UniversityId.Equals(universityId)).DefaultIfEmpty();
                return new JsonResult
                {
                    Data = faculties.Select(x => new {x.Name, x.FacultyId}).ToList(),
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