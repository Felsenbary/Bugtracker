
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class AdminProjectAssignModel
    {
        public ApplicationUser user { get; set; }        
        public Project project { get; set; }
        public string[] SelectedUsers { get; set; }
        public MultiSelectList ProjUsers { get; set;  }

    }

    
}