using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;
using CC.Connections.WebUI.Model;

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
            CharitySignup c = new CharitySignup();

            Password p = (Password)Session["member"];
            if (p != null)
                c = new CharitySignup(new Charity(id));
            else
            {
                ViewBag.Message = "You are not signed in yet";
                return View();
            }

            return View(c);
        }

        // POST: Charity/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CharitySignup csu)
        {
            try
            {
                csu.Location = new Charity(id).Location;//TEMP FIX for location missing
                try
                {
                    if (
                        //csu.confirmPassword.Pass == null ||
                        csu.Charity_Email == null ||
                        //csu.Category == null ||//TODO Category picker
                        csu.Charity_Cause == null ||
                        csu.Charity_Deductibility == null ||
                        csu.Charity_EIN == null ||
                        csu.Password == null ||
                        csu.Charity_Name == null
                        )//TODO check location
                    {
                        ViewBag.Message = "Please fill in every field";
                        return View(csu);
                    }
                    //else if (csu.confirmPassword.Pass != csu.Password.Pass)
                    //{
                    //    ViewBag.Message = "Passwords do not match";
                    //    return View(csu);
                    //}
                    else if (csu.Charity_Name.Trim().Length < 3)
                    {
                        ViewBag.Message = "Charity name must be at least 3 characters long";
                        return View(csu);
                    }
                    else if (!(csu.Charity_Email.Contains('@') && csu.Charity_Email.Contains('.') && csu.Charity_Email.Length > 6))
                    {
                        ViewBag.Message = "Email is invalid";
                        return View(csu);
                    }
                    else if (false)//TODO check for valid phone number
                    {
                        ViewBag.Message = "Phone number is invalid";
                        return View(csu);
                    }
                    Random r = new Random();
                    CC.Connections.BL.CategoryList categoryList = new CC.Connections.BL.CategoryList();
                    try
                    {
                        categoryList.LoadAll();
                        csu.Category = new Category(r.Next(1, categoryList.Count - 1));
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = e;
                    }
                    csu.Password.email = csu.Charity_Email;
                    csu.Password.MemberType = MemberType.CHARITY;
                    Session["member"] = csu.Password;
                    // Location loc = new Location(2);
                    //csu.Location = loc;//TEMP always set to location 2 because its currently null
                    csu.Update((Password)Session["member"]);


                    return RedirectToAction("Index", "Charity");//go to profile
                }
                catch (Exception ex)
                {
                    if (ex.Message != "The underlying provider failed on Open.")
                        if (Request.IsLocal)
                            ViewBag.Message = "Error: " + ex.InnerException.Message;
                        else
                            ViewBag.Message = "Error: " + ex.InnerException.Message;
                    else if (Request.IsLocal)
                        ViewBag.Message = "Error: could not access the database, check database connection. The underlying provider failed on Open.";//local error
                    else
                        ViewBag.Message = "Unable to process any sign up's at this time.";//specialized error handler


                    return View(csu);
                }

                csu.Update((Password)Session["member"]);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(csu);
            }
        }
    }
}