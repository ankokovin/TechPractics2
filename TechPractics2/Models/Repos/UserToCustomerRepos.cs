﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class UserToCustomerRepos : Repos <UserToCustomer>
    {
        public UserToCustomerRepos(Model1Container model, bool checkInputs = true, bool allowCascade = false) : base(model, checkInputs, allowCascade)
        {
        }

        public bool Add(User user, Customer customer, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.UserToCustomerSet where a.User.Id == user.Id && a.Customer.Id == customer.Id select a).Any())
                {
                    Res = "Уже есть данная связь";
                    return false;
                }
                UserToCustomer userToCustomer = new UserToCustomer();
                userToCustomer.User = user;
                userToCustomer.Customer = customer;
                cont.UserToCustomerSet.Add(userToCustomer);
                if (save) cont.SaveChanges();
                Res = "Успешное добавление";
                return true;
            }catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }

        public bool Change(int id, User user, Customer customer, out string Res, bool save = true)
        {
            try
            {
                if ((from o in cont.UserToCustomerSet where o.User.Id == user.Id && o.Customer.Id == customer.Id select o).Any())
                {
                    Res = "Уже есть данная связь";
                    return false;
                }
                var a = Find(id);
                a.Customer = customer;
                a.User = user;
                if (save) cont.SaveChanges();
                Res = "Успешное изменение";
                return true;
            }catch(Exception e)
            {
                Res = e.Message;
                return false;
            }
        }
        public bool Remove(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var a = Find(id);
                if (a == null){
                    Res = "Нет объекта с данным Id";
                    return false;
                }
                cont.UserToCustomerSet.Remove(a);
                if (save) cont.SaveChanges();
                Res = "Успешное удаление";
                return true;
            }catch(Exception e)
            {
                Res = e.Message;
                return false;
            }
        }
        public UserToCustomer Find(int id) => (from o in cont.UserToCustomerSet where o.Id == id select o).FirstOrDefault();


        public override IEnumerable<UserToCustomer> Select(Func<UserToCustomer, bool> predicate) => cont.UserToCustomerSet.Where(predicate).AsParallel();


    }
}