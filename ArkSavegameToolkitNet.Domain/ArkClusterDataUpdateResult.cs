using System;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkClusterDataUpdateResult
    {
        public bool Success { get; set; }
        public bool Cancelled { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}
