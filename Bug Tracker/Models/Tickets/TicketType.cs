using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        [Display(Name = "Ticket Type")]
        public string Name { get; set; }







        public virtual ICollection<Ticket>Tickets { get; set; }
        public TicketType()
            {
            this.Tickets = new HashSet<Ticket>();
        }
    }
}