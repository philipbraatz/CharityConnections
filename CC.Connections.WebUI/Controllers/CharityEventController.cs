using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;
using CC.Connections.WebUI.Model;

namespace CC.Connections.WebUI.Controllers
{
    public class CharityEventController : Controller
    {
        // GET: CharityEvent
        public ActionResult Index()
        {
            CharityEventList allEvents = new CharityEventList();
            allEvents.LoadAll();
            List<CharityEvent_with_Charity> charityEvents = new List<CharityEvent_with_Charity>();
            foreach (var ev in allEvents)
                charityEvents.Add(new CharityEvent_with_Charity(ev));
            return View(charityEvents);
        }

        // GET: CharityEvent/Details/5
        public ActionResult Details(int id)
        {
            return View(new CharityEvent_with_Charity(new CharityEvent(id)));
        }

        // GET: CharityEvent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CharityEvent/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CharityEvent/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CharityEvent/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CharityEvent/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CharityEvent/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
