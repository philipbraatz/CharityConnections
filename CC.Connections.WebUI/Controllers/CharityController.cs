﻿using System;
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
        
        public ActionResult CharityProfile()
        {
            if (Session != null && Session["member"] != null && ((Password)Session["Member"]).MemberType == MemberType.CHARITY)
            {
                Charity c = new Charity(((Password)Session["Member"]));
                int id = c.ID;
                return RedirectToAction("Details",new { id = new Charity(((Password)Session["Member"])).ID});
            }
            else if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
            else
                return RedirectToAction("Index", "Home");//go to index
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
                    allCharities.LoadAll();
                    //if (Session != null && Session["member"] != null)
                        //foreach (var ev in allCharities) ;
                            //ev.Member_Attendance = new AbsEventAtendee(ev.Event_ID, ((Password)Session["member"]).email);
                }
            }
            else
            {
                //convert to Model
                allCharities = new CharityList();
                allCharities.LoadAll();
                //if (Session != null && Session["member"] != null)
                    //foreach (var ev in allCharities)
                        //ev.Member_Attendance = new AbsEventAtendee(ev.Event_ID, ((Password)Session["member"]).email);

                //save
                Session["charities"] = allCharities;
            }

            return View(allCharities);
        }

        // GET: CharityEvent/CategoryView/2
        public ActionResult CategoryView(int id)
        {
            ViewBag.Title = new Category(id).Category_Desc;

            //load
            CharityList allCharities = new CharityList();
            if (Session != null && Session["charityList"] != null)
            {
                allCharities = ((CharityList)Session["charityList"]);//TODO have all views use Session for speed increase
                allCharities.Filter(id, SortBy.CATEGORY);
                if (allCharities.Count != CharityEventList.getCount())//reload to catch missing
                    allCharities.LoadWithFilter(id, SortBy.CATEGORY);
            }
            else
            {
                //convert to Model
                allCharities = new CharityList();
                allCharities.LoadWithFilter(id, SortBy.CATEGORY);

                //save
                Session["charityList"] = allCharities;
            }

            return View("Index", allCharities);
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
                c.Update((Password)Session["member"]);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(c);
            }
        }
    }
}