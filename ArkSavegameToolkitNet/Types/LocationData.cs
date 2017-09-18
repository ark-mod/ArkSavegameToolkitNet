using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LocationData
    {
        [JsonProperty]
        public float X { get; set; }
        [JsonProperty]
        public float Y { get; set; }
        [JsonProperty]
        public float Z { get; set; }
        [JsonProperty]
        public float Pitch { get; set; }
        [JsonProperty]
        public float Yaw { get; set; }
        [JsonProperty]
        public float Roll { get; set; }

        public LocationData()
        {
        }

        public LocationData(ArkArchive archive)
        {
            read(archive);
        }

        public override string ToString()
        {
            return "LocationData [x=" + X + ", y=" + Y + ", z=" + Z + ", pitch=" + Pitch + ", yaw=" + Yaw + ", roll=" + Roll + "]";
        }

        //public long Size
        //{
        //    get
        //    {
        //        return Float.BYTES * 6;
        //    }
        //}

        public void read(ArkArchive archive)
        {
            X = archive.GetFloat();
            Y = archive.GetFloat();
            Z = archive.GetFloat();
            Pitch = archive.GetFloat();
            Yaw = archive.GetFloat();
            Roll = archive.GetFloat();
        }

        public static void skip(ArkArchive archive)
        {
            archive.Position += /* float size */ 4 * 6;
        }

    }
}
