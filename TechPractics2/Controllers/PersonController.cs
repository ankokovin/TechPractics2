using System.Web.Mvc;
using TechPractics2.Models;

namespace TechPractics2.Controllers
{
    public class PersonController : DataController
    {
        public PersonController(DataManager dataManager) : base(dataManager) { }

        public ActionResult Index() => RedirectToAction("PersonCollection");

        public ActionResult PersonCollection()
        {
            ViewData.Model = dataManager.PersonRepos.SelectPersons(x => true);
            return View();
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.PersonRepos.FindPerson(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.PersonRepos.FindPerson(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Person person)
        {
            dataManager.PersonRepos.RemovePerson(person.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            return View();
        }

        public void Check(Models.EDM.Person person)
        {
            if (string.IsNullOrWhiteSpace(person.FIO))
                ModelState.AddModelError("FIO", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Person_FIO);
            else if (!Checker.IsFIO(person.FIO))
                ModelState.AddModelError("FIO", GlobalResources.SiteResources.WrongFormat + GlobalResources.SiteResources.Person_FIO);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(Models.EDM.Person person)
        {
            Check(person);
            if (ModelState.IsValid)
            {
            dataManager.PersonRepos.AddPerson(person.FIO, out string Res);
            return RedirectToAction("Index");
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.PersonRepos.FindPerson(id);
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Models.EDM.Person person)
        {
            Check(person);
            if (ModelState.IsValid)
            {
                dataManager.PersonRepos.ChangePerson(person.Id, person.FIO, out string Res);
                return RedirectToAction("Index");
            }
            ViewData.Model = dataManager.PersonRepos.FindPerson(person.Id);
            return View();
        }
    }
}