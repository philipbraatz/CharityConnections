using CC.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class CategoryController : BaseAPIController<CategoryCollection,Category,PL.Category>{}
    public class CharityEventController : BaseAPIController<CharityEventCollection, CharityEvent, PL.CharityEvent> { }
    public class CharityController  : BaseAPIController<CharityCollection,Charity,PL.Charity>
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
    public class ContactController : BaseAPIController<ContactCollection, Contact, PL.ContactInfo> { }
    public class EventAttendanceController : BaseAPIController<EventAttendanceCollection, AbsEventAttendee, PL.EventAttendance> 
    {
        // GET: api/EventAttendance/5
        public new AbsEventAttendee Get(Guid id)//gets by unique index
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances.Where(c => c.ID == id).FirstOrDefault();
        }

        // GET: api/EventAttendance/5
        public List<AbsEventAttendee> GetByEvent(Guid id)//gets by exact guid
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances.Where(c => c.EventID == id).ToList();
        }
        public List<AbsEventAttendee> GetByAttendee(string email)//gets by exact guid
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances.Where(c => c.VolunteerEmail == email).ToList();
        }

        public AbsEventAttendee GetBySearch(Guid id,string email)//gets by unique index
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances.Where(c => c.EventID == id && c.VolunteerEmail == email).FirstOrDefault();
        }
    }
    public class HelpingActionController : BaseAPIController<AbsHelpingActionCollection, AbsHelpingAction, PL.HelpingAction> { }
    public class LocationController : BaseAPIController<LocationCollection, Location, PL.Location> { }
    public class VolunteerController : BaseAPIController<VolunteerCollection, Volunteer, PL.Volunteer> { }
}
