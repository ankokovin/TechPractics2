using System;

namespace TechPractics2.Models.UtilityModels
{
    public class HasPrevUrl
    {
        public Uri PrevUrl;
    }

    public class SignInArgs : HasPrevUrl
    {
        public string Login;
        public string Password;
        public bool SaveToCookies;
    }
}