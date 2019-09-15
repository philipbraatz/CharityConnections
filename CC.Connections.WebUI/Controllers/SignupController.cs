using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CC.Connections.WebUI.Controllers
{
    public class SignupController : Controller
    {
        // GET: Signup
        public ActionResult Create(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }
    }
}