using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class MeterTypeController : DataController
    {
        public MeterTypeController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("MeterTypeCollection");

        public ActionResult MeterTypeCollection()
        {
            ViewData.Model = dataManager.MeterTypeRepos.SelectMeterTypes(x => true);
            return View();
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.MeterTypeRepos.FindMeterType(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.MeterTypeRepos.FindMeterType(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.MeterType city)
        {
            dataManager.MeterTypeRepos.RemoveMeterType(city.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(Models.EDM.MeterType city)
        {
            dataManager.MeterTypeRepos.AddMeterType(city.Name, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.MeterTypeRepos.FindMeterType(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Models.EDM.MeterType city)
        {
            dataManager.MeterTypeRepos.ChangeMeterType(city.Id, city.Name, out string Res);
            return RedirectToAction("Index");
        }
    }
}