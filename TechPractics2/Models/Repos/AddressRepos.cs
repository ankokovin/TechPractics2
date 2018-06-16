using System;
using System.Collections.Generic;
using System.Linq;
using TechPractics2.Models.EDM;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq.Expressions;
using LinqKit;

namespace TechPractics2.Models.Repos
{
    public class AddressRepos : Repos<Address>
    {
        public bool ParseAddress(string FullAddress, out string City, out string Street, out string House)
        {
            var res = (new Regex(", ").Split(FullAddress));
            if (res[0] != "Россия")
                throw new Exception(GlobalResources.SiteResources.NotRussia);
            int i = res.Length == 4 ? 0 : 1;
            City = res[i+1];
            Street = res[i+2];
            House = res[i+3];
            return true;
        }


        public AddressRepos(Model1Container model, bool checkInputs = true, bool allowCascade = false): base(model, checkInputs, allowCascade)
        {
          
        }
        /// <summary>
        /// Функция добавления адреса
        /// </summary>
        /// <param name="Flat">Номер квартиры</param>
        /// <param name="house">Дом</param>
        /// <param name="Res">Сообщение результата добавления</param>
        /// <returns>Результат добавления</returns>
        public bool Add(int Flat, House house, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.AddressSet where a.House.Id == house.Id && a.Flat == Flat select a).FirstOrDefault() != null)
                {
                    Res = "Уже есть данный адрес";
                    return false;
                }
                Address address = new Address();
                address.Flat = Flat;
                address.House = house;
                cont.AddressSet.Add(address);
                if (save) cont.SaveChanges();
                Res = "Успешное добавление";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }
        /// <summary>
        /// Функция изменения адреса
        /// </summary>
        /// <param name="id">Идентификационный номер адреса</param>
        /// <param name="Flat">Номер квартиры</param>
        /// <param name="house">Дом</param>
        /// <param name="Res">Сообщение результата изменения</param>
        /// <returns>Результат изменения</returns>
        public bool Change(int id, int Flat, House house, out string Res, bool save = true)
        {
            try
            {
                if ((from h in cont.AddressSet where h.Id != id && h.House.Id == house.Id && h.Flat == Flat select h).FirstOrDefault() != null)
                {
                    Res = "В городе " + house.Street.City + " на улице " + house.Street +
                        " в доме номер " + house.Number + " уже есть квартира номер " + Flat;
                    return false;
                }
                var a = Find(id);
                if (a == null)
                {
                    Res = "Нет адреса с данным идентификационным номером";
                    return false;
                }
                a.Flat = Flat;
                a.House = house;
                if (save) cont.SaveChanges();
                Res = "Изменение дома успешно";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }
        /// <summary>
        /// Функция удаления адреса по идентификационному номеру
        /// </summary>
        /// <param name="id">Идентификационный номер адреса</param>
        /// <param name="Res">Сообщение результата удаления</param>
        /// <returns>Результат удаления</returns>
        public bool Remove(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var a = Find(id);
                if (a == null)
                {
                    Res = "Нет адреса с таким идентификационным номером";
                    return false;
                }
                if (a.Order.Count == 0 || !check || AllowCascade)
                {
                    cont.AddressSet.Remove(a);
                    if (save) cont.SaveChanges();
                    Res = "Успешное удаления квартиры номер" + a.Flat + "дома номер " + a.House.Number +
                    " улицы " + a.House.Street + " города " + a.House.Street.City;
                    return true;
                }
                else
                {
                    Res = "Удаление отменено";
                    return false;
                }
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }
        /// <summary>
        /// Функция поиска адреса по идентификационному ключу
        /// </summary>
        /// <param name="id">Идентификационный ключ</param>
        /// <returns>Адрес</returns>
        public Address Find(int id) => (from a in cont.AddressSet where a.Id == id select a).FirstOrDefault();


        public override IEnumerable<Address> Select(Func<Address, bool> predicate) =>  cont.AddressSet.Where(predicate).AsParallel();

       
        

        /*
        public override DataTable table(IEnumerable<Address> addresses,string[] Selectors)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("№",typeof()))
        }*/
    }
}