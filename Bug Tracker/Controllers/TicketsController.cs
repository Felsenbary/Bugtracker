using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ProjectAssignHelper helper = new ProjectAssignHelper();
        public UserRoleAssignHelper roleHelper = new UserRoleAssignHelper();

       


        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var project = user.Projects;
            var tickets = project.SelectMany(t => t.Tickets).ToList();
            
            if (User.IsInRole("Admin"))
            {
                return View(db.Tickets.ToList());
            }
            else if (User.IsInRole("ProjectManager"))
            {
                return View(tickets);
            }

                return View(db.Tickets.Where(t => t.AssignedToUserId == userId || t.OwnerUserId == userId));
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize]

        public ActionResult Create(int id)
        {
            var newTicket = new Ticket();
            newTicket.ProjectId = id;
            newTicket.OwnerUserId = User.Identity.GetUserId();
            ViewBag.TicketPriorityId = new SelectList(db.Ticketpriorities, "Id", "Name", newTicket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Ticketstatuses, "Id", "Name", newTicket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", newTicket.TicketTypeId);
            return View(newTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Title,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        public ActionResult Create([Bind(Include = "Title,ProjectId,OwnerUserId,TicketTypeId,TicketPriorityId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {              
                ticket.Created = DateTime.Now;
                ticket.TicketStatusId = db.Ticketstatuses.FirstOrDefault(x => x.Name == "Unassigned").Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.Ticketpriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Ticketstatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            var roleId = db.Roles.FirstOrDefault(r => r.Name == "Developer").Id;

            var developer = new List<ApplicationUser>();
            foreach (var user in db.Users)
            {
                foreach(var role in user.Roles)
                {
                    if(role.RoleId == roleId)
                    {
                        developer.Add(user);
                    }
                    
                }
            }

            //ViewBag.ticketInfo = ticket.Project.Id;
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.ProjectName = db.Projects.FirstOrDefault(x => x.Id == ticket.ProjectId).Name;
            ViewBag.AssignedToUserId = new SelectList(developer, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.TicketPriorityId = new SelectList(db.Ticketpriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Ticketstatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
              

                var ticketHelper = new TicketHistoryHelper();
                var oldTicket = ticketHelper.GetTicketById(ticket.Id);

                // Assigned to user ticket history
                if (ticket.AssignedToUserId != oldTicket.AssignedToUserId)
                {
                    var ticketHistory = new TicketHistory();
                    var NewAssignedUser = db.Users.FirstOrDefault(u => u.Id == ticket.AssignedToUserId).FirstName;
                    ticketHistory.Changed = DateTime.Now;                   
                    ticketHistory.NewValue = NewAssignedUser;
                    ticketHistory.Property = "Assigned to";
                    ticketHistory.TicketId = ticket.Id;
                    ticketHistory.UserId = User.Identity.GetUserId();
                    db.TicketHistories.Add(ticketHistory);
                    db.SaveChanges();
                }
                var ticketNotification = new TicketNotification();
                //var user = db.Roles.FirstOrDefault(r => r.Name == "Developer");
                if (ticket.TicketStatusId == db.Ticketstatuses.FirstOrDefault(x => x.Name == "Assigned").Id)
                    {
                    ticketNotification.UserId = ticket.AssignedToUserId;
                    ticketNotification.TicketId = ticket.Id;
                    db.TicketNotifications.Add(ticketNotification);
                    db.SaveChanges();
                }
                // ticket type in ticket history
                if (ticket.TicketTypeId != oldTicket.TicketTypeId)
                {
                    var ticketHistory = new TicketHistory();
                    ticketHistory.Changed = DateTime.Now;                   
                    ticketHistory.NewValue = db.TicketTypes.Find(ticket.TicketTypeId).Name;
                    ticketHistory.Property = "Ticket Category";
                    ticketHistory.TicketId = ticket.Id;
                    ticketHistory.UserId = User.Identity.GetUserId();
                    db.TicketHistories.Add(ticketHistory);
                    db.SaveChanges();
                }
                // ticket priority in ticket history
                if (ticket.TicketPriorityId != oldTicket.TicketPriorityId)
                {
                    var ticketHistory = new TicketHistory();
                    ticketHistory.Changed = DateTime.Now;
                    ticketHistory.NewValue = db.Ticketpriorities.Find(ticket.TicketPriorityId).Name;
                    ticketHistory.Property = "Ticket Priority";
                    ticketHistory.TicketId = ticket.Id;
                    ticketHistory.UserId = User.Identity.GetUserId();
                    db.TicketHistories.Add(ticketHistory);
                    db.SaveChanges();
                }

                // ticket status in ticket history
                if (ticket.TicketStatusId != oldTicket.TicketStatusId)
                {
                    var ticketHistory = new TicketHistory();
                    ticketHistory.Changed = DateTime.Now;
                    ticketHistory.NewValue = db.Ticketstatuses.Find(ticket.TicketStatusId).Name;
                    ticketHistory.Property = "Ticket Status";
                    ticketHistory.TicketId = ticket.Id;
                    ticketHistory.UserId = User.Identity.GetUserId();
                    db.TicketHistories.Add(ticketHistory);
                    db.SaveChanges();
                }

                ticket.Updated = DateTime.Now;
                db.Tickets.Attach(ticket);
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.Ticketpriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.Ticketstatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
