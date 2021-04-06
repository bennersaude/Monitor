using System;
using System.Threading;

namespace Monitor.Domain.Business.Jobs
{
    public interface IMonitorJob
    {
        void StartMonitoring(CancellationToken ct);
    }
}