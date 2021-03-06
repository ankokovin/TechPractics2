//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TechPractics2.Models.EDM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class OrderEntry
    {
        public int Id { get; set; }
        public string RegNumer { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartTime { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> PersonId { get; set; }
        [Display(Name = "OrderEntry_Order", ResourceType = typeof(GlobalResources.SiteResources))]
        public virtual Order Order { get; set; }
        [Display(Name = "OrderEntry_Meter", ResourceType = typeof(GlobalResources.SiteResources))]
        public virtual Meter Meter { get; set; }
        [Display(Name = "OrderEntry_Status", ResourceType = typeof(GlobalResources.SiteResources))]
        public virtual Status Status { get; set; }
        [Display(Name = "OrderEntry_Person", ResourceType = typeof(GlobalResources.SiteResources))]
        public virtual Person Person { get; set; }
    }
}
