using System.Web.Mvc;
using TechPractics2.Models;
using GlobalResources;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System;
using System.Diagnostics;

namespace TechPractics2.Controllers
{

    public class CustomerController : DataController
    {

        public CustomerController(Models.DataManager dataManager): base(dataManager) { }

        public ActionResult Index() => RedirectToAction("CustomerCollection");

        public ActionResult CustomerCollection()
        {

            ViewData.Model = dataManager.CustomerRepos.SelectCustomers(x => true);

            return View();
        }

        public ActionResult Details(int id)
        {
            ViewData.Model = dataManager.CustomerRepos.SelectCustomers(x => x.Id == id).FirstOrDefault();
            if (ViewData.Model == null)
            {
                //Exception???
            }

            return View();

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(int id)
        {
            ViewData.Model = dataManager.CustomerRepos.SelectCustomers(x => x.Id == id).FirstOrDefault();
            if (ViewData.Model == null)
            {
                //Exception???
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, bool IsCompany)
        {
            if (IsCompany)
            {
                dataManager.CompanyRepos.RemoveCompany(id, out string Res);
            }else
            {
                dataManager.CustomerRepos.RemoveCustomer(id, out string Res);
            }

            return RedirectToAction("CustomerCollection");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Add()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(string FIO, string Passport,string PhoneNumber,bool IsCompany ,string CompanyName, string INN)
        {
            Check(FIO, Passport, PhoneNumber, IsCompany, CompanyName,  INN);
            if (ModelState.IsValid)
            {
                if (IsCompany)
                {
                    dataManager.CompanyRepos.AddCompany(FIO, Passport, PhoneNumber, CompanyName, INN, out string Res);
                }else
                {
                    dataManager.CustomerRepos.AddCustomer(FIO, Passport, PhoneNumber, out string Res);
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public void Check(string FIO, string Passport, string PhoneNumber, bool IsCompany, string CompanyName, string INN)
        {
            if (string.IsNullOrWhiteSpace(FIO))
            {
                ModelState.AddModelError(GlobalResources.SiteResources.Customer_FIO, SiteResources.PleaseInput + SiteResources.Customer_FIO + SiteResources.ToCompany);
            }
            else
            if (!Models.Checker.IsFIO(FIO))
            {
                ModelState.AddModelError(GlobalResources.SiteResources.Customer_FIO, SiteResources.Customer_FIO + SiteResources.WrongFormat);
            }
            if (string.IsNullOrWhiteSpace(Passport))
            {
                ModelState.AddModelError(GlobalResources.SiteResources.Customer_Passport, SiteResources.PleaseInput + SiteResources.Customer_Passport);
            }
            else if (!Models.Checker.IsPassportNumber(Passport))
            {
                ModelState.AddModelError(GlobalResources.SiteResources.Customer_Passport, SiteResources.WrongFormat + SiteResources.User_Password + SiteResources.TenNumber);
            }
            if (IsCompany)
            {
                if (string.IsNullOrWhiteSpace(CompanyName))
                {
                    ModelState.AddModelError(GlobalResources.SiteResources.Company_CompanyName, GlobalResources.SiteResources.PleaseInput + SiteResources.CompanyName);
                }// else if (!Checker.IsOKString)
                if (string.IsNullOrWhiteSpace(INN))
                {
                    ModelState.AddModelError(GlobalResources.SiteResources.Company_INN, SiteResources.PleaseInput + SiteResources.Company_INN);
                }
                else if (!Checker.IsINN(INN))
                {
                    ModelState.AddModelError(SiteResources.Company_INN, SiteResources.WrongFormat + SiteResources.Company_INN + SiteResources.TenNumber);
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            ViewData.Model = dataManager.CustomerRepos.SelectCustomers(x => x.Id == id).FirstOrDefault();
            if (ViewData.Model == null)
            {
                //Exception???
            }
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, string FIO, string Passport, string PhoneNumber, bool IsCompany, string CompanyName, string INN)
        {
            Check(FIO, Passport, PhoneNumber, IsCompany, CompanyName, INN);
            if (ModelState.IsValid)
            {
                if (IsCompany)
                {
                    dataManager.CompanyRepos.ChangeCompany(id, FIO, Passport, PhoneNumber, CompanyName, INN,out string Res);
                }else
                {
                    dataManager.CustomerRepos.ChangeCustomer(id, FIO, Passport, PhoneNumber, out string Res);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}