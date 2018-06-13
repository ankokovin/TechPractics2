using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class AddressController : DataController
    {
        public AddressController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("AddressCollection");

        public ActionResult AddressCollection()
        {
            ViewData.Model = dataManager.AddressRepos.SelectAddresss(x => true);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Houses = dataManager.HouseRepos.SelectHouses(x => true);
            ViewData[GlobalResources.SiteResources.Address_House] = new SelectList(Houses, "Id", "FullDisc");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(int Flat, int HouseId)
        {
            Check(Flat, HouseId);
            if (ModelState.IsValid)
            {
                dataManager.AddressRepos.AddAddress(Flat, dataManager.HouseRepos.FindHouse(HouseId), out string Res);
                return RedirectToAction("Index");
            }

            var Houses = dataManager.HouseRepos.SelectHouses(x => true);
            ViewData[GlobalResources.SiteResources.Address_House] = new SelectList(Houses, "Id", "FullDisc");
            return View();

        }

        public void Check(int Flat, int HouseId)
        {
            if (Flat < 0)
                ModelState.AddModelError("Flat", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Address_Flat);
            if (HouseId < 0)
                ModelState.AddModelError("HouseId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Address_House);
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.AddressRepos.FindAddress(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.AddressRepos.FindAddress(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Address address)
        {
            dataManager.AddressRepos.RemoveAddress(address.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.AddressRepos.FindAddress(id);

            var Houses = dataManager.HouseRepos.SelectHouses(x => true);
            ViewData[GlobalResources.SiteResources.Address_House] = new SelectList(Houses, "Id", "FullDisc");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id,int Flat, int HouseId)
        {
            Check(Flat, HouseId);
            if (ModelState.IsValid)
            {
                dataManager.AddressRepos.ChangeAddress(id, Flat, dataManager.HouseRepos.FindHouse(HouseId), out string Res);
                return RedirectToAction("Index");
            }

            ViewData.Model = dataManager.AddressRepos.FindAddress(id);

            var Houses = dataManager.HouseRepos.SelectHouses(x => true);
            ViewData[GlobalResources.SiteResources.Address_House] = new SelectList(Houses, "Id", "FullDisc");
            return View();
        }

    }
}