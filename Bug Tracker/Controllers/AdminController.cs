using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserRoleAssignHelper userRole = new UserRoleAssignHelper();


        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<AdminUserViewModel> userList = new List<AdminUserViewModel>();
            foreach (var users in db.Users.ToList())
            {
                var userCollection = new AdminUserViewModel();
                userCollection.user = users;
                userCollection.roles = userRole.ListUserRoles(users.Id).ToList();
                userList.Add(userCollection);
            }

            return View(userList);

        }
        //GET: SelectRole

        public ActionResult SelectRole(string userId)
        {
            var user = db.Users.Find(userId);
            var roleUser = new UserRoleViewModel();
            roleUser.Id = userId;
            roleUser.FirstName = user.FirstName;
            roleUser.LastName = user.LastName;
            roleUser.SelectedRoles = userRole.ListUserRoles(user.Id).ToArray();
            roleUser.Roles = new MultiSelectList(db.Roles, "Name", "Name", roleUser.SelectedRoles);

            return View(roleUser);
        }

        //POST: SelectRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectRole(UserRoleViewModel model)
        {

            var user = db.Users.Find(model.Id);
            foreach (var roleName in db.Roles.Select(r => r.Name).ToList())
            {
                userRole.RemoveUserFromRole(user.Id, roleName);
            }
            if (model.SelectedRoles != null)
            {
                foreach (var roleadd in model.SelectedRoles)
                {
                    userRole.AddUserToRole(user.Id, roleadd);
                }
            }
            return RedirectToAction("Index");
        }


    }
}