using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechPractics2.Models.EDM
{
    partial class Person
    {
        public override string ToString()
        {
            return FIO;
        }
        public String FullDisc => this.ToString();
    }
    partial class House
    {
        public override string ToString()
        {
            return Street + " " + Number;
        }
        public String FullDisc => this.ToString();
    }
    partial class Address
    {
        public override string ToString()
        {
            return House + " " + Flat;
        }
        public String FullDisc => ToString();
    }
    partial class City
    {
        public override string ToString()
        {
            return Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class Company
    {
        public override string ToString()
        {
            return base.ToString() + " " + CompanyName + " " + INN;
        }
        public String FullDisc => this.ToString();
    }
    partial class Customer
    {
        public override string ToString()
        {
            return FIO + " " + Passport;
        }
        public String FullDisc => this.ToString();
    }
    partial class Meter
    {
        public override string ToString()
        {
            return MeterType + " " + Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class MeterType
    {
        public override string ToString()
        {
            return Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class Order
    {
        public override string ToString()
        {
            return Id + " " + Customer + " " + Address + " " + OrderEntry.Count + " " + User;
        }
        public String FullDisc => this.ToString();
    }
    partial class OrderEntry
    {
        public override string ToString()
        {
            return Meter + " " + RegNumer + " " + Person + " " +Status + " " + StartTime + " " + EndTime;
        }
        public String FullDisc => this.ToString();
    }
    partial class Status
    {
        public override string ToString()
        {
            return Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class Stavka
    {
        public override string ToString()
        {
            return Person + " " + MeterType;
        }
        public String FullDisc => this.ToString();
    }
    partial class Street
    {
        public override string ToString()
        {
            return City + " " + Name;
        }
        public String FullDisc => this.ToString();
    }
    partial class User
    {
        public override string ToString()
        {
            return Login;
        }
        public String FullDisc => this.ToString();
    }
}

