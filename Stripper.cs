using System;
using System.Collections.Generic;
using System.Reflection;

namespace reflection
{
    public class Stripper
    {
        public static object Strip(object o)
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
            return new StrippedObject(propDict);
        }
    }
}
