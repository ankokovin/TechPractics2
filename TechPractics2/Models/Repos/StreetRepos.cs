using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class StreetRepos : Repos<Street>
    {
        public StreetRepos(Model1Container model, bool checkInputs = true, bool allowCascade = false) : base(model, checkInputs, allowCascade)
        {
           
        }
        /// <summary>
        /// Функция добавления улицы
        /// </summary>
        /// <param name="Name">Название улицы</param>
        /// <param name="city">Город</param>
        /// <param name="Res">Сообщение результата добавления</param>
        /// <returns>Результат добавления</returns>
        public bool Street(string Name, City city, out string Res, bool save = true)
        {
            try
            {
                if ((from s in cont.StreetSet where s.City.Id == city.Id && s.Name == Name select s).FirstOrDefault() != null)
                {
                    Res = "В городе " + city.Name + " уже есть улица " + Name;
                    return false;
                }
                Street street = new Street();
                street.Name = Name;
                street.City = city;
                cont.StreetSet.Add(street);
                if (save) cont.SaveChanges();
                Res = "Улица была добавлена успешно";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }
        /// <summary>
        /// Функция изменения улицы
        /// </summary>
        /// <param name="id">Идентификационный номер улицы</param>
        /// <param name="Name">Название улицы</param>
        /// <param name="city">Город</param>
        /// <param name="Res">Сообщение результата изменения</param>
        /// <returns>Результат изменения</returns>
        public bool Change(int id, string Name, City city, out string Res, bool save = true)
        {
            try
            {
                if ((from s in cont.StreetSet where s.City.Id == city.Id && s.Name == Name && s.Id != id select s).FirstOrDefault() != null)
                {
                    Res = "В городе " + city.Name + " уже есть улица " + Name;
                    return false;
                }
                var a = Find(id);
                if (a == null)
                {
                    Res = "Нет улицы с заданным идентификационным номером";
                    return false;
                }
                a.Name = Name;
                a.City = city;
                if (save) cont.SaveChanges();
                Res = "Изменение успешно";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }
        /// <summary>
        /// Функция удаления улицы по идентификационному номеру
        /// </summary>
        /// <param name="id">Идентификационный номер</param>
        /// <param name="Res">Сообщение результата удаления</param>
        /// <returns>Результат удаления</returns>
        public bool Remove(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var s = Find(id);
                if (s == null)
                {
                    Res = "Нет улицы с заданным идентификационным номером";
                    return false;
                }
                if (s.House.Count == 0 || !check || AllowCascade)
                {
                    cont.StreetSet.Remove(s);
                    if (save) cont.SaveChanges();
                    Res = "Удаление успешно";
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
        /// <summary>
        /// Функция поиска улицы по идентификационному номеру
        /// </summary>
        /// <param name="id">Идентификационный номер</param>
        /// <returns>Улица</returns>
        public Street Find(int id) => (from s in cont.StreetSet where s.Id == id select s).FirstOrDefault();

        public override IEnumerable<Street> Select(Func<Street, bool> predicate) => cont.StreetSet.Where(predicate).AsParallel();


    }
}