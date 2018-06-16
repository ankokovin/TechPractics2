using System.Web.Mvc;
using System.Linq;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class UserToCustomerController: DataController
    {
        public UserToCustomerController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("UserToCustomerCollection");

        public ActionResult UserToCustomerCollection()
        {

            ViewData.Model = dataManager.UserToCustomerRepos.Select(x => true);
            return View();
        }

        public void Check(int UserId, int CustomerId)
        {
            if (UserId < 0)
                ModelState.AddModelError("UserId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.UserToCustomer_User);
            if (CustomerId < 0)
                ModelState.AddModelError("CustomerId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.UserToCustomer_Customer);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Useres = dataManager.UsersRepos.Select(x => true);
            var Customers = dataManager.CustomerRepos.Select(x => true);

            ViewData[GlobalResources.SiteResources.UserToCustomer_User] = new SelectList(Useres, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.UserToCustomer_Customer] = new SelectList(Customers, "Id", "FullDisc");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(int UserId, int CustomerId)
        {
            Check(UserId, CustomerId);
            if (ModelState.IsValid)
            {
                dataManager.UserToCustomerRepos.Add(
                    dataManager.UsersRepos.Find(UserId),
                    dataManager.CustomerRepos.Select(x => x.Id == CustomerId).FirstOrDefault(),
                    out string Res
                    );
                return RedirectToAction("Index");
            }
            var Useres = dataManager.UsersRepos.Select(x => true);
            var Customers = dataManager.CustomerRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.UserToCustomer_User] = new SelectList(Useres, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.UserToCustomer_Customer] = new SelectList(Customers, "Id", "FullDisc");
            return View();
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.UserToCustomerRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.UserToCustomerRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.UserToCustomer order)
        {
            dataManager.UserToCustomerRepos.Remove(order.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.UserToCustomerRepos.Find(id);
            var Useres = dataManager.UsersRepos.Select(x => true);
            var Customers = dataManager.CustomerRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.UserToCustomer_User] = new SelectList(Useres, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.UserToCustomer_Customer] = new SelectList(Customers, "Id", "FullDisc");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, int UserId, int CustomerId)
        {
            Check(UserId, CustomerId);
            if (ModelState.IsValid)
            {
                dataManager.UserToCustomerRepos.Change(id,
                    dataManager.UsersRepos.Find(UserId),
                    dataManager.CustomerRepos.Find(CustomerId),
                    out string Res
                    );
                return RedirectToAction("Index");
            }
            ViewData.Model = dataManager.UserToCustomerRepos.Find(id);
            var Useres = dataManager.UsersRepos.Select(x => true);
            var Customers = dataManager.CustomerRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.UserToCustomer_User] = new SelectList(Useres, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.UserToCustomer_Customer] = new SelectList(Customers, "Id", "FullDisc");
            return View();
        }

        public ActionResult ExcelExport()
        {
            AnaliticController.ExportToExcel<Models.EDM.UserToCustomer>("UserToCustomer", this, dataManager);
            return RedirectToAction("Index");
        }
    }
}