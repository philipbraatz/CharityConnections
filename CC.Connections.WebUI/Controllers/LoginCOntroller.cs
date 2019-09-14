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

        public ActionResult Logout()
        {
            //Logged out
            HttpContext.Session["user"] = null;
            return View();
        }


        [HttpPost]
        public ActionResult Create(Member member, string returnurl)
        {
            ViewResult result = View(member);
            try
            {
                ViewBag.ReturnUrl = returnurl;

                if (member.Login())
                {
                    HttpContext.Session["member"] = member;
                    //return RedirectToAction("Index", "ProgDec");
                    return Redirect(returnurl);
                }
                ViewBag.Message = "Sorry. No soup for you!";
                return result;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(member);
            }
        }

        [HttpPost]
        public ActionResult Login(Member member, string returnurl)
        {
            try
            {
                if (member.Login())
                {
                    Session["member"] = member;
                    if (returnurl != null)
                        return Redirect(returnurl);
                    else
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


    }
}