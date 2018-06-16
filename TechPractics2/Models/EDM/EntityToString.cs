using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TechPractics2.Models.EDM
{
    interface IFullDisc
    {
        String FullDisc { get; }
    }

    partial class Person : IFullDisc
    {
        public override string ToString()
        {
            return FIO;
        }
        public String FullDisc => this.ToString();
    }
    partial class House : IFullDisc
    {
        public override string ToString()
        {
            return Street + " " + Number;
        }
        public String FullDisc => this.ToString();
    }
    partial class Address : IFullDisc
    {
        public override string ToString()
        {
            return House + " " + Flat;
        }
        public String FullDisc => ToString();
    }
    partial class City : IFullDisc
    {
        public override string ToString()
        {
            return Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class Company : IFullDisc
    {
        public override string ToString()
        {
            return base.ToString() + " " + CompanyName + " " + INN;
        }
    }
    partial class Customer : IFullDisc
    {
        public override string ToString()
        {
            return FIO + " " + Passport;
        }
        public String FullDisc => this.ToString();
    }
    partial class Meter : IFullDisc
    {
        public override string ToString()
        {
            return MeterType + " " + Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class MeterType : IFullDisc
    {
        public override string ToString()
        {
            return Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class Order : IFullDisc
    {
        public override string ToString()
        {
            return Id.ToString();
        }
        public String FullDisc => this.ToString();
    }
    partial class OrderEntry : IFullDisc
    {
        public override string ToString()
        {
            return Meter + " " + RegNumer + " " + Person + " " +Status + " " + StartTime + " " + EndTime;
        }
        public String FullDisc => this.ToString();
    }
    partial class Status : IFullDisc
    {
        public override string ToString()
        {
            return Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class Stavka : IFullDisc
    {
        public override string ToString()
        {
            return Person + " " + MeterType;
        }
        public String FullDisc => this.ToString();
    }
    partial class Street : IFullDisc
    {
        public override string ToString()
        {
            return City + " " + Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class User : IFullDisc
    {
        public override string ToString()
        {
            return Login;
        }
        public String FullDisc => this.ToString();
    }
    partial class UserToCustomer: IFullDisc
    {
        public override string ToString()
        {
            return User.Login + " " + Customer;
        }
        public String FullDisc => ToString();
    }
}

