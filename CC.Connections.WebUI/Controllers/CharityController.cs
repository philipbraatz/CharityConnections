using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Controllers
{
    public class CharityController : Controller
    {
        // GET: Charity
        public ActionResult CharityView(string returnUrl)
        {
             ViewBag.ReturnUrl = returnUrl;
             Password p = (Password)Session["member"];
             if (p != null)
                 return View(new Charity(p));
             else
                 return RedirectToAction("Index", "Home");    
        }

        // GET: Charity/Edit/5
        public ActionResult Edit(int id)
        {
            Charity c = new Charity();

            Password p = (Password)Session["member"];
            if (p != null)
                c = new Charity(id);
            else
            {
                ViewBag.Message = "You are not signed in yet";
                return View();
            }

            return View(c);
        }

        // POST: Charity/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Charity c)
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