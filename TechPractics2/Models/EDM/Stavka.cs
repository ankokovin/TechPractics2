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
    
    public partial class Stavka
    {
        public int Id { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual MeterType MeterType { get; set; }
    }
}
