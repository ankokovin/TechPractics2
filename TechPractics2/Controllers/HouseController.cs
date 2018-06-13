using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class HouseController : DataController
    {
        public HouseController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("HouseCollection");

        public ActionResult HouseCollection()
        {
            ViewData.Model = dataManager.HouseRepos.SelectHouses(x => true);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Streets = dataManager.StreetRepos.SelectStreets(x => true);
            ViewData[GlobalResources.SiteResources.House_Street] = new SelectList(Streets, "Id", "Name");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(string Number, int StreetId)
        {
            Check(Number, StreetId);
            if (ModelState.IsValid)
            {
                dataManager.HouseRepos.AddHouse(Number, dataManager.StreetRepos.FindStreet(StreetId), out string Res);
                return RedirectToAction("Index");
            }
            var Streets = dataManager.StreetRepos.SelectStreets(x => true);
            ViewData[GlobalResources.SiteResources.House_Street] = new SelectList(Streets, "Id", "Name");
            return View();

        }

        public void Check(string Number, int StreetId)
        {
            if (string.IsNullOrEmpty(Number))
                ModelState.AddModelError("Number", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.House_Number);
            else if (!Checker.IsHouseNumber(Number))
                ModelState.AddModelError("Number", GlobalResources.SiteResources.WrongFormat + GlobalResources.SiteResources.House_Number);
            if (StreetId < 0)
                ModelState.AddModelError("StreetId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.House_Street);
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.HouseRepos.FindHouse(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.HouseRepos.FindHouse(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.House house)
        {
            dataManager.HouseRepos.RemoveHouse(house.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.HouseRepos.FindHouse(id);
            var Streets = dataManager.StreetRepos.SelectStreets(x => true);
            ViewData[GlobalResources.SiteResources.House_Street] = new SelectList(Streets, "Id", "Name");
            return View(); 
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, string Number, int StreetId)
        {
            Check(Number, StreetId);
            if (ModelState.IsValid)
            {
                dataManager.HouseRepos.ChangeHouse(id, Number, dataManager.StreetRepos.FindStreet(StreetId), out string Res);
                return RedirectToAction("Index");
            }

            ViewData.Model = dataManager.HouseRepos.FindHouse(id);
            var Streets = dataManager.StreetRepos.SelectStreets(x => true);
            ViewData[GlobalResources.SiteResources.House_Street] = new SelectList(Streets, "Id", "Name");
            return View();
        }

    }
}