using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;


namespace personal_pages.Controllers
{
    public class ClassBooksController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: ClassBooks
        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.FullNameSortParm = string.IsNullOrEmpty(sortOrder) ? "fullname_desc" : "";
            ViewBag.TeacherSortParm = string.IsNullOrEmpty(sortOrder) ? "teacher_desc" : "";
            ViewBag.CourseSortParm = string.IsNullOrEmpty(sortOrder) ? "course_desc" : "";
            ViewBag.GradeSortParm = string.IsNullOrEmpty(sortOrder) ? "grade_desc" : "";
            ViewBag.PromotedSortParm = string.IsNullOrEmpty(sortOrder) ? "promoted_desc" : "";

            var classBooks = _db.ClassBooks.Include(c => c.Course).Include(c => c.User);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                classBooks = classBooks.Where(s => s.User.FirstName.Contains(searchString)
                                       || s.User.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "fullname_desc":
                    classBooks = classBooks.OrderByDescending(s => s.User.FirstName);
                    break;
                case "teacher_desc":
                    classBooks = classBooks.OrderByDescending(s => s.TeacherId);
                    break;
                case "course_desc":
                    classBooks = classBooks.OrderByDescending(s => s.Course.Name);
                    break;
                case "grade_desc":
                    classBooks = classBooks.OrderByDescending(s => s.Grade);
                    break;
                case "promoted_desc":
                    classBooks = classBooks.OrderByDescending(s => s.Promoted);
                    break;
                default:
                    classBooks = classBooks.OrderBy(s => s.User.FirstName);
                    break;
            }

            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(classBooks.ToPagedList(pageNumber, pageSize));
        }

        // GET: ClassBooks/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var classBook = await _db.ClassBooks.FindAsync(id);
            if (classBook == null)
            {
                return HttpNotFound();
            }
            return View(classBook);
        }

        // GET: ClassBooks/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
            ViewBag.StudentId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Student"), "UserId",
                "FullName");          
            return View();
        }

        // POST: ClassBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "ClassBookId,CourseId,StudentId,Grade")] ClassBook classBook)
        {
            if (ModelState.IsValid)
            {
                classBook.Grade_Date = DateTime.Now;
                classBook.Grade_modified = DateTime.Now;
                classBook.TeacherId = User.Identity.Name;
                classBook.ClassBookId = Guid.NewGuid();
                classBook.Promoted = classBook.Grade >= 5;
                _db.ClassBooks.Add(classBook);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name", classBook.CourseId);
            ViewBag.StudentId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Student"), "UserId",
                "FirstName");         
            return View(classBook);
        }

        // GET: ClassBooks/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var classBook = await _db.ClassBooks.FindAsync(id);
            if (classBook == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name", classBook.CourseId);
            ViewBag.StudentId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Student"), "UserId",
            "FullName");
            return View(classBook);
        }

        // POST: ClassBooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "ClassBookId,CourseId,StudentId,Grade,Grade_Date")] ClassBook classBook)
        {
            if (ModelState.IsValid)
            {
                classBook.TeacherId = User.Identity.Name;
                classBook.Grade_modified = DateTime.Now;
                classBook.Promoted = classBook.Grade >= 5;
                _db.Entry(classBook).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name", classBook.CourseId);
            ViewBag.StudentId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Student"), "UserId",
                "FullName");         
            return View(classBook);
        }

        // GET: ClassBooks/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var classBook = await _db.ClassBooks.FindAsync(id);
            if (classBook == null)
            {
                return HttpNotFound();
            }
            return View(classBook);
        }

        // POST: ClassBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var classBook = await _db.ClassBooks.FindAsync(id);
            _db.ClassBooks.Remove(classBook);
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