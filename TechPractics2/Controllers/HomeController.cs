using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System;

namespace TechPractics2.Controllers
{

    public class HomeController : DataController
    {
        public HomeController(Models.DataManager dataManager) : base(dataManager) { }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SignIn(bool signOut = false)
        {
            var cookie = Request.Cookies[GlobalResources.SiteResources.User];

            if (signOut)
            {
                Session.Remove(GlobalResources.SiteResources.User);

                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-int.Parse(GlobalResources.SiteResources.CookiesExpirationDays));
                    Response.Cookies.Add(cookie);
                }
            }

            if (cookie!=null && cookie.Expires > DateTime.Now)
            {
                return SignIn(cookie.Values[GlobalResources.SiteResources.User_Login], cookie.Values[GlobalResources.SiteResources.User_Password],true);
            }

            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(string Login, string Password, bool SaveToCookies)
        {
            string Source, Message;
            if (TechPractics2.Models.Checker.CheckLoginInfo(Login,Password,out Message,out Source)){
                Models.EDM.User user = dataManager.UsersRepos.TryEntry(Login, Password, out Message);
                if (user != null)
                {
                    if (ModelState.IsValid)
                    {
                        Session[GlobalResources.SiteResources.User] = user;

                        HttpCookie cookie = Request.Cookies[GlobalResources.SiteResources.User];

                        if (SaveToCookies&&(cookie==null||cookie.Expires < DateTime.Now))
                        {
                            cookie = new HttpCookie(GlobalResources.SiteResources.User);
                            cookie.Values[GlobalResources.SiteResources.User_Login] = user.Login;
                            cookie.Values[GlobalResources.SiteResources.User_Password] = user.Password;
                            cookie.Expires = DateTime.Now.AddDays(int.Parse(GlobalResources.SiteResources.CookiesExpirationDays));
                        }

                        //TODO: Redirect according to userType
                        return RedirectToAction("About");
                        //NEEDS CHANGE (ADDED FOR TEST)
                    }
                }
                else
                {
                    Source = GlobalResources.SiteResources.User_Password;
                }
            }
            
            ModelState.AddModelError(Source, Message);
            return View();

        }
    }
}