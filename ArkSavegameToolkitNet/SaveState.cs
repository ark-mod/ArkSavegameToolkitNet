using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public class SaveState : ISaveState
    {
        public float? GameTime { get; set; }
        public DateTime SaveTime { get; set; }
        public string MapName { get; set; }

        public static ISaveState FromSaveTime(DateTime saveTime)
        {
            return new SaveState { SaveTime = saveTime };
        }
    }
}
