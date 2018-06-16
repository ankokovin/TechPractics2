using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class StatusRepos : Repos<Status>
    {
        public StatusRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {
        }

        public bool Add(string Name, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.StatusSet where a.Name == Name select a).FirstOrDefault() != null)
                {
                    Res = "Уже есть данный статус";
                    return false;
                }
                Status status = new Status();
                status.Name = Name;
                cont.StatusSet.Add(status);
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

        public bool Change(int Id, string Name, out string Res, bool save = true)
        {
            try
            {
                if ((from h in cont.StatusSet where h.Id != Id && h.Name == Name select h).FirstOrDefault() != null)
                {
                    Res = "Уже есть данный статус";
                    return false;
                }
                var a = Find(Id);
                if (a == null)
                {
                    Res = "Нет статуса с данным идентификационным номером";
                    return false;
                }
                a.Name = Name;
                if (save) cont.SaveChanges();
                Res = "Изменение статуса успешно";
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
                var a = Find(id);
                if (a == null)
                {
                    Res = "Нет статуса с таким идентификационным номером";
                    return false;
                }
                if (a.OrderEntry.Count == 0 || !check || AllowCascade)
                {
                    cont.StatusSet.Remove(a);
                    if (save) cont.SaveChanges();
                    Res = "Успешное удаление статуса";
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

        public Status Find(int id) => (from o in cont.StatusSet where o.Id == id select o).FirstOrDefault();

        public override IEnumerable<Status> Select(Func<Status, bool> predicate) => cont.StatusSet.Where(predicate).AsParallel();

    }
}