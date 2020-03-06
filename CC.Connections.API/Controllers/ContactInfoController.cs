using CC.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class ContactInfoController : ApiController
    {

        // GET: api/Contact
        public ContactCollection Get()
        {
            ContactCollection Contacts = new ContactCollection();
            Contacts.LoadAll();
            return Contacts;
        }

        // GET: api/Contact/5
        public Contact Get(String id)
        {
            Contact Contact = new Contact { MemberEmail = id };
            Contact.LoadId(id);
            return Contact;
        }

        // POST: api/Contact
        public void Post([FromBody]Contact Contact)
        {
            Contact.Insert();
        }

        // PUT: api/Contact/5
        public void Put(Guid id, [FromBody]Contact Contact)
        {
            Contact.Update();
        }

        // DELETE: api/Contact/5
        public void Delete(String id)
        {
            Contact Contact = new Contact { MemberEmail = id };
            Contact.Delete();
        }
    }
}
