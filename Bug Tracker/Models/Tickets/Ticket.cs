using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Display(Name = "Ticket")]
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int ProjectId { get; set; }

        [Display(Name = "Category")]
        public int TicketTypeId { get; set; }

        [Display(Name ="Priority")]
        public int TicketPriorityId { get; set; }

        [Display(Name ="Status")]
        public int TicketStatusId { get; set; }

        [Display(Name ="Ticket submitter")]
        public string OwnerUserId { get; set; }

        [Display(Name ="Assigned to")]
        public string AssignedToUserId { get; set; }

        public Ticket()
            {

        this.TicketAttachments = new HashSet<TicketAttachment>();        
        this.TicketHistories = new HashSet<TicketHistory>();
        this.TicketNotifications = new HashSet<TicketNotification>();
        this.TicketComments = new HashSet<TicketComment>();
            }


        public virtual ICollection<TicketAttachment>TicketAttachments  {get; set;}
        public virtual ICollection<TicketHistory>TicketHistories  {get; set;}
        public virtual ICollection<TicketNotification>TicketNotifications  {get; set;}
        public virtual ICollection<TicketComment>TicketComments {get; set;}




        public virtual Project Project { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

    }
}