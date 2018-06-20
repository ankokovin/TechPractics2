using System;
using System.Collections.Generic;
using System.Data;
using TechPractics2.Models.EDM;
using LinqKit;
using System.Linq.Expressions;
using TSelector = System.Collections.Generic.Dictionary<string, System.Tuple<System.Linq.Expressions.Expression<System.Func<object, object>>,System.Type>>;
using TGetComp = System.Collections.Generic.Dictionary<System.Tuple<System.Type, string>, System.Linq.Expressions.Expression<System.Func<object,object, bool>>>;
using System.Reflection;
using System.Linq;
using System.Collections.ObjectModel;

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

        public static DataTable Table(IEnumerable<T> enumerable, IEnumerable<Tuple<string,Expression<Func<object,object>>>> selectors)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("№");
            foreach (var selector in selectors)
            {
                dataTable.Columns.Add(selector.Item1);
            }
            int i = 0;
            List<Tuple<string,Func<object, object>>> compiledSelectors = new List<Tuple<string, Func<object, object>>>();
            foreach (var selector in selectors)
            {
                compiledSelectors.Add(Tuple.Create(selector.Item1, selector.Item2.Compile()));
            }
            foreach (var item in enumerable)
            {
                var row = dataTable.NewRow();
                row["№"] = ++i;
                foreach( var selector in compiledSelectors)
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
            List<Tuple<string, Expression<Func<object, object>>>> selectors = new List<Tuple<string, Expression<Func<object, object>>>>();
            foreach(var item in Selectors[typeof(T)])
            {
                if (item.Key!="FullDisc")
                    selectors.Add(Tuple.Create(item.Key, item.Value.Item1));
            }
           
            return Table(enumerable,selectors);
        }

        public abstract IEnumerable<T> Select(Func<T, bool> predicate);


        private static TSelector GetTSelector<T1>()
        {
            TSelector selector = new TSelector();
            var prop = typeof(T1).GetProperties();
            foreach (var property in prop)
            {
                Expression<Func<object, object>> exp = u => u.GetType().InvokeMember(property.Name, BindingFlags.GetProperty, null, u, null);
                selector.Add(property.Name, Tuple.Create(exp, property.PropertyType));
            }
            return selector;
        }

        private static void InitializeSelectors()
        {
            _selectors = new Dictionary<Type, TSelector>
            {
                { typeof(Address), GetTSelector<Address>() },
                { typeof(House), GetTSelector<House>() },
                { typeof(Street), GetTSelector<Street>() },
                { typeof(City), GetTSelector<City>() },
                { typeof(Customer), GetTSelector<Company>() },
                { typeof(Order), GetTSelector<Order>() },
                { typeof(OrderEntry), GetTSelector<OrderEntry>() },
                { typeof(Meter), GetTSelector<Meter>() },
                { typeof(MeterType), GetTSelector<MeterType>() },
                { typeof(Status), GetTSelector<Status>() },
                { typeof(Person), GetTSelector<Person>() },
                { typeof(Stavka), GetTSelector<Stavka>() },
                { typeof(User), GetTSelector<User>() },
            };
            Expression<Func<object,object>> isCompany = x => x is Company;
            _selectors[typeof(Customer)].Add("IsCompany", Tuple.Create(isCompany, typeof(bool)));
            _selectors[typeof(User)].Remove("Password");
            /*
            {
                {
                    typeof(Address),
                    new TSelector
            {
                { "Flat", Tuple.Create(Expression<Func<object, object>> a = x => (x as Address)?.Flat),typeof(int))},
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
            */
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
            _comparisons = new TGetComp
            {
                { Tuple.Create(typeof(int), "=="), (x, y) => x == y },

                { Tuple.Create(typeof(int), "!-"), (x, y) => x != y },

                { Tuple.Create(typeof(int), "<"), (x, y) => (int)x < (int)y },

                { Tuple.Create(typeof(int), ">"), (x, y) => (int)x > (int)y },

                { Tuple.Create(typeof(DateTime?), "=="), (x, y) => (x == y) || (x == null && y == null) },

                { Tuple.Create(typeof(DateTime?), "!-"), (x, y) => x != y || (x == null ^ y == null) },

                { Tuple.Create(typeof(DateTime?), "<"), (x, y) => (x == null && y != null) || (DateTime?)x < (DateTime?)y },

                { Tuple.Create(typeof(DateTime?), ">"), (x, y) => (y == null && x != null) || (DateTime?)x > (DateTime?)y },

                { Tuple.Create(typeof(String), "=="), (x, y) => x == y },

                { Tuple.Create(typeof(String), "!-"), (x, y) => x != y }
            };
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
        
        public static object Parse(string input, Type t)
        {
            if (Type.GetTypeCode(t) == TypeCode.Int32)
            {
                return int.Parse(input);
            }
            else if (Type.GetTypeCode(t) == TypeCode.String)
            {
                return input;
            }
            else if (Type.GetTypeCode(t) == TypeCode.DateTime)
            {
                return DateTime.Parse(input);
            }
            else throw new NotSupportedException();
        }


        private static Expression<Func<T, bool>> GetPredicate(string property, string sign, string value)
        {
            var parameter = Expression.Parameter(typeof(T));
            MemberExpression body = null;
            var properties = property.Split('.');
            foreach (var propertyName in properties)
            {
                Expression instance = body;
                if (body == null)
                    instance = parameter;
                body = Expression.Property(instance, propertyName);
            }
            Type constant = properties[properties.Length - 1] == "Id" ? typeof(int) :
                properties[properties.Length - 1] == "StartTime" || properties[properties.Length - 1] == "EndTime" ? typeof(DateTime?) : typeof(string);
            var mconst = Expression.Constant(Parse(value,constant ));

            Expression compare=null;
            if (sign == "==")
            {
                 compare = Expression.Equal(body, mconst);
            }else if (sign == "!=")
            {
                compare = Expression.NotEqual(body, mconst);
            }else if (sign == "<")
            {
                compare = Expression.LessThan(body, mconst);
            }else if (sign == ">")
            {
                compare = Expression.GreaterThan(body, mconst);
            }else if (sign == "<=")
            {
                compare = Expression.LessThanOrEqual(body, mconst);
            }else if (sign == ">=")
            {
                compare = Expression.GreaterThanOrEqual(body, mconst);
            }

            var lambda =  Expression.Lambda<Func<T,bool>>(compare , parameter);

            return lambda.Expand();
        }

        public static Expression<Func<T, bool>> CreatePredicate(string input)
        {
            var words = input.Split();
            var expressions = new Stack<Expression<Func<T, bool>>>();
            var ops = new Stack<LogicOp>();
            var res = PredicateBuilder.New<T>();
            var temp = PredicateBuilder.New<T>();
            LogicOp op = LogicOp.None;
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
                        var exp = GetPredicate(words[i],words[i+1],words[i+2]);
                        if (op == LogicOp.And)
                        {
                            res = PredicateBuilder.And(res, exp);
                        }else if (op == LogicOp.Or)
                        {
                            res = PredicateBuilder.Or(res, exp);
                        }else
                        {
                            res = exp;
                        }
                        i += 2;
                        break;
                        
                }
            }

            return res;
        }

    }

}