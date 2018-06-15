using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class PersonRepos : Repos<Person>
    {
        public PersonRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {
        }
        public bool AddPerson(string FIO, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.PersonSet where a.FIO == FIO select a).FirstOrDefault() != null)
                {
                    Res = "Уже есть данный человек";
                    return false;
                }
                Person person = new Person();
                person.FIO = FIO;
                cont.PersonSet.Add(person);
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

        public bool ChangePerson(int Id, string FIO, out string Res, bool save = true)
        {
            try
            {
                if ((from h in cont.PersonSet where h.Id != Id && h.FIO == FIO select h).Any())
                {
                    Res = "Уже есть человек с данным ФИО";
                    return false;
                }
                var a = FindPerson(Id);
                if (a == null)
                {
                    Res = "Нет адреса с данным идентификационным номером";
                    return false;
                }
                a.FIO = FIO;
                if (save) cont.SaveChanges();
                Res = "Изменение ФИО успешно";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }

        public bool RemovePerson(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var a = FindPerson(id);
                if (a == null)
                {
                    Res = "Нет ФИО с таким идентификационным номером";
                    return false;
                }
                if (a.Stavka.Count == 0 || !check || AllowCascade)
                {
                    cont.PersonSet.Remove(a);
                    if (save) cont.SaveChanges();
                    Res = "Успешное удаление ФИО";
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

        public Person FindPerson(int id) => (from o in cont.PersonSet where o.Id == id select o).FirstOrDefault();

        public IEnumerable<Person> SelectPersons(Func<Person, bool> predicate) => cont.PersonSet.Where(predicate).AsParallel();

        public override DataTable table(IEnumerable<Person> enumerable)
        {
            throw new NotImplementedException();
        }
    }
}