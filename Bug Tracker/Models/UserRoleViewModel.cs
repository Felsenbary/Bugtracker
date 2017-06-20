using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class UserRoleViewModel
    {
        public ApplicationUser user { get; set; }
        public List<string> Role { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] SelectedRoles { get; set; }
        public MultiSelectList Roles { get; internal set; }
    }
}