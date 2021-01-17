using LedgerLib.Infrastructure;

using System;
using System.Linq;

namespace LedgerLib
{
    public static class ExtensionMethods
    {
        public static string Ordinalize(this int value)
        {
            if (value <= 0)
            {
                return value.ToString();
            }
            return (value % 100) switch
            {
                11 or 12 or 13 => value.ToString() + "th",
                _ => (value % 10) switch
                {
                    1 => value.ToString() + "st",
                    2 => value.ToString() + "nd",
                    3 => value.ToString() + "rd",
                    _ => value.ToString() + "th",
                },
            };
        }

        public static string GetDescriptionFromEnumValue<T>(this T value) where T : Enum =>
            typeof(T)
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() is not DescriptionAttribute attr ? value.ToString() : attr.Description;
    }
}
