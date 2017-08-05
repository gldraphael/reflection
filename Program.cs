using System;
using Newtonsoft.Json;

namespace reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            print("Hello World!");
            print(Person.Mock());
        }

        static void print(object o)
        {
            print(JsonConvert.SerializeObject(o));
        }
        static void print(string s)
        {
            Console.WriteLine(s);
        }
    }
}
