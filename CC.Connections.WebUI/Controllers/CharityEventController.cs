using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;
using CC.Connections.WebUI.Model;

namespace CC.Connections.WebUI.Controllers
{
    public class CharityEventController : Controller
    {
        // GET: CharityEvent
        public ActionResult Index()
        {
            CharityEventList allEvents = new CharityEventList();
            allEvents.LoadAll();
            List<CharityEvent_WithCharity> charityEvents = new List<CharityEvent_WithCharity>();
            foreach (var ev in allEvents)
            {
                if (Session != null && Session["member"] != null)
                    charityEvents.Add(new CharityEvent_WithCharity(ev,
                        ((Password)Session["member"]).email));
                else
                    charityEvents.Add(new CharityEvent_WithCharity(ev));
            }
            return View(charityEvents);
        }

        // GET: CharityEvent/Details/5
        public ActionResult Details(int id)
        {
            Password member;
            if (Session != null && Session["member"] != null)
            {
                member = (Password)Session["member"];
                return View(new CharityEvent_WithCharity(new CharityEvent(id),
                        member.email));
            }
            else
                return View(new CharityEvent_WithCharity(new CharityEvent(id)));

        }

        // GET: CharityEvent/Create
        public ActionResult Create()
        {
            ViewBag.TimeList = TimeUtils.TimeList;

            CharityEvent evnt = new CharityEvent
            {
                Location = new AbsLocation(),
            };
            Password credentals = (Password)Session["member"];
            if(credentals == null)
                return RedirectToAction("LoginView", "Login");
            if (credentals.MemberType == MemberType.CHARITY)
                evnt.Charity_ID = new Charity((Password)Session["member"]).ID;
            else
            {
                //ViewBag.Message = "Only charities can create a event.";
                ViewBag.Message = "Debug: Defaulting to charity ID =  1";
            }
            return View(new CharityEvent_WithTime(evnt));
        }

        // POST: CharityEvent/Create
        [HttpPost]
        public ActionResult Create(CharityEvent_WithTime charityEvent)
        {
            try
            {
                //charityEvent.StartTime = TimeUtils.ToTime(charityEvent.Time);
                //charityEvent.ENd
                charityEvent.Insert();
                return RedirectToAction("Index","CharityEvent");
            }
            catch(Exception e)
            {
                ViewBag.Message = "Error: " + e.Message;
                return View();
            }
        }

        // GET: CharityEvent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CharityEvent/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CharityEvent/Delete/5
        public ActionResult Delete(int id)
        {
            CharityEvent deleteEvent = new CharityEvent(id);
            deleteEvent.Delete();
            return RedirectToAction("Index", "CharityEvent");
        }

        // POST: CharityEvent/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                CharityEvent deleteEvent = new CharityEvent(id);
                deleteEvent.Delete();

                return RedirectToAction("Index", "CharityEvent");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Signup(int id)
        {
            CharityEvent evnt = new CharityEvent(id);
            if (Session != null && Session["member"] != null)
            {
                AbsEventAtendee atendee = new AbsEventAtendee(id, ((Password)Session["member"]).email);
                if (atendee.Exists())
                    if (atendee.Status != Status.GOING)
                        atendee.Update(Status.GOING);//interested -> going
                    else
                        evnt.RemoveMember(((Password)Session["member"]).email);//going -> not going
                else
                    evnt.AddMember(((Password)Session["member"]).email,Status.GOING);// not going -> going

            }
            else
                ViewBag.Message = "You need to sign in to do this.";
            return RedirectToAction("details", new { id = id });
        }

        public ActionResult Interested(int id)
        {
            CharityEvent evnt = new CharityEvent(id);
            if (Session != null && Session["member"] != null)
            {
                AbsEventAtendee atendee = new AbsEventAtendee(id, ((Password)Session["member"]).email);
                if (atendee.Exists())
                    if (atendee.Status != Status.INTERESTED)
                        atendee.Update(Status.INTERESTED);//going -> interested
                    else
                        evnt.RemoveMember(((Password)Session["member"]).email);//interested -> not interested
                else
                    evnt.AddMember(((Password)Session["member"]).email, Status.INTERESTED);// not interested -> interested

            }
            else
                ViewBag.Message = "You need to sign in to do this.";
            return RedirectToAction("details", new { id = id });
        }
    }
}
