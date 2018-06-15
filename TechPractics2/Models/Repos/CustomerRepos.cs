using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class CustomerRepos : Repos<Customer>
    {
        public CustomerRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {

        }

        public bool AddCustomer(string Name, string Passport, string PhoneNumber, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.CustomerSet where a.Passport == Passport && !(a is Company) select a).FirstOrDefault() != null)
                {
                    Res = "Уже есть данный частный заказчик";
                    return false;
                }
                
                Customer customer = new Customer();
                customer.FIO = Name;
                customer.Passport = Passport;
                customer.PhoneNumber = PhoneNumber;
                cont.CustomerSet.Add(customer);
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

        public bool ChangeCustomer(int Id, string Name, string Passport, string PhoneNumber, out string Res, bool save = true)
        {
            try
            {
                if ((from h in cont.CustomerSet where h.Id != Id && !(h is Company) && h.Passport == h.Passport select h).Any())
                {
                    Res = "Уже есть частный клиент с данным номером паспорта";
                    return false;
                }
                var a = FindCustomer(Id);
                if (a == null)
                {
                    Res = "Нет частного клиента с данным идентификационным номером";
                    return false;
                }
                a.Passport = Passport;
                a.FIO = Name;
                a.PhoneNumber = PhoneNumber;
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

        public bool RemoveCustomer(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var a = FindCustomer(id);
                if (a == null)
                {
                    Res = "Нет заказчика с таким идентификационным номером";
                    return false;
                }
                if (a is Company)
                {
                    Res = "Данный заказчик является компанией, а не частным лицом";
                    return false;
                }
                if (a.Order.Count == 0 || !check || AllowCascade)
                {
                    cont.CustomerSet.Remove(a);
                    if (save) cont.SaveChanges();
                    Res = "Успешное удаление";
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

        public Customer FindCustomer(int id) => (from o in cont.CustomerSet where o.Id == id select o).FirstOrDefault();

        public IEnumerable<Customer> SelectCustomers(Func<Customer, bool> predicate) => cont.CustomerSet.Where(predicate).AsParallel();

        public override DataTable table(IEnumerable<Customer> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}