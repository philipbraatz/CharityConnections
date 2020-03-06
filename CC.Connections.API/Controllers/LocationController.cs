using CC.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class LocationController : ApiController
    {
        // GET: api/Location
        public LocationCollection Get()
        {
            LocationCollection Locations = new LocationCollection();
            Locations.LoadAll();
            return Locations;
        }

        // GET: api/Location/5
        public Location Get(Guid id)
        {
            Location Location = new Location { ID = id };
            Location.LoadId();
            return Location;
        }

        // POST: api/Location
        public void Post([FromBody]Location Location)
        {
            Location.Insert();
        }

        // PUT: api/Location/5
        public void Put(Guid id, [FromBody]Location Location)
        {
            Location.Update();
        }

        // DELETE: api/Location/5
        public void Delete(Guid id)
        {
            Location Location = new Location { ID = id };
            Location.Delete();
        }
    }
}
