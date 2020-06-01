using System;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkPlayerExternal
    {
        public int Id { get; set; }
        public int? TribeId { get; set; }
        public string SteamId { get; set; }
        public string Name { get; set; }
        public string CharacterName { get; set; }
        public DateTime LastActiveTime { get; set; }
    }
}
