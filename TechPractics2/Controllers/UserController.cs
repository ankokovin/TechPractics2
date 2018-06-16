using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class UserController : DataController
    {
        public UserController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("UserCollection");

        public ActionResult UserCollection()
        {
            ViewData.Model = dataManager.UsersRepos.Select(x => true);
            return View();
        }



        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.UsersRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.UsersRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.User user)
        {
            dataManager.UsersRepos.Remove(user.Id, out string Res);
            return RedirectToAction("Index");
        }

        public void Check(Models.EDM.User user)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
                ModelState.AddModelError("Login", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.User_Login);

            if (string.IsNullOrWhiteSpace(user.Password))
                ModelState.AddModelError("Password", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.User_Password);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(Models.EDM.User user)
        {
            Check(user);
            if (ModelState.IsValid)
            {
                dataManager.UsersRepos.Add(user.UserType,user.Login,user.Password,out string Res);
                return RedirectToAction("Index");
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.UsersRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Models.EDM.User user)
        {
            Check(user);
            if (ModelState.IsValid)
            {
                dataManager.UsersRepos.Change(user.Id, user.UserType,user.Login,user.Password, out string Res);
                return RedirectToAction("Index");
            }
            ViewData.Model = dataManager.UsersRepos.Find(user.Id);
            return View();
        }

        public ActionResult ExcelExport()
        {
            AnaliticController.ExportToExcel<Models.EDM.User>("User", this, dataManager);
            return RedirectToAction("Index");
        }
    }
}