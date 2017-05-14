using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using personal_pages.Models;

namespace personal_pages.Controllers
{
    [Authorize]
    [Authorize(Roles = "Teacher, Admin, Secretary")]
    public class UsersController : Controller
    {
        // GET: Users
        public bool isAdminUser()
        {
            if (!User.Identity.IsAuthenticated) return false;
            var user = User.Identity;
            var context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(user.GetUserId());
            return s[0] == "Admin";
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            ViewBag.Name = "Not Logged IN";


            return View();
        }
    }
}