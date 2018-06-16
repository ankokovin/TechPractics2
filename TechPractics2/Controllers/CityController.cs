using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class CityController : DataController
    {
        public CityController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("CityCollection");

        public ActionResult CityCollection()
        {
            ViewData.Model = dataManager.CityRepos.Select(x => true);
            return View();
        }



        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.CityRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.CityRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.City city)
        {
            dataManager.CityRepos.Remove(city.Id, out string Res);
            return RedirectToAction("Index");
        }

        public void Check(Models.EDM.City city)
        {
            if (string.IsNullOrWhiteSpace(city.Name))
                ModelState.AddModelError("Name", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.City_Name);
           
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(Models.EDM.City city)
        {
            Check(city);
            if (ModelState.IsValid)
            {
                dataManager.CityRepos.Add(city.Name, out string Res);
                return RedirectToAction("Index");
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.CityRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Models.EDM.City city)
        {
            Check(city);
            if (ModelState.IsValid)
            {
                dataManager.CityRepos.Change(city.Id, city.Name, out string Res);
                return RedirectToAction("Index");
            }
            ViewData.Model = dataManager.CityRepos.Find(city.Id);
            return View();
        }

        public ActionResult ExcelExport()
        {
            AnaliticController.ExportToExcel<Models.EDM.City>("City", this, dataManager);
            return RedirectToAction("Index");
        }
    }
}