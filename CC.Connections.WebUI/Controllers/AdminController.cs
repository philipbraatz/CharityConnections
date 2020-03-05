using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CC.Connections.WebUI.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.Charities.ToList());
            }
        }

        // GET: Admin/Charity
        public ActionResult Charity()
        {
            using (CCEntities dc = new CCEntities())
            {
                return View(dc.Charities.ToList());
            }
        }
    }
}