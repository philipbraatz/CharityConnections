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
            AbsContactInfo c = new AbsContactInfo();
            Password p = (Password)Session["member"];

            return View(new AbsContactInfo(p.email));
        }


        
    }
}
