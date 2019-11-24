using System;
using System.Collections.Generic;
using System.Data;

namespace LedgerLib.Models
{
    public class DatabaseInfo
    {
        public string Name { get; set; }
        public double Size { get; set; }
        public double Unallocated { get; set; }
        public double Reserved { get; set; }
        public double Data { get; set; }
        public double IndexSize { get; set; }
        public double Unused { get; set; }

        public DatabaseInfo()
        {
            Name = string.Empty;
            Size = 0;
            Unallocated = 0;
            Reserved = 0;
            Data = 0;
            IndexSize = 0;
            Unused = 0;
        }

        public DatabaseInfo(DataSet dataset) : this()
        {
            Name = dataset.Tables[0].Rows[0][0] as string;
            Size = Parse(dataset.Tables[0].Rows[0][1] as string);
            Unallocated = Parse(dataset.Tables[0].Rows[0][2] as string);
            Reserved = Parse(dataset.Tables[1].Rows[0][0] as string);
            Data = Parse(dataset.Tables[1].Rows[0][1] as string);
            IndexSize = Parse(dataset.Tables[1].Rows[0][2] as string);
            Unused = Parse(dataset.Tables[1].Rows[0][3] as string);
        }

        private static readonly Dictionary<string, double> multipliers = new Dictionary<string, double>
        {
            ["KB"] = 1_000.0,
            ["MB"] = 1_000_000.0,
            ["GB"] = 1_000_000_000.0,
            ["TB"] = 1_000_000_000_000.0
        };

        private double Parse(string spec)
        {
            double ret = 0;

            if (string.IsNullOrEmpty(spec))
                return 0;
            string[] parts = spec.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                double.TryParse(spec, out ret);
            }
            else if (parts.Length == 2)
            {
                if (double.TryParse(parts[0], out double d))
                {
                    if (multipliers.ContainsKey(parts[1]))
                    {
                        d *= multipliers[parts[1]];
                    }
                    ret = d;
                }
            }
            return ret;
        }
    }
}
