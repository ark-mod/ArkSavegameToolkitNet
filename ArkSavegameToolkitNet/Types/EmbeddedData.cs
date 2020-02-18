using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class EmbeddedData
    {
        [JsonProperty]
        public virtual byte[][][] Data { get; set; }
        [JsonProperty]
        public string Path { get; set; }

        public EmbeddedData() { }

        public EmbeddedData(ArkArchive archive)
        {
            read(archive);
        }
        
        //public virtual int Size
        //{
        //    get
        //    {
        //        int size = ArkArchive.getStringLength(path) + 4;

        //        if (data != null)
        //        {
        //            size += data.Length * 4;
        //            foreach (sbyte[][] partData in data)
        //            {
        //                if (partData != null)
        //                {
        //                    size += partData.Length * 4;
        //                    foreach (sbyte[] blobData in partData)
        //                    {
        //                        size += blobData.Length;
        //                    }
        //                }
        //            }
        //        }

        //        return size;
        //    }
        //}

        public virtual void read(ArkArchive archive)
        {
            Path = archive.GetString();

            var partCount = archive.GetInt();

            Data = new byte[partCount][][];
            for (var part = 0; part < partCount; part++)
            {
                var blobCount = archive.GetInt();
                var partData = new byte[blobCount][];

                for (var blob = 0; blob < blobCount; blob++)
                {
                    var blobSize = archive.GetInt() * 4; // Array of 32 bit values
                    partData[blob] = archive.GetBytes(blobSize);
                }

                Data[part] = partData;
            }
        }

        public static void Skip(ArkArchive archive)
        {
            archive.SkipString();

            var partCount = archive.GetInt();
            for (var part = 0; part < partCount; part++)
            {
                var blobCount = archive.GetInt();
                for (var blob = 0; blob < blobCount; blob++)
                {
                    var blobSize = archive.GetInt() * 4;
                    archive.Position += blobSize;
                }
            }
        }

    }
}
