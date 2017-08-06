using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace reflection
{
    public class Stripper
    {
        public static object Strip(object o)
        {
            _ = o ?? throw new ArgumentNullException(nameof(o));

            var objectType = o.GetType();

            if(objectType.GetTypeInfo().IsEnum)
                return o;

            if(objectType.Namespace != nameof(reflection))
            {
                if(!objectType.IsConstructedGenericType)
                    return o;
                
                var typeArguments = objectType.GetGenericArguments().ToList();
                if(typeArguments.Count == 0)
                    return o;

                if(!typeArguments.Exists(t => t.Namespace == nameof(reflection)))
                    return o;

                if(objectType.IsArray || objectType.GetTypeInfo().GetInterface("IEnumerable") != null)
                {
                    //var ienumGenType = objectType.GenericTypeArguments[0];
                    var listInstance = Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(object))) as IList;
                    var enumerableObject = o as IEnumerable;
                    
                    foreach(var obj in enumerableObject)
                    {
                        var unwrappedValue = Strip(obj);
                        listInstance.Add(unwrappedValue);   
                    }
                    return listInstance;
                }
            }
            
            

            var propDict = new Dictionary<string, object>();
            var propList = objectType.GetProperties();
            foreach (var prop in propList)
            {
                var t = prop.PropertyType;
                
                if (t.IsConstructedGenericType && t.GetGenericTypeDefinition().FullName == typeof(reflection.Wrapper<>).FullName)
                {
                    object unwrappedValue = null;
                    var propValue = prop.GetValue(o);
                    if(propValue != null) {
                        var dataVal = t.GetProperty("data").GetValue(propValue);
                        if (dataVal != null)
                            unwrappedValue = Strip(dataVal);
                    }
                    propDict.Add(prop.Name, unwrappedValue);
                }
                else
                {
                    object unwrappedValue = null;
                    var propValue = prop.GetValue(o);
                    if(propValue != null) {
                        unwrappedValue = Strip(propValue);
                    }
                    propDict.Add(prop.Name, unwrappedValue);
                }

            }
            return new StrippedObject(propDict);
        }
    }
}
