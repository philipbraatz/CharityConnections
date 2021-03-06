using Doorfail.Connections.PL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//TODO use API
namespace Doorfail.Connections.WebUI.Controllers
{
    //TODO create views for each method
    public class AdminController : Controller
    {
        // GET: Admin
        [ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            //TODO some menu with table options and other things
            //TODO check if admin or localhost
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.Charities.ToList());
            }
        }

        // GET: Admin/Charity
        [ValidateAntiForgeryToken]
        public ActionResult Charity()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.Charities.ToList());
            }
        }

        // GET: Admin/Event
        [ValidateAntiForgeryToken]
        public ActionResult CharityEvent()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.CharityEvents.ToList());
            }
        }

        // GET: Admin/Volunteer
        [ValidateAntiForgeryToken]
        public ActionResult Volunteer()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.Volunteers.ToList());
            }
        }


        // GET: Admin/Contact
        [ValidateAntiForgeryToken]
        public ActionResult ContactInfo()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.ContactInfoes.ToList());
            }
        }

        // GET: Admin/Attendance
        [ValidateAntiForgeryToken]
        public ActionResult EventAttendace()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.EventAttendances.ToList());
            }
        }

        // GET: Admin/Location
        [ValidateAntiForgeryToken]
        public ActionResult Location()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.EventAttendances.ToList());
            }
        }

        // GET: Admin/HelpingAction
        [ValidateAntiForgeryToken]
        public ActionResult HelpingAction()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.HelpingActions.ToList());
            }
        }
    }
}