using System.Web.Mvc;
using TechPractics2.Models;
using System;

namespace TechPractics2.Controllers
{
    public class OrderEntryController : DataController
    {
        public OrderEntryController(DataManager dataManager): base(dataManager) { }

        public ActionResult Index() => RedirectToAction("OrderEntryCollection");

        public ActionResult OrderEntryCollection()
        {
            ViewData.Model = dataManager.OrderEntryRepos.SelectOrderEntrys(x => true);
            return View();
        }

        public void Check(DateTime? startTime, DateTime? endTime, string RegNum, int OrderId, int MeterId, int StatusId, bool HasPerson, int PersonId)
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
            if (HasPerson && PersonId < 0)
                ModelState.AddModelError("PersonId", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.OrderEntry_Person);
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.OrderEntryRepos.FindOrderEntry(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.OrderEntryRepos.FindOrderEntry(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Models.EDM.OrderEntry orderEntry)
        {
            dataManager.OrderEntryRepos.RemoveOrderEntry(orderEntry.Id, out string Res);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            var Orders = dataManager.OrderRepos.SelectOrders(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Order] = new SelectList(Orders, "Id", "Id");
            var Meters = dataManager.MeterRepos.SelectMeters(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Meter] = new SelectList(Meters, "Id", "Name");
            var Statuss = dataManager.StatusRepos.SelectStatuss(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Status] = new SelectList(Statuss, "Id", "Name");
            var Persons = dataManager.PersonRepos.SelectPersons(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Person] = new SelectList(Persons, "Id", "FIO");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(DateTime? startTime, DateTime? endTime, string RegNum, int OrderId, int MeterId, int StatusId, bool HasPerson, int PersonId)
        {
            Check(startTime, endTime, RegNum, OrderId, MeterId, StatusId, HasPerson, PersonId);
            if (ModelState.IsValid)
            {
                dataManager.OrderEntryRepos.AddOrderEntry(
                    dataManager.OrderRepos.FindOrder(OrderId),
                    startTime,
                    endTime,
                    RegNum,
                    dataManager.MeterRepos.FindMeter(MeterId),
                    HasPerson ? dataManager.PersonRepos.FindPerson(PersonId) : null,
                    dataManager.StatusRepos.FindStatus(StatusId),
                    out string Res
                    );
                return RedirectToAction("Index");
            }

            var Orders = dataManager.OrderRepos.SelectOrders(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Order] = new SelectList(Orders, "Id", "Id");
            var Meters = dataManager.MeterRepos.SelectMeters(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Meter] = new SelectList(Meters, "Id", "Name");
            var Statuss = dataManager.StatusRepos.SelectStatuss(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Status] = new SelectList(Statuss, "Id", "Name");
            var Persons = dataManager.PersonRepos.SelectPersons(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Person] = new SelectList(Persons, "Id", "FIO");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {

            var Orders = dataManager.OrderRepos.SelectOrders(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Order] = new SelectList(Orders, "Id", "Id");
            var Meters = dataManager.MeterRepos.SelectMeters(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Meter] = new SelectList(Meters, "Id", "Name");
            var Statuss = dataManager.StatusRepos.SelectStatuss(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Status] = new SelectList(Statuss, "Id", "Name");
            var Persons = dataManager.PersonRepos.SelectPersons(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Person] = new SelectList(Persons, "Id", "FIO");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int Id, DateTime? startTime, DateTime? endTime, string RegNum, int OrderId, int MeterId, int StatusId, bool HasPerson, int PersonId)
        {
            Check(startTime, endTime, RegNum, OrderId, MeterId, StatusId, HasPerson, PersonId);
            if (ModelState.IsValid)
            {
                dataManager.OrderEntryRepos.ChangeOrderEntry(
                    Id,
                    dataManager.OrderRepos.FindOrder(OrderId),
                    startTime,
                    endTime,
                    RegNum,
                    dataManager.MeterRepos.FindMeter(MeterId),
                    HasPerson ? dataManager.PersonRepos.FindPerson(PersonId) : null,
                    dataManager.StatusRepos.FindStatus(StatusId),
                    out string Res
                    );
                return RedirectToAction("Index");
            }
            var Orders = dataManager.OrderRepos.SelectOrders(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Order] = new SelectList(Orders, "Id", "Id");
            var Meters = dataManager.MeterRepos.SelectMeters(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Meter] = new SelectList(Meters, "Id", "Name");
            var Statuss = dataManager.StatusRepos.SelectStatuss(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Status] = new SelectList(Statuss, "Id", "Name");
            var Persons = dataManager.PersonRepos.SelectPersons(x => true);
            ViewData[GlobalResources.SiteResources.OrderEntry_Person] = new SelectList(Persons, "Id", "FIO");
            return View();
        }
    }
}