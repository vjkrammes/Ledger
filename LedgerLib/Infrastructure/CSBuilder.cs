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

        private static string HistoryServer = Constants.DefaultServer;
        private static string HistoryDatabase = Constants.DefaultHistoryDatabase;
        private static int HistoryTimeout = Constants.DefaultTimeout;
        private static bool HistoryPooling = Constants.DefaultPooling;
        private static bool HistoryTrustedConnection = Constants.DefaultTrustedConnection;
        private static bool HistoryMARS = Constants.DefaultMARS;

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

            HistoryServer = config[Constants.HistoryServerConfig] ?? Constants.DefaultServer;
            HistoryDatabase = config[Constants.HistoryDatabaseConfig] ?? Constants.DefaultHistoryDatabase;
            timeout = config[Constants.HistoryTimeoutConfig];
            if (!string.IsNullOrEmpty(timeout))
            {
                if (!int.TryParse(timeout, out HistoryTimeout) || HistoryTimeout <= 0)
                {
                    HistoryTimeout = Constants.DefaultTimeout;
                }
            }

            pool = config[Constants.HistoryPoolingConfig];
            if (!string.IsNullOrEmpty(pool))
            {
                HistoryPooling = pool.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            trusted = config[Constants.HistoryPoolingConfig];
            if (!string.IsNullOrEmpty(trusted))
            {
                HistoryTrustedConnection = trusted.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            mars = config[Constants.HistroyMARSConfig];
            if (!string.IsNullOrEmpty(mars))
            {
                HistoryMARS = mars.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }
        }

        public static void SetServer(string server) => Server = string.IsNullOrEmpty(server) ? Constants.DefaultServer : server;

        public static void SetDatabase(string db) => Database = string.IsNullOrEmpty(db) ? Constants.DefaultDatabase : db;

        public static void SetTimeout(int timeout) => Timeout = timeout <= 0 ? Constants.DefaultTimeout : timeout;

        public static void SetPooling(bool pool) => Pooling = pool;

        public static void SetTrustedConnection(bool tc) => TrustedConnection = tc;

        public static void SetMARS(bool mars) => MARS = mars;

        public static void SetHistoryServer(string server) =>
            HistoryServer = string.IsNullOrEmpty(server) ? Constants.DefaultServer : server;

        public static void SetHistoryDatabase(string db) =>
            HistoryDatabase = string.IsNullOrEmpty(db) ? Constants.DefaultHistoryDatabase : db;

        public static void SetHistoryTimeout(int timeout) => HistoryTimeout = timeout <= 0 ? Constants.DefaultTimeout : timeout;

        public static void SetHistoryPooling(bool pool) => HistoryPooling = pool;

        public static void SetHistoryTrustedConnection(bool tc) => HistoryTrustedConnection = tc;

        public static void SetHistoryMARS(bool mars) => HistoryMARS = mars;

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

        public static string BuildHistory()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Server=");
            sb.Append(HistoryServer);
            sb.Append(";Database=");
            sb.Append(HistoryDatabase);
            sb.Append($";Timeout={HistoryTimeout}");
            if (HistoryPooling)
            {
                sb.Append(";Pooling=true");
            }
            if (HistoryTrustedConnection)
            {
                sb.Append(";Trusted_Connection=true;");
            }
            if (HistoryMARS)
            {
                sb.Append(";MultipleActiveResultSets=true");
            }
            return sb.ToString();
        }
    }
}
