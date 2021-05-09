using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Doorfail.Connections.BL;
using Doorfail.Connections.WebUI.Model;
using Doorfail.DataConnection;
using Newtonsoft.Json;

namespace Doorfail.Connections.WebUI.Controllers
{
    public class CharityController : Controller
    {
        // GET: Charity Profile
        public ActionResult Details(string id)
        {
            try {
                Charity dCharity = apiHelper.getEmail<Charity>(id);//apiHelper.getEmail<Charity>(id);
                if (dCharity == null)
                {
                    ViewBag.Message = "That Charity does not exist";
                    return View();
                }
                return View(dCharity);
            }
            catch(Exception)
            {
                return RedirectToAction("Index");//returns you if you enter empty id
            }

           
        }

        [ChildActionOnly]
        public ActionResult DetailsPartial(string id)
        {
            try {
                Charity dCharity = apiHelper.getEmail<Charity>(id);
                if (dCharity == null)
                {
                    ViewBag.Message = "That Charity does not exist";
                    return PartialView();
                }
                return PartialView(dCharity);
            }
            catch (Exception e){
                ViewBag.Message = e.Message;
                return PartialView();
            }
            
        }

        public ActionResult CharityProfile()
        {
            if (SessionUtil.GetMemberType(Session) == MemberType.CHARITY)
            {
                try
                {
                    Charity c = apiHelper.fromPassword<Charity>((Password)Session["Member"]);//new Charity(((Password)Session["Member"]));
                } catch(HttpUnhandledException e)
                {
                    Content(e.Message);
                }
                return RedirectToAction("Details",new { 
                    id = ((Password)Session["Member"]).email.Replace('.','-')
                });
            }
            else if (ControllerContext.HttpContext.Request.UrlReferrer != null)
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());//go back
            else
                return RedirectToAction("Index", "Home");//go to index
        }

        
        // GET: List of Charities
        public ActionResult Index()
        {
            if (ViewBag.Title == null)//Check if filtering
                ViewBag.Title = "Charities";

            Session["charities"] = SessionUtil.GetList<Charity>(Session,"charities").ToArray();
            return View(Session["charities"]);
        }

        // GET: CharityEvent/CategoryView/2
        public ActionResult CategoryView(Guid id)
        {
            Category cat = apiHelper.getOne<PL.Category>(id);
            ViewBag.Title = cat.Desc;// new Category(id).Desc;

            //load
            CharityCollection allCharities = (CharityCollection)SessionUtil.GetList<Charity>(Session,"charities");
            Session["charities"] = allCharities.ToArray();
            allCharities.Filter(id, SortBy.CATEGORY);

            return View("Index", allCharities);
        }

        // GET: Charity/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("id cannot be null", nameof(id));

            CharitySignup c = new CharitySignup();

            Password p = (Password)Session["member"];
            try
            {
                if (p != null)
                {
                    c = new CharitySignup(apiHelper.getEmail<PL.Charity>(id));//.Replace('-', '.')));
                    //TODO BUG: idk why getEmail doesn't set ID
                    c.Email = id.Replace('-', '.');
                }
                else
                {
                    ViewBag.Message = "You are not signed in yet";
                    return RedirectToAction("Login", "LogInView");
                }
            }
            catch(HttpUnhandledException e)
            {
                return Content(e.Message);
            }

            return View(c);
        }

        // POST: Charity/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, CharitySignup csu)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentException("id cannot be null", nameof(id));

                //csu.Location = new Location(csu.Location);
                try
                {
                    if (
                        //csu.confirmPassword.Pass == null    ||
                        string.IsNullOrWhiteSpace(csu.Email)  ||
                         //csu.Category == null               ||//TODO Category picker
                         string.IsNullOrWhiteSpace(csu.Cause) ||
                         string.IsNullOrWhiteSpace(csu.EIN)   ||
                         csu.Password is null                 ||
                         string.IsNullOrWhiteSpace(csu.Name)  )
                         //TODO check location
                    {
                        ViewBag.Message = "Please fill in every field";
                        return View(csu);
                    }
                    //else if (csu.confirmPassword.Pass != csu.Password.Pass)
                    //{
                    //    ViewBag.Message = "Passwords do not match";
                    //    return View(csu);
                    //}
                    else if (csu.Name.Trim().Length < 3)
                    {
                        ViewBag.Message = "Charity name must be at least 3 characters long";
                        return View(csu);
                    }
                    else if (!(csu.Email.Contains('@') && csu.Email.Contains('.') && csu.Email.Length > 6))
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
                    //TODO implement category picker
                    CategoryCollection categoryList = new CategoryCollection (apiHelper.getAll<PL.Category>());//new Doorfail.Connections.BL.CategoryCollection();
                    csu.Category = categoryList.ElementAt(r.Next(0, categoryList.Count - 1));

                    csu.Password.email = csu.Email;
                    csu.Password.MemberType = MemberType.CHARITY;
                    Session["member"] = csu.Password;
                    // Location loc = new Location(2);
                    //csu.Location = loc;//TEMP always set to location 2 because its currently null
                    //csu.Update((Password)Session["member"]);
                    apiHelper.update<Charity>(csu);

                    return RedirectToAction("Index", "Charity");//go to profile
                }
                catch (Exception ex)
                {
                    if (ex.Message != "The underlying provider failed on Open.")
                        if (Request.IsLocal)
                        {
                            if (ex.InnerException != null)
                                ViewBag.Message = "Error: " + ex.InnerException.Message;
                            else
                                ViewBag.Message = "Error: " + ex;
                        }
                        else//published error message
                        {
                            if (ex.InnerException != null)
                                ViewBag.Message = "Error: " + ex.InnerException.Message;
                            else
                                ViewBag.Message = "Error: " + ex;
                        }
                    else if (Request.IsLocal)
                        ViewBag.Message = "Error: could not access the database, check database connection. The underlying provider failed on Open.";//local error
                    else
                        ViewBag.Message = "Unable to process any sign up's at this time.";//specialized error handler


                    return View(csu);
                }
            }
            catch(HttpUnhandledException e)
            {
                return Content(e.Message);
            }
        }
    }
}