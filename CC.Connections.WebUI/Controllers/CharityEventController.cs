using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Doorfail.Connections.BL;
using Doorfail.Connections.WebUI.Model;
using Doorfail.DataConnection;

namespace Doorfail.Connections.WebUI.Controllers
{
    public class CharityEventController : Controller
    {

        // GET: CharityEvent
        public ActionResult Index()
       {
            try
            {
                if (ViewBag.Title == null)
                    ViewBag.Title = "Volunteer Opportunities";

                CharityEventCollection allEvents = (CharityEventCollection)SessionUtil.GetList<CharityEvent>(Session,"CharityEvents"); ;//TODO refactor all these to use both session and api
                if (Session != null && Session["charityEvents"] != null)
                {
                    //allEvents = ((CharityEventCollection)Session["charityEvents"]);
                    //Idk why it would do this
                    if (false)//reload to catch missing //replace with api call
                    {
                        try
                        {
                            allEvents.LoadAll();
                            if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                                foreach (var ev in allEvents)
                                    ev.Member_Attendance = apiHelper.getAction<EventAttendee>("GetEmail", ev.ID);
                        }
                        catch (Exception e)
                        {
                            ViewBag.Message = e.Message;
                            return View(allEvents);
                        }
                    }
                }
                else
                {
                    //convert to Model
                    allEvents = (CharityEventCollection)apiHelper.getAll<CharityEvent>();
                    if (allEvents != null && Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                        foreach (var ev in allEvents)
                            ev.Member_Attendance = apiHelper.getAction<EventAttendee>("GetEmail", ev.ID);// new AbsEventAttendee(ev.ID, ((Password)Session["member"]).email);

                    //save
                    Session["charityEvents"] = allEvents.ToArray();
                }

                return View(allEvents);
            }
            catch(Exception e)
            {
                ViewBag.Message = e.Message;
                return View();
            }
        }

        // GET: CharityEvent/CategoryView/2
        public ActionResult CategoryView(Guid id)
        {
            ViewBag.Title = apiHelper.getOne<Category>(id).Desc;

            //load
            CharityEventCollection allEvents = (CharityEventCollection)SessionUtil.GetList<CharityEvent>(Session,"charityEvents");

            allEvents = ((CharityEventCollection)Session["charityEvents"]);
            allEvents.Filter(id, SortBy.CATEGORY);
            //if (allEvents.Count != CharityEventCollection.getCount())//reload to catch missing
            //{
            //    allEvents.LoadWithFilter(id, SortBy.CATEGORY);
            //    if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
            foreach (var ev in allEvents)
                ev.Member_Attendance = apiHelper.getAction<EventAttendee>("GetEmail", ev.ID);
            //}

            //   //convert to Model
            //   allEvents = new CharityEventCollection();
            //   allEvents.LoadWithFilter(id, SortBy.CATEGORY);
            //   if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
            //       foreach (var ev in allEvents)
            //           ev.Member_Attendance = apiHelper.getAction<EventAttendee>("GetEmail", ev.ID);
            //
            //   //save
            //   Session["charityEvents"] = allEvents;


            return View("Index",allEvents);
        }

        // GET: CharityEvent/CharityView/2
        public ActionResult CharityView(string id)
        {
            Charity c = apiHelper.fromPassword<Charity>((Password)Session["member"]);//new Charity((Password)Session["member"]);
            ViewBag.Title =  c.Name;

            //load
            CharityEventCollection allEvents = (CharityEventCollection)SessionUtil.GetList<CharityEvent>(Session,"charityEvents");
            //if (Session != null && Session["charityEvents"] != null)
            //{
            //allEvents = ((CharityEventCollection)Session["charityEvents"]);
            allEvents.Filter(id, SortBy.CHARITY);
                //if (allEvents.Count != CharityEventCollection.getCount())//reload to catch missing
                //{
                //allEvents.LoadWithFilter(id, SortBy.CHARITY);
            if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                foreach (var ev in allEvents)
                    ev.Member_Attendance = apiHelper.getAction<EventAttendee>("GetEmail", ev.ID);
           

            return View("Index", allEvents);
        }

        // GET: CharityEvent/Details/5
        public ActionResult Details(Guid id)
        {
            CharityEventCollection allEvents = (CharityEventCollection)apiHelper.getAll<CharityEvent>();
            CharityEvent detailEvent;

            allEvents = ((CharityEventCollection)Session["charityEvents"]);
            detailEvent = allEvents.Where(c => c.ID == id).FirstOrDefault();//grab the one we need
            //if (detailEvent == null)
            //{
            //    detailEvent = new CharityEvent(id,true);
            //    allEvents.Add(detailEvent);//add it because it was missing
            //    if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
            //        Session["charityEvents"] = allEvents;
            //}

            Password member;
            if (SessionUtil.GetMemberType(Session) == MemberType.VOLLUNTEER)
                detailEvent.Member_Attendance = new EventAttendee(detailEvent.ID, SessionUtil.GetMember(Session).email);

            return View(detailEvent);
        }
        //[ChildActionOnly]
        public ActionResult SideView(Guid id)
        {
            CharityEventCollection allEvents = (CharityEventCollection)SessionUtil.GetList<CharityEvent>(Session,"charityEvents");
            CharityEvent detailEvent;
            if (Session != null && Session["charityEvents"] != null)
            {
                allEvents = ((CharityEventCollection)Session["charityEvents"]);
                detailEvent = allEvents.Where(c => c.ID == id).FirstOrDefault();//grab the one we need
                if (detailEvent == null)
                {
                    detailEvent = new CharityEvent(id, true);
                    allEvents.Add(detailEvent);//add it because it was missing
                    if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                        Session["charityEvents"] = allEvents.ToArray();
                }
            }
            else
            {
                detailEvent = new CharityEvent(id, true);
                allEvents.Add(detailEvent);//load the only one we need and save it to list
                if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
                    Session["charityEvents"] = allEvents.ToArray();
            }

            Password member;
            if (Session != null && Session["member"] != null && ((Password)Session["member"]).MemberType == MemberType.VOLLUNTEER)
            {
                member = (Password)Session["member"];
                detailEvent.Member_Attendance = new EventAttendee(detailEvent.ID, member.email);
            }

            return PartialView(detailEvent);
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
                try
                {
                    evnt.Charity = apiHelper.fromPassword<Charity>((Password)Session["member"]);//charity id is set
                }
                catch(HttpUnhandledException e)
                {
                    return Content(e.Message);
                }
                evnt.CharityEmail = evnt.Charity.Email;
                evnt.Charity.Email = evnt.CharityEmail;
            }
            else
            {
                ViewBag.Message = "Only charities can create a event.";
                //ViewBag.Message = "Debug: Defaulting to charity ID =  1";
                //evnt.Charity_ID = 1;
                //evnt.charity.ID = 1;
            }
            Session["charityId"] = evnt.Charity.Email;
            return View(new CharityEventWithTime(evnt));
        }

        // POST: CharityEvent/Create
        [HttpPost]
        public ActionResult Create(CharityEventWithTime cevent)
        {
            ViewBag.TimeList = TimeUtils.TimeList;
            try
            {
                cevent.Charity = apiHelper.getEmail<Charity>((string)Session["charityId"]);//new Charity((string)Session["charityId"],true);
                cevent.CharityEmail = (string)Session["charityId"];

                   if (cevent.Charity    == null ||
                       cevent.Name       == null ||
                       cevent.Description== null ||
                       cevent.EndDate    == null || cevent.EndDate == DateTime.MinValue ||
                       cevent.EndTime    == null ||
                       cevent.StartDate  == null || cevent.StartDate == DateTime.MinValue ||
                       cevent.StartTime  == null
                   )//TODO check location
                {
                    ViewBag.Message = "Please fill out every required field";
                    return View(cevent);
                }
                else if (cevent.Name.Trim().Length < 3)
                {
                    ViewBag.Message = "Name must be at least 3 characters long";
                    return View(cevent);
                }
                else if (cevent.Description.Trim().Length < 15)
                {
                    ViewBag.Message = "Description must be at least 15 characters long";
                    return View(cevent);
                }
                else if (cevent.Requirements.Trim().Length < 4)
                {
                    cevent.Requirements = "None";
                }
                else if (cevent.StartDate < DateTime.Now ||
                         cevent.StartDate.Date > cevent.EndDate.Date)
                {
                    ViewBag.Message = "Cannot start on "+cevent.StartDate+" and end on "+cevent.EndDate;
                    return View(cevent);
                }
                else if (cevent.StartDate == cevent.EndDate &&
                    cevent.EndTime - cevent.StartTime == new TimeSpan(0, 0, 0))//Event that last 0 seconds
                {
                    ViewBag.Message = "Start time can not be the same as the End time";
                    return View(cevent);
                }

                CharityEventCollection events = new CharityEventCollection();
                if (Session == null || Session["charityEvents"] == null)
                {
                    //should not happen
                }
                else
                    events.AddRange((CharityEvent[])Session["charityEvents"]) ; //((CharityEventCollection)Session["charityEvents"]);

                cevent.Charity = apiHelper.getEmail<Charity>((string)Session["charityId"]);//new Charity((string)Session["charityId"],true);
                cevent.CharityEmail = cevent.Charity.Email;
                //apiHelper.create<CharityEvent>((CharityEvent)cevent);
                cevent.Location.ID = Guid.NewGuid();
                dynamic tempLoc = JsonDatabase.GetTable<PL.Location>();
                tempLoc.Add(cevent.Location.ToPL());

                dynamic temp = JsonDatabase.GetTable<PL.CharityEvent>();
                temp.Add(cevent.toPL());
                JsonDatabase.SetTable<PL.CharityEvent>(temp);
                JsonDatabase.SaveChanges();
                events.Add((CharityEvent)cevent);
                Session["charityEvents"] = events.ToArray();

                return RedirectToAction("Index", "CharityEvent");
            }
            catch(HttpUnhandledException e)
            {
                return Content(e.Message);
            }
            catch (Exception e)
            {
                if (Request.IsLocal)
                    ViewBag.Message = "Error: " + e.Message;
                else
                    ViewBag.Message = "Error: " + e.Message;
                return View();
            }
        }

        // GET: CharityEvent/Edit/5
        public ActionResult Edit(Guid id)
        {
            ViewBag.TimeList = TimeUtils.TimeList;

            CharityEventCollection allEvents = new CharityEventCollection();
            CharityEvent detailEvent;
            if (Session != null && Session["charityEvents"] != null)
            {
                allEvents = new CharityEventCollection();
                allEvents.AddRange((CharityEvent[])Session["charityEvents"]);
                detailEvent = allEvents.Where(c => c.ID == id).FirstOrDefault();//grab the one we need
                if (detailEvent == null)
                {
                    detailEvent = new CharityEvent(id,true);
                    allEvents.Add(detailEvent);//add it because it was missing
                    if (Session == null || Session["charityEvents"] == null)
                        Session["charityEvents"] = allEvents.ToArray();
                }
            }
            else
            {
                detailEvent = new CharityEvent(id,true);
                allEvents.Add(detailEvent);//load the only one we need and save it to list
                if (Session == null || Session["charityEvents"] == null)
                    Session["charityEvents"] = allEvents.ToArray();
            }

            Password credentals = (Password)Session["member"];
            if (credentals == null)
                return RedirectToAction("LoginView", "Login");
            if (credentals.MemberType != MemberType.CHARITY)
            {
                //ViewBag.Message = "Only charities can edit a event.";
                ViewBag.Message = "Debug: skipping authentication";
            }
            return View(new CharityEventWithTime(detailEvent));
        }

        // POST: CharityEvent/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, CharityEventWithTime collection)
        {
            ViewBag.TimeList = TimeUtils.TimeList;
            try
            {
                CharityEventCollection events;
                if (Session == null || Session["charityEvents"] == null)
                {
                    events = new CharityEventCollection
                    {
                        new CharityEvent(id,true)
                    };//should not happen
                }
                else
                {
                    events = new CharityEventCollection();
                    events.AddRange((CharityEvent[])Session["charityEvents"]);
                }

                CharityEvent editEvent = events.Where(c => c.ID == id).FirstOrDefault();
                //collection.Update();
                apiHelper.update<CharityEvent>(collection);
                editEvent = collection;
                Session["charityEvents"] = events.ToArray();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                if (Request.IsLocal)
                    ViewBag.Message = "Error: " + e.Message;
                else
                    ViewBag.Message = "Error: " + e.Message;
                return View();
            }
        }

        // GET: CharityEvent/Delete/5
        public ActionResult Delete(Guid id)
        {
            CharityEventCollection events;
            if (Session == null || Session["charityEvents"] == null)
            {
                events = new CharityEventCollection
                {
                    new CharityEvent(id,true)
                };//should not happen
            }
            else
            {
                events = new CharityEventCollection();
                events.AddRange((CharityEvent[])Session["charityEvents"]);
            }

            CharityEvent deleteEvent = events.Where(c => c.ID == id).FirstOrDefault();
            //deleteEvent.Delete();
            apiHelper.create<CharityEvent>(deleteEvent);
            events.Remove(deleteEvent);
            Session["charityEvents"] = events.ToArray();

            return RedirectToAction("Index", "CharityEvent");
        }

        public ActionResult Signup(Guid id)
        {
            CharityEvent evnt = new CharityEvent(id,true);
            if (Session != null && Session["member"] != null)
            {
                EventAttendee atendee = new EventAttendee(id, ((Password)Session["member"]).email);
                if (atendee.Exists())
                    if (atendee.VolunteerStatus != Status.GOING)
                        atendee.Update(Status.GOING);//interested -> going
                    else
                        evnt.RemoveMember(((Password)Session["member"]).email);//going -> not going
                else
                    evnt.AddMember(((Password)Session["member"]).email, Status.GOING);// not going -> going

                //update
                CharityEventCollection sesEvents = ((CharityEventCollection)Session["charityEvents"]);
                sesEvents.Remove(sesEvents.Where(c => c.ID == id).FirstOrDefault());
                sesEvents.Add(evnt);
                Session["charityEvents"] = sesEvents.ToArray();
            }
            else
                ViewBag.Message = "You need to sign in to do this.";
            return RedirectToAction("details", new { id = id });
        }

        public ActionResult Interested(Guid id)
        {
            CharityEvent evnt = new CharityEvent();
            if (Session != null && Session["charityEvents"] != null)
            {
                evnt = ((CharityEventCollection)Session["charityEvents"]).Where(c => c.ID == id).FirstOrDefault();

            }
            if (evnt == null || evnt.ID == null)
            {
                ViewBag.Message = "That Event does not exists :(";//should not happen
                return RedirectToAction("Index", "CharityEvent");
            }

            if (Session != null && Session["member"] != null)
            {
                EventAttendee atendee = new EventAttendee(id, ((Password)Session["member"]).email);
                if (atendee.Exists())
                    if (atendee.VolunteerStatus != Status.INTERESTED)
                        atendee.Update(Status.INTERESTED);//going -> interested
                    else
                        evnt.RemoveMember(((Password)Session["member"]).email);//interested -> not interested
                else
                    evnt.AddMember(((Password)Session["member"]).email, Status.INTERESTED);// not interested -> interested

                //update
                CharityEventCollection sesEvents = ((CharityEventCollection)Session["charityEvents"]);
                sesEvents.Remove(sesEvents.Where(c => c.ID == id).FirstOrDefault());
                sesEvents.Add(evnt);
                Session["charityEvents"] = sesEvents.ToArray();
            }
            else
                ViewBag.Message = "You need to sign in to do this.";
            return RedirectToAction("details", new { id = id });
        }
    }
}
