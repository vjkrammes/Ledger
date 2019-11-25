﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;

namespace LedgerClient.Infrastructure
{
    public static partial class ExtensionMethods
    {
        private static readonly Capitalizer _capitalizer;

        static ExtensionMethods()
        {
            _capitalizer = new Capitalizer();
        }

        public static string Capitalize(this string value) =>
            value.First().ToString().ToUpper() + string.Join(string.Empty, value.Skip(1));

        public static string Caseify(this string value) => _capitalizer.Transform(value);

        public static string GetDescriptionFromEnumValue<T>(this T value) where T : Enum
        {
            return !(typeof(T)
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() is DescriptionAttribute attr) ? value.ToString() : attr.Description;
        }

        public static bool ArrayEquals<T>(this T[] array1, T[] array2)
        {
            if (ReferenceEquals(array1, array2))
            {
                return true;
            }
            if (array1 is null)
            {
                if  (array2 is null)
                {
                    return true;
                }
                return false;
            }
            if (array2 is null)
            {
                return false;
            }
            IEqualityComparer<T> comp = EqualityComparer<T>.Default;
            for (int i = 0; i < array1.Length; i++)
            {
                if (!comp.Equals(array1[i], array2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static T[] ArrayCopy<T>(this T[] source)
        {
            T[] ret = new T[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                ret[i] = source[i];
            }
            return ret;
        }

        public static bool IsBetween(this DateTime date, DateTime start, DateTime end) => date >= start && date <= end;

        public static SolidColorBrush GetBrush(this long argb)
        {
            byte a, r, g, b;
            a = (byte)((argb >> 24) & 0xff);
            r = (byte)((argb >> 16) & 0xff);
            g = (byte)((argb >> 8) & 0xff);
            b = (byte)(argb & 0xff);
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }

        public static string Innermost(this Exception ex)
        {
            if (ex.InnerException is null)
            {
                return ex.Message;
            }
            return ex.InnerException.Innermost();
        }

        public static bool Contains(this string source, string pattern, StringComparison comp) =>
            source.IndexOf(pattern, comp) >= 0;

        public static bool Matches(this string s1, string s2, StringComparison comp = StringComparison.OrdinalIgnoreCase) =>
            s1.Equals(s2, comp);

        public static List<string> ToList(this StringCollection coll)
        {
            List<string> ret = new List<string>();
            foreach (var c in coll)
            {
                ret.Add(c);
            }
            return ret;
        }

        public static void Resort<T, U>(this List<T> coll, Func<T, U> pred) where U : IComparable<U>
        {
            coll.Sort((x, y) => pred.Invoke(x).CompareTo(pred.Invoke(y)));
        }
    }
}