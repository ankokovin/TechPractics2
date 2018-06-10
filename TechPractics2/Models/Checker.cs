using System.Text.RegularExpressions;

namespace TechPractics2.Models
{
    public static class Checker
    {
        public static bool IsEnglishOrNumbersString(string input) => (new Regex("^[A-Za-z0-9]+$")).IsMatch(input);

        public static bool IsLogin(string input) => IsEnglishOrNumbersString(input);

        public static bool IsPassword(string input) => IsEnglishOrNumbersString(input);

        public static bool IsHouseNumber(string s)
        {
            return (new Regex("^[1-9][0-9]*[А-Я]?$").IsMatch(s));
        }
        public static bool IsName(string s)
        {
            return (new Regex(@"^[А-ЯA-Z][\w\s]+$").IsMatch(s));
        }
        public static bool IsFIO(string s)
        {
            return (new Regex("^[А-Я][а-я]+ [А-Я][а-я]+ [А-Я][а-я]+$").IsMatch(s));
        }
        public static bool IsNumber(string s)
        {
            return int.TryParse(s, out int result)&&result>0;
        }
        public static bool IsPhoneNumber(string s) => IsNumberAr(s);

        private static bool IsNumberAr(string s)
        {
            return (new Regex(@"^\d+$")).IsMatch(s);
        }
        public static bool IsPassportNumber(string s)
        {
            return  IsNumberAr(s)&& s.Length == 10;
        }
        public static bool IsINN(string s)
        {
            return IsNumberAr(s) && s.Length == 10;
        }

        public static bool CheckLoginInfo(string Login, string Password, out string Message, out string Source)
        {
            if (string.IsNullOrEmpty(Login))
            {
                Source = GlobalResources.SiteResources.User_Login;
                Message = GlobalResources.SiteResources.LoginResponse_Empty;
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                Source = GlobalResources.SiteResources.User_Password;
                Message = GlobalResources.SiteResources.LoginResponse_Empty;
                return false;
            }
            if (!IsLogin(Login))
            {
                Source = GlobalResources.SiteResources.User_Login;
                Message = GlobalResources.SiteResources.LoginResponse_InvalidString;
                return false;
            }
            if (!IsPassword(Password))
            {
                Source = GlobalResources.SiteResources.User_Password;
                Message = GlobalResources.SiteResources.LoginResponse_InvalidString;
                return false;
            }
            Message = string.Empty;
            Source = string.Empty;
            return true;
        }
    }
}