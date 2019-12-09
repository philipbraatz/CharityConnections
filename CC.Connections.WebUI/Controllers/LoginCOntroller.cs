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
                if (c.MemberType == MemberType.CHARITY)
                    return RedirectToAction("CharityView", "Charity");
                else
                    return RedirectToAction("ProfileView", "VolunteerProfile");
            else
                return View();
        }

        public ActionResult SignUpView(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View(new ContactInfoSignup());
        }

        public ActionResult CharitySignUpView(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View(new CharitySignup());
        }

        public ActionResult LogoutView()
        {
            if (HttpContext.Session["member"] == null)
                ViewBag.Message = "You are not signed in yet";
            else
                ViewBag.Message = "You have signed out";

            //Logged out
            HttpContext.Session["member"] = null;

            if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
            else
                return RedirectToAction("Index", "Home");//go to index
        }

        [HttpPost]
        public ActionResult LoginView(Password passValue)
        {
            try
            {
                passValue.Login();
                if (passValue.MemberType != MemberType.GUEST)
                {
                    Session["member"] = passValue;
                    ViewBag.Message = "You have been logged in";
                    if (ControllerContext.HttpContext.Request.UrlReferrer != null && 
                        ControllerContext.HttpContext.Request.UrlReferrer.LocalPath !=  "/Login/LoginView")
                        return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
                    else
                        return RedirectToAction("Index", "Home");//go to index
                }
                else
                {
                    if (Request.IsLocal)
                        ViewBag.Message = "Incorrect Credentials (try using briandoe@gmail.com or stjude@gmail.com)";
                    else
                        ViewBag.Message = "Incorrect Credentials (try using briandoe@gmail.com or stjude@gmail.com)";
                    return View(passValue);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != "The underlying provider failed on Open.")
                    if (Request.IsLocal)
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
                    else
                        ViewBag.Message = "not an error... move along";
                else if (Request.IsLocal)
                    ViewBag.Message = "Error: could not access the database, check database connection. The underlying provider failed on Open.";//local error
                else
                    ViewBag.Message = "Unable to process any sign up's at this time.";//specialized error handler

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
                Volunteer newMember = new Volunteer(con.ContactInfo_Email, con.password.Pass, true);
                newMember.setContactInfo((AbsContact)con);
                newMember.Insert();

                Session["member"] = con.password;
                return RedirectToAction("ProfileView", "VolunteerProfile");
            }
            catch (Exception ex)
            {
                if (ex.Message != "The underlying provider failed on Open.")
                    if (Request.IsLocal)
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
                    else
                        ViewBag.Message = "not an error... move along";
                else if (Request.IsLocal)
                    ViewBag.Message = "Error: could not access the database, check database connection. The underlying provider failed on Open.";//local error
                else
                    ViewBag.Message = "Unable to process any sign up's at this time.";//specialized error handler

                return View(con);
            }
        }

        [HttpPost]
        public ActionResult CharitySignUpView(CharitySignup csu)
        {
            try
            {
                Session["charity"] = csu.Password;
                Charity newCharity = new Charity(csu.Charity_Email, csu.Password.Pass, true);
                newCharity.setCharityInfo((Charity)csu);
                newCharity.Insert((Password)Session["charity"]);


                if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                    return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
                else
                    return RedirectToAction("Index", "Home");//go to index
            }
            catch (Exception ex)
            {
                if (ex.Message != "The underlying provider failed on Open.")
                    if (Request.IsLocal)
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
                    else
                        ViewBag.Message = "not an error... move along";
                else if (Request.IsLocal)
                    ViewBag.Message = "Error: could not access the database, check database connection. The underlying provider failed on Open.";//local error
                else
                    ViewBag.Message = "Unable to process any sign up's at this time.";//specialized error handler


                return View(csu);
            }
        }

        public ActionResult AutoV_View()
        {
            Session["member"] = new Password("auto@login.com");
            if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
            else
                return RedirectToAction("Index", "Home");//go to index
        }
        public ActionResult AutoC_View()
        {
            Session["member"] = new Password("auto@login.net");
            if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
            else
                return RedirectToAction("Index", "Home");//go to index
        }
    }
}