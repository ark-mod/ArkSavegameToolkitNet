using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.Utils.Extensions;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkStructureElectricGenerator : ArkStructure
    {
        private static readonly ArkName _currentFuelTimeCache = ArkName.Create("CurrentFuelTimeCache");
        //private static readonly ArkName _bLastToggleActivated = ArkName.Create("bLastToggleActivated"); //last toggle status (does not tell us if it ran out of gas)
        private static readonly ArkName _bContainerActivated = ArkName.Create("bContainerActivated");

        internal static readonly new ArkNameTree _dependencies = new ArkNameTree
        {
            { _currentFuelTimeCache, null },
            { _bContainerActivated, null }
        };

        public ArkStructureElectricGenerator() : base()
        {
        }

        public ArkStructureElectricGenerator(GameObject structure, ArkSaveData saveState) : base(structure, saveState)
        {
            FuelTime = structure.GetPropertyValue<double?>(_currentFuelTimeCache);
            Activated = structure.GetPropertyValue<bool?>(_bContainerActivated) ?? false;
        }

        public double? FuelTime { get; set; }
        public bool Activated { get; set; }
    }
}

//[
//  {
//    "class": "ElectricGenerator_C",
//    "count": 7,
//    "props": [
//      "AttachedToDinoID1 (Int32) [*]",
//      "bContainerActivated (Boolean) [*]",
//      "bHasFuel (Boolean) [*]",
//      "bHasResetDecayTime (Boolean)",
//      "bIsLocked (Boolean)",
//      "bLastToggleActivated (Boolean) [*]",
//      "CurrentFuelTimeCache (Double) [*]",
//      "LastCheckedFuelTime (Double)",
//      "LastEnterStasisTime (Double)",
//      "LastInAllyRangeTime (Double)",
//      "LinkedStructures (ArkArrayObjectReference)",
//      "MaxHealth (Single)",
//      "MyInventoryComponent (ObjectReference)",
//      "OriginalCreationTime (Double)",
//      "OwnerName (String)",
//      "OwningPlayerID (Int32) [*]",
//      "OwningPlayerName (String) [*]",
//      "PlacedOnFloorStructure (ObjectReference) [*]",
//      "SaddleDino (ObjectReference) [*]",
//      "TargetingTeam (Int32)"
//    ]
//  }
//]