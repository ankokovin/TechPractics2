using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Data.Entity;
using TechPractics2.Models.EDM;
using TechPractics2.Models.Repos;

namespace TechPractics2.Models
{
    /// <summary>
    /// Операции, выполняемые над базой данных
    /// </summary>
    public  partial class DataManager
    {
        public  bool CheckInputs = true;
        
        public DataManager()
        {
            cont = new Model1Container();
        }

        /// <summary>
        /// Контекст базы данных
        /// </summary>
        public Model1Container cont;
        public UsersRepos UsersRepos;
        public AddressRepos AddressRepos;
        public HouseRepos HouseRepos;
        public StreetRepos StreetRepos;
        public CityRepos CityRepos;
        public OrderRepos OrderRepos;
        public CustomerRepos CustomerRepos;
        public CompanyRepos CompanyRepos;
        public OrderEntryRepos OrderEntryRepos;
        public StatusRepos StatusRepos;
        public MeterRepos MeterRepos;
        public MeterTypeRepos MeterTypeRepos;
        public StavkaRepos StavkaRepos;
        public PersonRepos PersonRepos;
        
        public Dictionary<EntityTypes, int> NextId;

        public void InitializeRepos(bool CheckInputs=true, bool AllowCascade = false)
        {
            UsersRepos = new UsersRepos(cont, CheckInputs, AllowCascade);
            AddressRepos = new AddressRepos(cont, CheckInputs, AllowCascade);
            HouseRepos = new HouseRepos(cont, CheckInputs, AllowCascade);
            StreetRepos = new StreetRepos(cont, CheckInputs, AllowCascade);
            CityRepos = new CityRepos(cont, CheckInputs, AllowCascade);
            OrderRepos = new OrderRepos(cont, CheckInputs, AllowCascade);
            CustomerRepos = new CustomerRepos(cont, CheckInputs, AllowCascade);
            CompanyRepos = new CompanyRepos(cont, CheckInputs, AllowCascade);
            OrderEntryRepos = new OrderEntryRepos(cont, CheckInputs, AllowCascade);
            StatusRepos = new StatusRepos(cont, CheckInputs, AllowCascade);
            MeterRepos = new MeterRepos(cont, CheckInputs, AllowCascade);
            MeterTypeRepos = new MeterTypeRepos(cont, CheckInputs, AllowCascade);
            StavkaRepos = new StavkaRepos(cont, CheckInputs, AllowCascade);
            PersonRepos = new PersonRepos(cont, CheckInputs, AllowCascade);
        }

        public void InicializeNextId()
        {
            InitializeRepos(false,true);
            NextId = new Dictionary<EntityTypes, int>();
            CheckInputs = false;
            UsersRepos.AddUser(UserType.Operator, "", "", out string res0);
            CheckInputs = true;
            NextId[EntityTypes.User] = (from p in cont.UserSet where p.Login.Length == 0 select p.Id).First();
            CityRepos.AddCity("", out string Res);
            NextId[EntityTypes.City] = (from p in cont.CitySet where p.Name.Length == 0 select p.Id).First();
            StreetRepos.AddStreet("",CityRepos.FindCity(NextId[EntityTypes.City]), out string Res1);
            NextId[EntityTypes.Street] = (from p in cont.StreetSet where p.Name.Length == 0 select p.Id).First();
            HouseRepos.AddHouse("", StreetRepos.FindStreet(NextId[EntityTypes.Street]), out string Res2);
            NextId[EntityTypes.House] = (from p in cont.HouseSet where p.Number.Length == 0 select p.Id).First();
            AddressRepos.AddAddress(-1, HouseRepos.FindHouse(NextId[EntityTypes.House]), out string res4);
            NextId[EntityTypes.Address] = (from p in cont.AddressSet where p.Flat == -1 select p.Id).First();
            CustomerRepos.AddCustomer("", "",null, out string res5);
            NextId[EntityTypes.Customer] = (from p in cont.CustomerSet where p.FIO.Length == 0 select p.Id).First();
            PersonRepos.AddPerson("", out string res6);
            NextId[EntityTypes.Person] = (from p in cont.PersonSet where p.FIO.Length == 0 select p.Id).First();
            MeterTypeRepos.AddMeterType("", out string res7);
            NextId[EntityTypes.MeterType] = (from p in cont.MeterTypeSet where p.Name.Length == 0 select p.Id).First();
            MeterRepos.AddMeter("", MeterTypeRepos.FindMeterType(NextId[EntityTypes.MeterType]), out string res8);
            NextId[EntityTypes.Meter] = (from p in cont.MeterSet where p.Name.Length == 0 select p.Id).First();
            StavkaRepos.AddStavka(MeterTypeRepos.FindMeterType(NextId[EntityTypes.MeterType]),PersonRepos.FindPerson(NextId[EntityTypes.Person]), out string res9);
            int mt = NextId[EntityTypes.MeterType];
            NextId[EntityTypes.Stavka] = (from p in cont.StavkaSet where p.MeterType.Id ==mt  select p.Id).First();
            OrderRepos.AddOrder(UsersRepos.FindUser(NextId[EntityTypes.User]),CustomerRepos.FindCustomer(NextId[EntityTypes.Customer]),AddressRepos.FindAddress(NextId[EntityTypes.Address]), out string res10, out int order);
            int ad = NextId[EntityTypes.Address];
            NextId[EntityTypes.Order] = (from p in cont.OrderSet where p.Address.Id ==  ad select p.Id).First();
            StatusRepos.AddStatus("", out string Res10);
            NextId[EntityTypes.Status] = (from p in cont.StatusSet where p.Name.Length == 0 select p.Id).First();
            OrderEntryRepos.AddOrderEntry(OrderRepos.FindOrder(NextId[EntityTypes.Order]),null, null, "",MeterRepos.FindMeter(NextId[EntityTypes.Meter]),
            PersonRepos.FindPerson(NextId[EntityTypes.Person]), StatusRepos.FindStatus(NextId[EntityTypes.Status]), out string res11);
            NextId[EntityTypes.OrderEntry] = (from p in cont.OrderEntrySet where p.RegNumer.Length == 0 select p.Id).First();
            CityRepos.RemoveCity(NextId[EntityTypes.City],out string res12,check:false);
            UsersRepos.RemoveUser(NextId[EntityTypes.User], out string res13, check: false);
            MeterTypeRepos.RemoveMeterType(NextId[EntityTypes.MeterType], out string res14, check: false);
            PersonRepos.RemovePerson(NextId[EntityTypes.Person], out string res15, check: false);
            CustomerRepos.RemoveCustomer(NextId[EntityTypes.Customer], out string res16, check: false);
            StatusRepos.RemoveStatus(NextId[EntityTypes.Status], out string res17, check: false);
        }

        public  IEnumerable<T2> NextSelect<T2>(Func<T2, bool> predicate , IEnumerable<T2> Prev) => (from p in Prev where predicate(p) select p).AsParallel();
    }
}