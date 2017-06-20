using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminProjectAssignController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();        
        ProjectAssignHelper userProjects = new ProjectAssignHelper();

        // GET: AdminProjectAssign
        [Authorize(Roles = "Admin")]
        public ActionResult AssignProjects(int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = db.Users.Find(User.Identity.GetUserId());
            var projectUser = new AdminProjectAssignModel();
            projectUser.project = project;
            projectUser.user = user;
            projectUser.SelectedUsers = project.Users.Select(u => u.Id).ToArray();
            projectUser.ProjUsers = new MultiSelectList(db.Users, "Id", "Fullname", projectUser.SelectedUsers);
            return View(projectUser);
        }

        //// POST: AdminProjectAssign
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult AssignProjects(AdminProjectAssignModel model)
        {

            foreach (var user in db.Users.ToList())
            {
                userProjects.RemoveUserFromProject(user.Id, model.project.Id);
            }

            if (model.SelectedUsers != null)
            {
                foreach (var projectAdd in model.SelectedUsers)
                {
                    userProjects.AddUserToProject(projectAdd, model.project.Id);
                }
            }


      


            return RedirectToAction("Index","Projects");
        }




        //public ActionResult SelectRole(UserRoleViewModel model)
        //{

        //    var user = db.Users.Find(model.Id);
        //    foreach (var roleName in db.Roles.Select(r => r.Name).ToList())
        //    {
        //        userRole.RemoveUserFromRole(user.Id, roleName);
        //    }
        //    foreach (var roleadd in model.SelectedRoles)
        //    {
        //        userRole.AddUserToRole(user.Id, roleadd);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}