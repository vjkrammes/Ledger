using System;
using System.Collections.Generic;
using System.Text;

namespace LedgerLib.Infrastructure
{
    public static class Constants
    {
        // "Numbers"

        public const double ProductVersion = 7.1;

        public const double MinumumIconHeight = 16.0;
        public const double DefaultIconHeight = 24.0;
        public const double MaximumIconHeight = 32.0;

        public const int DefaultTimeout = 10;
        public const bool DefaultPooling = true;
        public const bool DefaultTrustedConnection = true;
        public const bool DefaultMARS = true;

        public const int HashIterations = 25_000;
        public const int HashLength = 64;       // bytes, not bits
        public const int SaltLength = 64;       // same

        public const int LedgerPassword = 0;
        public const int Ledger5Password = 1;

        // Error exit codes, next = 907

        public const int MigrationFailed = 901;
        public const int CompaniesLoadFailed = 902;
        public const int AccountsLoadFailed = 903;
        public const int TransactionsLoadFailed = 904;
        public const int IdentitiesLoadFailed = 905;
        public const int NoPasswordEntered = 906;

        // Strings

        public const string ProductName = "Ledger";

        public const string Alt0 = "Alt0";
        public const string Alt1 = "Alt1";
        public const string Background = "Background";
        public const string Border = "Border";
        public const string Foreground = "Foreground";
        public const string IconHeight = "IconHeight";
        public const string Locator = "Locator";

        public const string ConfigurationFilename = "appsettings.json";

        public const string DefaultServer = @".\SQLEXPRESS";
        public const string DefaultDatabase = "Ledger";
        public const string DefaultHistoryDatabase = "Ledger5";

        public const string ServerConfig = "Database:Server";
        public const string DatabaseConfig = "Database:Database";
        public const string TimeoutConfig = "Database:Timeout";
        public const string PoolingConfig = "Database:Pooling";
        public const string TrustedConnectionConfig = "Database:TrustedConnection";
        public const string MARSConfig = "Database:MARS";

        public const string HistoryServerConfig = "History:Server";
        public const string HistoryDatabaseConfig = "History:Database";
        public const string HistoryTimeoutConfig = "History:Timeout";
        public const string HistoryPoolingConfig = "History:Pooling";
        public const string HistoryTrustedConnectionConfig = "History:TrustedConnection";
        public const string HistroyMARSConfig = "History:MARS";

        public const string True = "true";
        public const string Reload = "reload";
        public const string Load = "load";
        public const string Count = "Count";
        public const string Indexer = "Item[]";
        public const string Keys = "Keys";
        public const string Values = "Values";

        public const string Date = "date";
        public const string Datetime2 = "datetime2";
        public const string HistoryMoneyFormat = "decimal(15,2)";
        public const string MoneyFormat = "decimal(12,2)";
        public const string Varbinary = "varbinary(max)";

        public const string Checkmark = "/resources/checkmark-32.png";

        public const string DBE = "Database Error";
        public const string IOE = "I/O Error";

        public const string DuplicateKey = "An item with the same key has already been added.";
    }
}
