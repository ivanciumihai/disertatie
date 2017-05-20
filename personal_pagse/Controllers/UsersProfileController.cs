using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Elmah;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using personal_pages.Helpers;
using personal_pages.Models;
using PagedList;



namespace personal_pages.Controllers
{
    [Authorize]
    public class UsersProfileController : Controller
    {
        private readonly personal_pageEntities _db = new personal_pageEntities();
        private ApplicationUserManager _userManager;

        private ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        // GET: UsersProfile
        public async Task<ViewResult> Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.LastNameSortParm = string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            ViewBag.FirstDepSortParm = string.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            ViewBag.UnivSortParm = string.IsNullOrEmpty(sortOrder) ? "univ_desc" : "";
            ViewBag.FacSortParm = string.IsNullOrEmpty(sortOrder) ? "fac_desc" : "";
            ViewBag.DepSortParm = string.IsNullOrEmpty(sortOrder) ? "dep_desc" : "";
            ViewBag.GroupSortParm = string.IsNullOrEmpty(sortOrder) ? "group_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var users =
                _db.Users.Include(u => u.Departament)
                    .Include(u => u.Education_Form)
                    .Include(u => u.Faculty)
                    .Include(u => u.Faculty.University)
                    .Include(u => u.AspNetRole)
                    .Include(u => u.AspNetUser);


            if (User.IsInRole("Teacher"))
            {
                var strCurrentUserId = User.Identity.GetUserId();
                var userDetails = await _db.Users.FindAsync(strCurrentUserId);

                users =
                    _db.Users.Include(u => u.Departament)
                        .Include(u => u.Education_Form)
                        .Include(u => u.Faculty)
                        .Include(u => u.Faculty.University)
                        .Include(u => u.AspNetRole)
                        .Include(u => u.AspNetUser)
                        .Where(u => u.DepID == userDetails.DepID);
            }

            if (User.IsInRole("Secretary"))
            {
                var strCurrentUserId = User.Identity.GetUserId();
                var userDetails = await _db.Users.FindAsync(strCurrentUserId);

                users =
                    _db.Users.Include(u => u.Departament)
                        .Include(u => u.Education_Form)
                        .Include(u => u.Faculty)
                        .Include(u => u.Faculty.University)
                        .Include(u => u.AspNetRole)
                        .Include(u => u.AspNetUser)
                        .Where(u => u.FacultyId == userDetails.FacultyId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "lastname_desc":
                    users = users.OrderByDescending(s => s.LastName);
                    break;
                case "firstname_desc":
                    users = users.OrderByDescending(s => s.FirstName);
                    break;
                case "fac_desc":
                    users = users.OrderByDescending(s => s.Faculty.Name);
                    break;
                case "dep_desc":
                    users = users.OrderByDescending(s => s.Departament.Name);
                    break;
                case "group_desc":
                    users = users.OrderByDescending(s => s.GroupNumber);
                    break;
                case "univ_desc":
                    users = users.OrderByDescending(s => s.Faculty.University.Name);
                    break;
                default:
                    users = users.OrderBy(s => s.LastName);
                    break;
            }
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        // GET: UsersProfile/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _db.Users.FindAsync(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.ImagePath == null)
            {
                user.ImagePath = "default-avatar.png";
            }
            return View(user);
        }

        // GET: UsersProfile/Create
        [Authorize(Roles = "Admin, Secretary")]
        public async Task<ViewResult> Create()
        {
            var ll = _db.Users.ToList();
            List<string> bb = ll.Select(a => a.UserId).ToList();
            if (User.IsInRole("Admin"))
            {
                ViewBag.DepID = new SelectList(_db.Departaments, "DepId", "Name");
                ViewBag.Ed_Form = new SelectList(_db.Education_Form, "id", "name");
                ViewBag.FacultyId = new SelectList(_db.Faculties, "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
                ViewBag.RoleId = new SelectList(_db.AspNetRoles.Where(a => a.Name != "Admin"), "Id", "Name");
                ViewBag.UserId = new SelectList(_db.AspNetUsers.Where(x => x.User.UserId != x.Id), "Id", "UserName");
            }
            else
            {
                var strCurrentUserId = User.Identity.GetUserId();
                var userDetails = await _db.Users.FindAsync(strCurrentUserId);
                ViewBag.DepID = new SelectList(_db.Departaments, "DepId", "Name");
                ViewBag.Ed_Form = new SelectList(_db.Education_Form, "id", "name");
                ViewBag.FacultyId = new SelectList(_db.Faculties.Where(x=>x.FacultyId == userDetails.FacultyId), "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities.Where(x=>x.UniversityId==userDetails.UniversityId), "UniversityId", "Name");
                ViewBag.RoleId = new SelectList(_db.AspNetRoles.Where(a => a.Name != "Admin"), "Id", "Name");
                ViewBag.UserId = new SelectList(_db.AspNetUsers.Where(x => x.User.UserId != x.Id && x.User.FacultyId == userDetails.FacultyId), "Id", "UserName");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Secretary")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                user.Reg_date = DateTime.Now;
                user.FirstName = StringHelper.CutWhiteSpace(user.FirstName.ToTitleCase(TitleCase.All));
                user.LastName = StringHelper.CutWhiteSpace(user.LastName.ToTitleCase(TitleCase.All));
                user.FatherName = StringHelper.CutWhiteSpace(user.FatherName.ToTitleCase(TitleCase.All));
                var context = new ApplicationDbContext();
                var roles = await context.Users
                                    .Where(u => u.Id == user.UserId)
                                    .SelectMany(u => u.Roles)
                                    .Join(context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r)
                                    .ToListAsync();

                foreach (var role in roles)
                {
                    user.RoleId = role.Id;
                }

                if (image != null)
                {
                    var imageName = (user.FirstName + "." + user.LastName + Path.GetExtension(image.FileName)).Replace(" ", "");
                    var physicalPath = Server.MapPath("~/Img/" + imageName);
                    image.SaveAs(physicalPath);
                    user.ImagePath = imageName;
                }

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            if (User.IsInRole("Admin"))
            {
                ViewBag.DepID = new SelectList(_db.Departaments, "DepId", "Name");
                ViewBag.Ed_Form = new SelectList(_db.Education_Form, "id", "name");
                ViewBag.FacultyId = new SelectList(_db.Faculties, "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
                ViewBag.RoleId = new SelectList(_db.AspNetRoles.Where(a => a.Name != "Admin"), "Id", "Name");
                ViewBag.UserId = new SelectList(_db.AspNetUsers.Where(x => x.User.UserId != x.Id), "Id", "UserName");
            }
            else
            {
                var strCurrentUserId = User.Identity.GetUserId();
                var userDetails = await _db.Users.FindAsync(strCurrentUserId);
                ViewBag.DepID = new SelectList(_db.Departaments, "DepId", "Name");
                ViewBag.Ed_Form = new SelectList(_db.Education_Form, "id", "name");
                ViewBag.FacultyId = new SelectList(_db.Faculties.Where(x => x.FacultyId == userDetails.FacultyId), "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities.Where(x => x.UniversityId == userDetails.UniversityId), "UniversityId", "Name");
                ViewBag.RoleId = new SelectList(_db.AspNetRoles.Where(a => a.Name != "Admin"), "Id", "Name");
                ViewBag.UserId = new SelectList(_db.AspNetUsers.Where(x => x.User.UserId != x.Id && x.User.FacultyId == userDetails.FacultyId), "Id", "UserName");
            }
            return View(user);
        }

        // GET: UsersProfile/Edit/
        [Authorize(Roles = "Admin, Secretary")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
            {
                ViewBag.DepID = new SelectList(_db.Departaments, "DepId", "Name", user.DepID);
                ViewBag.Ed_Form = new SelectList(_db.Education_Form, "id", "name", user.Ed_Form);
                ViewBag.FacultyId = new SelectList(_db.Faculties, "FacultyId", "Name", user.FacultyId);
                ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name",
                    user.Faculty.UniversityId);
                ViewBag.RoleId = User.IsInRole("Admin")
                    ? new SelectList(_db.AspNetRoles, "Id", "Name")
                    : new SelectList(_db.AspNetRoles.Where(a => a.Name != "Admin"), "Id", "Name");
            }
            else
            {
                var strCurrentUserId = User.Identity.GetUserId();
                var userDetails = await _db.Users.FindAsync(strCurrentUserId);
                ViewBag.DepID = new SelectList(_db.Departaments, "DepId", "Name");
                ViewBag.Ed_Form = new SelectList(_db.Education_Form, "id", "name");
                ViewBag.FacultyId = new SelectList(_db.Faculties.Where(x => x.FacultyId == userDetails.FacultyId), "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities.Where(x => x.UniversityId == userDetails.UniversityId), "UniversityId", "Name");
                ViewBag.RoleId = new SelectList(_db.AspNetRoles.Where(a => a.Name != "Admin"), "Id", "Name");
                ViewBag.UserId = new SelectList(_db.AspNetUsers.Where(x => x.User.UserId != x.Id && x.User.FacultyId == userDetails.FacultyId), "Id", "UserName");
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Secretary")]
        public async Task<ActionResult> Edit(User user, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(user).State = EntityState.Modified;
                user.Reg_date = _db.Users.Where(x => x.UserId == user.UserId).Select(x => x.Reg_date).FirstOrDefault();
                user.FirstName = StringHelper.CutWhiteSpace(user.FirstName.ToTitleCase(TitleCase.All));
                user.LastName = StringHelper.CutWhiteSpace(user.LastName.ToTitleCase(TitleCase.All));
                user.FatherName = StringHelper.CutWhiteSpace(user.FatherName.ToTitleCase(TitleCase.All));

                if (image != null)
                {
                    if (user.ImagePath != null)
                    {
                        var filePath = Server.MapPath("~/Img/" + user.ImagePath);
                        System.IO.File.Delete(filePath);
                    }

                    var imageName = (user.FirstName + "." + user.LastName + Path.GetExtension(image.FileName)).Replace(" ", "");
                    var physicalPath = Server.MapPath("~/Img/" + imageName);
                    image.SaveAs(physicalPath);
                    user.ImagePath = imageName;
                }

                var userDetails = await _db.AspNetUsers.FindAsync(user.UserId);
                var oldRole = userDetails?.AspNetRoles.First();

                var newuserRole = await _db.AspNetRoles.FindAsync(user?.RoleId);
                if (oldRole?.Name != newuserRole.Name)
                {
                    await UserManager.AddToRoleAsync(user.UserId, newuserRole?.Name);
                    await UserManager.RemoveFromRoleAsync(user.UserId, oldRole?.Name);
                }
                await _db.SaveChangesAsync();


                return RedirectToAction("Index");
            }
            if (User.IsInRole("Admin"))
            {
                ViewBag.DepID = new SelectList(_db.Departaments, "DepId", "Name");
                ViewBag.Ed_Form = new SelectList(_db.Education_Form, "id", "name");
                ViewBag.FacultyId = new SelectList(_db.Faculties, "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities, "UniversityId", "Name");
                ViewBag.RoleId = new SelectList(_db.AspNetRoles.Where(a => a.Name != "Admin"), "Id", "Name");
                ViewBag.UserId = new SelectList(_db.AspNetUsers.Where(x => x.User.UserId != x.Id), "Id", "UserName");
            }
            else
            {
                var strCurrentUserId = User.Identity.GetUserId();
                var userDetails = await _db.Users.FindAsync(strCurrentUserId);
                ViewBag.DepID = new SelectList(_db.Departaments, "DepId", "Name");
                ViewBag.Ed_Form = new SelectList(_db.Education_Form, "id", "name");
                ViewBag.FacultyId = new SelectList(_db.Faculties.Where(x => x.FacultyId == userDetails.FacultyId), "FacultyId", "Name");
                ViewBag.UniversityId = new SelectList(_db.Universities.Where(x => x.UniversityId == userDetails.UniversityId), "UniversityId", "Name");
                ViewBag.RoleId = new SelectList(_db.AspNetRoles.Where(a => a.Name != "Admin"), "Id", "Name");
                ViewBag.UserId = new SelectList(_db.AspNetUsers.Where(x => x.User.UserId != x.Id && x.User.FacultyId == userDetails.FacultyId), "Id", "UserName");
            }
            return View(user);
        }

        // GET: UsersProfile/Delete/5
        [Authorize(Roles = "Admin, Secretary")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.ImagePath == null)
            {
                user.ImagePath = "default-avatar.png";
            }

            return View(user);
        }

        // POST: UsersProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin, Secretary")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user != null) _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Process is used");
                var user = await _db.Users.FindAsync(id);
                ErrorSignal.FromCurrentContext().Raise(ex);
                return View(user);
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

        public JsonResult GetUserNameRole(string id)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var firstOrDefault = userManager.GetRoles(id).FirstOrDefault();
            var roles = firstOrDefault != null && firstOrDefault.Contains("Admin");
            return new JsonResult { Data = roles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetFaculty(Guid universityId)
        {
            var faculties = _db.Faculties.Where(a => a.UniversityId.Equals(universityId)).DefaultIfEmpty();
            return new JsonResult { Data = faculties.Select(x => new { x.Name, x.FacultyId }).ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetUserRole(string id)
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var firstOrDefault = userManager.GetRoles(id).FirstOrDefault();
            return new JsonResult { Data = firstOrDefault, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
