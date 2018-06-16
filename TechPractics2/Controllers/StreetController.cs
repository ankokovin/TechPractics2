using System.Linq;
using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{


    public class StreetController : DataController
    {
        public StreetController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("StreetCollection");

        public ActionResult StreetCollection()
        {
            ViewData.Model = dataManager.StreetRepos.Select(x => true);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Cities = dataManager.CityRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.Street_City] = new SelectList(Cities,"Id","Name");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(string Name, int CityId)
        {
            Check(Name, CityId);
            if (ModelState.IsValid)
            {
                dataManager.StreetRepos.Street(Name, dataManager.CityRepos.Find(CityId), out string Res);
                return RedirectToAction("Index");
            }
            var Cities = dataManager.CityRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.Street_City] = new SelectList(Cities, "Id", "Name");
            return View();

        }

        public void Check(string Name, int CityId)
        {
            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Street_Name);
            if (CityId < 0)
                ModelState.AddModelError("CityId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Street_City);
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.StreetRepos.Find(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.StreetRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Street street)
        {
            dataManager.StreetRepos.Remove(street.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.StreetRepos.Find(id);
            var Cities = dataManager.CityRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.Street_City] = new SelectList(Cities, "Id", "Name");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, string Name, int CityId)
        {
            Check(Name, CityId);
            if (ModelState.IsValid)
            {
                dataManager.StreetRepos.Change(id, Name, dataManager.CityRepos.Find(CityId), out string Res);
                return RedirectToAction("Index");
            }
            ViewData.Model = dataManager.StreetRepos.Find(id);
            var Cities = dataManager.CityRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.Street_City] = new SelectList(Cities, "Id", "Name");
            return View();
        }

        public ActionResult ExcelExport()
        {
            AnaliticController.ExportToExcel<Models.EDM.Street>("Street", this, dataManager);
            return RedirectToAction("Index");
        }
    }
}