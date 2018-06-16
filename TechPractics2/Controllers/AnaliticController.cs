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

        public static MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }

        public ActionResult ExportToExcelQuery(){
            ExportToExcel<Models.EDM.OrderEntry>("OrderEntry",this,dataManager);
            var url = System.Web.HttpContext.Current.Request.UrlReferrer;
            if (url != null)
            {
                return Redirect(url.AbsolutePath);
            }
            return RedirectToAction("Index");
        }

        public static void ExportToExcel<T>(string workSheetName,Controller controller, DataManager dataManager)
        {

            //gv.DataSource = dataManager.OrderEntryRepos.SelectOrderEntrys(x=>true).Select(x=>_OrderEntry.Trans(x));



            var select = dataManager.GetSelect<T>();
            var dataTable = Repos<T>.Table(select.Select(x=>true));

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
    }
}