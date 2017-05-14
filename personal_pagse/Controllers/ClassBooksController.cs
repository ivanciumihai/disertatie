using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using personal_pages.Models;
using PagedList;


namespace personal_pages.Controllers
{
    [Authorize]
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
            var strCurrentUserId = User.Identity.GetUserId();

            if (User.IsInRole("Student"))
            {
                 classBooks = _db.ClassBooks.Include(c => c.Course).Include(c => c.User).Where(x=>x.StudentId==strCurrentUserId);
            }

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
                                       || s.User.LastName.Contains(searchString));
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

            var user = await _db.Users.FindAsync(classBook.StudentId);
            if (user != null && user.ImagePath == null)
            {
                user.ImagePath = "default-avatar.png";
            }
            return View(classBook);
        }

        // GET: ClassBooks/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
            ViewBag.StudentId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Student"), "UserId",
                "FullName");
            ViewBag.DepId = new SelectList(_db.Departaments, "DepId", "Name");
            ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
            return View();
        }

        // POST: ClassBooks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassBook classBook)
        {
            if (ModelState.IsValid)
            {
                var getUsers = _db.ClassBooks.Any(x => x.StudentId == classBook.StudentId && x.CourseId==classBook.CourseId);

                if (getUsers)
                {
                    ModelState.AddModelError(string.Empty, "Student already has a grade on this");
                    ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
                    ViewBag.StudentId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Student"), "UserId",
                        "FullName");
                    ViewBag.DepId = new SelectList(_db.Departaments, "DepId", "Name");
                    ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
                    ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
                    return View(classBook);
                }
                classBook.Grade_Date = DateTime.Now;
                classBook.Grade_modified = DateTime.Now;
                classBook.TeacherId = User.Identity.Name;
                classBook.ClassBookId = Guid.NewGuid();
                classBook.Promoted = classBook.Grade != null && Math.Round((double)classBook.Grade) >= 5;
                _db.ClassBooks.Add(classBook);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name", classBook.CourseId);
            ViewBag.StudentId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Student"), "UserId",
                "FirstName");
            ViewBag.DepId = new SelectList(_db.Departaments, "DepId", "Name");
            ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
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
            ViewBag.DepId = new SelectList(_db.Departaments, "DepId", "Name");
            ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
            return View(classBook);
        }

        // POST: ClassBooks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ClassBookId,CourseId,StudentId,Grade,Promoted,TeacherId,Grade_Date,Grade_modified")] ClassBook classBook)
        {
            if (ModelState.IsValid)
            {
                var getUsers = _db.ClassBooks.Any(x => x.StudentId == classBook.StudentId && x.CourseId == classBook.CourseId && x.Grade == classBook.Grade);
                var classBooksPage = await _db.ClassBooks.FindAsync(classBook.CourseId);
                if (getUsers)
                {
                    ModelState.AddModelError(string.Empty, "Student already has this grade");
                    ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name");
                    ViewBag.StudentId = new SelectList(_db.Users.Where(a => a.AspNetRole.Name == "Student"), "UserId",
                        "FullName");
                    ViewBag.DepId = new SelectList(_db.Departaments, "DepId", "Name");
                    ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
                    ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
                    return View(classBooksPage);
                }
              //  classBook.User = classBook.User;
                classBook.TeacherId = User.Identity.Name;
                classBook.Grade_modified = DateTime.Now;
                classBook.Promoted = classBook.Grade != null && Math.Round((double)classBook.Grade) >= 5;
                _db.Entry(classBook).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "Name", classBook.CourseId);
            ViewBag.DepId = new SelectList(_db.Departaments, "DepId", "Name");
            ViewBag.FacultyId = new SelectList(_db.Departaments, "FacultyId", "Name");
            ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
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

        public JsonResult GetStudents(Guid depId)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByName("Student").Users.ToList();
            IQueryable<User> users = null;

            foreach (var user in role)
                users = _db.Users.Where(x => (x.RoleId == user.RoleId) && (x.DepID == depId));
            var usersName = users.Select(x => new { x.UserId, x.FirstName }).ToList();
            var fullname = string.Empty;
            foreach (var p in usersName.Select(i => _db.Users.Where(x => x.UserId == i.UserId)).SelectMany(plm => plm))
            {
                fullname = p.FullName;
            }
            try
            {
                return new JsonResult
                {
                    Data = users.Select(x => new {x.UserId, fullname}).ToList(),
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

        public JsonResult GetCourse(Guid depId)
        {
            var courses = _db.Courses.Where(a => a.DepartamentId.Equals(depId)).DefaultIfEmpty();
            try
            {
                return new JsonResult
                {
                    Data = courses.Select(x => new { x.Name, x.CourseId }).ToList(),
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