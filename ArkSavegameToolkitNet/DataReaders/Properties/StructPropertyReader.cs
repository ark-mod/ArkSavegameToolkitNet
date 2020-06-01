using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class StructPropertyReader
    {
        private static readonly ArkName _itemNetId = ArkName.Create("ItemNetID");
        private static readonly ArkName _itemNetInfo = ArkName.Create("ItemNetInfo");
        private static readonly ArkName _transform = ArkName.Create("Transform");
        private static readonly ArkName _primalPlayerDataStruct = ArkName.Create("PrimalPlayerDataStruct");
        private static readonly ArkName _primalPlayerCharacterConfigStruct = ArkName.Create("PrimalPlayerCharacterConfigStruct");
        private static readonly ArkName _primalPersistentCharacterStatsStruct = ArkName.Create("PrimalPersistentCharacterStatsStruct");
        private static readonly ArkName _tribeData = ArkName.Create("TribeData");
        private static readonly ArkName _tribeGovernment = ArkName.Create("TribeGovernment");
        private static readonly ArkName _terrainInfo = ArkName.Create("TerrainInfo");
        private static readonly ArkName _arkInventoryData = ArkName.Create("ArkInventoryData");
        private static readonly ArkName _dinoOrderGroup = ArkName.Create("DinoOrderGroup");
        private static readonly ArkName _arkDinoData = ArkName.Create("ARKDinoData");
        private static readonly ArkName _customItemByteArrays = ArkName.Create("CustomItemByteArrays");
        private static readonly ArkName _customItemDoubles = ArkName.Create("CustomItemDoubles");

        private static readonly ArkName _vector = ArkName.Create("Vector");
        private static readonly ArkName _vector2d = ArkName.Create("Vector2D");
        private static readonly ArkName _quat = ArkName.Create("Quat");
        private static readonly ArkName _color = ArkName.Create("Color");
        private static readonly ArkName _linearColor = ArkName.Create("LinearColor");
        private static readonly ArkName _rotator = ArkName.Create("Rotator");
        private static readonly ArkName _uniqueNetIdRepl = ArkName.Create("UniqueNetIdRepl");

        public static StructProperty Get(ArchiveReader ar, int dataSize, bool propertyIsExcluded = false, ArkNameTree exclusivePropertyNameTreeChildNode = null)
        {
            if (propertyIsExcluded)
            {
                ar.SkipName();
                ar.Advance(dataSize);
                return null;
            }

            StructProperty result = null;

            try
            {
                ar._structureLog?.PushStack("StructProperty");
                var pos = ar._position;

                var structType = ar.GetName("structType");

                var end = ar._position + dataSize;

                result = Get(ar, structType, exclusivePropertyNameTreeChildNode);

                if (result == null)
                {
                    // skip struct
                    //todo: logging
                }
                if (ar._position != end)
                {
                    ar.AdvanceTo(end);
                }
            }
            finally
            {
                ar._structureLog?.PopStack();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StructProperty Get(ArchiveReader ar, ArkName structType, ArkNameTree exclusivePropertyNameTreeChildNode = null)
        {
            StructProperty result = null;

            if (structType.Equals(_itemNetId) || structType.Equals(_transform)
                                || structType.Equals(_primalPlayerDataStruct) || structType.Equals(_primalPlayerCharacterConfigStruct)
                                || structType.Equals(_primalPersistentCharacterStatsStruct) || structType.Equals(_tribeData)
                                || structType.Equals(_tribeGovernment) || structType.Equals(_terrainInfo)
                                || structType.Equals(_itemNetInfo) || structType.Equals(_arkInventoryData)
                                || structType.Equals(_dinoOrderGroup) || structType.Equals(_arkDinoData)
                                || structType.Equals(_customItemByteArrays) || structType.Equals(_customItemDoubles))
                result = PropertyListStructReader.Get(ar, exclusivePropertyNameTreeChildNode);
            else if (structType.Equals(_vector) || structType.Equals(_rotator))
                return VectorStructReader.Get(ar);
            else if (structType.Equals(_vector2d))
                return Vector2dStructReader.Get(ar);
            else if (structType.Equals(_quat))
                return QuatStructReader.Get(ar);
            else if (structType.Equals(_color))
                result = ColorStructReader.Get(ar);
            else if (structType.Equals(_linearColor))
                result = LinearColorStructReader.Get(ar);
            else if (structType.Equals(_uniqueNetIdRepl))
                return UniqueNetIdRepltructReader.Get(ar);
            else
            {
                Debug.WriteLine($"Unknown Struct Type {structType} at {ar._position:X} trying to read as StructPropertyList");              
                return PropertyListStructReader.Get(ar, exclusivePropertyNameTreeChildNode);
            }

            return result;
        }
    }
}
