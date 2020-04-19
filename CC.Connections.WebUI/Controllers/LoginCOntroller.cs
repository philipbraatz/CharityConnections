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
        [ValidateAntiForgeryToken]
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
            return View(new VolunteerSignup());
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
                if(passValue.email == null)
                {
                    ViewBag.Message = "Please enter your email (try using briandoe@gmail.com or stjude@gmail.com)";
                    return View(passValue);
                }
                else if(passValue.Pass == null)
                {
                    ViewBag.Message = "Please enter your password";
                    return View(passValue);
                }

                passValue.Login();
                if (passValue.MemberType != MemberType.GUEST)
                {
                    Session["member"] = passValue;
                    ViewBag.Message = "You have been logged in";
                    //if (ControllerContext.HttpContext.Request.UrlReferrer != null && 
                    //    ControllerContext.HttpContext.Request.UrlReferrer.LocalPath !=  "/Login/LoginView")
                    //    return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
                    //else
                    //    return RedirectToAction("Index", "Home");//go to index
                    if (passValue.MemberType == MemberType.VOLLUNTEER)
                        return RedirectToAction("ProfileView", "VolunteerProfile");
                    else if(passValue.MemberType == MemberType.VOLLUNTEER)
                        return RedirectToAction("CharityProfile", "Charity");
                    else
                        return RedirectToAction("index", "home");
                }
                else
                {
                    ViewBag.Message = "Incorrect Credentials ";
                    return View(passValue);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != "The underlying provider failed on Open.")
                    if (Request.IsLocal)
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
                    else
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
                else if (Request.IsLocal)
                    ViewBag.Message = "Error: could not access the database, check database connection. The underlying provider failed on Open.";//local error
                else
                    ViewBag.Message = "Unable to process any sign up's at this time.";//specialized error handler

                return View(passValue);
            }
            RedirectToAction("index", "home");
        }

        [HttpPost]
        public ActionResult SignUpView(VolunteerSignup con)
        {
            try
            {
                //TODO add location not null
                if( con.confirmPassword.Pass == null ||  
                    con.Email == null ||
                    con.ContactInfo.FName == null ||
                    con.ContactInfo.LName == null ||
                    con.ContactInfo.Phone == null ||
                    con.ContactInfo.DateOfBirth == null ||
                    con.password == null
                    )
                {
                    ViewBag.Message = "Please fill out every required field";
                    return View(con);
                }
                else if(con.confirmPassword.Pass != con.password.Pass)
                {
                    ViewBag.Message = "Passwords do not match";
                    return View(con);
                }
                else if (DateTime.Now.Year - con.ContactInfo.DateOfBirth.Year < 13)
                {
                    ViewBag.Message = "You must be 13 years or older to register as a member";
                    return View(con);
                }
                else if(!(con.Email.Contains('@') && con.Email.Contains('.') && con.Email.Length > 6))
                {
                    ViewBag.Message = "Email is invalid";
                    return View(con);
                }
                else if(false)//TODO check for valid phone number
                {
                    ViewBag.Message = "Phone number is invalid";
                    return View(con);
                }
                else if (con.ContactInfo.FName.Trim().Length < 3)
                {
                    ViewBag.Message = "First name must be at least 3 characters long";
                    return View(con);
                }
                else if (con.ContactInfo.LName.Trim().Length < 3)
                {
                    ViewBag.Message = "Last name must be at least 3 characters long";
                    return View(con);
                }

                Volunteer newMember = new Volunteer(con.Email, con.password.Pass, true);
                newMember.setVolunteer(newMember);
                con.password.email = con.Email;
                con.password.MemberType = MemberType.VOLLUNTEER;
                newMember.Insert(con.password);
                //if (newMember.Insert(con.password))
                //{
                    Session["member"] = con.password;
                    return RedirectToAction("ProfileView", "VolunteerProfile");
                //}
                //else
                //{
                //    ViewBag.Message = "Email already in use, please use a different email.";
                //    return View(con);
                //}
            }
            catch (Exception ex)
            {
                if (ex.Message != "The underlying provider failed on Open.")
                    if (Request.IsLocal)
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
                    else
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
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
                if (csu.confirmPassword.Pass == null ||
                    csu.Email == null ||
                    //csu.Category == null ||//TODO Category picker
                    csu.Cause == null ||
                    csu.EIN == null ||
                    csu.Password == null ||
                    csu.Name == null
                    )//TODO check location
                {
                    ViewBag.Message = "Please fill out every required field";
                    return View(csu);
                }
                else if (csu.confirmPassword.Pass != csu.Password.Pass)
                {
                    ViewBag.Message = "Passwords do not match";
                    return View(csu);
                }
                else if (csu.Name.Trim().Length < 3)
                {
                    ViewBag.Message = "Charity name must be at least 3 characters long";
                    return View(csu);
                }
                else if (!(csu.Email.Contains('@') && csu.Email.Contains('.') && csu.Email.Length > 6))
                {
                    ViewBag.Message = "Email is invalid";
                    return View(csu);
                }
                else if (false)//TODO check for valid phone number
                {
                    ViewBag.Message = "Phone number is invalid";
                    return View(csu);
                }
                Random r = new Random();
                CC.Connections.BL.CategoryCollection categoryList = new CC.Connections.BL.CategoryCollection();
                try
                {
                    categoryList.LoadAll();
                    csu.Category = CategoryCollection.INSTANCE.ElementAt(r.Next(1, categoryList.Count-1));
                }
                catch (Exception e)
                {
                    ViewBag.Message = e;
                }
                csu.Password.email = csu.Email;
                csu.Password.MemberType = MemberType.CHARITY;
                Session["member"] = csu.Password;
               // Location loc = new Location(2);
                //csu.Location = loc;//TEMP always set to location 2 because its currently null
                csu.Insert((Password)Session["member"]);


                return RedirectToAction("CharityProfile", "Charity");//go to profile
            }
            catch (Exception ex)
            {
                if (ex.Message != "The underlying provider failed on Open.")
                    if (Request.IsLocal)
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
                    else
                        ViewBag.Message = "Error: " + ex.InnerException.Message;
                else if (Request.IsLocal)
                    ViewBag.Message = "Error: could not access the database, check database connection. The underlying provider failed on Open.";//local error
                else
                    ViewBag.Message = "Unable to process any sign up's at this time.";//specialized error handler


                return View(csu);
            }
        }

        public ActionResult AutoV_View()
        {
            Session["member"] = new Password("auto@LogIns.com");
            if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
            else
                return RedirectToAction("Index", "Home");//go to index
        }
        public ActionResult AutoC_View()
        {
            Session["member"] = new Password("auto@LogIns.net");
            if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
            else
                return RedirectToAction("Index", "Home");//go to index
        }
    }
}