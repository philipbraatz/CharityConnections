using CC.Connections.BL;
using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    /// <summary>
    /// TODO fill out
    /// </summary>
    public class CategoryController : BaseAPIController<CategoryCollection,BL.Category,PL.Category>{}
    /// <summary>
    /// TODO fill out
    /// </summary>
    public class CharityEventController : BaseAPIController<CharityEventCollection, BL.CharityEvent, PL.CharityEvent> { }
    /// <summary>
    /// TODO fill out
    /// </summary>
    public class CharityController  : BaseAPIController<CharityCollection,BL.Charity,PL.Charity>
    {
        //private Password setPassword(Charity Charity, string pass)
        //{
        //    return new Password(Charity.Email, pass, MemberType.CHARITY);
        //}
        //
        //// POST: api/Charity
        //public void Post([FromBody]Charity Charity, String Pass)
        //    => base.Post(Charity,setPassword(Charity, Pass));
        //
        //
        //// PUT: api/Charity/5
        //public void Put(Guid id, [FromBody]Charity Charity, String Pass)
        //    => base.Put(id,Charity,setPassword(Charity, Pass));
        //
        //
        //// DELETE: api/Charity/5
        //public void Delete(String id, String Pass)
        //=> base.Delete(id,setPassword(base.Get(id), Pass));
    }
    /// <summary>
    /// TODO fill out
    /// </summary>
    public class ContactController : BaseAPIController<ContactCollection, Contact, PL.ContactInfo> { }
    /// <summary>
    /// TODO fill out
    /// </summary>
    public class EventAttendanceController : BaseAPIController<EventAttendanceCollection, EventAttendee, PL.EventAttendance> 
    {
        // GET: api/EventAttendance/5
        /// <summary>
        /// Gets a single event attendance record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EventAttendee</returns>
        public new EventAttendee Get(Guid id)//gets by unique index
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances.Where(c => c.ID == id).FirstOrDefault();
        }

        // GET: api/EventAttendance/5
        /// <summary>
        /// Gets all event attendance records for an event
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List<EventAttendee></returns>
        public List<EventAttendee> GetByEvent(Guid id)//gets by exact guid
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances.Where(c => c.EventID == id).ToList();
        }
        /// <summary>
        /// Gets all event attendance records for an attendee
        /// </summary>
        /// <param name="email"></param>
        /// <returns>List<EventAttendee></returns>
        public List<EventAttendee> GetByAttendee(string email)//gets by exact guid
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances.Where(c => c.VolunteerEmail == email).ToList();
        }

        /// <summary>
        /// Gets a single event from event ID and attendee email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns>EventAttendee</returns>
        public EventAttendee GetBySearch(Guid id,string email)//gets by unique index
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances.Where(c => c.EventID == id && c.VolunteerEmail == email).FirstOrDefault();
        }
    }
    /// <summary>
    /// TODO fill out
    /// </summary>
    public class HelpingActionController : BaseAPIController<HelpingActionCollection, AbsHelpingAction, PL.HelpingAction> { }
    /// <summary>
    /// TODO fill out
    /// </summary>
    public class LocationController : BaseAPIController<LocationCollection, BL.Location, PL.Location> { }
    /// <summary>
    /// TODO fill out
    /// </summary>
    public class VolunteerController : BaseAPIController<VolunteerCollection,BL. Volunteer, PL.Volunteer> { }

    /// <summary>
    /// TODO fill out
    /// </summary>
    public class TestController : BaseAPIController<TestCollection, BL.Test,PL.Test>
    {
        /// <summary>
        /// For web api connection testing
        /// </summary>
        /// <returns>Dummy Text</returns>
        [HttpGet]
        public String DoTest()//gets by exact guid
        {
            return PL.Test.t;
        }
    }
}
