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

        [HttpPost]
        public ActionResult SignUpView(ContactInfoSignup con)
        {
            try
            {
                //TODO add location not null
                if( con.confirmPassword.Pass == null ||  
                    con.ContactInfo_Email == null ||
                    con.ContactInfo_FName == null ||
                    con.ContactInfo_LName == null ||
                    con.ContactInfo_Phone == null ||
                    con.DateOfBirth == null ||
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
                else if (DateTime.Now.Year - con.DateOfBirth.Year < 13)
                {
                    ViewBag.Message = "You must be 13 years or older to register as a member";
                    return View(con);
                }
                else if(!(con.ContactInfo_Email.Contains('@') && con.ContactInfo_Email.Contains('.') && con.ContactInfo_Email.Length > 6))
                {
                    ViewBag.Message = "Email is invalid";
                    return View(con);
                }
                else if(false)//TODO check for valid phone number
                {
                    ViewBag.Message = "Phone number is invalid";
                    return View(con);
                }
                else if (con.ContactInfo_FName.Trim().Length < 3)
                {
                    ViewBag.Message = "First name must be at least 3 characters long";
                    return View(con);
                }
                else if (con.ContactInfo_LName.Trim().Length < 3)
                {
                    ViewBag.Message = "Last name must be at least 3 characters long";
                    return View(con);
                }

                Volunteer newMember = new Volunteer(con.ContactInfo_Email, con.password.Pass, true);
                newMember.setContactInfo((AbsContact)con);
                con.password.email = con.ContactInfo_Email;
                con.password.MemberType = MemberType.VOLLUNTEER;
                newMember.Insert(con.password);

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
                if (csu.confirmPassword.Pass == null ||
                    csu.Charity_Email == null ||
                    //csu.Category == null ||//TODO Category picker
                    csu.Charity_Cause == null ||
                    csu.Charity_Deductibility == null ||
                    csu.Charity_EIN == null ||
                    csu.Password == null ||
                    csu.Charity_Name == null
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
                else if (csu.Charity_Name.Trim().Length < 3)
                {
                    ViewBag.Message = "Charity name must be at least 3 characters long";
                    return View(csu);
                }
                else if (!(csu.Charity_Email.Contains('@') && csu.Charity_Email.Contains('.') && csu.Charity_Email.Length > 6))
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
                CC.Connections.BL.CategoryList categoryList = new CC.Connections.BL.CategoryList();
                try
                {
                    categoryList.LoadAll();
                    csu.Category = new Category(r.Next(1, categoryList.Count-1));
                }
                catch (Exception e)
                {
                    ViewBag.Message = e;
                }
                csu.Password.email = csu.Charity_Email;
                csu.Password.MemberType = MemberType.CHARITY;
                Session["member"] = csu.Password;
                csu.Location = new Location(2);//TEMP always set to location 2 because its currently null
                csu.Insert((Password)Session["member"]);


                return RedirectToAction("CharityProfile", "Charity");//go to profile
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