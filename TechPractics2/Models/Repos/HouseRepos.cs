using System;
using System.Collections.Generic;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class HouseRepos : Repos
    {
       
        public HouseRepos(Model1Container model, bool checkInputs = true, bool allowCascade = false) : base(model, checkInputs, allowCascade)
        {
           
        } 
        /// <summary>
          /// Функция добавления дома
          /// </summary>
          /// <param name="Number">Номер дома</param>
          /// <param name="FlatsCount">Количество квартир</param>
          /// <param name="street">Улица</param>
          /// <param name="Res">Сообщение результата добавления</param>
          /// <returns>Результат добавления</returns>
        public bool AddHouse(string Number, Street street, out string Res, bool save = true)
        {
            try
            {
                if ((from h in cont.HouseSet where h.Street.Id == street.Id && h.Number == Number select h).FirstOrDefault() != null)
                {
                    Res = "Уже есть дом номер " + Number + " на улице " + street;
                    return false;
                }
                House house = new House();
                house.Street = street;
                house.Number = Number;
                cont.HouseSet.Add(house);
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
        /// <summary>
        /// Функция изменения дома
        /// </summary>
        /// <param name="id">Идентификационный номер дома</param>
        /// <param name="Number">Номер дома</param>
        /// <param name="FlatsCount">Количество квартир</param>
        /// <param name="street">Улица</param>
        /// <param name="Res">Сообщение результата изменения</param>
        /// <returns>Результат изменения</returns>
        public bool ChangeHouse(int id, string Number, Street street, out string Res, bool save = true)
        {
            try
            {
                if ((from h in cont.HouseSet where h.Id != id && h.Street.Id == street.Id && h.Number == Number select h).FirstOrDefault() != null)
                {
                    Res = "В городе " + street.City + " на улице " + street + " уже есть дом номер " + Number;
                    return false;
                }
                var a = FindHouse(id);
                if (a == null)
                {
                    Res = "Нет дома с данным идентификационным номером";
                    return false;
                }
                a.Street = street;
                a.Number = Number;
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
        /// <summary>
        /// Функция удаления дома
        /// </summary>
        /// <param name="id">Идентификационный номер дома</param>
        /// <param name="Res">Сообщение результата удаления</param>
        /// <returns>Результат удаления</returns>
        public bool RemoveHouse(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var a = FindHouse(id);
                if (a == null)
                {
                    Res = "Нет дома с таким идентификационным номером";
                    return false;
                }
                if (a.Address.Count == 0 || !check || AllowCascade)
                {
                    cont.HouseSet.Remove(a);
                    if (save) cont.SaveChanges();
                    Res = "Успешное удаления дома номер " + a.Number +
                    " улицы " + a.Street + " города " + a.Street.City;
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
        /// Функция поиска дома по идентификационному номеру
        /// </summary>
        /// <param name="id">Идентификационный номер</param>
        /// <returns>Дом</returns>
        public House FindHouse(int id) => (from s in cont.HouseSet where s.Id == id select s).FirstOrDefault();

        public IEnumerable<House> SelectHouses(Func<House, bool> predicate) => cont.HouseSet.Where(predicate).AsParallel();
    }
}