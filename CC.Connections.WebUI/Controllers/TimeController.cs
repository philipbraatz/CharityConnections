using CC.Connections.WebUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimePickerMVC.Controllers
{
    public class TimeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.TimeList = TimeUtils.TimeList;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string selectedTime)
        {
            ViewBag.Message = "Selected Time: " + selectedTime;
            return View();
        }
    }
}