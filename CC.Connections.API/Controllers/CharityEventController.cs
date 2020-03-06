using CC.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class CharityEventController : ApiController
    {
        // GET: api/CharityEvent
        public CharityEventCollection Get()
        {
            CharityEventCollection CharityEvents = new CharityEventCollection();
            CharityEvents.LoadAll();
            return CharityEvents;
        }

        // GET: api/CharityEvent/5
        public CharityEvent Get(Guid id)
        {
            CharityEvent CharityEvent = new CharityEvent { ID = id };
            CharityEvent.LoadId();
            return CharityEvent;
        }

        //TODO require password for updating charity
        // POST: api/CharityEvent
        public void Post([FromBody]CharityEvent CharityEvent)
        {
            CharityEvent.Insert();
        }

        // PUT: api/CharityEvent/5
        public void Put(Guid id, [FromBody]CharityEvent CharityEvent)
        {
            CharityEvent.Update();
        }

        // DELETE: api/CharityEvent/5
        public void Delete(Guid id)
        {
            CharityEvent CharityEvent = new CharityEvent { ID = id };
            CharityEvent.Delete();
        }
    }
}
