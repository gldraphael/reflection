using System.Reflection;
using System.Reflection.Emit;
using System.Dynamic;
using System.Collections.Generic;

namespace reflection
{
    /// <summary>
    /// Taken from <see href="https://stackoverflow.com/a/15819760/1750297">https://stackoverflow.com/a/15819760/1750297</see>
    /// </summary>
    public sealed class StrippedObject : DynamicObject
    {
        private readonly Dictionary<string, object> _properties;

        public StrippedObject(Dictionary<string, object> properties)
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
