using ArkSavegameToolkitNet.Exceptions;
using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class StructRegistry
    {
        private static ILog _logger = LogManager.GetLogger(typeof(StructRegistry));

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

        private static readonly ArkName _vector = ArkName.Create("Vector");
        private static readonly ArkName _vector2d = ArkName.Create("Vector2D");
        private static readonly ArkName _quat = ArkName.Create("Quat");
        private static readonly ArkName _color = ArkName.Create("Color");
        private static readonly ArkName _linearColor = ArkName.Create("LinearColor");
        private static readonly ArkName _rotator = ArkName.Create("Rotator");
        private static readonly ArkName _uniqueNetIdRepl = ArkName.Create("UniqueNetIdRepl");

        public static IStruct read(ArkArchive archive, ArkName structType)
        {
            if (structType.Equals(_itemNetId) || structType.Equals(_transform)
                || structType.Equals(_primalPlayerDataStruct) || structType.Equals(_primalPlayerCharacterConfigStruct)
                || structType.Equals(_primalPersistentCharacterStatsStruct) || structType.Equals(_tribeData)
                || structType.Equals(_tribeGovernment) || structType.Equals(_terrainInfo)
                || structType.Equals(_itemNetInfo) || structType.Equals(_arkInventoryData)
                || structType.Equals(_dinoOrderGroup) || structType.Equals(_arkDinoData)) return new StructPropertyList(archive, structType);
            else if (structType.Equals(_vector) || structType.Equals(_rotator)) return new StructVector(archive, structType);
            else if (structType.Equals(_vector2d)) return new StructVector2d(archive, structType);
            else if (structType.Equals(_quat)) return new StructQuat(archive, structType);
            else if (structType.Equals(_color)) return new StructColor(archive, structType);
            else if (structType.Equals(_linearColor)) return new StructLinearColor(archive, structType);
            else if (structType.Equals(_uniqueNetIdRepl)) return new StructUniqueNetIdRepl(archive, structType);
            else
            {
                _logger.Warn($"Unknown Struct Type {structType} at {archive.Position:X} trying to read as StructPropertyList");
                return new StructPropertyList(archive, structType);
            }
        }
    }
}
