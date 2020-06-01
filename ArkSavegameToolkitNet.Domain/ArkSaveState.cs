using ArkSavegameToolkitNet.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public interface ISaveState
    {
        float? GameTime { get; set; }
        DateTime SaveTime { get; set; }
        string MapName { get; set; }
    }

    public class SaveState : ISaveState
    {
        public float? GameTime { get; set; }
        public DateTime SaveTime { get; set; }
        public string MapName { get; set; }

        public static ISaveState FromSaveTime(DateTime saveTime)
        {
            return new SaveState { SaveTime = saveTime };
        }

        public static SaveState FromArkSaveData(ArkSaveData save)
        {
            return new SaveState { SaveTime = save.savedAt, GameTime = save.gameTime, MapName = save.mapName };
        }
    }
}
