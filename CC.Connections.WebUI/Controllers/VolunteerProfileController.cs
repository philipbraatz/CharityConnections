﻿using System;
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
                return View(new AbsContact(p));
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: VolunteerProfile/Edit/5
        public ActionResult Edit(int id)
        {
            AbsContact c = new AbsContact();

            Password p = (Password)Session["member"];
            if (p != null)
                c = new AbsContact(p);
            else
            {
                ViewBag.Message = "You are not signed in yet";
                return View();
            }

            return View(c);
        }

        // POST: VolunteerProfile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AbsContact c)
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
