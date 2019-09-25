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

        public ActionResult Login(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }

        public ActionResult SignUp(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }

        public ActionResult Logout()
        {
            //Logged out
            HttpContext.Session["member"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Password pass)
        {
            try
            {
                if (pass.Login())
                {
                    Session["member"] = pass;
                    ViewBag.Message = "You have been logged in";
                    //if (returnurl != null)
                    //    return Redirect(returnurl);
                    //else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Incorrect Credentials";
                    return View(pass);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(pass);
            }
        }

        //Fake successful login
        //[HttpPost]
        //public ActionResult Login()
        //{
        //    return RedirectToAction("Index", "Home");
        //}

        [HttpPost]
        public ActionResult SignUp(ContactInfo contact)
        {
            //TODO
            return RedirectToAction("Index", "Home");
        }
    }
}