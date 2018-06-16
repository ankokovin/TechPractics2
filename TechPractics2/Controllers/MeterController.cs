using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class MeterController : DataController
    {

        public MeterController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("MeterCollection");

        public ActionResult MeterCollection()
        {
            ViewData.Model = dataManager.MeterRepos.Select(x => true);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Cities = dataManager.MeterTypeRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.Meter_MeterType] = new SelectList(Cities, "Id", "Name");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(string Name, int MeterTypeId)
        {
            Check(Name, MeterTypeId);
            if (ModelState.IsValid)
            {
                dataManager.MeterRepos.Add(Name, dataManager.MeterTypeRepos.Find(MeterTypeId), out string Res);
                return RedirectToAction("Index");
            }
            var Cities = dataManager.MeterTypeRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.Meter_MeterType] = new SelectList(Cities, "Id", "Name");
            return View();

        }

        public void Check(string Name, int MeterTypeId)
        {
            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Meter_Name);
            if (MeterTypeId < 0)
                ModelState.AddModelError("MeterTypeId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Meter_MeterType);
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.MeterRepos.Find(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.MeterRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Meter street)
        {
            dataManager.MeterRepos.Remove(street.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.MeterRepos.Find(id);
            var Cities = dataManager.MeterTypeRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.Meter_MeterType] = new SelectList(Cities, "Id", "Name");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, string Name, int MeterTypeId)
        {
            Check(Name, MeterTypeId);
            if (ModelState.IsValid)
            {
                dataManager.MeterRepos.Change(id, Name, dataManager.MeterTypeRepos.Find(MeterTypeId), out string Res);
                return RedirectToAction("Index");
            }
            ViewData.Model = dataManager.MeterRepos.Find(id);
            var Cities = dataManager.MeterTypeRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.Meter_MeterType] = new SelectList(Cities, "Id", "Name");
            return View();
        }
        public ActionResult ExcelExport()
        {
            AnaliticController.ExportToExcel<Models.EDM.Meter>("Meter", this, dataManager);
            return RedirectToAction("Index");
        }
    }
}