using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public static class ISaveStateExtensions
    {
        /// <summary>
        /// Get the approximate time span since an event in-game.
        /// </summary>
        public static TimeSpan? GetApproxTimeElapsedSince(this ISaveState self, double? gametime)
        {
            var dss = GetApproxTimeSpanSinceSave(self);
            return gametime.HasValue && dss.HasValue && self?.GameTime > 0 ? TimeSpan.FromSeconds(self.GameTime.Value - gametime.Value) + dss.Value : (TimeSpan?)null;
        }

        /// <summary>
        /// Get the approximate date and time when an event in-game will occur
        /// </summary>
        public static DateTime? GetApproxDateTimeOf(this ISaveState self, double? gametime)
        {
            return gametime.HasValue && self.SaveTime != DateTime.MinValue && self?.GameTime > 0 ? self.SaveTime.AddSeconds(gametime.Value - self.GameTime.Value) : (DateTime?)null;
        }

        private static TimeSpan? GetApproxTimeSpanSinceSave(this ISaveState self)
        {
            return self?.SaveTime != DateTime.MinValue ? DateTime.UtcNow - self.SaveTime : (TimeSpan?)null;
        }
    }
}
