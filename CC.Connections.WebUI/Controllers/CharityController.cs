using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Controllers
{
    public class CharityController : Controller
    {
        // GET: Charity Profile
        public ActionResult Details(int id)
        {
            return View(new Charity(id));
        }


        // GET: List of Charities
        public ActionResult Index()
        {
            if (ViewBag.Title == null)
                ViewBag.Title = "Charities";

            //load
            CharityList allCharities = new CharityList();
            if (Session != null && Session["charities"] != null)
            {
                allCharities = ((CharityList)Session["charities"]);
                if (allCharities.Count != CharityList.getCount())//reload to catch missing
                {
                    allCharities.LoadList();
                    //if (Session != null && Session["member"] != null)
                        //foreach (var ev in allCharities) ;
                            //ev.Member_Attendance = new AbsEventAtendee(ev.Event_ID, ((Password)Session["member"]).email);
                }
            }
            else
            {
                //convert to Model
                allCharities = new CharityList();
                allCharities.LoadList();
                //if (Session != null && Session["member"] != null)
                    //foreach (var ev in allCharities)
                        //ev.Member_Attendance = new AbsEventAtendee(ev.Event_ID, ((Password)Session["member"]).email);

                //save
                Session["charities"] = allCharities;
            }

            return View(allCharities);
        }

        // GET: Charity/Edit/5
        public ActionResult Edit(int id)
        {
            Charity c = new Charity();

            Password p = (Password)Session["member"];
            if (p != null)
                c = new Charity(id);
            else
            {
                ViewBag.Message = "You are not signed in yet";
                return View();
            }

            return View(c);
        }

        // POST: Charity/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Charity c)
        {
            try
            {
                // TODO: Add update logic here
                c.Update();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(c);
            }
        }
    }
}