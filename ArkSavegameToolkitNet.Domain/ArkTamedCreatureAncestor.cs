using ArkSavegameToolkitNet.Arrays;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkTamedCreatureAncestor
    {
        private static readonly ArkName _femaleDinoID1 = ArkName.Create("FemaleDinoID1");
        private static readonly ArkName _femaleDinoID2 = ArkName.Create("FemaleDinoID2");
        private static readonly ArkName _femaleName = ArkName.Create("FemaleName");
        private static readonly ArkName _maleDinoID1 = ArkName.Create("MaleDinoID1");
        private static readonly ArkName _maleDinoID2 = ArkName.Create("MaleDinoID2");
        private static readonly ArkName _maleName = ArkName.Create("MaleName");

        public ArkTamedCreatureAncestor() { }

        public static ArkTamedCreatureAncestor[] FromPropertyValue(ArkArrayStruct ancestorsArrayStruct)
        {
            return ancestorsArrayStruct?.OfType<StructPropertyList>().Select(x => new ArkTamedCreatureAncestor
            {
                FemaleId1 = x.GetPropertyValue<uint>(_femaleDinoID1),
                FemaleId2 = x.GetPropertyValue<uint>(_femaleDinoID2),
                FemaleName = x.GetPropertyValue<string>(_femaleName),
                MaleId1 = x.GetPropertyValue<uint>(_maleDinoID1),
                MaleId2 = x.GetPropertyValue<uint>(_maleDinoID2),
                MaleName = x.GetPropertyValue<string>(_maleName)
            }).ToArray();
        }

        public uint FemaleId1 { get; set; }
        public uint FemaleId2 { get; set; }
        public ulong FemaleId => ((ulong)FemaleId1 << 32) | FemaleId2;
        public string FemaleName { get; set; }

        public uint MaleId1 { get; set; }
        public uint MaleId2 { get; set; }
        public ulong MaleId => ((ulong)MaleId1 << 32) | MaleId2;
        public string MaleName { get; set; }
    }
}
