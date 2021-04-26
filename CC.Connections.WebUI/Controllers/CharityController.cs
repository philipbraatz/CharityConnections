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
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            id = id.Replace('-', '.');
            if(id == null)
            {
                return RedirectToAction("Index");//returns you if you enter empty id
            }

            Charity dCharity = JsonDatabase.GetTable<PL.Charity>().Where(c => c.CharityEmail == id).FirstOrDefault();//apiHelper.getEmail<Charity>(id);
            //} catch(Exception e)
            //{
            //    throw e;
            //}
            if (dCharity == null)
            {
                ViewBag.Message = "That Charity does not exist";
                return View();
            }
            return View(dCharity);
        }

        [ChildActionOnly]
        public ActionResult DetailsPartial(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            id = id.Replace('-', '.');
            if (id == null)
            {
                return PartialView();
            }
            Charity dCharity = apiHelper.getEmail<Charity>(id);
            if (dCharity == null)
            {
                ViewBag.Message = "That Charity does not exist";
                return PartialView();
            }
            return PartialView(dCharity);
        }

        public ActionResult CharityProfile()
        {
            if (Session != null && Session["member"] != null && ((Password)Session["Member"]).MemberType == MemberType.CHARITY)
            {
                Charity c = new Charity(((Password)Session["Member"]));
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
            if (ViewBag.Title == null)
                ViewBag.Title = "Charities";

            //load
            dynamic testDumb = (CharityCollection)apiHelper.getAll<Charity>();
            CharityCollection allCharities = testDumb;
            if (Session != null && Session["charities"] != null)
            {
                allCharities = ((CharityCollection)Session["charities"]);
                if (allCharities.Count != CharityCollection.getCount())//reload to catch missing
                    allCharities.LoadAll();
            }
            else
            {
                //convert to Model
                allCharities = (CharityCollection)apiHelper.getAll<Charity>() ?? new CharityCollection();
                allCharities.LoadAll();
                //save
                Session["charities"] = allCharities;
            }

            return View(allCharities);
        }

        // GET: CharityEvent/CategoryView/2
        public ActionResult CategoryView(Guid id)
        {
            ViewBag.Title = new Category(id).Desc;

            //load
            CharityCollection allCharities = new CharityCollection();
            if (Session != null && Session["charityList"] != null)
            {
                allCharities = ((CharityCollection)Session["charityList"]);//TODO have all views use Session for speed increase
                allCharities.Filter(id, SortBy.CATEGORY);
                if (allCharities.Count != CharityEventCollection.getCount())//reload to catch missing
                    allCharities.LoadWithFilter(id, SortBy.CATEGORY);
            }
            else
            {
                //convert to Model
                allCharities = (CharityCollection)apiHelper.getAll<Charity>();
                allCharities.LoadWithFilter(id, SortBy.CATEGORY);

                //save
                Session["charityList"] = allCharities;
            }

            return View("Index", allCharities);
        }

        // GET: Charity/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("id cannot be null", nameof(id));

            CharitySignup c = new CharitySignup();

            Password p = (Password)Session["member"];
            if (p != null)
                c = new CharitySignup(apiHelper.getEmail<Charity>(id.Replace('-', '.')));
            else
            {
                ViewBag.Message = "You are not signed in yet";
                return View();
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

                csu.Location = apiHelper.getEmail<Charity>(id.Replace('-', '.')).Location;//TEMP FIX for location missing
                try
                {
                    if (
                        //csu.confirmPassword.Pass == null ||
                        csu.Email == null ||
                        //csu.Category == null ||//TODO Category picker
                        csu.Cause == null ||
                        csu.EIN == null ||
                        csu.Password == null ||
                        csu.Name == null
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
                    Doorfail.Connections.BL.CategoryCollection categoryList = new Doorfail.Connections.BL.CategoryCollection();
                    try
                    {
                        categoryList.LoadAll();
                        csu.Category = CategoryCollection.INSTANCE.ElementAt(r.Next(0, categoryList.Count - 1));
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = e;
                    }
                    csu.Password.email = csu.Email;
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