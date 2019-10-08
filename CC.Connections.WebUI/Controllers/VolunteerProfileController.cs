using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Controllers
{
    public class VolunteerProfileController : Controller
    {
        // GET: VolunteerProfile
        public ActionResult ProfileView(string id)
        {
            ViewBag.ReturnUrl = id;
            ContactInfo c = new ContactInfo();
            Password p = (Password)Session["member"];

            return View(new ContactInfo(p.email));
        }


        
    }
}
