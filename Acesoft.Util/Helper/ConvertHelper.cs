using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Acesoft.Util
{
    public static class ConvertHelper
    {
        public static IDictionary<string, object> ObjectToDictionary(object value)
        {
            var dictionary = value as IDictionary<string, object>;
            if (dictionary != null)
            {
                return new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
            }

            dictionary = new Dictionary<string, object>();
            if (value != null)
            {
                foreach (var property in Dynamic.GetProperties(value.GetType()))
                {
                    dictionary[property.Name] = Dynamic.GetPropertyGetter(property)(value);
                }
            }
            return dictionary;
        }

        public static T DictionaryToObject<T>(IDictionary<string, object> source) where T : new()
        {
            var value = new T();
            foreach (var property in Dynamic.GetProperties(value.GetType()))
            {
                if (source.ContainsKey(property.Name))
                {
                    Dynamic.GetPropertySetter(property)(value, source[property.Name]);
                }
            }
            return value;
        }

        /// <summary>
        /// 此处使用DynamicObject而非ExpandoObject，以提高性能
        /// </summary>
        public static dynamic DictionaryToAnonymous(IDictionary<string, object> source)
        {
            return new DictDynamicObject(source);
        }

        class DictDynamicObject : DynamicObject
        {
            IDictionary<string, object> values;
            public DictDynamicObject(IDictionary<string, object> source)
            {
                this.values = source;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return values.TryGetValue(binder.Name, out result);
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                values[binder.Name] = value;
                return true;
            }
        }
    }
}
