using System;
using System.Linq;
using Newtonsoft.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace reflection
{
    public class Program
    {
        static Person model = PersonMocker.GetMockPerson();

        static void Main(string[] args)
        {
            if(args.Length > 0 && args[0] == "benchmark")
            {
                runBenchmarks();
            }
            else
            {
                print("Hello World!");
                print(model);
                print(Stripper.Strip(model));
                print(staticUnwrap(model));    
            }
        }

#region Benchmarks
        public static BenchmarkDotNet.Reports.Summary runBenchmarks()
        {
            return BenchmarkRunner.Run<Program>();            
        }

        [Benchmark]
        public static object test1() => Stripper.Strip(model);

        [Benchmark]
        public static object test2() => staticUnwrap(model);

        static object staticUnwrap(Person p)
        {
            return new {
                p.Name,
                Favourites = p.Favourites?.data,
                Education = p.Education?.data.Select(e => new {
                    e.Major,
                    e.Type,
                    Grade = e.Grade?.data
                })
            };
        }
#endregion

#region Print functions

        static void print(object o)
        {
            print(JsonConvert.SerializeObject(o));
        }
        static void print(string s)
        {
            Console.WriteLine(s);
        }

    }
#endregion

}
