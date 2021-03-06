﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class MeterTypeRepos : Repos <MeterType>
    {
        public MeterTypeRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {
        }


        public bool Add(string Name, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.MeterTypeSet where a.Name == Name select a).FirstOrDefault() != null)
                {
                    Res = "Уже есть данный тип приборов учёта";
                    return false;
                }
                MeterType meterType = new MeterType();
                meterType.Name = Name;
                cont.MeterTypeSet.Add(meterType);
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
                if ((from h in cont.MeterTypeSet where h.Id != Id && h.Name == Name select h).FirstOrDefault() != null)
                {
                    Res = "Уже есть тип приборов учёта с данным названием";
                    return false;
                }
                var a = Find(Id);
                if (a == null)
                {
                    Res = "Нет типа приборов учёта с данным идентификационным номером";
                    return false;
                }
                a.Name = Name;
                if (save) cont.SaveChanges();
                Res = "Изменение прибора учёта успешно";
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
                    Res = "Нет типа приборов учёта с таким идентификационным номером";
                    return false;
                }
                if ((a.Meter.Count == 0) && (a.Stavka.Count == 0) || !check ||
                    AllowCascade)
                {
                    cont.MeterTypeSet.Remove(a);
                    if (save) cont.SaveChanges();
                    Res = "Успешное удаление типа приборов учёта";
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

        public MeterType Find(int id) => (from o in cont.MeterTypeSet where o.Id == id select o).FirstOrDefault();

        public override IEnumerable<MeterType> Select(Func<MeterType, bool> predicate) => cont.MeterTypeSet.Where(predicate).AsParallel();


    }
}