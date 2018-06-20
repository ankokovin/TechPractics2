using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TechPractics2.Models.UtilityModels
{
    public class OperatorViewModel
    {
        public string FullAddress { get; set; }
        public int Flat { get; set; }
        public string FIO { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsCompany { get; set; }
        public string CompanyName { get; set; }
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