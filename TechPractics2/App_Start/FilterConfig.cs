﻿using System.Web;
using System.Web.Mvc;

namespace TechPractics2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleCustomError());
        }
    }
}
