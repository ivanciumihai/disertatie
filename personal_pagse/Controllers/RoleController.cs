using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using personal_pages.Models;

namespace personal_pages.Controllers
{
    [Authorize(Roles = "Teacher, Admin, Secretary")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }

        /// <summary>
        ///     Get All Roles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        public bool isAdminUser()
        {
            if (!User.Identity.IsAuthenticated) return false;
            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(user.GetUserId());
            return s[0] == "Admin" || s[0] == "Teacher";
        }

        /// <summary>
        ///     Create  a New role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        ///     Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}