using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System;
using System.Diagnostics;
using TechPractics2.Models.UtilityModels;

namespace TechPractics2.Controllers
{

    public class HomeController : DataController
    {
        public HomeController(Models.DataManager dataManager) : base(dataManager) {  }

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
        public ActionResult AutoSignIn()
        {
            if (Session[GlobalResources.SiteResources.User] == null)
            {
                var cookie = Request.Cookies[GlobalResources.SiteResources.User];

                if (cookie != null)
                {
                    return SignIn(cookie.Values[GlobalResources.SiteResources.User_Login], cookie.Values[GlobalResources.SiteResources.User_Password], true, true,false);
                }
            }
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(string Login, string Password, bool SaveToCookies, bool redirectToIndex=false, bool manual=true)
        {
            Debug.WriteLine("Trying to sign in");
            Debug.WriteLine("Login:"+ Login);
            Debug.WriteLine("Password:"+ Password);
            string Source=string.Empty, Message;
            if (!manual || TechPractics2.Models.Checker.CheckLoginInfo(Login, Password, out Message,out Source)){
                Models.EDM.User user = dataManager.UsersRepos.TryEntry(Login, Password,Request.UserHostAddress ,out Message);
                if (user != null)
                {

                    Session[GlobalResources.SiteResources.User] = user;

                    HttpCookie cookie = Request.Cookies[GlobalResources.SiteResources.User];

                    if (SaveToCookies)
                    {
                        if (cookie == null)
                        {
                            cookie = new HttpCookie(GlobalResources.SiteResources.User);
                        }
                        cookie.Values[GlobalResources.SiteResources.User_Login] = user.Login;
                        cookie.Values[GlobalResources.SiteResources.User_Password] = Message;
                        cookie.Expires = DateTime.Now.AddDays(int.Parse(GlobalResources.SiteResources.CookiesExpirationDays));
                        Response.Cookies.Add(cookie);
                    }
                    Session[GlobalResources.SiteResources.Remember] = SaveToCookies;
                    if (!redirectToIndex)
                        return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.AbsolutePath);
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            if (redirectToIndex)
                Session[GlobalResources.SiteResources.Last_Url] = "/Home/Index";
            else
                Session[GlobalResources.SiteResources.Last_Url]= System.Web.HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            Session[GlobalResources.SiteResources.Last_sing_in_error] = new Tuple<string,string>(Source, Message);
            if (manual)
                return RedirectToAction("FailedLogIn");
            else
                return RedirectToAction("Index");
        }

        public ActionResult SuccessLogIn()
        {
            return View();
        }

        public ActionResult FailedLogIn()
        {
            return View();
        }

        public ActionResult LoginRedirect()
        {
            string url = Session[GlobalResources.SiteResources.Last_Url] as string;
            Debug.WriteLine("RedirectURL:"+url);
            Session.Remove(GlobalResources.SiteResources.Last_Url);
            if (url!=null)
                return Redirect(url);
            return RedirectToAction("Index", "Home");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignOut()
        {
            Session.Remove(GlobalResources.SiteResources.User);
            if (Session[GlobalResources.SiteResources.Remember] != null)
            {
                if (!(bool)Session[GlobalResources.SiteResources.Remember])
                {
                    HttpCookie httpCookie = Request.Cookies[GlobalResources.SiteResources.User];
                    if (httpCookie != null)
                    {
                        httpCookie.Expires = DateTime.Now.AddDays(-int.Parse(GlobalResources.SiteResources.CookiesExpirationDays));
                        Response.Cookies.Add(httpCookie);
                    }

                }
                Session.Remove(GlobalResources.SiteResources.Remember);
            }
            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.AbsolutePath);
        }
    }
}