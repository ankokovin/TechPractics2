﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class MeterRepos : Repos<Meter>
    {
        public MeterRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {
        }

        public bool Add(string Name, MeterType meterType, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.MeterSet where a.Name == Name select a).FirstOrDefault() != null)
                {
                    Res = "Уже есть прибор учёта с данным названием";
                    return false;
                }
                Meter meter = new Meter();
                meter.Name = Name;
                meter.MeterType = meterType;
                cont.MeterSet.Add(meter);
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

        public bool Change(int Id, string Name, MeterType meterType, out string Res, bool save = true)
        {
            try
            {
                if ((from h in cont.MeterSet where h.Id != Id && h.Name == Name select h).Any())
                {
                    Res = "Уже есть прибор учёта с данным названием";
                    return false;
                }
                var a = Find(Id);
                if (a == null)
                {
                    Res = "Нет прибора учёта с данным идентификационным номером";
                    return false;
                }
                a.Name = Name;
                a.MeterType = meterType;
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

        public bool Remove(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var a = Find(id);
                if (a == null)
                {
                    Res = "Нет прибора учёта с таким идентификационным номером";
                    return false;
                }
                if (a.OrderEntry.Count == 0 || !check || AllowCascade)
                {
                    cont.MeterSet.Remove(a);
                    if (save) cont.SaveChanges();
                    Res = "Успешное удаление прибора учёта";
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

        public Meter Find(int id) => (from o in cont.MeterSet where o.Id == id select o).FirstOrDefault();

        public override IEnumerable<Meter> Select(Func<Meter, bool> predicate) => cont.MeterSet.Where(predicate).AsParallel();

    }
}