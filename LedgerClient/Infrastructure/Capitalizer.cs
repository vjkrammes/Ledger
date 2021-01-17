using Humanizer;

using System;
using System.Text;

namespace LedgerClient.Infrastructure
{
    public class Capitalizer : IStringTransformer
    {
        public string Transform(string value)
        {
            var parts = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var part in parts)
            {
                sb.Append(part.Capitalize());
                sb.Append(' ');
            }
            return sb.ToString().TrimEnd(' ');
        }
    }
}
