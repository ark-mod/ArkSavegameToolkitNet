using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Domain.Internal;
using ArkSavegameToolkitNet.Utils.Extensions;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkStructureCropPlot : ArkStructure
    {
        private static readonly ArkName _cropPhaseFertilizerCache = ArkName.Create("CropPhaseFertilizerCache");
        private static readonly ArkName _waterAmount = ArkName.Create("WaterAmount");
        private static readonly ArkName _plantedCrop = ArkName.Create("PlantedCrop");

        internal static readonly new ArkNameTree _dependencies = new ArkNameTree
        {
            { _cropPhaseFertilizerCache, null },
            { _waterAmount, null },
            { _plantedCrop, null }
        };

        public ArkStructureCropPlot() : base()
        {
        }

        public ArkStructureCropPlot(GameObject structure, ArkSaveData saveState) : base(structure, saveState)
        {
            FertilizerAmount = structure.GetPropertyValue<float?>(_cropPhaseFertilizerCache);
            WaterAmount = structure.GetPropertyValue<float>(_waterAmount);

            var plantedCropBlueprintGeneratedClass = structure.GetProperty<ObjectReferencePathProperty>(_plantedCrop)?.path?.Token;
            PlantedCropClassName = plantedCropBlueprintGeneratedClass?.SubstringAfterLast('.');
        }

        public float? FertilizerAmount { get; set; }
        public float WaterAmount { get; set; }
        public string PlantedCropClassName { get; set; }
    }
}

//[
//  {
//    "class": "CropPlotLarge_SM_C",
//    "count": 12,
//    "props": [
//      "bHasResetDecayTime (Boolean)",
//      "bIsFertilized (Boolean)",
//      "bIsLocked (Boolean)",
//      "bIsWatered (Boolean) [*]",
//      "CropPhaseFertilizerCache (Single)",
//      "CropRefreshInterval (Single)",
//      "CurrentCropPhase (ArkByteValue)",
//      "Health (Single) [*]",
//      "IrrigationWaterTap (ObjectReference) [*]",
//      "LastCropRefreshTime (Double)",
//      "LastEnterStasisTime (Double)",
//      "LastInAllyRangeTime (Double)",
//      "MaxHealth (Single)",
//      "MyCropStructure (ObjectReference)",
//      "MyInventoryComponent (ObjectReference)",
//      "OriginalCreationTime (Double)",
//      "OwnerName (String)",
//      "PlacedOnFloorStructure (ObjectReference) [*]",
//      "PlantedCrop (ObjectReference)",
//      "StructuresPlacedOnFloor (ArkArrayObjectReference)",
//      "TargetingTeam (Int32)",
//      "WaterAmount (Single)"
//    ]
//  },
//  {
//    "class": "CropPlotMedium_SM_C",
//    "count": 34,
//    "props": [
//      "bHasFruitItems (Boolean)",
//      "bHasResetDecayTime (Boolean)",
//      "bIsFertilized (Boolean) [*]",
//      "bIsSeeded (Boolean) [*]",
//      "bIsWatered (Boolean)",
//      "CropPhaseFertilizerCache (Single) [*]",
//      "CropRefreshInterval (Single)",
//      "CurrentCropPhase (ArkByteValue) [*]",
//      "IrrigationWaterTap (ObjectReference)",
//      "LastCropRefreshTime (Double)",
//      "LastEnterStasisTime (Double)",
//      "LastInAllyRangeTime (Double)",
//      "MaxHealth (Single)",
//      "MyInventoryComponent (ObjectReference)",
//      "NumGreenHouseStructures (ArkByteValue)",
//      "OriginalCreationTime (Double)",
//      "OwnerName (String)",
//      "PlacedOnFloorStructure (ObjectReference)",
//      "PlantedCrop (ObjectReference) [*]",
//      "TargetingTeam (Int32)",
//      "WaterAmount (Single)"
//    ]
//  },
//  {
//    "class": "CropPlotSmall_SM_C",
//    "count": 102,
//    "props": [
//      "bHasFruitItems (Boolean) [*]",
//      "bHasResetDecayTime (Boolean)",
//      "bIsSeeded (Boolean) [*]",
//      "bIsWatered (Boolean)",
//      "CropRefreshInterval (Single)",
//      "IrrigationWaterTap (ObjectReference)",
//      "LastCropRefreshTime (Double)",
//      "LastEnterStasisTime (Double)",
//      "LastInAllyRangeTime (Double)",
//      "MaxHealth (Single)",
//      "MyInventoryComponent (ObjectReference)",
//      "NumGreenHouseStructures (ArkByteValue)",
//      "OriginalCreationTime (Double)",
//      "OwnerName (String)",
//      "PlacedOnFloorStructure (ObjectReference)",
//      "TargetingTeam (Int32)",
//      "WaterAmount (Single)"
//    ]
//  }
//]