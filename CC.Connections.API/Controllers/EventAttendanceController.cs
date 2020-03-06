using CC.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class EventAttendanceController : ApiController
    {
        // GET: api/EventAttendance
        public EventAttendanceCollection Get()
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            return EventAttendances;
        }

        // GET: api/EventAttendance/5
        public AbsEventAtendee Get(Guid id)
        {
            EventAttendanceCollection EventAttendances = new EventAttendanceCollection();
            EventAttendances.LoadAll();
            AbsEventAtendee EventAttendance = EventAttendances.Where(c=> c.ID == id).FirstOrDefault();
            return EventAttendance;
        }

        // POST: api/EventAttendance
        public void Post([FromBody]AbsEventAtendee EventAttendance)
        {
            EventAttendance.Insert();
        }

        // PUT: api/EventAttendance/5
        public void Put(Guid id, [FromBody]AbsEventAtendee EventAttendance)
        {
            EventAttendance.Update();
        }

        // DELETE: api/EventAttendance/5
        public void Delete(Guid id)
        {
            AbsEventAtendee EventAttendance = new AbsEventAtendee { ID = id };
            EventAttendance.Delete();
        }
    }
}
