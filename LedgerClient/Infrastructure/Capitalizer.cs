using System;
using System.Text;

using Humanizer;

namespace LedgerClient.Infrastructure
{
    public class Capitalizer : IStringTransformer
    {
        public string Transform(string value)
        {
            string[] parts = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            foreach (var part in parts)
            {
                sb.Append(part.Capitalize());
                sb.Append(" ");
            }
            return sb.ToString().TrimEnd(' ');
        }
    }
}
