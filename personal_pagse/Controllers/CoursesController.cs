using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Elmah;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using personal_pages;
using personal_pages.Helpers;
using personal_pages.Models;
using PagedList;

namespace Personal_Pages.Controllers
{
    public class CoursesController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        // GET: Courses
        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameDepSortParm = string.IsNullOrEmpty(sortOrder) ? "depname_desc" : "";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var courses = _db.Courses.Include(c => c.Departament).Include(c => c.User);

            if (!string.IsNullOrEmpty(searchString))
                courses = courses.Where(s => s.User.FirstName.Contains(searchString)
                                             || s.User.LastName.Contains(searchString));

            switch (sortOrder)
            {
                case "name_desc":
                    courses = courses.OrderByDescending(s => s.Name);
                    break;
                case "depname_desc":
                    courses = courses.OrderByDescending(s => s.Departament.Name);
                    break;
                default:
                    courses = courses.OrderBy(s => s.Name);
                    break;
            }

            const int pageSize = 10;
            var pageNumber = page ?? 1;

            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        // GET: Courses/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var course = await _db.Courses.FindAsync(id);
            if (course == null)
                return HttpNotFound();
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            ViewBag.DepartamentId = new SelectList(_db.Departaments, "DepId", "Name");
            ViewBag.TeacherId = new SelectList(_db.Users.Where(x => x.AspNetRole.Name == "Teacher"), "UserId",
                "FullName");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseId = Guid.NewGuid();
                course.Name = StringHelper.CutWhiteSpace(course.Name.ToTitleCase(TitleCase.All));

                _db.Courses.Add(course);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentId = new SelectList(_db.Departaments, "DepId", "Name", course.DepartamentId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(x => x.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var course = await _db.Courses.FindAsync(id);
            if (course == null)
                return HttpNotFound();
            ViewBag.DepartamentId = new SelectList(_db.Departaments, "DepId", "Name", course.DepartamentId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(x => x.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(course).State = EntityState.Modified;
                course.Name = StringHelper.CutWhiteSpace(course.Name.ToTitleCase(TitleCase.All));

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartamentId = new SelectList(_db.Departaments, "DepId", "Name", course.DepartamentId);
            ViewBag.TeacherId = new SelectList(_db.Users.Where(x => x.AspNetRole.Name == "Teacher"), "UserId",
                "FirstName");
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = await _db.Courses.FindAsync(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var course = await _db.Courses.FindAsync(id);
                _db.Courses.Remove(course);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Process is used");
                var course = await _db.Courses.FindAsync(id);
                ErrorSignal.FromCurrentContext().Raise(ex);
                return View(course);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult GetTeacher(Guid depId)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByName("Teacher").Users.ToList();
            IQueryable<User> users = null;

            foreach (var plm in role)
                users = _db.Users.Where(x => (x.RoleId == plm.RoleId) && (x.DepID == depId));
            var usersName = users.Select(x => new {x.UserId, x.FirstName}).ToList();
            var fullname = string.Empty;
            foreach (var i in usersName)
            {
                var plm = _db.Users.Where(x => x.UserId == i.UserId);
                foreach (var p in plm)
                {
                   fullname = p.FullName;
                }

            }

            return new JsonResult
            {
                Data = users.Select(x => new {x.UserId, fullname }).ToList(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}