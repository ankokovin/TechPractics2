﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{

    public class StavkaRepos : Repos<Stavka>
    {
        public StavkaRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {
        }
        public bool Add(MeterType meterType, Person person, out string Res, bool save = true)
        {
            try
            {
                if ((from a in cont.StavkaSet where a.MeterType.Id == meterType.Id && a.Person.Id == person.Id select a).FirstOrDefault() != null)
                {
                    Res = "Уже есть данная ставка";
                    return false;
                }
                Stavka stavka = new Stavka();
                stavka.MeterType = meterType;
                stavka.Person = person;
                cont.StavkaSet.Add(stavka);
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

        public bool Change(int Id, MeterType meterType, Person person, out string Res, bool save = true)
        {
            try
            {
                if ((from h in cont.StavkaSet where h.Id != Id && h.MeterType.Id == meterType.Id && h.Person.Id == person.Id select h).Any())
                {
                    Res = "Уже есть данная ставка у данного человека";
                    return false;
                }
                var a = Find(Id);
                if (a == null)
                {
                    Res = "Нет ставки с данным идентификационным номером";
                    return false;
                }
                a.MeterType = meterType;
                a.Person = person;
                if (save) cont.SaveChanges();
                Res = "Изменение ставки успешно";
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
                    Res = "Нет ставки с таким идентификационным номером";
                    return false;
                }
                cont.StavkaSet.Remove(a);
                if (save) cont.SaveChanges();
                Res = "Успешное удаление ставки";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }

        public Stavka Find(int id) => (from o in cont.StavkaSet where o.Id == id select o).FirstOrDefault();

        public override IEnumerable<Stavka> Select(Func<Stavka, bool> predicate) => cont.StavkaSet.Where(predicate).AsParallel();


    }
}