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
        public ActionResult ProfileView(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Password p = (Password)Session["member"];
            if (p != null)
                return View(new AbsContactInfo(p));
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: VolunteerProfile/Edit/5
        public ActionResult Edit(int id)
        {
            AbsContactInfo c = new AbsContactInfo();

            Password p = (Password)Session["member"];
            if (p != null)
                c = new AbsContactInfo(p);
            else
            {
                ViewBag.Message = "You are not signed in yet";
                return View();
            }

            return View(c);
        }

        // POST: VolunteerProfile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AbsContactInfo c)
        {
            try
            {
                // TODO: Add update logic here
                c.Update();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(c);
            }
        }

    }
}
