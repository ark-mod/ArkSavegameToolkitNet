using ArkSavegameToolkitNet.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArkSavegameToolkitNet.Utils.Extensions
{
    public static class ArkSaveDataExtensions
    {
        /// <summary>
        /// Get the approximate time span since an event in-game.
        /// </summary>
        public static TimeSpan? GetApproxTimeElapsedSince(this ArkSaveData self, double? gametime)
        {
            var dss = GetApproxTimeSpanSinceSave(self);
            return gametime.HasValue && dss.HasValue && self?.gameTime > 0 ? TimeSpan.FromSeconds(self.gameTime - gametime.Value) + dss.Value : (TimeSpan?)null;
        }

        /// <summary>
        /// Get the approximate date and time when an event in-game will occur
        /// </summary>
        public static DateTime? GetApproxDateTimeOf(this ArkSaveData self, double? gametime)
        {
            return gametime.HasValue && self.savedAt != DateTime.MinValue && self?.gameTime > 0 ? self.savedAt.AddSeconds(gametime.Value - self.gameTime) : (DateTime?)null;
        }

        private static TimeSpan? GetApproxTimeSpanSinceSave(this ArkSaveData self)
        {
            return self?.savedAt != DateTime.MinValue ? DateTime.UtcNow - self.savedAt : (TimeSpan?)null;
        }
    }
}
