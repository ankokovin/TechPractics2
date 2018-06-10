using System.Web.Mvc;

namespace TechPractics2.Controllers
{
    public class DataController : Controller
    {
        protected Models.DataManager dataManager;
        public DataController(Models.DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
    }
}