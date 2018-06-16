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
            ViewData.Model = dataManager.HouseRepos.Select(x => true);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Streets = dataManager.StreetRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.House_Street] = new SelectList(Streets, "Id", "FullDisc");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(string Number, int StreetId)
        {
            Check(Number, StreetId);
            if (ModelState.IsValid)
            {
                dataManager.HouseRepos.Add(Number, dataManager.StreetRepos.Find(StreetId), out string Res);
                return RedirectToAction("Index");
            }
            var Streets = dataManager.StreetRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.House_Street] = new SelectList(Streets, "Id", "FullDisc");
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
            ViewData.Model = dataManager.HouseRepos.Find(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.HouseRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.House house)
        {
            dataManager.HouseRepos.Remove(house.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.HouseRepos.Find(id);
            var Streets = dataManager.StreetRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.House_Street] = new SelectList(Streets, "Id", "FullDisc");
            return View(); 
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, string Number, int StreetId)
        {
            Check(Number, StreetId);
            if (ModelState.IsValid)
            {
                dataManager.HouseRepos.Change(id, Number, dataManager.StreetRepos.Find(StreetId), out string Res);
                return RedirectToAction("Index");
            }

            ViewData.Model = dataManager.HouseRepos.Find(id);
            var Streets = dataManager.StreetRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.House_Street] = new SelectList(Streets, "Id", "FullDisc");
            return View();
        }
        public ActionResult ExcelExport()
        {
            AnaliticController.ExportToExcel<Models.EDM.House>("House", this, dataManager);
            return RedirectToAction("Index");
        }
    }
}