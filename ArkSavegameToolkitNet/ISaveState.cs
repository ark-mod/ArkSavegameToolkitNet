using System;

namespace ArkSavegameToolkitNet
{
    public interface ISaveState
    {
        float? GameTime { get; set; }
        DateTime SaveTime { get; set; }
        string MapName { get; set; }
    }
}