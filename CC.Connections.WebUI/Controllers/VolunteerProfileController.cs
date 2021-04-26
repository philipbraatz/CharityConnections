using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Doorfail.Connections.BL;
using Doorfail.Connections.WebUI.Model;

namespace Doorfail.Connections.WebUI.Controllers
{
    //TODO rename to VolunteerController
    public class VolunteerProfileController : Controller
    {
        // GET: VolunteerProfile/ProfileView
        public ActionResult ProfileView(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Password p = (Password)Session["member"];
            if (p != null)
                try {
                    return View(new Volunteer(p.email,true));
                }
                catch (Exception e)
                {
                    if (Request.IsLocal)
                        ViewBag.Message = "Error: " + e.Message;
                    else
                        ViewBag.Message = "Error: " + e.Message;
                    return View(new Volunteer());//should not happen
                }
            else
                return RedirectToAction("Index", "Home");//Im logged out, go to home or sign in
        }

        // GET: VolunteerProfile/Edit/5
        public ActionResult Edit(int id)
        {
            VolunteerSignup c = new VolunteerSignup();

            Password p = (Password)Session["member"];
            if (p != null)
                c.setVolunteer(new Volunteer(p.email,true));
            else
            {
                ViewBag.Message = "You are not signed in yet";
                return View();
            }

            return View(c);
        }

        // POST: VolunteerProfile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, VolunteerSignup con)
        {
            try
            {
                //TODO add location not null
                //TODO require password entry
                //TODO convert to try Catch
                if (
                    //con.confirmPassword == null ||
                    //con.confirmPassword.Pass == null ||
                    con.Email == null ||
                    con.ContactInfo.FName == null ||
                    con.ContactInfo.LName == null ||
                    con.ContactInfo.Phone == null ||
                    con.ContactInfo.DateOfBirth == null ||
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
                else if (DateTime.Now.Year - con.ContactInfo.DateOfBirth.Year < 13)
                {
                    ViewBag.Message = "You must be older than 13 years old";
                    return View(con);
                }
                else if (!(Regex.IsMatch(con.ContactInfo.MemberEmail,
                    "/ ^w +[+.w -] *@([w -] +.) * w +[w -] *.([a - z]{ 2,4}| d +)$/ i") && con.Email.Length > 6))
                {
                    ViewBag.Message = "Email is invalid";
                    return View(con);
                }
                else if (false)//TODO check for valid phone number
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
                else if(Regex.IsMatch(con.ContactInfo.Phone,
                    @"^([\(]{ 1}[0 - 9]{ 3}[\)]{ 1}[\.| |\-]{ 0,1}| ^[0 - 9]{ 3}[\.|\-| ]?)?[0 - 9]{ 3} (\.|\-| )?[0 - 9]{ 4}$"))
                {
                    ViewBag.Message = "Not a valid phone number";
                    return View(con);
                }
                apiHelper.update<Contact>(con.ContactInfo);
                //con.ContactInfo.Update();
                return RedirectToAction("ProfileView", "VolunteerProfile");
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View(con);
            }
        }

        // GET: VolunteerProfile/Delete/5
        public ActionResult Delete(string id)
        {
            Volunteer c = new Volunteer();
            c.Email = id;
            c.LoadId();
            return View(c);
        }

        // POST: VolunteerProfile/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Volunteer c)
        {
            try
            {
                apiHelper.delete<Contact,String>(c.ContactInfo.MemberEmail);
                //c.ContactInfo.Delete();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(c);
            }
        }

    }
}
