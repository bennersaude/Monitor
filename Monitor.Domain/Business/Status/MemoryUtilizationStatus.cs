using System;
using System.Diagnostics;

namespace Monitor.Domain.Business.Status
{
    public class MemoryUtilizationStatus
    {
        public MemoryUtilizationStatus()
        {
            var proc = Process.GetCurrentProcess();
            AllocatedMegabytes = proc.PrivateMemorySize64 / 1024 / 1024;
            Gen0Collections = GC.CollectionCount(0);
            Gen1Collections = GC.CollectionCount(1);
            Gen2Collections = GC.CollectionCount(2);
            HostName = System.Environment.MachineName;
        }

        public long AllocatedMegabytes { get; }
        public int Gen0Collections { get; }
        public int Gen1Collections { get; }
        public int Gen2Collections { get; }
        public string HostName { get; }

        public override string ToString()
        {
            return $"{AllocatedMegabytes} MB";
        }

    }
}