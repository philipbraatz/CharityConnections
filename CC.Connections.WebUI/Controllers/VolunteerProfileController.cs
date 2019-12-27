using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;
using CC.Connections.WebUI.Model;

namespace CC.Connections.WebUI.Controllers
{
    public class VolunteerProfileController : Controller
    {
        // GET: VolunteerProfile/ProfileView
        public ActionResult ProfileView(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Password p = (Password)Session["member"];
            if (p != null)
                try {
                    return View(new AbsContact(p));
                }
                catch (Exception e)
                {
                    if (Request.IsLocal)
                        ViewBag.Message = "Error: " + e.Message;
                    else
                        ViewBag.Message = "Error: " + e.Message;
                    return View(new AbsContact());//should not happen
                }
            else if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
            else
                return RedirectToAction("Index", "Home");//go to index
        }

        // GET: VolunteerProfile/Edit/5
        public ActionResult Edit(int id)
        {
            ContactInfoSignup c = new ContactInfoSignup();

            Password p = (Password)Session["member"];
            if (p != null)
                c.setContactInfo(new AbsContact(p));
            else
            {
                ViewBag.Message = "You are not signed in yet";
                return View();
            }

            return View(c);
        }

        // POST: VolunteerProfile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ContactInfoSignup con)
        {
            try
            {
                //TODO add location not null
                //TODO require password entry
                //TODO convert to try Catch
                if (
                    //con.confirmPassword == null ||
                    //con.confirmPassword.Pass == null ||
                    con.ContactInfo_Email == null ||
                    con.ContactInfo_FName == null ||
                    con.ContactInfo_LName == null ||
                    con.ContactInfo_Phone == null ||
                    con.DateOfBirth == null ||
                    con.password == null
                    )
                {
                    ViewBag.Message = "Please fill in every field";
                    return View(con);
                }
                //else if (con.confirmPassword.Pass != con.password.Pass)
                //{
                //    ViewBag.Message = "Invalid Password";
                //    return View(con);
                //}
                else if (DateTime.Now.Year - con.DateOfBirth.Year < 13)
                {
                    ViewBag.Message = "You must be older than 13 years old";
                    return View(con);
                }
                else if (!(con.ContactInfo_Email.Contains('@') && con.ContactInfo_Email.Contains('.') && con.ContactInfo_Email.Length > 6))
                {
                    ViewBag.Message = "Email is invalid";
                    return View(con);
                }
                else if (false)//TODO check for valid phone number
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

                con.Update();
                return RedirectToAction("ProfileView", "VolunteerProfile");
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View(con);
            }
        }

        // GET: VolunteerProfile/Delete/5
        public ActionResult Delete(int id)
        {
            AbsContact c = new AbsContact();
            c.contact_ID = id;
            c.LoadId();
            return View(c);
        }

        // POST: VolunteerProfile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, AbsContact c)
        {
            try
            {
                // TODO: Add delete logic here
                c.Delete();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(c);
            }
        }

    }
}
