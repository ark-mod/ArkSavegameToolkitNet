using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Utils.Extensions;
using System.Linq;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkTamedCreatureAncestor
    {
        private static readonly ArkName _dinoAncestors = ArkName.Create("DinoAncestors");
        private static readonly ArkName _dinoAncestorsMale = ArkName.Create("DinoAncestorsMale");

        private static readonly ArkName _eggDinoAncestors = ArkName.Create("EggDinoAncestors");
        private static readonly ArkName _eggDinoAncestorsMale = ArkName.Create("EggDinoAncestorsMale");

        private static readonly ArkName _myArkData = ArkName.Create("MyArkData");
        private static readonly ArkName _arkItems = ArkName.Create("ArkItems");
        private static readonly ArkName _arkTributeItem = ArkName.Create("ArkTributeItem");

        private static readonly ArkName _femaleDinoID1 = ArkName.Create("FemaleDinoID1");
        private static readonly ArkName _femaleDinoID2 = ArkName.Create("FemaleDinoID2");
        private static readonly ArkName _femaleName = ArkName.Create("FemaleName");
        private static readonly ArkName _maleDinoID1 = ArkName.Create("MaleDinoID1");
        private static readonly ArkName _maleDinoID2 = ArkName.Create("MaleDinoID2");
        private static readonly ArkName _maleName = ArkName.Create("MaleName");

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            {
                _dinoAncestors,
                new ArkNameTree
                {
                    { _femaleDinoID1, null },
                    { _femaleDinoID2, null },
                    { _femaleName, null },
                    { _maleDinoID1, null },
                    { _maleDinoID2, null },
                    { _maleName, null }
                }
            },
            {
                _dinoAncestorsMale,
                new ArkNameTree
                {
                    { _femaleDinoID1, null },
                    { _femaleDinoID2, null },
                    { _femaleName, null },
                    { _maleDinoID1, null },
                    { _maleDinoID2, null },
                    { _maleName, null }
                }
            },
            {
                _eggDinoAncestors,
                new ArkNameTree
                {
                    { _femaleDinoID1, null },
                    { _femaleDinoID2, null },
                    { _femaleName, null },
                    { _maleDinoID1, null },
                    { _maleDinoID2, null },
                    { _maleName, null }
                }
            },
            {
                _eggDinoAncestorsMale,
                new ArkNameTree
                {
                    { _femaleDinoID1, null },
                    { _femaleDinoID2, null },
                    { _femaleName, null },
                    { _maleDinoID1, null },
                    { _maleDinoID2, null },
                    { _maleName, null }
                }
            },
            {
                _myArkData,
                new ArkNameTree
                {
                    {
                        _arkItems,
                        new ArkNameTree
                        {
                            {
                                _arkTributeItem,
                                new ArkNameTree
                                {
                                    {
                                        _eggDinoAncestors,
                                        new ArkNameTree
                                        {
                                            { _femaleDinoID1, null },
                                            { _femaleDinoID2, null },
                                            { _femaleName, null },
                                            { _maleDinoID1, null },
                                            { _maleDinoID2, null },
                                            { _maleName, null }
                                        }
                                    },
                                    {
                                        _eggDinoAncestorsMale,
                                        new ArkNameTree
                                        {
                                            { _femaleDinoID1, null },
                                            { _femaleDinoID2, null },
                                            { _femaleName, null },
                                            { _maleDinoID1, null },
                                            { _maleDinoID2, null },
                                            { _maleName, null }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        public ArkTamedCreatureAncestor() { }

        public static ArkTamedCreatureAncestor[] FromPropertyValue(StructArrayProperty ancestorsArrayStruct)
        {
            return ancestorsArrayStruct?.values?.OfType<PropertyListStructProperty>().Select(x => new ArkTamedCreatureAncestor
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
