using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class OrderRepos : Repos<Order>
    {
      
        public OrderRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {
           
        }
        public bool AddOrder(User user, Customer customer, Address address, out string Res, out int result, int? id = null, bool save = true)
        {
            try
            {
                if (id != null && (from p in cont.OrderSet where p.Id == id select p).Any())
                {
                    Res = "Уже есть заказ с данным номером";
                    result = -1;
                    return false;
                }
                Order order = new Order();
                order.Customer = customer;
                order.Address = address;
                order.User = user;
                order.Id = id == null ? ((from p in cont.OrderSet select p).Any() ? (from p in cont.OrderSet select p.Id).Max() + 1 : 1) : (int)id;
                cont.OrderSet.Add(order);
                result = order.Id;
                if (save) cont.SaveChanges();
                Res = "Успешное добавление";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                result = -1;
                return false;
            }
        }

        public bool ChangeOrder(int Id, Customer customer, Address address, out string Res, bool save = true)
        {
            try
            {
                var a = FindOrder(Id);
                if (a == null)
                {
                    Res = "Нет заказа с данным идентификационный номером";
                    return false;
                }
                a.Address = address;
                a.Customer = customer;
                if (save) cont.SaveChanges();
                Res = "Изменение заказа успешно";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }

        public bool RemoveOrder(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var a = FindOrder(id);
                if (a == null)
                {
                    Res = "Нет заказа с таким идентификационным номером";
                    return false;
                }
                if (a.OrderEntry.Count == 0 || !check || AllowCascade)
                {
                    cont.OrderSet.Remove(a);
                    if (save) cont.SaveChanges();
                    Res = "Успешное удаление заказа";
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

        public Order FindOrder(int id) => (from o in cont.OrderSet where o.Id == id select o).FirstOrDefault();

        public IEnumerable<Order> SelectOrders(Func<Order, bool> predicate) => cont.OrderSet.Where(predicate).AsParallel();

        public override DataTable table(IEnumerable<Order> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}