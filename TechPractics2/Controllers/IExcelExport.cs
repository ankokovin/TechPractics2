using System.Web.Mvc;

namespace TechPractics2.Controllers
{
    public interface IExcelExport
    {
        ActionResult ExcelExport();
    }
}