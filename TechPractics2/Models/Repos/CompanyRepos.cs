using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class CompanyRepos : Repos<Company>
    {
        public CompanyRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {

        }

        public bool Add(string Name, string Passport, string PhoneNumber, string CompanyName, string INN, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.CustomerSet where (a is Company) && ((a as Company).INN == INN) select a)
                    .FirstOrDefault() != null)
                {
                    Res = "Уже есть данная компания";
                    return false;
                }
                Company company = new Company();
                company.FIO = Name;
                company.Passport = Passport;
                company.INN = INN;
                company.CompanyName = CompanyName;
                company.PhoneNumber = PhoneNumber;
                cont.CustomerSet.Add(company);
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

        public bool Change(int Id, string Name, string Passport, string PhoneNumber, string CompanyName, string INN, out string Res, bool save = true)
        {

            try
            {
                if ((from h in cont.CustomerSet where h.Id != Id && (h is Company) && (h as Company).INN == INN select h).Any())
                {
                    Res = "Уже есть компания с данным ИНН";
                    return false;
                }
                var a = Find(Id);
                if (a == null)
                {
                    Res = "Нет компании с данным идентификационным номером";
                    return false;
                }
                a.Passport = Passport;
                a.FIO = Name;
                a.PhoneNumber = PhoneNumber;
                if (save) cont.SaveChanges();
                Res = "Изменение компании успешно";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }

        }

        public bool Remove(int id, out string Res, bool save = true, bool check = true)
        {

            try
            {
                var a = cont.CustomerSet.Find(id);
                if (a == null)
                {
                    Res = "Нет заказчика с таким идентификационным номером";
                    return false;
                }
                if (!(a is Company))
                {
                    Res = "Данный заказчик является  не компанией, а частным лицом";
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

        public Company Find(int id) => (from o in cont.CustomerSet where o.Id == id select o).FirstOrDefault() as Company;

        public override IEnumerable<Company> Select(Func<Company, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}