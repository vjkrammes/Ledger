using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using LedgerClient.ECL.DTO;
using LedgerLib.HistoryEntities;
using LedgerLib.Infrastructure;

namespace LedgerClient.Infrastructure
{

    // convert from ExplorerItemType to icon uri

    [ValueConversion(typeof(ExplorerItemType), typeof(Uri))]
    public sealed class ExplorerItemTypeConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is ExplorerItemType v))
            {
                return null;
            }
            return v.GetIconFromEnumValue();
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from bool to its inverse

    [ValueConversion(typeof(bool), typeof(bool))]
    public sealed class BoolToInverseConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from DueDateType to display

    [ValueConversion(typeof(DueDateType), typeof(string))]
    public sealed class DueDateTypeConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is DueDateType ddt))
            {
                return string.Empty;
            }
            return ddt.GetDescriptionFromEnumValue();
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from object to URI where if null, return null else return checkmark

    [ValueConversion(typeof(object), typeof(Uri))]
    public sealed class ObjectToCheckmarkConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            return value == null ? null : new Uri(Constants.Checkmark, UriKind.Relative);
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from DateTime to either "Never" if default, or a compressed date/time string

    [ValueConversion(typeof(DateTime), typeof(string))]
    public sealed class LastCopyDateConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is DateTime dt))
            {
                return string.Empty;
            }
            if (dt == default)
            {
                return "Never";
            }
            return dt.ToShortDateString() + $" {dt.Hour:00}:{dt.Minute:00}:{dt.Second:00}";
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from encrypted string + salt to plaintext string, parm is true to obscure all but the first 3 letters
    // ONLY WORKS WITH SALTED STRINGS

    public sealed class EncryptedStringWithSaltConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object parm, CultureInfo lang)
        {
            if (values.Length != 2 || values.Any(x => x == DependencyProperty.UnsetValue))
            {
                return string.Empty;
            }
            bool obscure = false;
            if (parm is bool)
            {
                obscure = (bool)parm;
            }
            if (!(values[0] is string cypher) || !(values[1] is byte[] salt))
            {
                return string.Empty;
            }
            var plain = Tools.Locator.StringCypher.Decrypt(cypher, Tools.Locator.PasswordManager.Get(Constants.LedgerPassword), salt);
            if (!obscure)
            {
                return plain;
            }
            return plain[..3] + new string('*', plain.Length - 3);
        }

        public object[] ConvertBack(object value, Type[] t, object parm, CultureInfo lang)
        {
            throw new NotImplementedException();
        }
    }

    // convert from encrypted string to plaintext string, parm is true to obscure all but the first 3 letters
    // ONLY WORKS WITH UNSALTED STRINGS

    [ValueConversion(typeof(string), typeof(string), ParameterType = typeof(bool))]
    public sealed class EncryptedStringConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            bool obscure = false;
            if (parm is string)
            {
                obscure = (bool)parm;
            }
            if (!(value is string v))
            {
                return value;
            }
            var plain = Tools.Locator.StringCypher.Decrypt(v, Tools.Locator.PasswordManager.Get(Constants.LedgerPassword));
            if (!obscure)
            {
                return plain;
            }
            return plain[..3] + new string('*', plain.Length - 3);
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from Visibility to its inverse (hidden => vis, vis => hidden)

    [ValueConversion(typeof(Visibility), typeof(Visibility))]
    public sealed class VisibilityToInverseConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is Visibility v))
            {
                return Visibility.Visible;
            }
            return v == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from bool (ShowPassword) to string for button text

    [ValueConversion(typeof(bool), typeof(string))]
    public sealed class ShowPasswordToButtonTextConverter : IValueConverter
    {
        private static readonly Dictionary<bool, string> _texts = new Dictionary<bool, string>
        {
            [true] = "Hide",
            [false] = "Show"
        };

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is bool v))
            {
                return string.Empty;
            }
            return _texts[v];
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    
    // convert from Statusbar visibility to text for context menu

    [ValueConversion(typeof(Visibility), typeof(string))]
    public sealed class VisibilityToMenuHeaderConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is Visibility v))
                return "??";
            return v switch
            {
                Visibility.Visible => "Hide Status Bar",
                _ => "Show Status Bar"

            };
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from visibility to icon for context menu

    [ValueConversion(typeof(Visibility), typeof(Uri))]
    public sealed class VisibilityToMenuIconConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is Visibility v))
            {
                return null;
            }
            return v switch
            {
                Visibility.Visible => new Uri("/resources/cancel-32.png", UriKind.Relative),
                _ => new Uri("/resources/view-32.png", UriKind.Relative)
            };
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from bool to checkmark uri

    [ValueConversion(typeof(bool), typeof(Uri))]
    public sealed class BoolToCheckmarkConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is bool v) || !v)
                return null;
            return new Uri(Constants.Checkmark, UriKind.Relative);
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert to/from string and decimal

    [ValueConversion(typeof(decimal), typeof(string))]
    public sealed class DecimalConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is decimal d) || d == 0)
                return string.Empty;
            return d.ToString("c2");
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is string v) || string.IsNullOrEmpty(v))
                return 0M;
            string val = v;
            if (v.EndsWith("."))
                val = v.TrimEnd('.');
            if (!decimal.TryParse(val, out decimal d))
                return 0M;
            return d;
        }
    }

    // convert from datetime to display where default = 'initial', maxdate = 'current' else toshortdatestring()

    [ValueConversion(typeof(DateTime), typeof(string))]
    public sealed class DateToDisplayConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is DateTime d))
                return string.Empty;
            if (d == default)
                return "Initial";
            if (d == DateTime.MaxValue)
                return "Current";
            return d.ToShortDateString();
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from account and accountnumber to display format which is <account-type-description> + last 4 digits of account number
    // values[0] is account
    // values[1] is account.number

    public sealed class AccountToDisplayConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object parm, CultureInfo lang)
        {
            if (values.Length != 2 || values.Any(x => x == DependencyProperty.UnsetValue))
                return string.Empty;
            if (!(values[0] is Account a))
                return string.Empty;
            if (!(values[1] is AccountNumber an))
                return string.Empty;
            return Tools.FormatAccountNumber(a, an);
        }

        public object[] ConvertBack(object value, Type[] t, object parm, CultureInfo lang)
        {
            throw new NotImplementedException();
        }
    }

    // convert from AccountHistory and accountnumberhistory to display format as above

    public sealed class AccountHistoryToDisplayConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object parm, CultureInfo lang)
        {
            if (values.Length != 2 || values.Any(x => x == DependencyProperty.UnsetValue))
            {
                return string.Empty;
            }
            if (!(values[0] is AccountHistoryEntity a) || !(values[1] is AccountNumberHistoryEntity an))
            {
                return string.Empty;
            }
            return Tools.FormatAccountNumber(a, an);
            
        }

        public object[] ConvertBack(object value, Type[] t, object parm, CultureInfo lang)
        {
            throw new NotImplementedException();
        }
    }

    // convert from int to display where <= 0 == string.empty

    [ValueConversion(typeof(int), typeof(string))]
    public sealed class IdToDisplayConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is int v) || v <= 0)
            {
                return string.Empty;
            }
            return v.ToString();
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is string v) || !int.TryParse(v, out int val))
            {
                return 0;
            }
            return val;
        }
    }

    // convert from phone number to display. if len != 10 return value, else return formatted string based on parm:
    //
    //  parm = 1        ... (xxx) xxx-xxxx
    //  parm = 2        ... xxx-xxx-xxxx
    //  else            ... return value

    [ValueConversion(typeof(string), typeof(string), ParameterType = typeof(int))]
    public sealed class PhoneNumberConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            int which = 0;
            if (parm is int)
            {
                which = (int)parm;
            }
            if (!(value is string phone) || phone.Length != 10)
            {
                return value;
            }
            var ac = phone[0..3];
            var exch = phone[3..6];
            var last4 = phone[6..];
            return which switch
            {
                1 => "(" + ac + ") " + exch + "-" + last4,
                2 => ac + "-" + exch + "-" + last4,
                _ => value
            };
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is string phone))
            {
                return value;
            }
            string ret = phone.Replace("-", "").Replace("(", "").Replace(")", "").TrimStart('1');
            return ret.Length == 10 ? ret : phone;
        }
    }

    // convert from string to tooltip where string.isnullorempty => null to prevent blank tooltip popup

    [ValueConversion(typeof(string), typeof(string))]
    public sealed class StringToTooltipConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is string v) || string.IsNullOrEmpty(v))
            {
                return null;
            }
            return v;
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from string with CR/LF to string without CR/LF

    [ValueConversion(typeof(string), typeof(string))]
    public sealed class LongStringToStringConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is string v))
            {
                return value;
            }
            return v.Replace("\r\n", " ");
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from decimal to foreground color wher < 0 == red, 0 == black, > 0 == green

    [ValueConversion(typeof(decimal), typeof(SolidColorBrush))]
    public sealed class MoneyToForegroundConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is decimal d) || d == 0M)
            {
                return Brushes.Black;
            }
            return d < 0M ? Brushes.Red : Brushes.Green;
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from string to Uri where Urikind is in parm: 'r', 'a', or 'ra'

    [ValueConversion(typeof(string), typeof(Uri), ParameterType = typeof(string))]
    public sealed class StringToUriConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            UriKind kind = UriKind.Relative;
            if (parm is string k)
            {
                switch (k.ToLower())
                {
                    case "a":
                        kind = UriKind.Absolute;
                        break;
                    case "ra":
                        kind = UriKind.RelativeOrAbsolute;
                        break;
                }
            }
            if (!(value is string uri) || string.IsNullOrEmpty(uri))
            {
                return null;
            }
            return new Uri(uri, kind);
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from full path to header and back

    [ValueConversion(typeof(string), typeof(string))]
    public class UriToHeaderConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is string uri))
            {
                return value;
            }
            return uri.Replace("resources/", "").Replace("-32.png", "").TrimStart('/').Capitalize();
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is string header))
            {
                return value;
            }
            return "/resources/" + header.ToLower() + "-32.png";
        }
    }

    // convert from two doubles (size and quota) to xxx of xxx used

    public sealed class DoublesToTooltipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type t, object parm, CultureInfo lang)
        {
            if (values.Length != 2 || values.Any(x => x == DependencyProperty.UnsetValue))
            {
                return null;
            }
            if (!(values[0] is double v1) || !(values[1] is double v2))
            {
                return null;
            }
            string p1 = Tools.Normalize(v1);
            string p2 = Tools.Normalize(v2);
            if (v2 == double.MaxValue)
            {
                return $"{p1} used";
            }
            return $"{p1} used out of {p2}";
        }

        public object[] ConvertBack(object value, Type[] t, object parm, CultureInfo lang)
        {
            throw new NotImplementedException();
        }
    }

    // convert from double to solidcolorbrush where <= 0.75 == green, <= 0.9 == yellow, else red

    [ValueConversion(typeof(double), typeof(SolidColorBrush))]
    public sealed class DoubleToColorConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is double v))
            {
                return Brushes.Black;
            }
            return v <= 0.75 ? Brushes.Green : v <= 0.9 ? Brushes.Yellow : Brushes.Red;
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from PasswordStrength to description

    [ValueConversion(typeof(PasswordStrength), typeof(string))]
    public sealed class PasswordStrengthConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is PasswordStrength ps))
            {
                return null;
            }
            return ps.GetDescriptionFromEnumValue();
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from PasswordStrength to foreground color

    [ValueConversion(typeof(PasswordStrength), typeof(SolidColorBrush))]
    public sealed class PasswordStrengthToForegroundConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            if (!(value is PasswordStrength ps))
            {
                return Brushes.Black;
            }
            return ps switch
            {
                PasswordStrength.VeryWeak => new SolidColorBrush(Color.FromArgb(255, 192, 0, 0)),
                PasswordStrength.Weak => new SolidColorBrush(Color.FromArgb(255, 128, 0, 0)),
                PasswordStrength.Medium => Brushes.Gray,
                PasswordStrength.Strong => new SolidColorBrush(Color.FromArgb(255, 0, 128, 0)),
                PasswordStrength.VeryStrong => new SolidColorBrush(Color.FromArgb(255, 0, 192, 0)),
                _ => Brushes.Black
            };
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from bool to visibility where true == visible else collapsed, inverted with parm

    [ValueConversion(typeof(bool), typeof(Visibility), ParameterType = typeof(bool))]
    public sealed class BoolToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            bool invert = false;
            if (parm is bool)
            {
                invert = (bool)parm;
            }
            if (!(value is bool v))
            {
                return Visibility.Visible;
            }
            if (invert)
            {
                return v ? Visibility.Collapsed : Visibility.Visible;
            }
            return v ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from int (count) to visibility where 0 == visible else collapsed, inverted with parm

    [ValueConversion(typeof(int), typeof(Visibility), ParameterType = typeof(bool))]
    public sealed class CountToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            bool invert = false;
            if (parm is bool)
            {
                invert = (bool)parm;
            }
            if (!(value is int v))
            {
                return Visibility.Visible;
            }
            if (invert)
            {
                return v == 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            return v == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    // convert from int (count) to enabled, where 0 = false else true, inverted with parm

    [ValueConversion(typeof(int), typeof(bool), ParameterType = typeof(bool))]
    public sealed class CountToEnabledConverter : IValueConverter
    {

        public object Convert(object value, Type t, object parm, CultureInfo lang)
        {
            bool invert = false;
            if (parm is bool)
            {
                invert = (bool)parm;
            }
            if (!(value is int v))
            {
                return false;
            }
            if (invert)
            {
                return v == 0;
            }
            return v != 0;
        }

        public object ConvertBack(object value, Type t, object parm, CultureInfo lang)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
