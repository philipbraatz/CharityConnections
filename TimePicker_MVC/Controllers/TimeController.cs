using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimePicker_MVC.Controllers
{
    public class TimeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
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