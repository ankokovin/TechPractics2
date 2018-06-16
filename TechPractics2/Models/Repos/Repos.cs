using System;
using System.Collections.Generic;
using System.Data;
using TechPractics2.Models.EDM;
using LinqKit;
using System.Linq.Expressions;
using TSelector = System.Collections.Generic.Dictionary<string, System.Tuple<System.Func<object, object>,System.Type>>;
using TGetComp = System.Collections.Generic.Dictionary<System.Tuple<System.Type, string>, System.Func<object,object, bool>>;

namespace TechPractics2.Models.Repos
{

    public interface IRepos<T>
    {
        IEnumerable<T> Select(Func<T, bool> predicate);

    }

    public enum LogicOp { None, And, Or }

    public abstract class Repos<T> : IRepos<T>
    {
        protected Model1Container cont;
        protected bool AllowCascade;
        protected bool CheckInputs;
        public Repos(Model1Container model, bool checkInputs = true, bool allowCascade = false)
        {
            cont = model;
            CheckInputs = checkInputs;
            AllowCascade = allowCascade;
        }

        public static DataTable Table(IEnumerable<T> enumerable, IEnumerable<Tuple<string,Func<object,object>>> selectors)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("№");
            foreach (var selector in selectors)
            {
                dataTable.Columns.Add(selector.Item1);
            }
            int i = 0;
            foreach (var item in enumerable)
            {
                var row = dataTable.NewRow();
                row["№"] = ++i;
                foreach (var selector in selectors)
                {
                    object prod = selector.Item2(item);
                    if (prod is IFullDisc disc)
                    {
                        row[selector.Item1] = disc;
                    }
                    else row[selector.Item1] = prod;
                }
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        public static DataTable Table(IEnumerable<T> enumerable)
        {
            List<Tuple<string, Func<object, object>>> selectors = new List<Tuple<string, Func<object, object>>>();
            foreach(var item in Selectors[typeof(T)])
            {
                if (item.Key!="FullDisc")
                    selectors.Add(Tuple.Create(item.Key, item.Value.Item1));
            }
           
            return Table(enumerable,selectors);
        }

        public abstract IEnumerable<T> Select(Func<T, bool> predicate);

        


     
        private static void InitializeSelectors()
        {
            _selectors = new Dictionary<Type, TSelector>
            {
                {
                    typeof(Address),
                    new TSelector
            {
                { "Flat", Tuple.Create(new Func<object, object>(x => (x as Address)?.Flat),typeof(int))},
                { "House", Tuple.Create(new Func<object, object>(x => (x as Address)?.House),typeof(House)) },
                { "FullDisc", Tuple.Create(new Func<object, object>(x => (x as Address)?.FullDisc),typeof(string)) }
            }
                },
                {
                    typeof(House),
                    new TSelector
            {
                {"Number", Tuple.Create(new Func<object, object>(x=>(x as House)?.Number),typeof(string)) },
                {"Street", Tuple.Create(new Func<object, object>(x=>(x as House)?.Street),typeof(Street)) },
                { "FullDisc", Tuple.Create(new Func<object, object>(x => (x as House)?.FullDisc),typeof(string)) }
            }
                },
                {
                    typeof(Street),
                    new TSelector
            {
                 {"Name", Tuple.Create(new Func<object, object>(x=>(x as Street)?.Name),typeof(string)) },
                 {"City", Tuple.Create(new Func<object, object>(x=>(x as Street)?.City),typeof(City)) },
                 { "FullDisc", Tuple.Create(new Func<object, object>(x => (x as Street)?.FullDisc),typeof(string)) }
            }
                },
                {
                    typeof(City),
                    new TSelector
            {
                 {"Name", Tuple.Create(new Func<object, object>(x=>(x as City)?.Name),typeof(string)) },
                 { "FullDisc", Tuple.Create(new Func<object, object>(x => (x as City)?.FullDisc),typeof(string)) }
            }
                },

                {
                    typeof(Customer),
                    new TSelector
            {
                { "FIO", Tuple.Create(new Func<object, object>(x => (x as Customer)?.FIO),typeof(string)) },
                { "PhoneNumber", Tuple.Create(new Func<object, object>(x => (x as Customer)?.PhoneNumber),typeof(string)) },
                { "Passport", Tuple.Create(new Func<object, object>(x => (x as Customer)?.Passport),typeof(string)) },
                { "CompanyName", Tuple.Create(new Func<object, object>(x => (x as Company)?.CompanyName),typeof(string)) },
                { "INN", Tuple.Create(new Func<object, object>(x => (x as Company)?.INN),typeof(string)) },
                { "FullDisc", Tuple.Create(new Func<object, object>(x => (x as Customer)?.FullDisc),typeof(string)) }
            }
                },
                {
                    typeof(Order),
                    new TSelector
            {
                {"Customer", Tuple.Create(new Func<object,object>(x=>(x as Order)?.Customer),typeof(Customer)) },
                {"Address", Tuple.Create(new Func<object,object>(x=>(x as Order)?.Address),typeof(Address)) },
                {"Id", Tuple.Create(new Func<object, object>(x=>(x as Order)?.Id),typeof(int)) },
                {"User", Tuple.Create(new Func<object, object>(x=>(x as Order)?.User),typeof(User)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as Order)?.FullDisc),typeof(string)) },
            }
                },
                {
                    typeof(OrderEntry),
                    new TSelector
            {
                {"StartTime", Tuple.Create(new Func<object,object>(x=>(x as OrderEntry)?.StartTime),typeof(DateTime?)) },
                {"EndTime", Tuple.Create(new Func<object,object>(x=>(x as OrderEntry)?.EndTime),typeof(DateTime?)) },
                {"Status", Tuple.Create(new Func<object, object>(x=>(x as OrderEntry)?.Status),typeof(Status)) },
                {"Meter", Tuple.Create(new Func<object, object>(x=>(x as OrderEntry)?.Meter),typeof(Meter)) },
                {"Order", Tuple.Create(new Func<object, object>(x=>(x as OrderEntry)?.Order),typeof(Order)) },
                {"Person", Tuple.Create(new Func<object, object>(x=>(x as OrderEntry)?.Person),typeof(Person)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as OrderEntry)?.FullDisc),typeof(string)) },
            }
                },
                {
                    typeof(Meter),
                    new TSelector
            {
                {"MeterType", Tuple.Create(new Func<object,object>(x=>(x as Meter)?.MeterType),typeof(MeterType)) },
                {"Name", Tuple.Create(new Func<object,object>(x=>(x as Meter)?.Name),typeof(string)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as Meter)?.FullDisc),typeof(string)) },
            }
                },
                 {
                    typeof(MeterType),
                    new TSelector
            {
                {"Name", Tuple.Create(new Func<object,object>(x=>(x as MeterType)?.Name),typeof(string)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as MeterType)?.FullDisc),typeof(string)) },
            }
                },
                {
                    typeof(Person),
                    new TSelector
            {
                {"FIO", Tuple.Create(new Func<object,object>(x=>(x as Person)?.FIO),typeof(string)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as Person)?.FullDisc),typeof(string)) },
            }
                },
                {
                    typeof(Status),
                    new TSelector
            {
                {"Name", Tuple.Create(new Func<object,object>(x=>(x as Status)?.Name),typeof(string)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as Status)?.FullDisc),typeof(string)) },
            }
                },
                {
                    typeof(Stavka),
                    new TSelector
            {
                {"MeterType", Tuple.Create(new Func<object,object>(x=>(x as Stavka)?.MeterType),typeof(MeterType)) },
                {"Person", Tuple.Create(new Func<object,object>(x=>(x as Stavka)?.Person),typeof(Person)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as Stavka)?.FullDisc),typeof(string)) },
            }
                },
                {
                    typeof(User),
                    new TSelector
            {
                {"Login", Tuple.Create(new Func<object,object>(x=>(x as User)?.Login),typeof(string)) },
                {"UserType", Tuple.Create(new Func<object,object>(x=>(x as User)?.UserType),typeof(UserType)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as User)?.FullDisc),typeof(string)) },
            }
                },
                {
                    typeof(UserToCustomer),
                    new TSelector
            {
                {"User", Tuple.Create(new Func<object,object>(x=>(x as UserToCustomer)?.User),typeof(User)) },
                {"Customer", Tuple.Create(new Func<object,object>(x=>(x as UserToCustomer)?.Customer),typeof(Customer)) },
                {"FullDisc", Tuple.Create(new Func<object, object>(x=>(x as UserToCustomer)?.FullDisc),typeof(string)) },
            }
                },
            };
        }

        private static Dictionary<Type, TSelector> _selectors;

        public static Dictionary<Type, TSelector> Selectors
        {
            get
            {
                if (_selectors == null)
                {
                    InitializeSelectors();
                }
                return _selectors;
            }
        }

        private static void InitializeComp()
        {
            _comparisons = new TGetComp();

            _comparisons.Add(Tuple.Create(typeof(int), "=="), (x, y) => x == y);
        }

        private static TGetComp _comparisons;

        public static  TGetComp Comparrisons
        {
            get
            {
                if (_comparisons == null)
                    InitializeComp();
                return _comparisons;
            }
        }
        

        public static Expression<Func<T, bool>> CreatePredicate(string input)
        {
            var words = input.Split();
            var expressions = new Stack<Expression<Func<T, bool>>>();
            var ops = new Stack<LogicOp>();
            bool s = true;
            var res = PredicateBuilder.New<T>();
            var temp = PredicateBuilder.New<T>();
            LogicOp op = LogicOp.None;
            Func<object,object> prevSelector = null;
            Type currentType = typeof(T);

            for (int i = 0; i < words.Length; i++)
            {
                switch (words[i])
                {
                    case "(":
                        expressions.Push(res);
                        res = PredicateBuilder.New<T>();
                        temp = PredicateBuilder.New<T>();
                        ops.Push(op);
                        op = LogicOp.None;
                        break;
                    case ")":
                        var prev = expressions.Pop();
                        op = ops.Pop();
                        if (op == LogicOp.None)
                        {
                            res = prev;
                        }
                        else if (op == LogicOp.And)
                        {
                            res = prev.And(res);
                        }
                        else if (op == LogicOp.Or)
                        {
                            res = prev.Or(res);
                        }
                        break;
                    case "and":
                        op = LogicOp.And;
                        break;
                    case "or":
                        op = LogicOp.Or;
                        break;
                    default:
                        if (Selectors[typeof(T)].ContainsKey(words[i]))
                        {
                            var nselector = Selectors[typeof(T)][words[i]];
                            currentType = nselector.Item2;
                            if (prevSelector == null)
                                prevSelector = nselector.Item1;
                            else
                                prevSelector = new Func<object, object>(x => prevSelector(nselector.Item1(x)));
                        }else if (Comparrisons.ContainsKey(Tuple.Create(currentType, words[i])))
                        {

                        }
                        break;

                }
            }

            throw new NotImplementedException();
        }

    }

}
