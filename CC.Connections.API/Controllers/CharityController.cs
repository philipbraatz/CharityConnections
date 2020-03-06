using CC.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class CharityController : ApiController
    {
        private Password setPassword(Charity Charity,string pass)
        {
            return new Password(Charity.Email, pass, MemberType.CHARITY);
        }

        // GET: api/Charity
        public CharityCollection Get()
        {
            CharityCollection Charitys = new CharityCollection();
            Charitys.LoadAll();
            return Charitys;
        }

        // GET: api/Charity/5
        public Charity Get(String id)
        {
            Charity Charity = new Charity {Email = id };
            Charity.LoadId(id);
            return Charity;
        }

        // POST: api/Charity
        public void Post([FromBody]Charity Charity,String Pass)
        {
            Charity.Insert(setPassword(Charity,Pass));
        }

        // PUT: api/Charity/5
        public void Put(Guid id, [FromBody]Charity Charity, String Pass)
        {
            Charity.Update(setPassword(Charity, Pass));
        }

        // DELETE: api/Charity/5
        public void Delete(String id, String Pass)
        {
            Charity Charity = new Charity { Email = id };
            Charity.Delete(setPassword(Charity, Pass));
        }
    }
}
