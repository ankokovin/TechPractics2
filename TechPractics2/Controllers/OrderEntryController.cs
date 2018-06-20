using System.Web.Mvc;
using TechPractics2.Models;
using System;
using System.Collections.Generic;

namespace TechPractics2.Controllers
{
    public class OrderEntryController : DataController
    {
        public OrderEntryController(DataManager dataManager): base(dataManager) { }

        public ActionResult Index() => RedirectToAction("OrderEntryCollection");

        public ActionResult OrderEntryCollection()
        {
            ViewData.Model = dataManager.OrderEntryRepos.Select(x => true);
            return View();
        }

        public ActionResult Selection(IList<Models.EDM.OrderEntry> collection)
        {
            ViewData.Model = collection;
            return View();
        }

        public void Check(DateTime? startTime, DateTime? endTime, string RegNum, int OrderId, int MeterId, int StatusId, int? PersonId)
        {
            if (endTime!=null && startTime == null)
            {
                ModelState.AddModelError("startTime", "TODO");
            }
            if (startTime > endTime)
            {
                ModelState.AddModelError("startTime", "TODO2");
            }
             if (!string.IsNullOrWhiteSpace(RegNum) && !Checker.IsNumber(RegNum))
                ModelState.AddModelError("RegNum", GlobalResources.SiteResources.WrongFormat + GlobalResources.SiteResources.OrderEntry_RegNum);
            if (OrderId < 0)
                ModelState.AddModelError("OrderId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.OrderEntry_Order);
            if (MeterId < 0)
                ModelState.AddModelError("MeterId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.OrderEntry_Meter);
            if (StatusId < 0)
                ModelState.AddModelError("StatusId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.OrderEntry_Status);
            if (PersonId < 0)
                ModelState.AddModelError("PersonId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.OrderEntry_Person);
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.OrderEntryRepos.Find(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.OrderEntryRepos.Find(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.Address orderEntry)
        {
            dataManager.OrderEntryRepos.Remove(orderEntry.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Orders = dataManager.OrderRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Order] = new SelectList(Orders, "Id", "Id");
            var Meters = dataManager.MeterRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Meter] = new SelectList(Meters, "Id", "Name");
            var Statuss = dataManager.StatusRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Status] = new SelectList(Statuss, "Id", "Name");
            var Persons = dataManager.PersonRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Person] = new SelectList(Persons, "Id", "FIO");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(DateTime? startTime, DateTime? endTime, string RegNumer, int OrderId, int MeterId, int StatusId,bool HasPerson, int? PersonId)
        {
            Check(startTime, endTime, RegNumer, OrderId, MeterId, StatusId, PersonId);
            if (ModelState.IsValid)
            {
                dataManager.OrderEntryRepos.Add(
                    dataManager.OrderRepos.Find(OrderId),
                    startTime,
                    endTime,
                    RegNumer,
                    dataManager.MeterRepos.Find(MeterId),
                    PersonId!=null&&HasPerson ? dataManager.PersonRepos.Find((int)PersonId) : null,
                    dataManager.StatusRepos.Find(StatusId),
                    out string Res
                    );
                return RedirectToAction("Index");
            }

            var Orders = dataManager.OrderRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Order] = new SelectList(Orders, "Id", "Id");
            var Meters = dataManager.MeterRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Meter] = new SelectList(Meters, "Id", "Name");
            var Statuss = dataManager.StatusRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Status] = new SelectList(Statuss, "Id", "Name");
            var Persons = dataManager.PersonRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Person] = new SelectList(Persons, "Id", "FIO");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {

            var Orders = dataManager.OrderRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Order] = new SelectList(Orders, "Id", "Id");
            var Meters = dataManager.MeterRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Meter] = new SelectList(Meters, "Id", "Name");
            var Statuss = dataManager.StatusRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Status] = new SelectList(Statuss, "Id", "Name");
            var Persons = dataManager.PersonRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Person] = new SelectList(Persons, "Id", "FIO");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int Id, DateTime? startTime, DateTime? endTime, string RegNumer, int OrderId, int MeterId, int StatusId,bool HasPerson, int? PersonId)
        {
            Check(startTime, endTime, RegNumer, OrderId, MeterId, StatusId, PersonId);
            if (ModelState.IsValid)
            {
                dataManager.OrderEntryRepos.Change(
                    Id,
                    dataManager.OrderRepos.Find(OrderId),
                    startTime,
                    endTime,
                    RegNumer,
                    dataManager.MeterRepos.Find(MeterId),
                    PersonId!=null&&HasPerson ? dataManager.PersonRepos.Find((int)PersonId) : null,
                    dataManager.StatusRepos.Find(StatusId),
                    out string Res
                    );
                return RedirectToAction("Index");
            }
            var Orders = dataManager.OrderRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Order] = new SelectList(Orders, "Id", "Id");
            var Meters = dataManager.MeterRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Meter] = new SelectList(Meters, "Id", "Name");
            var Statuss = dataManager.StatusRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Status] = new SelectList(Statuss, "Id", "Name");
            var Persons = dataManager.PersonRepos.Select(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Person] = new SelectList(Persons, "Id", "FIO");
            return View();
        }

        public ActionResult ExcelExport()
        {
            AnaliticController.ExportToExcel<Models.EDM.OrderEntry>("OrderEntry",(IEnumerable<Models.EDM.OrderEntry>)ViewData.Model,this);
            return RedirectToAction("Index");
        }
    }
}