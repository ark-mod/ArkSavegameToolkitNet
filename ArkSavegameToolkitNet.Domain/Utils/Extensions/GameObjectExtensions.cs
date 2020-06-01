using ArkSavegameToolkitNet.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArkSavegameToolkitNet.Domain.Utils.Extensions
{
    public static class GameObjectExtensions
    {
        public static ArkTamedCreature AsTamedCreature(this GameObject self, GameObject status, GameObject inventory, ArkSaveData saveState)
        {
            return new ArkTamedCreature(self, status, inventory, saveState);
        }

        public static ArkTamedCreature AsTamedCreature(this GameObject self, GameObject status, ArkSaveData saveState)
        {
            return new ArkTamedCreature(self, status, saveState);
        }

        public static ArkWildCreature AsWildCreature(this GameObject self, GameObject status, ArkSaveData saveState)
        {
            return new ArkWildCreature(self, status, saveState);
        }

        public static ArkPlayer AsPlayer(this GameObject self, GameObject player, DateTime profileSaveTime, ArkSaveData saveState)
        {
            return new ArkPlayer(self, player, profileSaveTime, saveState);
        }

        public static ArkPlayer AsPlayer(this ArkPlayerExternal self)
        {
            return new ArkPlayer(self);
        }

        public static ArkTribe AsTribe(this GameObject self, DateTime tribeSaveTime)
        {
            return new ArkTribe(self, tribeSaveTime);
        }

        public static ArkStructure AsStructure(this GameObject self, ArkSaveData saveState)
        {
            if (self?.className?.Token != null && ArkToolkitSettings.Instance.ObjectTypes != null)
            {
                var className = self.className.Token;
                List<string> classNames = null;

                // crop plots
                if (ArkToolkitSettings.Instance.ObjectTypes.TryGetValue(ObjectType.StructureCropPlot, out classNames)
                    && classNames.Contains(className, StringComparer.Ordinal))
                {
                    return new ArkStructureCropPlot(self, saveState);
                }

                // electric generators
                if (ArkToolkitSettings.Instance.ObjectTypes.TryGetValue(ObjectType.StructureElectricGenerator, out classNames)
                    && classNames.Contains(className, StringComparer.Ordinal))
                {
                    return new ArkStructureElectricGenerator(self, saveState);
                }
            }

            // fallback to default
            return new ArkStructure(self, saveState);
        }

        public static ArkItem AsItem(this GameObject self, ArkSaveData saveState)
        {
            if (self?.className?.Token != null)
            {
                var className = self.className.Token;
                if (className.Equals("PrimalItem_WeaponEmptyCryopod_C", StringComparison.Ordinal))
                    return new ArkItemCryopod(self, saveState);

            }

            return new ArkItem(self, saveState);
        }

        public static ArkCloudInventory AsCloudInventory(this GameObject self, string steamId, ClusterSaveGameData saveState)
        {
            return new ArkCloudInventory(steamId, self, saveState);
        }
    }
}
