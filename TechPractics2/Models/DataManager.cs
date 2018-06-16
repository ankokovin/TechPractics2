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
            InitializeRepos();
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
        public UserToCustomerRepos UserToCustomerRepos;
        
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
            UserToCustomerRepos = new UserToCustomerRepos(cont, CheckInputs, AllowCascade);

            Reposes = new Dictionary<Type, object>()
            {
                {typeof(Address),AddressRepos },
                {typeof(House),HouseRepos },
                {typeof(Street),StreetRepos },
                {typeof(City),CityRepos },
                {typeof(Order),OrderRepos },
                {typeof(OrderEntry),OrderEntryRepos },
                {typeof(Customer),CustomerRepos },
                {typeof(Company),CompanyRepos },
                {typeof(User),UsersRepos },
                {typeof(Meter),MeterRepos },
                {typeof(MeterType),MeterTypeRepos },
                {typeof(Status),StatusRepos },
                {typeof(Person),PersonRepos },
                {typeof(Stavka),StavkaRepos },
                {typeof(UserToCustomer),UserToCustomerRepos },
            };
        }

        public void InicializeNextId()
        {
            InitializeRepos(false,true);
            NextId = new Dictionary<EntityTypes, int>();
            CheckInputs = false;
            UsersRepos.Add(UserType.Operator, "", "", out string res0);
            CheckInputs = true;
            NextId[EntityTypes.User] = (from p in cont.UserSet where p.Login.Length == 0 select p.Id).First();
            CityRepos.Add("", out string Res);
            NextId[EntityTypes.City] = (from p in cont.CitySet where p.Name.Length == 0 select p.Id).First();
            StreetRepos.Street("",CityRepos.Find(NextId[EntityTypes.City]), out string Res1);
            NextId[EntityTypes.Street] = (from p in cont.StreetSet where p.Name.Length == 0 select p.Id).First();
            HouseRepos.Add("", StreetRepos.Find(NextId[EntityTypes.Street]), out string Res2);
            NextId[EntityTypes.House] = (from p in cont.HouseSet where p.Number.Length == 0 select p.Id).First();
            AddressRepos.Add(-1, HouseRepos.Find(NextId[EntityTypes.House]), out string res4);
            NextId[EntityTypes.Address] = (from p in cont.AddressSet where p.Flat == -1 select p.Id).First();
            CustomerRepos.Add("", "",null, out string res5);
            NextId[EntityTypes.Customer] = (from p in cont.CustomerSet where p.FIO.Length == 0 select p.Id).First();
            PersonRepos.Add("", out string res6);
            NextId[EntityTypes.Person] = (from p in cont.PersonSet where p.FIO.Length == 0 select p.Id).First();
            MeterTypeRepos.Add("", out string res7);
            NextId[EntityTypes.MeterType] = (from p in cont.MeterTypeSet where p.Name.Length == 0 select p.Id).First();
            MeterRepos.Add("", MeterTypeRepos.Find(NextId[EntityTypes.MeterType]), out string res8);
            NextId[EntityTypes.Meter] = (from p in cont.MeterSet where p.Name.Length == 0 select p.Id).First();
            StavkaRepos.Add(MeterTypeRepos.Find(NextId[EntityTypes.MeterType]),PersonRepos.Find(NextId[EntityTypes.Person]), out string res9);
            int mt = NextId[EntityTypes.MeterType];
            NextId[EntityTypes.Stavka] = (from p in cont.StavkaSet where p.MeterType.Id ==mt  select p.Id).First();
            OrderRepos.Add(UsersRepos.Find(NextId[EntityTypes.User]),CustomerRepos.Find(NextId[EntityTypes.Customer]),AddressRepos.Find(NextId[EntityTypes.Address]), out string res10, out int order);
            int ad = NextId[EntityTypes.Address];
            NextId[EntityTypes.Order] = (from p in cont.OrderSet where p.Address.Id ==  ad select p.Id).First();
            StatusRepos.Add("", out string Res10);
            NextId[EntityTypes.Status] = (from p in cont.StatusSet where p.Name.Length == 0 select p.Id).First();
            OrderEntryRepos.Add(OrderRepos.Find(NextId[EntityTypes.Order]),null, null, "",MeterRepos.Find(NextId[EntityTypes.Meter]),
            PersonRepos.Find(NextId[EntityTypes.Person]), StatusRepos.Find(NextId[EntityTypes.Status]), out string res11);
            NextId[EntityTypes.OrderEntry] = (from p in cont.OrderEntrySet where p.RegNumer.Length == 0 select p.Id).First();
            CityRepos.Remove(NextId[EntityTypes.City],out string res12,check:false);
            UsersRepos.Remove(NextId[EntityTypes.User], out string res13, check: false);
            MeterTypeRepos.Remove(NextId[EntityTypes.MeterType], out string res14, check: false);
            PersonRepos.Remove(NextId[EntityTypes.Person], out string res15, check: false);
            CustomerRepos.Remove(NextId[EntityTypes.Customer], out string res16, check: false);
            StatusRepos.Remove(NextId[EntityTypes.Status], out string res17, check: false);
            InitializeRepos();
        }

        public  IEnumerable<T2> NextSelect<T2>(Func<T2, bool> predicate , IEnumerable<T2> Prev) => (from p in Prev where predicate(p) select p).AsParallel();

        public Dictionary<Type, object> Reposes { get; private set; }

        public IRepos<T> GetSelect<T>()
        {
            return Reposes[typeof(T)] as IRepos<T>;
        }

        
    }
}