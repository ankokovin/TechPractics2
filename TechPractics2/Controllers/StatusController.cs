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
            ViewData.Model = dataManager.StatusRepos.SelectStatuss(x => true);

            return View();
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.StatusRepos.FindStatus(id);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.StatusRepos.FindStatus(id);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Status status)
        {
            dataManager.StatusRepos.RemoveStatus(status.Id, out string Res);

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
            dataManager.StatusRepos.AddStatus(status.Name, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.StatusRepos.FindStatus(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Models.EDM.Status status)
        {
            dataManager.StatusRepos.ChangeStatus(status.Id, status.Name, out string Res);
            return View();
        }
    }
}