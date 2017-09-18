
using ArkSavegameToolkitNet.Domain;
using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public static class IGameObjectExtensions
    {
        public static ArkTamedCreature AsTamedCreature(this IGameObject self, IGameObject status, ISaveState saveState)
        {
            return new ArkTamedCreature(self, status, saveState);
        }

        public static ArkWildCreature AsWildCreature(this IGameObject self, IGameObject status, ISaveState saveState)
        {
            return new ArkWildCreature(self, status, saveState);
        }

        public static ArkPlayer AsPlayer(this IGameObject self, IGameObject player, DateTime profileSaveTime, ISaveState saveState)
        {
            return new ArkPlayer(self, player, profileSaveTime, saveState);
        }

        public static ArkPlayer AsPlayer(this ArkPlayerExternal self)
        {
            return new ArkPlayer(self);
        }

        public static ArkTribe AsTribe(this IGameObject self, DateTime tribeSaveTime)
        {
            return new ArkTribe(self, tribeSaveTime);
        }

        public static ArkStructure AsStructure(this IGameObject self, ISaveState saveState)
        {
            if (self?.ClassName?.Token != null)
            {
                var className = self.ClassName.Token;
                if (className.Equals("CropPlotSmall_SM_C", StringComparison.Ordinal)
                    || className.Equals("CropPlotMedium_SM_C", StringComparison.Ordinal)
                    || className.Equals("CropPlotLarge_SM_C", StringComparison.Ordinal))
                    return new ArkStructureCropPlot(self, saveState);
                if (className.Equals("ElectricGenerator_C", StringComparison.Ordinal))
                    return new ArkStructureElectricGenerator(self, saveState);
            }
            return new ArkStructure(self, saveState);
        }

        public static ArkItem AsItem(this IGameObject self, ISaveState saveState)
        {
            return new ArkItem(self, saveState);
        }

        public static ArkCloudInventory AsCloudInventory(this IGameObject self, string steamId, ISaveState saveState, ICloudInventoryDinoData[] dinoData)
        {
            return new ArkCloudInventory(steamId, self, saveState, dinoData);
        }
    }
}
