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
            Password c = (Password)Session["member"];
            if (c != null)
                return View(c);
            else
                return View();
            //return View();
            //ViewBag.ReturnUrl = returnurl;

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
                        return RedirectToAction("ProfileView", "VolunteerProfile");
                }
                else
                {
                    ViewBag.Message = "Incorrect Credentials";
                    return View(passValue);
                }
            }
            catch (Exception ex)
            {
                if(ex.Message != "The underlying provider failed on Open.")
                    ViewBag.Message = ex.Message;
                else
                    ViewBag.Message = "Unable to process any login's at this time.";//specialized error handler

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
            try
            {
                Volunteer newMember = new Volunteer(con.ContactInfo_Email, con.password.Pass,true);
                newMember.setContactInfo((AbsContact)con);
                newMember.Insert();

                Session["member"] = con.password;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                if (ex.Message != "The underlying provider failed on Open.")
                    ViewBag.Message = ex.Message;
                else
                    ViewBag.Message = "Unable to process any sign up's at this time.";//specialized error handler

                return View(con);
            }
        }
    }
}