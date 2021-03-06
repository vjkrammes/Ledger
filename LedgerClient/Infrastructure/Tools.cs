﻿using LedgerClient.ECL.DTO;

using LedgerLib.HistoryEntities;
using LedgerLib.Infrastructure;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LedgerClient.Infrastructure
{
    public static class Tools
    {
        public static Locator Locator => Application.Current.Resources[Constants.Locator] as Locator ?? new Locator();

        public static string GetShortTitle() => $"{Constants.ProductName} {Constants.ProductVersion:0.00}";

        public static string Hexify(byte[] array)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            var sb = new StringBuilder();
            sb.Append($"[length={array.Length}]: 0x");
            for (var i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static void ConcurrencyError(string entity, string operation)
        {
            var msg = $"The {entity} was changed by another user. The {operation} operation has been cancelled";
            PopupManager.Popup(msg, "Concurrency Conflict", PopupButtons.Ok, PopupImage.Information);
        }

        public static string FormatAccountNumber(Account acct, AccountNumber acctnum)
        {
            if (acct is null || acctnum is null)
            {
                return "Unknown";
            }
            var sb = new StringBuilder();
            sb.Append(acct.AccountType?.Description ?? "Unknown");
            sb.Append(' ');
            var accountnumber = Locator.StringCypher.Decrypt(acctnum.Number, Locator.PasswordManager.Get(Constants.LedgerPassword),
                acctnum.Salt);
            var numpart = accountnumber.Length < 4 ? accountnumber : accountnumber[^4..];
            sb.Append(numpart);
            return sb.ToString();
        }

        public static string FormatAccountNumber(AccountHistoryEntity acct, AccountNumberHistoryEntity acctnum)
        {
            if (acct is null || acctnum is null)
            {
                return "Unknown";
            }
            var sb = new StringBuilder();
            sb.Append(acct.AccountType?.Description ?? "Unknown");
            sb.Append(' ');
            var accountnumber = Locator.StringCypher.Decrypt(acctnum.Number, Locator.PasswordManager.Get(Constants.Ledger5Password));
            var numpart = accountnumber.Length < 4 ? accountnumber : accountnumber[^4..];
            sb.Append(numpart);
            return sb.ToString();
        }

        public static byte[] GenerateSalt(int length)
        {
            var ret = new byte[length];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(ret);
            return ret;
        }

        public static byte[] GenerateHash(byte[] password, byte[] salt, int iterations, int length)
        {
            using var derivebytes = new Rfc2898DeriveBytes(password, salt, iterations);
            return derivebytes.GetBytes(length);
        }

        public static bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2) =>
            start1.IsBetween(start2, end2) ||
            end1.IsBetween(start2, end2) ||
            start2.IsBetween(start1, end1) ||
            end2.IsBetween(start1, end1);

        public static IEnumerable<T> GetValues<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();

        public static BitmapImage CachedImage(string uri) => CachedImage(new Uri(uri));

        public static BitmapImage CachedImage(Uri uri)
        {
            var ret = new BitmapImage();
            ret.BeginInit();
            ret.UriSource = uri;
            ret.CacheOption = BitmapCacheOption.OnLoad;
            ret.EndInit();
            return ret;
        }

        public static Uri PackedUri(string res) => new Uri($"pack://application:,,,/LedgerClient;component/resources/{res}");

        public static string Normalize(double d)
        {
            if (d < 1_000)
            {
                return d.ToString("n0") + " bytes";
            }

            if (d < 1_000_000)
            {
                return Math.Round(d / 1_000, 2).ToString("n2") + " KB";
            }

            if (d < 1_000_000_000)
            {
                return Math.Round(d / 1_000_000, 2).ToString("n2") + " MB";
            }

            return Math.Round(d / 1_000_000_000, 2).ToString("n2") + " GB";
        }

        public static IEnumerable<string> GetImages(Assembly a)
        {
            var ret = new List<string>();
            var resources = a.GetManifestResourceNames();
            foreach (var r in resources)
            {
                if (!r.Contains("g.resources"))
                {
                    continue;
                }
                ResourceSet rs = null;
                try
                {
                    rs = new ResourceSet(a.GetManifestResourceStream(r));
                }
                catch
                {
                    rs?.Dispose();
                    continue;
                }
                var hashes = rs.Cast<DictionaryEntry>().ToList();
                rs.Dispose();
                var keys = new List<string>();
                foreach (var hash in hashes)
                {
                    if (!hash.Key.ToString().EndsWith("-32.png"))
                    {
                        continue;
                    }
                    keys.Add(hash.Key.ToString());
                }
                var uris = from k in keys orderby k select k;   // sort
                foreach (var uri in uris)
                {
                    var u = uri;
                    if (!u.StartsWith("/"))
                    {
                        u = "/" + u;
                    }
                    var l = u.ToLower();
                    if (l.Contains("database", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    if (!l.Contains("resources/", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    ret.Add(u);
                }
            }
            ret.Insert(0, "");
            return ret;
        }
    }
}
