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
    
    public partial class Order
    {
        public Order()
        {
            this.OrderEntry = new HashSet<OrderEntry>();
        }
    
        public int Id { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderEntry> OrderEntry { get; set; }
        public virtual Address Address { get; set; }
    }
}