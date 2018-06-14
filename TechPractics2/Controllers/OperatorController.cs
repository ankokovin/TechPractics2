using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechPractics2.Models;
using GlobalResources;

namespace TechPractics2.Controllers
{
    public class OperatorController : DataController
    {
        public OperatorController(DataManager dataManager) : base(dataManager) { }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {

            ViewData[GlobalResources.SiteResources.Meter] = dataManager.MeterRepos.SelectMeters(x => true);
            int count = (ViewData[GlobalResources.SiteResources.Meter] as IEnumerable<Models.EDM.Meter>).Count();
            Models.UtilityModels.OperatorViewModel operatorViewModel = new Models.UtilityModels.OperatorViewModel();
            ViewData.Model = operatorViewModel;


            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(Models.UtilityModels.OperatorViewModel operatorViewModel)
        {
            string City, House, Street;
            Check(operatorViewModel.Flat, operatorViewModel.FIO, operatorViewModel.Passport, 
                operatorViewModel.PhoneNumber, operatorViewModel.IsCompany, operatorViewModel.CompanyName, operatorViewModel.INN,
                operatorViewModel.FullAddress);
            if (dataManager.AddressRepos.ParseAddress(operatorViewModel.FullAddress, out City, out Street, out House) && ModelState.IsValid)
            {
                Models.EDM.City city = dataManager.CityRepos.SelectCitys(x => x.Name == City).FirstOrDefault();
                if (city == null)
                {
                    throw new Exception("Данный город не поддерживается :(");
                }
                Models.EDM.Street street = dataManager.StreetRepos.SelectStreets(x => x.Name == Street && x.City.Id == city.Id).FirstOrDefault();
                if (street == null)
                {
                    dataManager.StreetRepos.AddStreet(Street, city, out string Res);
                    street = dataManager.StreetRepos.SelectStreets(x => x.Name == Street && x.City.Id == city.Id).FirstOrDefault();
                }
                Models.EDM.House house = dataManager.HouseRepos.SelectHouses(x => x.Number == House && x.Street.Id == street.Id).FirstOrDefault();
                if (house == null)
                {
                    dataManager.HouseRepos.AddHouse(House, street, out string Res);
                    house = dataManager.HouseRepos.SelectHouses(x => x.Number == House && x.Street.Id == street.Id).FirstOrDefault();
                }
                Models.EDM.Address address = dataManager.AddressRepos.SelectAddresss(x => x.Flat == operatorViewModel.Flat && x.House.Id == house.Id).FirstOrDefault();
                if (address == null)
                {
                    dataManager.AddressRepos.AddAddress(operatorViewModel.Flat, house, out string Res);
                    address = dataManager.AddressRepos.SelectAddresss(x => x.Flat == operatorViewModel.Flat && x.House.Id == house.Id).FirstOrDefault();
                }
                Models.EDM.Customer customer;
                if (operatorViewModel.IsCompany)
                {
                    customer = dataManager.CustomerRepos.SelectCustomers(x =>(x is Models.EDM.Company)
                    && x.Passport==operatorViewModel.Passport
                    && ((Models.EDM.Company)x).INN == operatorViewModel.INN).FirstOrDefault();
                }
                else
                {
                    customer = dataManager.CustomerRepos.SelectCustomers(x => x.Passport == operatorViewModel.Passport 
                    && !(x is Models.EDM.Company)).FirstOrDefault();
                }
                if (customer == null)
                {
                    if (operatorViewModel.IsCompany)
                    {
                        dataManager.CompanyRepos.AddCompany(operatorViewModel.FIO, operatorViewModel.Passport, operatorViewModel.PhoneNumber,
                            operatorViewModel.CompanyName, operatorViewModel.INN, out string Res);
                        customer = dataManager.CustomerRepos.SelectCustomers(x => (x is Models.EDM.Company)
                             && x.Passport == operatorViewModel.Passport
                             && ((Models.EDM.Company)x).INN == operatorViewModel.INN).FirstOrDefault();
                    }else
                    {
                        dataManager.CustomerRepos.AddCustomer(operatorViewModel.FIO, operatorViewModel.Passport, operatorViewModel.PhoneNumber,
                            out string Res);
                        customer = dataManager.CustomerRepos.SelectCustomers(x => x.Passport == operatorViewModel.Passport
                            && !(x is Models.EDM.Company)).FirstOrDefault();
                    }
                }
                Models.EDM.User user = Session[GlobalResources.SiteResources.User] as Models.EDM.User;
                dataManager.OrderRepos.AddOrder(
                    dataManager.UsersRepos.FindUser(user.Id),
                    dataManager.CustomerRepos.SelectCustomers(x=>x.Id==customer.Id).First(), 
                    dataManager.AddressRepos.FindAddress(address.Id), 
                    out string res, out int result);
                var order = dataManager.OrderRepos.FindOrder(result);
                var status = dataManager.StatusRepos.FindStatus(1);
                if (operatorViewModel.MetersCounts != null)
                {
                    var Meters = dataManager.MeterRepos.SelectMeters(x => true).ToArray();
                    for (int i = 0; i < operatorViewModel.MetersCounts.Count; i++)
                    {
                        for (int j = 0; j < operatorViewModel.MetersCounts[i]; ++j)
                        {
                            dataManager.OrderEntryRepos.AddOrderEntry(order, 
                                DateTime.Now,
                                null,
                                null,
                                dataManager.MeterRepos.FindMeter(Meters[i].Id),
                                null,
                                status,
                                out string Res);
                        }
                    }
                }
            }
            ViewData[GlobalResources.SiteResources.Meter] = dataManager.MeterRepos.SelectMeters(x => true);
            int count = (ViewData[GlobalResources.SiteResources.Meter] as IEnumerable<Models.EDM.Meter>).Count();
            return View();
        }

        public void Check(int Flat, string FIO,string Passport, string PhoneNumber, bool IsCompany, string CompanyName, string INN, string FullAddress)
        {
            if (string.IsNullOrWhiteSpace(FullAddress))
            {
                ModelState.AddModelError("FullAddress", SiteResources.PleaseInput + " полный адрес");
            }
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
            if (Flat < 0)
                ModelState.AddModelError("Flat", GlobalResources.SiteResources.PleaseInput + GlobalResources.SiteResources.Address_Flat);
        }
    }
}