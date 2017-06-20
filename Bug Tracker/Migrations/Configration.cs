using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BugTracker.Models;
using System.Data.Entity.Migrations;
using System.Linq;
namespace BugTracker.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "felsenbary@outlook.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "felsenbary@outlook.com",
                    Email = "felsenbary@outlook.com",
                    FirstName = "Fred",
                    LastName = "Elsenbary"
                }, "Abc&123!");
            }
           
            var userId = userManager.FindByEmail("felsenbary@outlook.com").Id;
            userManager.AddToRole(userId, "Admin");

            //TODO: Write code to seed the Ticket Priority table

            //TODO: Write code to seed the Ticket Status table

            /*
             TODO: Write code to seed all other tables that need 
                   to be seeded. Which are most likely the tables involved 
                   with building the dropdown controls in the Ticket create view
            */
        }
    }
}