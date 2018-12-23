using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft
{
    public static class Extensions
    {
        public static T If<T>(this T t, bool predicate, params Action<T>[] actions)
        {
            if (predicate)
            {
                actions.Each(action => action(t));
            }
            return t;
        }

        public static T If<T>(this T t, Predicate<T> predicate, params Action<T>[] actions)
        {
            if (predicate(t))
            {
                actions.Each(action => action(t));
            }
            return t;
        }

        public static T While<T>(this T t, Predicate<T> predicate, params Action<T>[] actions)
        {
            while (predicate(t))
            {
                actions.Each(action => action(t));
            }
            return t;
        }

        public static T Do<T>(this T t, params Action<T>[] actions)
        {
            actions.Each(action => action(t));
            return t;
        }
    }
}
