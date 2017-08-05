using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Dynamic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace reflection
{
    public class Program
    {
        static Person model = Person.Mock();
        static void Main(string[] args)
        {
            print("Hello World!");
            //var model = Person.Mock();
            print(model);
            print(unwrap(model));

            var summary = BenchmarkRunner.Run<Program>();
        }

        [Benchmark]
        public static object test1() => unwrap(model);

        [Benchmark]
        public static object test2() => staticUnwrap(model);

        static object staticUnwrap(Person p)
        {
            return new {
                p.Name,
                Favourites = p.Favourites.data
            };
        }

        static object unwrap(object o)
        {
            _ = o ?? throw new ArgumentNullException(nameof(o));
            
            var propDict = new Dictionary<string, object>();
            var propList = o.GetType().GetProperties();
            foreach (var prop in propList)
            {
                var t = prop.PropertyType;
                var propValue = prop.GetValue(o);

                if (t.IsConstructedGenericType && t.GetGenericTypeDefinition().FullName == typeof(reflection.Wrapper<>).FullName)
                {
                    var dataVal = t.GetProperty("data").GetValue(propValue);
                    propDict.Add(prop.Name, dataVal);
                }
                else
                {
                    propDict.Add(prop.Name, propValue);
                }

            }
            return GetDynamicObject(propDict);
        }

        static void print(object o)
        {
            print(JsonConvert.SerializeObject(o));
        }
        static void print(string s)
        {
            Console.WriteLine(s);
        }

        public static dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new MyDynObject(properties);
        }

    }

    /// <summary>
    /// Taken from <see href="https://stackoverflow.com/a/15819760/1750297">https://stackoverflow.com/a/15819760/1750297</see>
    /// </summary>
    public sealed class MyDynObject : DynamicObject
    {
        private readonly Dictionary<string, object> _properties;

        public MyDynObject(Dictionary<string, object> properties)
        {
            _properties = properties;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                result = _properties[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_properties.ContainsKey(binder.Name))
            {
                _properties[binder.Name] = value;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
