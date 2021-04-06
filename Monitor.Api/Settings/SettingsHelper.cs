using System;
using Microsoft.Extensions.Configuration;

namespace Monitor.Api.Settings
{
    public class SettingsHelper: ISettingsHelper
    {
        private readonly IConfiguration configuration;
        public const string HEALTHCHECK_MEMORY_MEGABYTES_LIMIT = "HEALTHCHECK_MEMORY_MEGABYTES_LIMIT";

        public SettingsHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string HealthCheckMemoryMegabytesLimit => GetSettingValue(HEALTHCHECK_MEMORY_MEGABYTES_LIMIT);

        private string GetSettingValue(string name)
        {
            return Environment.GetEnvironmentVariable(name)
                ?? configuration?.GetSection($"AppSettings:{name}")?.Value;
        }

        private string GetConnectionString(string name)
        {
            return Environment.GetEnvironmentVariable(name)
                ?? configuration?.GetConnectionString(name);
        }
    }
}