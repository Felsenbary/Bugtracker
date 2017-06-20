using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using BugTracker.Models;

namespace BugTracker.Models
{
    public class TicketHistoryHelper
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        public Ticket GetTicketById(int id)
        {
            return db.Tickets.FirstOrDefault(x => x.Id == id);
        }

        //public void ticketHistoryChange(int intTicketId, string strTitle, string strStatus, string strType, string strPriority, string strAssignedTo, string strCurrentUser)
        //{
        //    var oldTicket = db.Tickets.Find(intTicketId);

        //            if(oldTicket.Title != strTitle)
        //            {
        //                insertIntoHistory(strTitle, oldTicket.Title, "Title", strCurrentUser, intTicketId);
        //            }

        //            if (oldTicket.TicketStatus != strStatus)
        //            {
        //                insertIntoHistory(strStatus, oldTicket.TicketStatus, "Status", strCurrentUser, intTicketId);
        //            }

        //            if (oldTicket.TicketType != strType)
        //            {
        //                insertIntoHistory(strStatus, oldTicket.TicketType, "Type", strCurrentUser, intTicketId);
        //            }

        //            if (oldTicket.TicketPriority != strPriority)
        //            {
        //                insertIntoHistory(strStatus, oldTicket.TicketType, "Type", strCurrentUser, intTicketId);
        //            }

        //            if (oldTicket.TicketPriority != strPriority)
        //            {
        //                insertIntoHistory(strStatus, oldTicket.TicketType, "Type", strCurrentUser, intTicketId);
        //            }









        //            //var oldTicketComment = db.TicketComments.Find(ticketComment.Id);

        //            //    if(oldTicketComment.Id != ticketComment.Id)
        //            //{
        //            //    ticketHistory.Changed = DateTime.Now;
        //            //    ticketHistory.NewValue = ticketComment.Comment;
        //            //    ticketHistory.OldValue = oldTicketComment.
        //            //    ticketHistory.Property = "Ticket Comment";
        //            //    ticketHistory.TicketId = ticket.Id;
        //            //    ticketHistory.UserId = userId;
        //            //}



        //        }

        //              public bool insertIntoHistory( string newValue, string oldValue, string property, string userId, int ticketId)
        //        {
        //            TicketHistory ticketHistory = new TicketHistory();

        //            ticketHistory.Changed = DateTime.Now;
        //            ticketHistory.NewValue = newValue;
        //            ticketHistory.OldValue = oldValue;
        //            ticketHistory.Property = property;
        //            ticketHistory.TicketId = ticketId;
        //            ticketHistory.UserId = userId;
        //            db.TicketHistories.Add(ticketHistory);
        //            db.SaveChanges();
        //            return true ;
        //        }
        //    }
    }
}