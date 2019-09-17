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

        public ActionResult Create(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }

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
            HttpContext.Session["user"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Login(Member member, string returnurl)
        {
            try
            {
                if (member.Login())
                {
                    Session["member"] = member;
                    //if (returnurl != null)
                    //    return Redirect(returnurl);
                    //else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Incorrect Credentials";
                    return View(member);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(member);
            }
        }

        //Fake successful login
        //[HttpPost]
        //public ActionResult Login()
        //{
        //    return RedirectToAction("Index", "Home");
        //}

        [HttpPost]
        public ActionResult SignUp()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}