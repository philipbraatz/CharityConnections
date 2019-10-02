using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Controllers
{
    public class LoginController : Controller
    {

        public ActionResult LoginView(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }

        public ActionResult SignUpView(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }

        public ActionResult LogoutView()
        {
            if (HttpContext.Session["member"] == null)
                ViewBag.Message = "You are not signed in yet";
            else
                ViewBag.Message = "You have signed out";
            //Logged out
            HttpContext.Session["member"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult LoginView(Password passValue)
        {
            try
            {
                if (passValue.Login())
                {
                    Session["member"] = passValue;
                    ViewBag.Message = "You have been logged in";
                    //if (returnurl != null)
                    //    return Redirect(returnurl);
                    //else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Incorrect Credentials";
                    return View(passValue);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(passValue);
            }
        }

        //Fake successful login
        //[HttpPost]
        //public ActionResult Login()
        //{
        //    return RedirectToAction("Index", "Home");
        //}

        [HttpPost]
        public ActionResult SignUpView(ContactInfo contact)
        {
            //TODO
            Session["member"] = contact;
            return RedirectToAction("Index", "Home");
        }
    }
}