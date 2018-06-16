using System;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using TechPractics2.Models.EDM;


namespace TechPractics2.Models
{

    public class XMLQuery
    {
        DataManager dataManager;
        public XMLQuery(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(LessCont));
        public void Serialize(Stream s)
        {
            using (StreamWriter sw = new StreamWriter(s))
            {
                LessCont Cont = new LessCont(dataManager);
                Cont.Create();
                xmlSerializer.Serialize(sw, Cont);
            }
        }

        public void Deserialize(Stream s)
        {
            Serialize(new FileStream("backup.xml",FileMode.Create));
            using (StreamReader sr = new StreamReader(s))
            {
                LessCont lessCont = (LessCont)xmlSerializer.Deserialize(sr);
                lessCont.Update();
            }
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "Address")]
    public class _Address
    {
        public int Id { get; set; }
        public int Flat { get; set; }
        public int HouseId { get; set; }
        public static  _Address Trans(Address address) => new _Address { Id = address.Id, Flat = address.Flat, HouseId=address.House.Id};
    }
    [Serializable]
    [XmlRoot(ElementName = "House")]
    public class _House
    {
        public string Number { get; set; }
        public int Id { get; set; }
        public int StreetId { get; set; }
        public static _House Trans(House house) => new _House { Id = house.Id, Number = house.Number, StreetId = house.Street.Id};
    }
    [Serializable]
    [XmlRoot(ElementName = "Street")]
    public class _Street
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public static _Street Trans(Street street) => new _Street { Id = street.Id, Name = street.Name, CityId = street.City.Id};
    }
    [Serializable]
    [XmlRoot(ElementName = "City")]
    public class _City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static _City Trans(City city) => new _City { Id = city.Id, Name = city.Name};
    }
    [Serializable]
    [XmlRoot(ElementName = "User")]
    public class _User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public static _User Trans(User user)=> new _User { Id = user.Id,Login = user.Login, Password=user.Password, UserType = user.UserType};
    }
    [Serializable]
    [XmlRoot(ElementName = "Order")]
    public class _Order
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public static _Order Trans(Order order)=> new _Order { Id = order.Id, AddressId = order.Address.Id, CustomerId = order.Customer.Id, UserId=order.User.Id};
    }
    [Serializable]
    [XmlRoot(ElementName = "Customer")]
    [XmlInclude(typeof(_Company))]
    public class _Customer
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Passport { get; set; }
        public string PhoneNumber { get; set; }
        public static _Customer Trans(Customer customer)
        {
            if (customer is Company c)
            {
                return new _Company { Id = c.Id, FIO = c.FIO, Passport = customer.Passport, CompanyName = c.CompanyName, INN = c.INN,PhoneNumber = c.PhoneNumber };
            }
            else
            {
                return new _Customer
                {
                    Id = customer.Id,
                    FIO = customer.FIO,
                    Passport = customer.Passport,
                    PhoneNumber = customer.PhoneNumber
                };
            }
        }
    }
    [Serializable]
    [XmlRoot(ElementName = "Company")]
    public class _Company : _Customer
    {
        public string CompanyName { get; set; }
        public string INN { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "OrderEntry")]
    public class _OrderEntry
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StatusId { get; set; }
        public int MeterId { get; set; }
        public int PersonId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string RegNum { get; set; }
        public static _OrderEntry Trans(OrderEntry orderEntry) => 
            new _OrderEntry { Id=orderEntry.Id, MeterId = orderEntry.Meter.Id, OrderId = orderEntry.Order.Id, PersonId = orderEntry.PersonId??-1, StatusId = orderEntry.Status.Id,
            startTime = orderEntry.StartTime??DateTime.MinValue, endTime = orderEntry.EndTime??DateTime.MinValue, RegNum = orderEntry.RegNumer??string.Empty};
    }
    [Serializable]
    [XmlRoot(ElementName = "Meter")]
    public class _Meter
    {
        public int Id { get; set; }
        public int MeterTypeId { get; set; }
        public string Name { get; set; }
        public static _Meter Trans(Meter meter) => new _Meter { Id = meter.Id, Name = meter.Name, MeterTypeId = meter.MeterType.Id};
    }
    [Serializable]
    [XmlRoot(ElementName = "MeterType")]
    public class _MeterType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static _MeterType Trans(MeterType meter) => new _MeterType { Id = meter.Id, Name = meter.Name};
    }
    [Serializable]
    [XmlRoot(ElementName = "Status")]
    public class _Status
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static _Status Trans(Status status)=> new _Status { Id = status.Id, Name = status.Name};
    }
    [Serializable]
    [XmlRoot(ElementName = "Person")]
    public class _Person
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public static _Person Trans(Person person)=> new _Person { Id = person.Id, FIO = person.FIO};
    }
    [Serializable]
    [XmlRoot(ElementName = "Stavka")]
    public class _Stavka
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int MeterTypeId { get; set; }
        public static _Stavka Trans(Stavka stavka)=> new _Stavka { Id = stavka.Id, MeterTypeId = stavka.MeterType.Id, PersonId = stavka.Person.Id};
    }
    [Serializable]
    [XmlRoot(ElementName = "Database")]
    public class LessCont
    {
        private DataManager dataManager;
        public LessCont(DataManager DataManager) {
            dataManager = DataManager;
            OrderSet = new List<_Order>(dataManager.cont.OrderSet.Count());
            OrderEntrySet = new List<_OrderEntry>(dataManager.cont.OrderEntrySet.Count());
            StatusSet = new List<_Status>(dataManager.cont.StatusSet.Count());
            CustomerSet = new List<_Customer>(dataManager.cont.CustomerSet.Count());
            MeterSet = new List<_Meter>(dataManager.cont.MeterSet.Count());
            MeterTypeSet = new List<_MeterType>(dataManager.cont.MeterTypeSet.Count());
            StavkaSet = new List<_Stavka>(dataManager.cont.StavkaSet.Count());
            PersonSet = new List<_Person>(dataManager.cont.PersonSet.Count());
            AddressSet = new List<_Address>(dataManager.cont.AddressSet.Count());
            HouseSet = new List<_House>(dataManager.cont.HouseSet.Count());
            StreetSet = new List<_Street>(dataManager.cont.StreetSet.Count());
            CitySet = new List<_City>(dataManager.cont.CitySet.Count());
            UserSet = new List<_User>(dataManager.cont.UserSet.Count());
        }
        [XmlArrayItem(ElementName ="Order")]
        public List<_Order> OrderSet;
        [XmlArrayItem(ElementName = "OrderEntry")]
        public  List<_OrderEntry> OrderEntrySet;
        [XmlArrayItem(ElementName = "Status")]
        public  List<_Status> StatusSet;
        [XmlArrayItem(ElementName = "Customer")]
        public  List<_Customer> CustomerSet;
        [XmlArrayItem(ElementName = "Meter")]
        public  List<_Meter> MeterSet;
        [XmlArrayItem(ElementName = "MeterType")]
        public  List<_MeterType> MeterTypeSet;
        [XmlArrayItem(ElementName = "Stavka")]
        public  List<_Stavka> StavkaSet;
        [XmlArrayItem(ElementName = "Person")]
        public  List<_Person> PersonSet;
        [XmlArrayItem(ElementName = "Address")]
        public  List<_Address> AddressSet;
        [XmlArrayItem(ElementName = "House")]
        public  List<_House> HouseSet;
        [XmlArrayItem(ElementName = "Street")]
        public  List<_Street> StreetSet;
        [XmlArrayItem(ElementName = "City")]
        public  List<_City> CitySet;
        [XmlArrayItem(ElementName = "User")]
        public List<_User> UserSet;
        public void Create()
        {
            foreach (Order o in dataManager.cont.OrderSet) OrderSet.Add(_Order.Trans(o));
            foreach (OrderEntry o in dataManager.cont.OrderEntrySet) OrderEntrySet.Add(_OrderEntry.Trans(o));
            foreach (Status o in dataManager.cont.StatusSet) StatusSet.Add(_Status.Trans(o));
            foreach (Customer o in dataManager.cont.CustomerSet) CustomerSet.Add(_Customer.Trans(o));
            foreach (Meter o in dataManager.cont.MeterSet) MeterSet.Add(_Meter.Trans(o));
            foreach (MeterType o in dataManager.cont.MeterTypeSet) MeterTypeSet.Add(_MeterType.Trans(o));
            foreach (Stavka o in dataManager.cont.StavkaSet) StavkaSet.Add(_Stavka.Trans(o));
            foreach (Person o in dataManager.cont.PersonSet) PersonSet.Add(_Person.Trans(o));
            foreach (Address o in dataManager.cont.AddressSet) AddressSet.Add(_Address.Trans(o));
            foreach (House o in dataManager.cont.HouseSet) HouseSet.Add(_House.Trans(o));
            foreach (Street o in dataManager.cont.StreetSet) StreetSet.Add(_Street.Trans(o));
            foreach (City o in dataManager.cont.CitySet) CitySet.Add(_City.Trans(o));
            foreach (User o in dataManager.cont.UserSet) UserSet.Add(_User.Trans(o));
        }
        public void Update()
        {
            dataManager.InicializeNextId();
            int idx = dataManager.NextId[EntityTypes.City];
            Dictionary<int, int> CityIndices = new Dictionary<int, int>();
            foreach (_City c in CitySet)
            {
                if (dataManager.CityRepos.Add(c.Name,out string Res))
                {
                    CityIndices.Add(c.Id, ++idx);
                }else
                {
                    CityIndices.Add(c.Id, (from p in dataManager.cont.CitySet where p.Name == c.Name select p.Id).First());
                }
            }
            Dictionary<int, int> StreetIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.Street];
            foreach (_Street s in StreetSet)
            {
                int cid = CityIndices[s.CityId];
                if (dataManager.StreetRepos.Street(s.Name, dataManager.CityRepos.Find(cid), out string Res))
                {
                    StreetIndices.Add(s.Id, ++idx);
                }else
                {
                    StreetIndices.Add(s.Id, (from p in dataManager.cont.StreetSet where p.Name == s.Name && p.City.Id == cid select p.Id).First());
                }
            }
           Dictionary<int, int> HouseIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.House];
            foreach (_House h in HouseSet)
            {
                int sid = StreetIndices[h.StreetId];
                if (dataManager.HouseRepos.Add(h.Number, dataManager.StreetRepos.Find(sid), out string Res))
                    HouseIndices.Add(h.Id, ++idx);
                else
                    HouseIndices.Add(h.Id, (from p in dataManager.cont.HouseSet where p.Number == h.Number && p.Street.Id == sid select p.Id).First());
            }
            Dictionary<int, int> AddressesIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.Address];
            foreach(_Address a in AddressSet)
            {
                int hid = HouseIndices[a.HouseId];
                if (dataManager.AddressRepos.Add(a.Flat, dataManager.HouseRepos.Find(hid), out string Res))
                    AddressesIndices.Add(a.Id, ++idx);
                else
                    AddressesIndices.Add(a.Id, (from p in dataManager.cont.AddressSet where p.Flat == a.Flat && p.House.Id == hid select p.Id).First());
            }
            Dictionary<int, int> UserIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.User];
            foreach(_User u in UserSet)
            {
                if (dataManager.UsersRepos.Add(u.UserType, u.Login, u.Password, out string Res))
                    UserIndices.Add(u.Id, ++idx);
                else
                    UserIndices.Add(u.Id, (from p in dataManager.cont.UserSet where p.Login == u.Login select p.Id).First());
            }
            Dictionary<int, int> MeterTypeindices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.MeterType];
            foreach (_MeterType m in MeterTypeSet)
            {
                if (dataManager.MeterTypeRepos.Add(m.Name, out string Res))
                    MeterTypeindices.Add(m.Id, ++idx);
                else
                    MeterTypeindices.Add(m.Id, (from p in dataManager.cont.MeterTypeSet where p.Name == m.Name select p.Id).First());
            }
            Dictionary<int, int> MeterIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.Meter];
            foreach (_Meter m in MeterSet)
            {
                int mt = MeterTypeindices[m.MeterTypeId];
                if (dataManager.MeterRepos.Add(m.Name, dataManager.MeterTypeRepos.Find(mt), out string Res))
                    MeterIndices.Add(m.Id, ++idx);
                else
                    MeterIndices.Add(m.Id, (from p in dataManager.cont.MeterSet where p.Name == m.Name && p.MeterType.Id == mt select p.Id).First());
            }
            Dictionary<int, int> PersonIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.Person];
            foreach (_Person p in PersonSet)
            {
                if (dataManager.PersonRepos.Add(p.FIO, out string Res))
                    PersonIndices.Add(p.Id, ++idx);
                else
                    PersonIndices.Add(p.Id, (from q in dataManager.cont.PersonSet where q.FIO == p.FIO select q.Id).First());
            }
            Dictionary<int, int> StavkaIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.Stavka];
            foreach (_Stavka s in StavkaSet)
            {
                int mt = MeterTypeindices[s.MeterTypeId];
                int pid = PersonIndices[s.PersonId];
                if (dataManager.StavkaRepos.Add(dataManager.MeterTypeRepos.Find(mt), dataManager.PersonRepos.Find(pid), out string Res))
                    StavkaIndices.Add(s.Id, ++idx);
                else
                    StavkaIndices.Add(s.Id, (from p in dataManager.cont.StavkaSet where p.MeterType.Id == mt && p.Person.Id ==pid select p.Id).First());
            }
            Dictionary<int, int> CustomerIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.Customer];
            foreach (_Customer c in CustomerSet)
            {
                if (c is _Company m)
                {
                    if (dataManager.CompanyRepos.Add(m.FIO, m.Passport,m.PhoneNumber, m.CompanyName, m.INN, out string Res))
                        CustomerIndices.Add(m.Id, ++idx);
                    else
                        CustomerIndices.Add(m.Id, (from p in dataManager.cont.CustomerSet where p is Company && (p as Company).INN == m.INN select p.Id).First());
                }else
                {
                    if (dataManager.CustomerRepos.Add(c.FIO, c.Passport, c.PhoneNumber, out string Res))
                        CustomerIndices.Add(c.Id, ++idx);
                    else
                        CustomerIndices.Add(c.Id, (from p in dataManager.cont.CustomerSet where !(p is Company) && p.Passport == c.Passport select p.Id).First());
                }
            }
            Dictionary<int, int> OrderIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.Order];
            foreach (_Order o in OrderSet)
            {
                int uid = UserIndices[o.UserId];
                int cid = CustomerIndices[o.CustomerId];
                int aid = AddressesIndices[o.AddressId];
                dataManager.OrderRepos.Add(dataManager.UsersRepos.Find(uid), dataManager.CustomerRepos.Find(cid),
                      dataManager.AddressRepos.Find(aid), out string Res,out int id,++idx);
                    OrderIndices.Add(o.Id, idx);
            }
            Dictionary<int, int> StatusIndices = new Dictionary<int, int>();
            idx = dataManager.NextId[EntityTypes.Status];
            foreach (_Status s in StatusSet)
            {
                if (dataManager.StatusRepos.Add(s.Name, out string res))
                    StatusIndices[s.Id] = ++idx;
                else
                    StatusIndices[s.Id] = (from p in dataManager.cont.StatusSet where p.Name == s.Name select p.Id).First();
            }
            foreach (_OrderEntry o in OrderEntrySet)
            {
                int oid = OrderIndices[o.OrderId];
                int mid = MeterIndices[o.MeterId];
                int pid = o.PersonId != null ? PersonIndices[(int)o.PersonId] : -1;
                int sid = StatusIndices[o.StatusId];
                dataManager.OrderEntryRepos.Add(dataManager.OrderRepos.Find(oid), o.startTime, o.endTime, o.RegNum, dataManager.MeterRepos.Find(mid),
                    o.PersonId != null ? dataManager.PersonRepos.Find(pid) : null, dataManager.StatusRepos.Find(sid), out string Res);
            }
        }
    }
}

