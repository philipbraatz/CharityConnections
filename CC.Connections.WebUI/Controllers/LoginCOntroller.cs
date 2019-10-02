using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CC.Connections.BL;
using CC.Connections.WebUI.Model;

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
            return View(new ContactInfoSignup());
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
        public ActionResult SignUpView(ContactInfoSignup con)
        {
            Member newMember = new Member(con.Email, con.password.Pass, 1);
            newMember.setContactInfo((ContactInfo)con);
            newMember.Insert();

            Session["member"] = newMember.Password;
            return RedirectToAction("Index", "Home");
        }
    }
}