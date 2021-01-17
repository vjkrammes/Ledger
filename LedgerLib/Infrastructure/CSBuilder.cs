using System;
using System.Text;

namespace LedgerLib.Infrastructure
{
    public static class CSBuilder
    {
        private static string _server = Constants.DefaultServer;
        private static string _database = Constants.DefaultDatabase;
        private static int _timeout = Constants.DefaultTimeout;
        private static bool _pooling = Constants.DefaultPooling;
        private static bool _trustedConnection = Constants.DefaultTrustedConnection;
        private static bool _mars = Constants.DefaultMARS;

        private static string _historyServer = Constants.DefaultServer;
        private static string _historyDatabase = Constants.DefaultHistoryDatabase;
        private static int _historyTimeout = Constants.DefaultTimeout;
        private static bool _historyPooling = Constants.DefaultPooling;
        private static bool _historyTrustedConnection = Constants.DefaultTrustedConnection;
        private static bool _historyMARS = Constants.DefaultMARS;

        static CSBuilder()
        {
            var config = ConfigurationFactory.Create();
            _server = config[Constants.ServerConfig] ?? Constants.DefaultServer;
            _database = config[Constants.DatabaseConfig] ?? Constants.DefaultDatabase;

            var timeout = config[Constants.TimeoutConfig];
            if (!string.IsNullOrEmpty(timeout))
            {
                if (!int.TryParse(timeout, out CSBuilder._timeout) || CSBuilder._timeout <= 0)
                {
                    _timeout = Constants.DefaultTimeout;
                }
            }

            var pool = config[Constants.PoolingConfig];
            if (!string.IsNullOrEmpty(pool))
            {
                _pooling = pool.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            var trusted = config[Constants.TrustedConnectionConfig];
            if (!string.IsNullOrEmpty(trusted))
            {
                _trustedConnection = trusted.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            var mars = config[Constants.MARSConfig];
            if (!string.IsNullOrEmpty(mars))
            {
                _mars = mars.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            _historyServer = config[Constants.HistoryServerConfig] ?? Constants.DefaultServer;
            _historyDatabase = config[Constants.HistoryDatabaseConfig] ?? Constants.DefaultHistoryDatabase;
            timeout = config[Constants.HistoryTimeoutConfig];
            if (!string.IsNullOrEmpty(timeout))
            {
                if (!int.TryParse(timeout, out _historyTimeout) || _historyTimeout <= 0)
                {
                    _historyTimeout = Constants.DefaultTimeout;
                }
            }

            pool = config[Constants.HistoryPoolingConfig];
            if (!string.IsNullOrEmpty(pool))
            {
                _historyPooling = pool.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            trusted = config[Constants.HistoryPoolingConfig];
            if (!string.IsNullOrEmpty(trusted))
            {
                _historyTrustedConnection = trusted.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }

            mars = config[Constants.HistroyMARSConfig];
            if (!string.IsNullOrEmpty(mars))
            {
                _historyMARS = mars.Equals(Constants.True, StringComparison.OrdinalIgnoreCase);
            }
        }

        public static void SetServer(string server) => _server = string.IsNullOrEmpty(server) ? Constants.DefaultServer : server;

        public static void SetDatabase(string db) => _database = string.IsNullOrEmpty(db) ? Constants.DefaultDatabase : db;

        public static void SetTimeout(int timeout) => CSBuilder._timeout = timeout <= 0 ? Constants.DefaultTimeout : timeout;

        public static void SetPooling(bool pool) => _pooling = pool;

        public static void SetTrustedConnection(bool tc) => _trustedConnection = tc;

        public static void SetMARS(bool mars) => _mars = mars;

        public static void SetHistoryServer(string server) =>
            _historyServer = string.IsNullOrEmpty(server) ? Constants.DefaultServer : server;

        public static void SetHistoryDatabase(string db) =>
            _historyDatabase = string.IsNullOrEmpty(db) ? Constants.DefaultHistoryDatabase : db;

        public static void SetHistoryTimeout(int timeout) => _historyTimeout = timeout <= 0 ? Constants.DefaultTimeout : timeout;

        public static void SetHistoryPooling(bool pool) => _historyPooling = pool;

        public static void SetHistoryTrustedConnection(bool tc) => _historyTrustedConnection = tc;

        public static void SetHistoryMARS(bool mars) => _historyMARS = mars;

        public static string Build()
        {
            var sb = new StringBuilder();
            sb.Append("Server=");
            sb.Append(_server);
            sb.Append(";Database=");
            sb.Append(_database);
            sb.Append($";Timeout={_timeout}");
            if (_pooling)
            {
                sb.Append(";Pooling=true");
            }
            if (_trustedConnection)
            {
                sb.Append(";Trusted_Connection=true");
            }
            if (_mars)
            {
                sb.Append(";MultipleActiveResultSets=true");
            }
            return sb.ToString();
        }

        public static string BuildHistory()
        {
            var sb = new StringBuilder();
            sb.Append("Server=");
            sb.Append(_historyServer);
            sb.Append(";Database=");
            sb.Append(_historyDatabase);
            sb.Append($";Timeout={_historyTimeout}");
            if (_historyPooling)
            {
                sb.Append(";Pooling=true");
            }
            if (_historyTrustedConnection)
            {
                sb.Append(";Trusted_Connection=true;");
            }
            if (_historyMARS)
            {
                sb.Append(";MultipleActiveResultSets=true");
            }
            return sb.ToString();
        }
    }
}
