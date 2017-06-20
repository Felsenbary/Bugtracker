using BugTracker.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserRoleAssignHelper userRole = new UserRoleAssignHelper();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated == true)
            {
                var userCollection = new AdminUserViewModel();
                var user = db.Users.Find(User.Identity.GetUserId());
                userCollection.user = user;
                userCollection.roles = userRole.ListUserRoles(user.Id).ToList();

                return View(userCollection);
            }
            return RedirectToAction("Login","Account");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}