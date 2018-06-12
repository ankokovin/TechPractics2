using System;
using System.Collections.Generic;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class CityRepos : Repos
    {
       
        public CityRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model, checkInputs, allowCascade)
        {
           
        }
        /// <summary>
        /// Добавить город
        /// </summary>
        /// <param name="Name">Название города</param>
        /// <param name="Res">Сообщение о результате</param>
        /// <returns>Результат добавления</returns>
        public bool AddCity(string Name, out string Res, bool save = true)
        {
            try
            {
                if ((from c in cont.CitySet where c.Name == Name select c).Count() > 0)
                {
                    Res = "Данный город уже добавлен";
                    return false;
                }
                City city = new City
                {
                    Name = Name
                };
                cont.CitySet.Add(city);
                if (save) cont.SaveChanges();
                Res = "Город " + Name + " был добавлен успешно";
                return true;
            }
            catch (Exception e)
            {
                //TODO: обработка
                Res = e.Message;
                return false;
            }
        }
        /// <summary>
        /// Удалить город
        /// </summary>
        /// <param name="id">Идентификатор города</param>
        /// <param name="Res">Сообщение о результате</param>
        /// <returns>Результат удаления</returns>
        public bool RemoveCity(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var c = cont.CitySet.Find(id);
                if (c == null)
                {
                    Res = "Город не обнаружен";
                    return false;
                }
                if (c.Street.Count == 0 || !check || AllowCascade)
                {
                    cont.CitySet.Remove(c);
                    if (save) cont.SaveChanges();
                    Res = "Город " + c.Name + " c id " + id + " был успешно удалён";
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
                //TODO: обработка
                Res = e.Message;
                return false;
            }
        }
        /// <summary>
        /// Изменить город
        /// </summary>
        /// <param name="id">Идентификационный номер города</param>
        /// <param name="Name">Новое имя города</param>
        /// <param name="Res">Сообщение о результате</param>
        /// <returns>Результат изменения</returns>
        public bool ChangeCity(int id, string Name, out string Res, bool save = true)
        {
            try
            {
                if ((from c in cont.CitySet where c.Name == Name && c.Id != id select c).Any())
                {
                    Res = "Уже есть город с именем " + Name;
                    return false;
                }
                var a = (from c in cont.CitySet where c.Id == id select c).FirstOrDefault();
                if (a == null)
                {
                    Res = "Нет города с идентификационным номером " + id;
                    return false;
                }
                a.Name = Name;
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
        /// Функция поиска города по идентификационному номеру
        /// </summary>
        /// <param name="Id">Идентификационный номер</param>
        /// <returns>Город</returns>
        public City FindCity(int Id) => (from c in cont.CitySet where c.Id == Id select c).FirstOrDefault();

        public IEnumerable<City> SelectCitys(Func<City, bool> predicate) => cont.CitySet.Where(predicate).AsParallel();
    }
}