using System.Web.Mvc;
using System.Linq;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class OrderController : DataController
    {
        public OrderController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("OrderCollection");

        public ActionResult OrderCollection()
        {
            ViewData.Model = dataManager.OrderRepos.SelectOrders(x => true);
            return View();
        }

        public void Check(int AddressId, int CustomerId)
        {
            if (AddressId < 0)
                ModelState.AddModelError("AddressId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Order_Address);
            if (CustomerId < 0)
                ModelState.AddModelError("CustomerId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Order_Customer);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Addresses = dataManager.AddressRepos.SelectAddresss(x => true);
            var Customers = dataManager.CustomerRepos.SelectCustomers(x => true);

            ViewData[GlobalResources.SiteResources.Order_Address] = new SelectList(Addresses, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.Order_Customer] = new SelectList(Customers, "Id", "FullDisc");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(int AddressId, int CustomerId)
        {
            Check(AddressId, CustomerId);
            if (ModelState.IsValid)
            {
                dataManager.OrderRepos.AddOrder(
                    dataManager.UsersRepos.FindUser(((Models.EDM.User)Session[GlobalResources.SiteResources.User]).Id),
                    dataManager.CustomerRepos.SelectCustomers(x => x.Id == CustomerId).FirstOrDefault(),
                    dataManager.AddressRepos.FindAddress(AddressId),
                    out string Res,
                    out int result
                    );
                return RedirectToAction("Index");
            }
            var Addresses = dataManager.AddressRepos.SelectAddresss(x => true);
            var Customers = dataManager.CustomerRepos.SelectCustomers(x => true);
            ViewData[GlobalResources.SiteResources.Order_Address] = new SelectList(Addresses, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.Order_Customer] = new SelectList(Customers, "Id", "FullDisc");
            return View();
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.OrderRepos.FindOrder(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.OrderRepos.FindOrder(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Order order)
        {
            dataManager.OrderRepos.RemoveOrder(order.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.OrderRepos.FindOrder(id);
            var Addresses = dataManager.AddressRepos.SelectAddresss(x => true);
            var Customers = dataManager.CustomerRepos.SelectCustomers(x => true);
            ViewData[GlobalResources.SiteResources.Order_Address] = new SelectList(Addresses, "Id", "FullDisc") ;
            ViewData[GlobalResources.SiteResources.Order_Customer] = new SelectList(Customers, "Id", "FullDisc") ;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, int AddressId, int CustomerId)
        {
            Check(AddressId, CustomerId);
            if (ModelState.IsValid)
            {
                dataManager.OrderRepos.ChangeOrder(id,
                    dataManager.CustomerRepos.FindCustomer(CustomerId),
                    dataManager.AddressRepos.FindAddress(AddressId),
                    out string Res
                    );
                return RedirectToAction("Index");
            }
            ViewData.Model = dataManager.OrderRepos.FindOrder(id);
            var Addresses = dataManager.AddressRepos.SelectAddresss(x => true);
            var Customers = dataManager.CustomerRepos.SelectCustomers(x => true);
            ViewData[GlobalResources.SiteResources.Order_Address] = new SelectList(Addresses, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.Order_Customer] = new SelectList(Customers, "Id", "FullDisc");
            return View();
        }
    }
}