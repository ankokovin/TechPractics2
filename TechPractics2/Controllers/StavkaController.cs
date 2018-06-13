using System.Web.Mvc;
using System.Linq;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{

    public class StavkaController : DataController
    {
        public StavkaController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("StavkaCollection");

        public ActionResult StavkaCollection()
        {
            ViewData.Model = dataManager.StavkaRepos.SelectStavkas(x => true);
            return View();
        }

        public void Check(int PersonId, int MeterTypeId)
        {
            if (PersonId < 0)
                ModelState.AddModelError("PersonId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Stavka_Person);
            if (MeterTypeId < 0)
                ModelState.AddModelError("MeterTypeId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Stavka_MeterType);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Persones = dataManager.PersonRepos.SelectPersons(x => true);
            var MeterTypes = dataManager.MeterTypeRepos.SelectMeterTypes(x => true);

            ViewData[GlobalResources.SiteResources.Stavka_Person] = new SelectList(Persones, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.Stavka_MeterType] = new SelectList(MeterTypes, "Id", "FullDisc");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(int PersonId, int MeterTypeId)
        {
            Check(PersonId, MeterTypeId);
            if (ModelState.IsValid)
            {
                dataManager.StavkaRepos.AddStavka(
                    dataManager.MeterTypeRepos.SelectMeterTypes(x => x.Id == MeterTypeId).FirstOrDefault(),
                    dataManager.PersonRepos.FindPerson(PersonId),
                    out string Res
                    );
                return RedirectToAction("Index");
            }
            var Persones = dataManager.PersonRepos.SelectPersons(x => true);
            var MeterTypes = dataManager.MeterTypeRepos.SelectMeterTypes(x => true);
            ViewData[GlobalResources.SiteResources.Stavka_Person] = new SelectList(Persones, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.Stavka_MeterType] = new SelectList(MeterTypes, "Id", "FullDisc");
            return View();
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.StavkaRepos.FindStavka(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.StavkaRepos.FindStavka(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Stavka order)
        {
            dataManager.StavkaRepos.RemoveStavka(order.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.StavkaRepos.FindStavka(id);
            var Persones = dataManager.PersonRepos.SelectPersons(x => true);
            var MeterTypes = dataManager.MeterTypeRepos.SelectMeterTypes(x => true);
            ViewData[GlobalResources.SiteResources.Stavka_Person] = new SelectList(Persones, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.Stavka_MeterType] = new SelectList(MeterTypes, "Id", "FullDisc");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, int PersonId, int MeterTypeId)
        {
            Check(PersonId, MeterTypeId);
            if (ModelState.IsValid)
            {
                dataManager.StavkaRepos.ChangeStavka(id,
                    dataManager.MeterTypeRepos.FindMeterType(MeterTypeId),
                    dataManager.PersonRepos.FindPerson(PersonId),
                    out string Res
                    );
                return RedirectToAction("Index");
            }
            ViewData.Model = dataManager.StavkaRepos.FindStavka(id);
            var Persones = dataManager.PersonRepos.SelectPersons(x => true);
            var MeterTypes = dataManager.MeterTypeRepos.SelectMeterTypes(x => true);
            ViewData[GlobalResources.SiteResources.Stavka_Person] = new SelectList(Persones, "Id", "FullDisc");
            ViewData[GlobalResources.SiteResources.Stavka_MeterType] = new SelectList(MeterTypes, "Id", "FullDisc");
            return View();
        }
    }
}