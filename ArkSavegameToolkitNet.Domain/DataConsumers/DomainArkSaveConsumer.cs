using ArkSavegameToolkitNet.DataConsumers;
using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Domain.Utils.Extensions;
using ArkSavegameToolkitNet.Utils;
using ArkSavegameToolkitNet.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkSavegameToolkitNet.Domain.DataConsumers
{
    public class DomainArkSaveConsumer : IDataConsumer
    {
        private static readonly ArkName _myCharacterStatusComponent = ArkName.Create("MyCharacterStatusComponent");
        private static readonly ArkName _linkedPlayerDataID = ArkName.Create("LinkedPlayerDataID");

        public List<ArkItem> Items { get; set; } = new List<ArkItem>();
        public List<ArkTamedCreature> TamedCreatures { get; set; } = new List<ArkTamedCreature>();
        public List<ArkWildCreature> WildCreatures { get; set; } = new List<ArkWildCreature>();
        public List<ArkStructure> Structures { get; set; } = new List<ArkStructure>();
        public List<GameObject> StatusComponents { get; set; } = new List<GameObject>();
        public List<GameObject> PlayerCharacters { get; set; } = new List<GameObject>();

        public Dictionary<int, GameObject> StatusComponentsMap { get; set; } = new Dictionary<int, GameObject>();
        public ILookup<ulong, GameObject> PlayerCharactersMap { get; set; }

        public ArkSaveData SaveData { get; set; }

        [Flags]
        private enum GameObjectIs
        {
            None = 0,
            IsCreature = 1 << 0,
            IsTamedCreature = 1 << 1,
            IsWildCreature = 1 << 2,
            IsRaftCreature = 1 << 3,
            IsStructure = 1 << 4,
            IsInventory = 1 << 5,
            IsTamedCreatureInventory = 1 << 6,
            IsWildCreatureInventory = 1 << 7,
            IsStructureInventory = 1 << 8,
            IsPlayerCharacterInventory = 1 << 9,
            IsStatusComponent = 1 << 10,
            IsDinoStatusComponent = 1 << 11,
            IsPlayerCharacterStatusComponent = 1 << 12,
            IsDroppedItem = 1 << 13,
            IsPlayerCharacter = 1 << 14,
            IsStructurePaintingComponent = 1 << 15,
            IsDeathItemCache = 1 << 16,
            IsSomethingElse = 1 << 17
        }

        private static ArkName _dinoId1 = ArkName.Create("DinoID1", 0);
        private static ArkName _tamerString = ArkName.Create("TamerString", 0);
        private static ArkName _tamingTeamID = ArkName.Create("TamingTeamID", 0);
        private static ArkName _ownerName = ArkName.Create("OwnerName", 0);
        private static ArkName _bHasResetDecayTime = ArkName.Create("bHasResetDecayTime", 0);
        private static ArkName _bInitializedMe = ArkName.Create("bInitializedMe", 0);
        private static ArkName _currentStatusValues = ArkName.Create("CurrentStatusValues", 0);
        private static ArkName _raft_bp_c = ArkName.Create("Raft_BP_C", 0);
        private static ArkName _motorraft_bp_c = ArkName.Create("MotorRaft_BP_C", 0);
        private static ArkName _structurePaintingComponent = ArkName.Create("StructurePaintingComponent", 0);
        private static ArkName _male = ArkName.Create("PlayerPawnTest_Male_C", 0);
        private static ArkName _female = ArkName.Create("PlayerPawnTest_Female_C", 0);
        private static ArkName _droppedItem = ArkName.Create("DroppedItemGenericLowQuality_C", 0);

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            { _dinoId1, null },
            { _tamerString, null },
            { _tamingTeamID, null },
            { _ownerName, null },
            { _bHasResetDecayTime, null },
            { _bInitializedMe, null },
            { _currentStatusValues, null }
        };

        public void Completed()
        {
            // Remove duplicates from object collection (objects are sometimes duplicated for structures, creatures etc.)
            // var objects = save.Objects.GroupBy(x => x.Names, new ArkNameCollectionComparer()).Select(x => x.OrderBy(y => y.ObjectId).First()).ToArray();
            // Note: objects.GroupBy(x => x.Names.Last().Token) would also get creature, status- and inventory component together
            var cmp = new ArkNameCollectionComparer();

            Items = Items.GroupBy(x => x._item.names, cmp).Select(x => x.OrderBy(y => y._item.objectId).First()).ToList();
            TamedCreatures = TamedCreatures.GroupBy(x => x._creature.names, cmp).Select(x => x.OrderBy(y => y._creature.objectId).First()).ToList();
            WildCreatures = WildCreatures.GroupBy(x => x._creature.names, cmp).Select(x => x.OrderBy(y => y._creature.objectId).First()).ToList();
            Structures = Structures.GroupBy(x => x._structure.names, cmp).Select(x => x.OrderBy(y => y._structure.objectId).First()).ToList();
            StatusComponents = StatusComponents.GroupBy(x => x.names, cmp).Select(x => x.OrderBy(y => y.objectId).First()).ToList();
            PlayerCharacters = PlayerCharacters.GroupBy(x => x.names, cmp).Select(x => x.OrderBy(y => y.objectId).First()).ToList();

            StatusComponentsMap = StatusComponents.ToDictionary(x => x.objectId, x => x);
            PlayerCharactersMap = PlayerCharacters.ToLookup(x => x.GetPropertyValue<ulong>(_linkedPlayerDataID), x => x);

            foreach (var c in TamedCreatures)
            {
                if (!StatusComponentsMap.TryGetValue(c._creature.GetProperty<ObjectReferenceIdProperty>(_myCharacterStatusComponent).id, out var status))
                    continue;

                c.UpdateStatus(status);
            }

            foreach (var c in WildCreatures)
            {
                if (!StatusComponentsMap.TryGetValue(c._creature.GetProperty<ObjectReferenceIdProperty>(_myCharacterStatusComponent).id, out var status))
                    continue;

                c.UpdateStatus(status);
            }
        }

        public void Push<TDataType>(TDataType entry) where TDataType : IDataEntry
        {
            if (entry is GameObject)
            {
                var obj = entry as GameObject;
                var isFlags = GameObjectIs.None;

                if (obj.isItem)
                {
                    Items.Add(obj.AsItem(SaveData));

                    goto SkipRest;
                }

                if (obj.Properties.ContainsKey(_ownerName) || obj.Properties.ContainsKey(_bHasResetDecayTime))
                {
                    if (obj.className.Token.StartsWith("DeathItemCache_"))
                    {
                        isFlags |= GameObjectIs.IsDeathItemCache;
                        goto SkipRest;
                    }

                    isFlags |= GameObjectIs.IsStructure;

                    Structures.Add(obj.AsStructure(SaveData));

                    goto SkipRest;
                }

                if (obj.Properties.ContainsKey(_dinoId1)) isFlags |= GameObjectIs.IsCreature;
                if (((isFlags & GameObjectIs.IsCreature) != 0) && (obj.Properties.ContainsKey(_tamerString) || obj.Properties.ContainsKey(_tamingTeamID)))
                {
                    isFlags |= GameObjectIs.IsTamedCreature;
                    TamedCreatures.Add(obj.AsTamedCreature(null, SaveData));
                }
                if (((isFlags & GameObjectIs.IsCreature) != 0) && !((isFlags & GameObjectIs.IsTamedCreature) != 0))
                {
                    isFlags |= GameObjectIs.IsWildCreature;
                    WildCreatures.Add(obj.AsWildCreature(null, SaveData));
                }
                if (((isFlags & GameObjectIs.IsTamedCreature) != 0) && (obj.className.Equals(_raft_bp_c) || obj.className.Equals(_motorraft_bp_c))) isFlags |= GameObjectIs.IsRaftCreature;
                if (((isFlags & GameObjectIs.IsCreature) != 0)) goto SkipRest;

                if (obj.Properties.ContainsKey(_currentStatusValues)) isFlags |= GameObjectIs.IsStatusComponent;
                if (((isFlags & GameObjectIs.IsStatusComponent) != 0) && obj.className.Token.StartsWith("DinoCharacterStatusComponent_")) isFlags |= GameObjectIs.IsDinoStatusComponent;
                if (((isFlags & GameObjectIs.IsStatusComponent) != 0) && !((isFlags & GameObjectIs.IsDinoStatusComponent) != 0) && obj.className.Token.StartsWith("PlayerCharacterStatusComponent_")) isFlags |= GameObjectIs.IsPlayerCharacterStatusComponent;
                if (((isFlags & GameObjectIs.IsStatusComponent) != 0))
                {
                    StatusComponents.Add(obj);
                    goto SkipRest;
                }

                if (obj.Properties.ContainsKey(_bInitializedMe) || obj.className.Token.StartsWith("DinoTamedInventoryComponent_")) isFlags |= GameObjectIs.IsInventory;
                if (((isFlags & GameObjectIs.IsInventory) != 0) && obj.className.Token.StartsWith("PrimalInventoryBP_")) isFlags |= GameObjectIs.IsStructureInventory;
                if (((isFlags & GameObjectIs.IsInventory) != 0) && !((isFlags & GameObjectIs.IsStructureInventory) != 0) && obj.className.Token.StartsWith("DinoTamedInventoryComponent_")) isFlags |= GameObjectIs.IsTamedCreatureInventory;
                if (((isFlags & GameObjectIs.IsInventory) != 0) && !(((isFlags & GameObjectIs.IsStructureInventory) != 0) || ((isFlags & GameObjectIs.IsTamedCreatureInventory) != 0)) && obj.className.Token.StartsWith("PrimalInventoryComponent")) isFlags |= GameObjectIs.IsPlayerCharacterInventory;
                if (((isFlags & GameObjectIs.IsInventory) != 0) && !(((isFlags & GameObjectIs.IsStructureInventory) != 0) || ((isFlags & GameObjectIs.IsTamedCreatureInventory) != 0) || ((isFlags & GameObjectIs.IsPlayerCharacterInventory) != 0)) && obj.className.Token.StartsWith("DinoWildInventoryComponent_")) isFlags |= GameObjectIs.IsWildCreatureInventory;
                if (((isFlags & GameObjectIs.IsInventory) != 0)) goto SkipRest;

                if (obj.className.Equals(_structurePaintingComponent))
                {
                    isFlags |= GameObjectIs.IsStructurePaintingComponent;
                    goto SkipRest;
                }

                if (obj.className.Equals(_droppedItem))
                {
                    isFlags |= GameObjectIs.IsDroppedItem;
                    goto SkipRest;
                }

                if (obj.className.Equals(_male) || obj.className.Equals(_female))
                {
                    isFlags |= GameObjectIs.IsPlayerCharacter;

                    PlayerCharacters.Add(obj);
                    goto SkipRest;
                }

                isFlags |= GameObjectIs.IsSomethingElse;

            SkipRest:;
            }
            else if (entry is ArkSaveData)
            {
                SaveData = entry as ArkSaveData;
            }
        }
    }
}
