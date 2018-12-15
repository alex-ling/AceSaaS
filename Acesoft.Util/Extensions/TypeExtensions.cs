using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Acesoft
{
    public static class TypeExtensions
    {
        public static bool IsAnonymous(this Type type)
        {
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public static IList MakeEmptyList(this Type type)
        {
            var listType = typeof(List<>).MakeGenericType(type);
            return (IList)Dynamic.GetInstanceCreator(listType)();
        }

        public static T ToObject<T>(this object obj)
        {
            return ToObject<T>(obj, default(T));
        }

        public static T ToObject<T>(this object obj, T defaultValue)
        {
            if (obj == null || obj == Convert.DBNull || !obj.ToString().HasValue())
            {
                return defaultValue;
            }

            if (obj is T)
            {
                return (T)obj;
            }

            return obj.ToString().ToObject<T>();
        }
    }
}
