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
            if (ViewBag.Title == null)
                ViewBag.Title = "Volunteer Opportunities";

            //load
            CharityEventList allEvents = new CharityEventList();
            if (Session != null && Session["charityEvents"] != null)
            {
                allEvents = ((CharityEventList)Session["charityEvents"]);
                if (allEvents.Count != CharityEventList.getCount())//reload to catch missing
                {
                    try
                    {
                        allEvents.LoadAll();
                        if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                            foreach (var ev in allEvents)
                                ev.Member_Attendance = new AbsEventAtendee(ev.Event_ID, ((Password)Session["member"]).email);
                    }
                    catch(Exception e)
                    {
                        ViewBag.Message = e.Message;
                        return View(allEvents);
                    }
                }
            }
            else
            {
                //convert to Model
                allEvents = new CharityEventList();
                allEvents.LoadAll();
                if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                    foreach (var ev in allEvents)
                        ev.Member_Attendance = new AbsEventAtendee(ev.Event_ID, ((Password)Session["member"]).email);

                //save
                Session["charityEvents"] = allEvents;
            }

            return View(allEvents);
        }

        // GET: CharityEvent/CategoryView/2
        public ActionResult CategoryView(int id)
        {
            ViewBag.Title = new Category(id).Category_Desc;

            //load
            CharityEventList allEvents = new CharityEventList();
            if (Session != null && Session["charityEvents"] != null)
            {
                allEvents = ((CharityEventList)Session["charityEvents"]);
                allEvents.Filter(id, SortBy.CATEGORY);
                if (allEvents.Count != CharityEventList.getCount())//reload to catch missing
                {
                    allEvents.LoadWithFilter(id, SortBy.CATEGORY);
                    if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                        foreach (var ev in allEvents)
                            ev.Member_Attendance = new AbsEventAtendee(ev.Event_ID, ((Password)Session["member"]).email);
                }
            }
            else
            {
                //convert to Model
                allEvents = new CharityEventList();
                allEvents.LoadWithFilter(id, SortBy.CATEGORY);
                if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                    foreach (var ev in allEvents)
                        ev.Member_Attendance = new AbsEventAtendee(ev.Event_ID, ((Password)Session["member"]).email);

                //save
                Session["charityEvents"] = allEvents;
            }

            return View("Index",allEvents);
        }

        // GET: CharityEvent/Details/5
        public ActionResult Details(int id)
        {
            CharityEventList allEvents = new CharityEventList();
            CharityEvent detailEvent;
            if (Session != null && Session["charityEvents"] != null)
            {
                allEvents = ((CharityEventList)Session["charityEvents"]);
                detailEvent = allEvents.Where(c => c.Event_ID == id).FirstOrDefault();//grab the one we need
                if (detailEvent == null)
                {
                    detailEvent = new CharityEvent(id);
                    allEvents.Add(detailEvent);//add it because it was missing
                    if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                        Session["charityEvents"] = allEvents;
                }
            }
            else
            {
                detailEvent = new CharityEvent(id);
                allEvents.Add(detailEvent);//load the only one we need and save it to list
                if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                    Session["charityEvents"] = allEvents;
            }

            Password member;
            if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
            {
                member = (Password)Session["member"];
                detailEvent.Member_Attendance = new AbsEventAtendee(detailEvent.Event_ID, member.email);
            }

            return View(detailEvent);
        }

        // GET: CharityEvent/Create
        public ActionResult Create()
        {
            ViewBag.TimeList = TimeUtils.TimeList;

            CharityEvent evnt = new CharityEvent
            {
                Location = new Location(),
            };
            Password credentals = (Password)Session["member"];
            if (credentals == null)
                return RedirectToAction("LoginView", "Login");
            if (credentals.MemberType == MemberType.CHARITY)
            {
                evnt.charity = new Charity((Password)Session["member"]);//charity id is set
                evnt.Charity_ID = evnt.charity.ID;
                evnt.charity.ID = evnt.Charity_ID;
            }
            else
            {
                ViewBag.Message = "Only charities can create a event.";
                //ViewBag.Message = "Debug: Defaulting to charity ID =  1";
                //evnt.Charity_ID = 1;
                //evnt.charity.ID = 1;
            }
            Session["charityId"] = evnt.charity.ID;
            return View(new CharityEvent_WithTime(evnt));
        }

        // POST: CharityEvent/Create
        [HttpPost]
        public ActionResult Create(CharityEvent_WithTime charityEvent)
        {
            try
            {
                CharityEventList events;
                if (Session == null || Session["charityEvents"] == null)
                {
                    events = new CharityEventList();//should not happen
                }
                else
                    events = ((CharityEventList)Session["charityEvents"]);

                charityEvent.charity = new Charity((int)Session["charityId"]);
                charityEvent.Charity_ID = charityEvent.charity.ID;
                charityEvent.Insert();
                events.Add(charityEvent);
                Session["charityEvents"] = events;

                return RedirectToAction("Index", "CharityEvent");
            }
            catch (Exception e)
            {
                if (Request.IsLocal)
                    ViewBag.Message = "Error: " + e.Message;
                else
                    ViewBag.Message = "not an error... move along";
                return View();
            }
        }

        // GET: CharityEvent/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.TimeList = TimeUtils.TimeList;

            CharityEventList allEvents = new CharityEventList();
            CharityEvent detailEvent;
            if (Session != null && Session["charityEvents"] != null)
            {
                allEvents = ((CharityEventList)Session["charityEvents"]);
                detailEvent = allEvents.Where(c => c.Event_ID == id).FirstOrDefault();//grab the one we need
                if (detailEvent == null)
                {
                    detailEvent = new CharityEvent(id);
                    allEvents.Add(detailEvent);//add it because it was missing
                    if (Session == null || Session["charityEvents"] == null)
                        Session["charityEvents"] = allEvents;
                }
            }
            else
            {
                detailEvent = new CharityEvent(id);
                allEvents.Add(detailEvent);//load the only one we need and save it to list
                if (Session == null || Session["charityEvents"] == null)
                    Session["charityEvents"] = allEvents;
            }

            Password credentals = (Password)Session["member"];
            if (credentals == null)
                return RedirectToAction("LoginView", "Login");
            if (credentals.MemberType != MemberType.CHARITY)
            {
                //ViewBag.Message = "Only charities can edit a event.";
                ViewBag.Message = "Debug: skipping authentication";
            }
            return View(new CharityEvent_WithTime(detailEvent));
        }

        // POST: CharityEvent/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CharityEvent_WithTime collection)
        {
            try
            {
                CharityEventList events;
                if (Session == null || Session["charityEvents"] == null)
                {
                    events = new CharityEventList
                    {
                        new CharityEvent(id)
                    };//should not happen
                }
                else
                    events = ((CharityEventList)Session["charityEvents"]);

                CharityEvent editEvent = events.Where(c => c.Event_ID == id).FirstOrDefault();
                collection.Update();
                editEvent = collection;
                Session["charityEvents"] = events;

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                if (Request.IsLocal)
                    ViewBag.Message = "Error: " + e.Message;
                else
                    ViewBag.Message = "not an error... move along";
                return View();
            }
        }

        // GET: CharityEvent/Delete/5
        public ActionResult Delete(int id)
        {
            CharityEventList events;
            if (Session == null || Session["charityEvents"] == null)
            {
                events = new CharityEventList
                {
                    new CharityEvent(id)
                };//should not happen
            }
            else
                events = ((CharityEventList)Session["charityEvents"]);

            CharityEvent deleteEvent = events.Where(c => c.Event_ID == id).FirstOrDefault();
            deleteEvent.Delete();
            events.Remove(deleteEvent);
            Session["charityEvents"] = events;

            return RedirectToAction("Index", "CharityEvent");
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
                    evnt.AddMember(((Password)Session["member"]).email, Status.GOING);// not going -> going

                //update
                CharityEventList sesEvents = ((CharityEventList)Session["charityEvents"]);
                sesEvents.Remove(sesEvents.Where(c => c.Event_ID == id).FirstOrDefault());
                sesEvents.Add(evnt);
                Session["charityEvents"] = sesEvents;
            }
            else
                ViewBag.Message = "You need to sign in to do this.";
            return RedirectToAction("details", new { id = id });
        }

        public ActionResult Interested(int id)
        {
            CharityEvent evnt = new CharityEvent();
            if (Session != null && Session["charityEvents"] != null)
            {
                evnt = ((CharityEventList)Session["charityEvents"]).Where(c => c.Event_ID == id).FirstOrDefault();

            }
            if (evnt == null || evnt.Event_ID <= 0)
            {
                ViewBag.Message = "That Event does not exists :(";//should not happen
                return RedirectToAction("Index", "CharityEvent");
            }

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

                //update
                CharityEventList sesEvents = ((CharityEventList)Session["charityEvents"]);
                sesEvents.Remove(sesEvents.Where(c => c.Event_ID == id).FirstOrDefault());
                sesEvents.Add(evnt);
                Session["charityEvents"] = sesEvents;
            }
            else
                ViewBag.Message = "You need to sign in to do this.";
            return RedirectToAction("details", new { id = id });
        }
    }
}
