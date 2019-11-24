using System;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace LedgerLib.Infrastructure
{
    public static class CSBuilder
    {
        private static string Server = Constants.DefaultServer;
        private static string Database = Constants.DefaultDatabase;
        private static int Timeout = Constants.DefaultTimeout;
        private static bool Pooling = Constants.DefaultPooling;
        private static bool TrustedConnection = Constants.DefaultTrustedConnection;
        private static bool MARS = Constants.DefaultMARS;

        static CSBuilder()
        {
            IConfiguration config = ConfigurationFactory.Create();
            Server = config[Constants.ServerConfig] ?? Constants.DefaultServer;
            Database = config[Constants.DatabaseConfig] ?? Constants.DefaultDatabase;

            string timeout = config[Constants.TimeoutConfig];
            if (!string.IsNullOrEmpty(timeout))
            {
                if (!int.TryParse(timeout, out Timeout) || Timeout <= 0)
                {
                    Timeout = Constants.DefaultTimeout;
                }
            }

            string pool = config[Constants.PoolingConfig];
            if (!string.IsNullOrEmpty(pool))
            {
                Pooling = pool.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            string trusted = config[Constants.TrustedConnectionConfig];
            if (!string.IsNullOrEmpty(trusted))
            {
                TrustedConnection = trusted.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            string mars = config[Constants.MARSConfig];
            if (!string.IsNullOrEmpty(mars))
            {
                MARS = mars.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }
        }

        public static void SetServer(string server) => Server = string.IsNullOrEmpty(server) ? Constants.DefaultServer : server;

        public static void SetDatabase(string db) => Database = string.IsNullOrEmpty(db) ? Constants.DefaultDatabase : db;

        public static void SetTimeout(int timeout) => Timeout = timeout <= 0 ? Constants.DefaultTimeout : timeout;

        public static void SetPooling(bool pool) => Pooling = pool;

        public static void SetTrustedConnection(bool tc) => TrustedConnection = tc;

        public static void SetMARS(bool mars) => MARS = mars;

        public static string Build()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=");
            sb.Append(Server);
            sb.Append(";Database=");
            sb.Append(Database);
            sb.Append($";Timeout={Timeout}");
            if (Pooling)
            {
                sb.Append(";Pooling=true");
            }
            if (TrustedConnection)
            {
                sb.Append(";Trusted_Connection=true");
            }
            if (MARS)
            {
                sb.Append(";MultipleActiveResultSets=true");
            }
            return sb.ToString();
        }
    }
}
