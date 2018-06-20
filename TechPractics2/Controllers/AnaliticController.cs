using ClosedXML.Excel;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TechPractics2.Models;
using System.Web.UI;
using System.Linq;
using System.Data;
using System;
using TechPractics2.Models.Repos;
using System.Reflection;
using System.Linq.Expressions;
using LinqKit;
using System.Diagnostics;

namespace TechPractics2.Controllers
{
    public class AnaliticController : DataController
    {
        public AnaliticController(DataManager dataManager) : base(dataManager) { }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(object arg)
        {
            return View();
        }

       

        public ActionResult ExportToExcelQuery(){
            ExportToExcel<Models.EDM.Address>("OrderEntry",this,dataManager);
            var url = System.Web.HttpContext.Current.Request.UrlReferrer;
            if (url != null)
            {
                return Redirect(url.AbsolutePath);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Query(Models.UtilityModels.AnaliticModel model)
        {
            try
            {
                model.Exp = model.Exp.Substring(0, model.Exp.Length - 1);
                switch (model.MType)
                {
                    case "OrderEntry":
                        var predOrderEntry = Repos<Models.EDM.OrderEntry>.CreatePredicate(model.Exp);
                        ExportToExcel("OrderEntry", dataManager.cont.OrderEntrySet.AsExpandable().Where(predOrderEntry), this);
                        break;
                    case "Order":
                        var predOrder = Repos<Models.EDM.Order>.CreatePredicate(model.Exp);
                        ExportToExcel("Order", dataManager.cont.OrderSet.AsExpandable().Where(predOrder), this);
                        break;
                    case "Customer":
                        var predCustomer = Repos<Models.EDM.Customer>.CreatePredicate(model.Exp);
                        ExportToExcel("Customer", dataManager.cont.CustomerSet.AsExpandable().Where(predCustomer), this);
                        break;
                    case "Address":
                        var predAddress = Repos<Models.EDM.Address>.CreatePredicate(model.Exp);
                        ExportToExcel("Address", dataManager.cont.AddressSet.AsExpandable().Where(predAddress), this);
                        break;
                    case "House":
                        var predHouse = Repos<Models.EDM.House>.CreatePredicate(model.Exp);
                        ExportToExcel("House", dataManager.cont.HouseSet.AsExpandable().Where(predHouse), this);
                        break;
                    case "Street":
                        var predStreet = Repos<Models.EDM.Street>.CreatePredicate(model.Exp);
                        ExportToExcel("Street", dataManager.cont.StreetSet.AsExpandable().Where(predStreet), this);
                        break;
                    case "City":
                        var predCity = Repos<Models.EDM.City>.CreatePredicate(model.Exp);
                        ExportToExcel("City", dataManager.cont.CitySet.AsExpandable().Where(predCity), this);
                        break;
                    case "User":
                        var predUser = Repos<Models.EDM.User>.CreatePredicate(model.Exp);
                        ExportToExcel("User", dataManager.cont.UserSet.AsExpandable().Where(predUser), this);
                        break;
                    case "Meter":
                        var predMeter = Repos<Models.EDM.Meter>.CreatePredicate(model.Exp);
                        ExportToExcel("Meter", dataManager.cont.MeterSet.AsExpandable().Where(predMeter), this);
                        break;
                    case "MeterType":
                        var predMeterType = Repos<Models.EDM.MeterType>.CreatePredicate(model.Exp);
                        ExportToExcel("MeterType", dataManager.cont.MeterTypeSet.AsExpandable().Where(predMeterType), this);
                        break;
                    case "Status":
                        var predStatus = Repos<Models.EDM.Status>.CreatePredicate(model.Exp);
                        ExportToExcel("Status", dataManager.cont.StatusSet.AsExpandable().Where(predStatus), this);
                        break;
                }
                
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return View();
        }

        public static void ExportToExcel<T>(string workSheetName, IEnumerable<T> ts, Controller controller)
        {
            var dataTable = Repos<T>.Table(ts);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable, workSheetName);

                //wb.SaveAs(folderPath + "DataGridViewExport.xlsx");
                string myName = controller.Server.UrlEncode("Test" + "_" +
                DateTime.Now.ToShortDateString() + ".xlsx");
                MemoryStream stream = GetStream(wb);// The method is defined below
                controller.Response.Clear();
                controller.Response.Buffer = true;
                controller.Response.AddHeader("content-disposition",
                "attachment; filename=" + myName);
                controller.Response.ContentType = "application/vnd.ms-excel";
                controller.Response.BinaryWrite(stream.ToArray());
                controller.Response.End();
            }
        }

        public static void ExportToExcel<T>(string workSheetName,Controller controller, DataManager dataManager)
        {
            
            var select = dataManager.GetSelect<T>();
            var dataTable = Repos<T>.Table(select.Select(x=>true));

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable, workSheetName);
                
                string myName = controller.Server.UrlEncode("Test" + "_" +
                DateTime.Now.ToShortDateString() + ".xlsx");
                MemoryStream stream = GetStream(wb);// The method is defined below
                controller.Response.Clear();
                controller.Response.Buffer = true;
                controller.Response.AddHeader("content-disposition",
                "attachment; filename=" + myName);
                controller.Response.ContentType = "application/vnd.ms-excel";
                controller.Response.BinaryWrite(stream.ToArray());
                controller.Response.End();
            }
            
        }

        public static MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
    }
}