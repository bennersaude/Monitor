using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Monitor.Api.Settings;
using Monitor.Domain.Business.Status;
using Monitor.Domain.Helper;
using Newtonsoft.Json.Linq;

namespace Monitor.Api.HealthCheck
{
    public class MemoryHealthCheck: IHealthCheck
    {
        private const string ALLOCATED_MEGABYTES = "AllocatedMegaBytes";
        private readonly ISettingsHelper settingsHelper;

        public MemoryHealthCheck(ISettingsHelper settingsHelper)
        {
            this.settingsHelper = settingsHelper;
        }

        private long HEALTHCHECK_MEMORY_MEGABYTES_LIMIT
        {
            get
            {
                if (long.TryParse(settingsHelper.HealthCheckMemoryMegabytesLimit,
                    out long value))
                {
                    return value;
                }
                return 512;
            }
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            var memoryUtilization = new MemoryUtilizationStatus();
            var data = GetMemoryUsageData();
            var status = (memoryUtilization.AllocatedMegabytes < HEALTHCHECK_MEMORY_MEGABYTES_LIMIT) ? HealthStatus.Healthy : HealthStatus.Unhealthy;

            return Task.FromResult(new HealthCheckResult(
                status,
                description: $"Memory limit: {HEALTHCHECK_MEMORY_MEGABYTES_LIMIT} megabytes.",
                exception: null,
                data: GetMemoryUsageData(memoryUtilization)));
        }

        public static Dictionary<string, object> GetMemoryUsageData(MemoryUtilizationStatus memoryUtilization = null)
        {
            return new Dictionary<string, object>()
            {
                { "MemoryUtilization", JsonHelper.GetJToken(GetMemoryUsage(memoryUtilization)) },
            };
        }

        public static MemoryUtilizationStatus GetMemoryUsage(MemoryUtilizationStatus memoryUtilization = null)
        {
            return memoryUtilization ?? new MemoryUtilizationStatus();
        }
    }
}