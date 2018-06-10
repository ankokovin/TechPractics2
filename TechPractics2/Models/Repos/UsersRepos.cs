using System;
using System.Collections.Generic;
using System.Linq;
using TechPractics2.Models.EDM;

namespace TechPractics2.Models.Repos
{
    public class UsersRepos : Repos
    {
        public UsersRepos(Model1Container model, bool checkInputs=true, bool allowCascade=false) : base(model,checkInputs,allowCascade)
        {
           
        }
        /// <summary>
        /// Функция добавления нового пользователя
        /// </summary>
        /// <param name="userType">Тип нового пользователя</param>
        /// <param name="Login">Логин пользователя</param>
        /// <param name="Password">Пароль пользователя</param>
        /// <returns>Результат добавления</returns>
        public bool AddUser(UserType userType, string Login, string Password, out string Res, bool save = true)
        {
            try
            {
                if (CheckInputs)
                {
                    if (Login.Length == 0)
                    {
                        Res = "Логин не может быть пустой строкой";
                        return false;
                    }
                    if (Password.Length == 0)
                    {
                        Res = "Пароль не может быть пустой строкой";
                        return false;
                    }
                }
                if ((from u in cont.UserSet where u.Login == Login select u).Count() > 0)
                {
                    Res = "Пользователь с данным именем уже существует";
                    return false;
                }
                TechPractics2.Models.EDM.User user = new TechPractics2.Models.EDM.User
                {
                    Login = Login,
                    Password = Password,
                    UserType = userType
                };
                cont.UserSet.Add(user);
                if (save) cont.SaveChanges();
                Res = "Пользователь " + Login + " успешно добавлен";
                return true;
            }
            catch (Exception e)
            {
                Res = e.Message;
                return false;
            }
        }
        /// <summary>
        /// Функция изменения пользователя
        /// </summary>
        /// <param name="Id">Идентификационный номер пользователя</param>
        /// <param name="userType">Тип пользователя</param>
        /// <param name="Login">Имя пользователя</param>
        /// <param name="Password">Пароль</param>
        /// <param name="Res">Сообщение результата изменения</param>
        /// <returns>Результат изменения</returns>
        public bool ChangeUser(int Id, UserType userType, string Login, string Password, out string Res, bool save = true)
        {
            try
            {
                if (CheckInputs)
                {
                    if (Login.Length == 0)
                    {
                        Res = "Логин не может быть пустой строкой";
                        return false;
                    }
                    if (Password.Length == 0)
                    {
                        Res = "Пароль не может быть пустой строкой";
                        return false;
                    }
                }
                if ((from u in cont.UserSet where u.Login == Login && u.Id != Id select u).Any())
                {
                    Res = "Пользователь с данным именем уже существует";
                    return false;
                }
                var a = (from u in cont.UserSet where u.Id == Id select u).FirstOrDefault();
                if (a == null)
                {
                    Res = "Нет пользователя с таким идентификационным номером";
                    return false;
                }
                a.Login = Login;
                a.Password = Password;
                a.UserType = userType;
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
        /// Функция удаления пользователя
        /// </summary>
        /// <param name="id">Идентификационный номер пользователя</param>
        /// <param name="Res">Сообщение результата удаления</param>
        /// <returns>Результат удаления</returns>
        public  bool RemoveUser(int id, out string Res, bool save = true, bool check = true)
        {
            try
            {
                var u = cont.UserSet.Find(id);
                if (u == null)
                {
                    Res = "Нет пользователя с данным идентификационным номером";
                    return false;
                }
                if (u.Order.Count == 0 || !check || AllowCascade)
                {
                    cont.UserSet.Remove(u);
                    if (save) cont.SaveChanges();
                    Res = "Пользователь " + u.Login + " успешно удалён";
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
        /// Поиск пользователя по идентификационному номеру
        /// </summary>
        /// <param name="Id">Идентификационный номер</param>
        /// <returns>Пользователь</returns>
        public  User FindUser(int Id) => (from u in cont.UserSet where u.Id == Id select u).FirstOrDefault();
        /// <summary>
        /// Функция входа в систему
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <param name="Password">Пароль</param>
        /// <param name="Message">Получаемое сообщение</param>
        /// <returns>Пользователь, под которым происходит вход, null при ошибке входа</returns>
        public  User TryEntry(string Login, string Password, out string Message)
        {
            //TODO: login in with token
            try
            {
                Message = string.Empty;
                var user = (from u in cont.UserSet
                            where u.Login == Login
                            select u).ToList();
                if (user.Count == 0)
                {
                    Message += GlobalResources.SiteResources.LoginResponse_NoUser;
                    return null;
                }
                else
                {
                    foreach (User u in user)
                        if (u.Password == Password)
                        {
                            //Login complete
                            //TODO: create token and pass it to Message
                            Message = Password;
                            return u;
                        }
                    Message += "Неверный пароль";
                    return null;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }


        public IEnumerable<User> SelectUsers(Func<User, bool> predicate) =>
            (from p in cont.UserSet.Local
             where predicate(p)
             select p).AsParallel();
    }
}