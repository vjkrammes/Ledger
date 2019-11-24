using System.IO;

using Microsoft.Extensions.Configuration;

namespace LedgerLib.Infrastructure
{
    public static class ConfigurationFactory
    {
        public static IConfiguration Create() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Constants.ConfigurationFilename)
            .Build();
    }
}
