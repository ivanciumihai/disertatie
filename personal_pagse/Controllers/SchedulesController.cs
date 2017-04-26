using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace personal_pages.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Schedules
        public async Task<ActionResult> Index()
        {
            var schedules = _db.Schedules.Include(s => s.Course).Include(s => s.User);
            return View(await schedules.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var schedule = await _db.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
            ViewBag.TeacherId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View();
        }

        // POST: Schedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                schedule.ScheduleId = Guid.NewGuid();
                _db.Schedules.Add(schedule);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name", schedule.CourseId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var schedule = await _db.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name", schedule.CourseId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(schedule).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name", schedule.CourseId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var schedule = await _db.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var schedule = await _db.Schedules.FindAsync(id);
            _db.Schedules.Remove(schedule);
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