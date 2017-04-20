using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using personal_pages.Models;

//[assembly: OwinStartupAttribute(typeof(personal_pages.Startup))]

namespace personal_pages
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login
        private void createRolesandUsers()
        {
            var context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User 
            //   if (!roleManager.RoleExists("Admin"))
            //  {

            // first we create Admin rool
            var role = new IdentityRole();
            role.Name = "secretary";
            roleManager.Create(role);

            //Here we create a Admin super user who will maintain the website				

            var user = new ApplicationUser();
            user.UserName = "secretary";
            user.Email = "secretary@gmail.com";

            var userPWD = "Wargames1992!";

            user.Reg_Date = DateTime.Now;

            var chkUser = UserManager.Create(user, userPWD);

            //Add default User to Role Admin
            if (chkUser.Succeeded)
            {
                var result1 = UserManager.AddToRole(user.Id, "secretary");
            }
            //}
        }
    }
}