using System;
using System.Collections.Generic;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class OrderEntryRepos : Repos
    {
        public OrderEntryRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {
            
        }
        public bool AddOrderEntry(Order order, DateTime? startTime, DateTime? endTime
           , string RegNumber, Meter meter, Person person, Status status, out string Res, bool save = true)
        {
            try
            {
                if (person == null || (from s in person.Stavka where s.MeterType == meter.MeterType select s).Any())
                {
                    OrderEntry orderEntry = new OrderEntry();
                    orderEntry.Order = order;
                    orderEntry.StartTime = startTime;
                    orderEntry.EndTime = endTime;
                    orderEntry.Meter = meter;
                    orderEntry.RegNumer = RegNumber;
                    orderEntry.Status = status;
                    orderEntry.Person = person;
                    cont.OrderEntrySet.Add(orderEntry);
                    if (save) cont.SaveChanges();
                    Res = "Успешное добавление заказа";
                    return true;
                }
                else
                {
                    Res = "Данный работник не имеет необходимой ставки";
                    return false;
                }
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }

        public bool ChangeOrderEntry(int Id, Order order, DateTime? startTime, DateTime? endTime,
             string RegNumber, Meter meter, Person person, Status status, out string Res, bool save = true)
        {
            try
            {
                var a = FindOrderEntry(Id);
                if (a == null)
                {
                    Res = "Нет заказной позиции с данным идентификационным номером";
                    return false;
                }
                if ((from s in person.Stavka where s.MeterType == meter.MeterType select s).Any())
                {
                    a.Meter = meter;
                    a.Order = order;
                    a.RegNumer = RegNumber;
                    a.StartTime = startTime;
                    a.EndTime = endTime;
                    a.Status = status;
                    a.Person = person;
                    if (save) cont.SaveChanges();
                    Res = "Изменение заказной позиции успешно";
                    return true;
                }
                else
                {
                    Res = "Данный работник не имеет необходимой ставки";
                    return false;
                }
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }

        public bool RemoveOrderEntry(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var a = FindOrderEntry(id);
                if (a == null)
                {
                    Res = "Нет заказной позиции с таким идентификационным номером";
                    return false;
                }
                cont.OrderEntrySet.Remove(a);
                if (save) cont.SaveChanges();
                Res = "Успешное удаление заказной позиции";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }

        public OrderEntry FindOrderEntry(int id) => (from o in cont.OrderEntrySet where o.Id == id select o).FirstOrDefault();

        public IEnumerable<OrderEntry> SelectOrderEntrys(Func<OrderEntry, bool> predicate) => cont.OrderEntrySet.Where(predicate).AsParallel();
    }
}