using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechPractics2.Models.UtilityModels
{
    public class OperatorViewModel
    {
        [Display(Name = "FullAddress",ResourceType = typeof(GlobalResources.SiteResources))]
        public string FullAddress { get; set; }
        [Display(Name = "Address_Flat", ResourceType = typeof(GlobalResources.SiteResources))]
        public int Flat { get; set; }
        [Display(Name = "Customer_FIO", ResourceType = typeof(GlobalResources.SiteResources))]
        public string FIO { get; set; }
        [Display(Name = "Customer_Passport", ResourceType = typeof(GlobalResources.SiteResources))]
        public string Passport { get; set; }
        [Display(Name = "Customer_PhoneNumber", ResourceType = typeof(GlobalResources.SiteResources))]
        public string PhoneNumber { get; set; }
        [Display(Name = "Company", ResourceType = typeof(GlobalResources.SiteResources))]
        public bool IsCompany { get; set; }
        [Display(Name = "Company_CompanyName", ResourceType = typeof(GlobalResources.SiteResources))]
        public string CompanyName { get; set; }
        [Display(Name = "Company_INN", ResourceType = typeof(GlobalResources.SiteResources))]
        public string INN { get; set; }
        
        public List<int> MetersCounts { get; set; }

    }

    public class MakeOrderViewModel : OperatorViewModel
    {
        public bool UseProfile { get; set; }
        public int ProfileId { get; set; }
    }

    public class AnaliticModel
    {
        public string MType { get; set; }
        public string Exp { get; set; }
    }
}