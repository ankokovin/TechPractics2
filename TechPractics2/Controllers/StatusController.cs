using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{


    public class StatusController : DataController
    {
        public StatusController(DataManager dataManager): base(dataManager) { }

        public ActionResult Index() => RedirectToAction("StatusCollection");

        public ActionResult StatusCollection()
        {
            ViewData.Model = dataManager.StatusRepos.Select(x => true);

            return View();
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.StatusRepos.Find(id);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.StatusRepos.Find(id);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Status status)
        {
            dataManager.StatusRepos.Remove(status.Id, out string Res);

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(Models.EDM.Status status)
        {
            dataManager.StatusRepos.Add(status.Name, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.StatusRepos.Find(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Models.EDM.Status status)
        {
            dataManager.StatusRepos.Change(status.Id, status.Name, out string Res);
            return View();
        }

        public ActionResult ExcelExport()
        {
            AnaliticController.ExportToExcel<Models.EDM.Status>("Status", this, dataManager);
            return RedirectToAction("Index");
        }
    }
}