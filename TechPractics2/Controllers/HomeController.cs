using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System;
using System.Diagnostics;
using TechPractics2.Models;
using GlobalResources;


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

        public ActionResult Back()
        {
            var url = System.Web.HttpContext.Current.Request.UrlReferrer;
            if (url != null)
            {
                return Redirect(url.AbsolutePath);
            }
            return RedirectToAction("Index");
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SignUp()
        {
            return View();
        }

        public void Check(Models.EDM.User user)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
                ModelState.AddModelError("Login", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.User_Login);

            if (string.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError("Password", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.User_Password);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignUp(Models.EDM.User user)
        {
            Check(user);
            if (ModelState.IsValid)
            {
                bool res = dataManager.UsersRepos.AddUser(Models.EDM.UserType.Normal, user.Login, user.Password, out string Res);
                if (res)
                    return SignIn(user.Login,user.Password,true,true);
                else
                    ModelState.AddModelError("Login", Res);
            }
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewMyProfiles()
        {
            var User = Session[GlobalResources.SiteResources.User] as Models.EDM.User;
            if (User == null) return RedirectToAction("Index");
            ViewData.Model = dataManager.UserToCustomerRepos.SelectUserToCustomers(x => x.User.Id == User.Id).Select(x=>x.Customer);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateProfile()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateProfile(string FIO, string Passport, string PhoneNumber, bool IsCompany, string CompanyName, string INN)
        {
            Check(FIO, Passport, PhoneNumber, IsCompany, CompanyName, INN);
            if (ModelState.IsValid)
            {
                var user = Session[GlobalResources.SiteResources.User] as Models.EDM.User;
                if (IsCompany)
                {
                    if (dataManager.CustomerRepos.SelectCustomers(x => x is Models.EDM.Company
                    && x.Passport == Passport
                    && (x as Models.EDM.Company).INN == INN).Any())
                    {
                        ModelState.AddModelError("FIO", "Уже есть заказчик с данным номером паспорта и ИНН компании");
                    }else
                    {
                        dataManager.CompanyRepos.AddCompany(FIO, Passport, PhoneNumber, CompanyName, INN, out string Res);
                        dataManager.UserToCustomerRepos.AddUserToCustomer(
                            dataManager.UsersRepos.FindUser(user.Id),
                            dataManager.CustomerRepos.SelectCustomers(x => x is Models.EDM.Company
                                    && x.Passport == Passport
                                    && (x as Models.EDM.Company).INN == INN).First(),
                            out Res
                            );
                        return RedirectToAction("ViewMyProfiles");
                    }
                }else
                {
                    if (dataManager.CustomerRepos.SelectCustomers(x => !(x is Models.EDM.Company)
                    && x.Passport == Passport
                    ).Any())
                    {
                        ModelState.AddModelError("FIO", "Уже есть заказчик с таким номером паспорта");
                    }else
                    {
                        dataManager.CustomerRepos.AddCustomer(FIO, Passport, PhoneNumber,out string Res);
                        dataManager.UserToCustomerRepos.AddUserToCustomer(
                            dataManager.UsersRepos.FindUser(user.Id),
                            dataManager.CustomerRepos.SelectCustomers(x => !(x is Models.EDM.Company)
                                && x.Passport == Passport
                            ).First(),
                            out Res
                            );
                        return RedirectToAction("ViewMyProfiles");
                    }
                }
            }
            return View();
        }

        public void Check(string FIO, string Passport, string PhoneNumber, bool IsCompany, string CompanyName, string INN)
        {
            if (string.IsNullOrWhiteSpace(FIO))
            {
                ModelState.AddModelError(GlobalResources.SiteResources.Customer_FIO, SiteResources.PleaseInput + SiteResources.Customer_FIO + SiteResources.ToCompany);
            }
            else
            if (!Models.Checker.IsFIO(FIO))
            {
                ModelState.AddModelError(GlobalResources.SiteResources.Customer_FIO, SiteResources.Customer_FIO + SiteResources.WrongFormat);
            }
            if (string.IsNullOrWhiteSpace(Passport))
            {
                ModelState.AddModelError(GlobalResources.SiteResources.Customer_Passport, SiteResources.PleaseInput + SiteResources.Customer_Passport);
            }
            else if (!Models.Checker.IsPassportNumber(Passport))
            {
                ModelState.AddModelError(GlobalResources.SiteResources.Customer_Passport, SiteResources.WrongFormat + SiteResources.User_Password + SiteResources.TenNumber);
            }
            if (IsCompany)
            {
                if (string.IsNullOrWhiteSpace(CompanyName))
                {
                    ModelState.AddModelError(GlobalResources.SiteResources.Company_CompanyName, GlobalResources.SiteResources.PleaseInput + SiteResources.CompanyName);
                }// else if (!Checker.IsOKString)
                if (string.IsNullOrWhiteSpace(INN))
                {
                    ModelState.AddModelError(GlobalResources.SiteResources.Company_INN, SiteResources.PleaseInput + SiteResources.Company_INN);
                }
                else if (!Checker.IsINN(INN))
                {
                    ModelState.AddModelError(SiteResources.Company_INN, SiteResources.WrongFormat + SiteResources.Company_INN + SiteResources.TenNumber);
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShareProfile(int id)
        {
            ViewData.Model = dataManager.CustomerRepos.SelectCustomers(x => x.Id == id).FirstOrDefault();
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShareProfile(int customerId, string Login)
        {
            var user = dataManager.UsersRepos.SelectUsers(x => x.Login == Login).FirstOrDefault();
            if (user!=null)
            {
                dataManager.UserToCustomerRepos.AddUserToCustomer(
                    dataManager.UsersRepos.FindUser(user.Id),
                    dataManager.CustomerRepos.SelectCustomers(x => x.Id == customerId).First(),
                    out string Res
                    );
                return RedirectToAction("ViewMyProfiles");
            }else
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином не существует");
            }
            ViewData.Model = dataManager.CustomerRepos.SelectCustomers(x => x.Id == customerId).FirstOrDefault();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveProfile(int id)
        {
            ViewData.Model = dataManager.CustomerRepos.SelectCustomers(x => x.Id == id).FirstOrDefault();
            return View();
        }

        public ActionResult RemoveProfile(Models.EDM.Customer customer)
        {
            var user = Session[GlobalResources.SiteResources.User] as Models.EDM.User;
            dataManager.UserToCustomerRepos.AddUserToCustomer(
                dataManager.UsersRepos.FindUser(user.Id),
                dataManager.CustomerRepos.SelectCustomers(x => x.Id == customer.Id).First(),
                out string Res
                );
            return RedirectToAction("ViewMyProfiles");
        }
    }
}