using ArkSavegameToolkitNet.Arrays;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkItem : ArkGameDataContainerBase
    {
        private static readonly ArkName _itemId = ArkName.Create("ItemId");
        private static readonly ArkName _itemId1 = ArkName.Create("ItemID1");
        private static readonly ArkName _itemId2 = ArkName.Create("ItemID2");
        private static readonly ArkName _ownerInventory = ArkName.Create("OwnerInventory");
        private static readonly ArkName _itemQuantity = ArkName.Create("ItemQuantity");
        private static readonly ArkName _bIsBlueprint = ArkName.Create("bIsBlueprint");
        private static readonly ArkName _bIsEngram = ArkName.Create("bIsEngram");
        private static readonly ArkName _bHideFromInventoryDisplay = ArkName.Create("bHideFromInventoryDisplay");
        private static readonly ArkName _customItemDescription = ArkName.Create("CustomItemDescription");
        private static readonly ArkName _customItemName = ArkName.Create("CustomItemName"); 
        private static readonly ArkName _itemRating = ArkName.Create("ItemRating");
        private static readonly ArkName _itemQualityIndex = ArkName.Create("ItemQualityIndex");
        private static readonly ArkName _savedDurability = ArkName.Create("SavedDurability");
        private static readonly ArkName _creationTime = ArkName.Create("CreationTime");
        private static readonly ArkName _originalItemDropLocation = ArkName.Create("OriginalItemDropLocation");

        private static readonly ArkName[] _itemStatValues = new[]
        {
            ArkName.Create("ItemStatValues", 0), //Effectiveness
            ArkName.Create("ItemStatValues", 1), //Armor
            ArkName.Create("ItemStatValues", 2), //Max Durability
            ArkName.Create("ItemStatValues", 3), //Weapon Damage
            ArkName.Create("ItemStatValues", 4), //Weapon Clip Ammo
            ArkName.Create("ItemStatValues", 5), //Hypothermic Insulation
            ArkName.Create("ItemStatValues", 6), //Weight
            ArkName.Create("ItemStatValues", 7) //Hyperthermic Insulation
        };

        private static readonly ArkName _eggDinoAncestors = ArkName.Create("EggDinoAncestors");
        private static readonly ArkName _eggDinoAncestorsMale = ArkName.Create("EggDinoAncestorsMale");
        private static readonly ArkName[] _eggColorSetIndices = new[]
        {
            ArkName.Create("EggColorSetIndices", 0),
            ArkName.Create("EggColorSetIndices", 1),
            ArkName.Create("EggColorSetIndices", 2),
            ArkName.Create("EggColorSetIndices", 3),
            ArkName.Create("EggColorSetIndices", 4),
            ArkName.Create("EggColorSetIndices", 5)
        };
        private static readonly ArkName[] _eggNumberOfLevelUpPointsApplied = new[]
        {
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 0), //health
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 1), //stamina
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 2), //torpor
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 3), //oxygen
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 4), //food
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 5), //water
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 6), //temperature
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 7), //weight
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 8), //melee damage
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 9), //movement speed
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 10), //fortitude
            ArkName.Create("EggNumberOfLevelUpPointsApplied", 11) //crafting speed
        };
        private static readonly ArkName _eggRandomMutationsFemale = ArkName.Create("EggRandomMutationsFemale");
        private static readonly ArkName _eggRandomMutationsMale = ArkName.Create("EggRandomMutationsMale");

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            {
                _itemId,
                new ArkNameTree
                {
                    { _itemId1, null },
                    { _itemId2, null }
                }
            },
            { _ownerInventory, null },
            { _itemQuantity, null },
            { _bIsBlueprint, null },
            { _bIsEngram, null },
            { _bHideFromInventoryDisplay, null },
            { _customItemDescription, null },
            { _customItemName, null },
            { _itemRating, null },
            { _savedDurability, null },
            { _itemQualityIndex, null },
            { _creationTime, null },
            { _itemStatValues[0], null },
            { _itemStatValues[1], null },
            { _itemStatValues[2], null },
            { _itemStatValues[3], null },
            { _itemStatValues[4], null },
            { _itemStatValues[5], null },
            { _itemStatValues[6], null },
            { _itemStatValues[7], null },
            { _eggDinoAncestors, null },
            { _eggDinoAncestorsMale, null },
            { _eggColorSetIndices[0], null },
            { _eggColorSetIndices[1], null },
            { _eggColorSetIndices[2], null },
            { _eggColorSetIndices[3], null },
            { _eggColorSetIndices[4], null },
            { _eggColorSetIndices[5], null },
            { _eggNumberOfLevelUpPointsApplied[0], null },
            { _eggNumberOfLevelUpPointsApplied[1], null },
            { _eggNumberOfLevelUpPointsApplied[2], null },
            { _eggNumberOfLevelUpPointsApplied[3], null },
            { _eggNumberOfLevelUpPointsApplied[4], null },
            { _eggNumberOfLevelUpPointsApplied[5], null },
            { _eggNumberOfLevelUpPointsApplied[6], null },
            { _eggNumberOfLevelUpPointsApplied[7], null },
            { _eggNumberOfLevelUpPointsApplied[8], null },
            { _eggNumberOfLevelUpPointsApplied[9], null },
            { _eggNumberOfLevelUpPointsApplied[10], null },
            { _eggNumberOfLevelUpPointsApplied[11], null },
            { _eggRandomMutationsMale, null },
            { _eggRandomMutationsFemale, null },
            { _originalItemDropLocation, null }
        };

        // Filters
        internal static Func<ArkItem, bool> Filter_RealItems = new Func<ArkItem, bool>((i) => !(i.IsEngram || i.HideFromInventoryDisplay));

        internal IGameObject _item;
        private ISaveState _saveState;

        internal void Decouple()
        {
            _item = null;
        }

        public ArkItem()
        {
        }

        public ArkItem(IGameObject item, ISaveState saveState) : this()
        {
            _item = item;
            _saveState = saveState;

            //Id = item.Index;
            ClassName = item.ClassName.Name;
            var itemId = item.GetPropertyValue<StructPropertyList>(_itemId);

            //todo: Scorched Earth store buff data as items that have no id. There may be other cases.
            //Do we want missing item ids to be set to 0 or do we need to change the Id-properties to be nullable?

            if (itemId != null)
            {
                Id1 = itemId.GetPropertyValue<uint>(_itemId1);
                Id2 = itemId.GetPropertyValue<uint>(_itemId2);
            }
            OwnerInventoryId = item.GetPropertyValue<ObjectReference>(_ownerInventory)?.ObjectId;
            //OwnerContainerId
            Quantity = item.GetPropertyValue<uint?>(_itemQuantity) ?? 1;
            IsBlueprint = item.GetPropertyValue<bool?>(_bIsBlueprint) ?? false;
            IsEngram = item.GetPropertyValue<bool?>(_bIsEngram) ?? false;
            HideFromInventoryDisplay = item.GetPropertyValue<bool?>(_bHideFromInventoryDisplay) ?? false;
            CustomDescription = item.GetPropertyValue<string>(_customItemDescription);
            CustomName = item.GetPropertyValue<string>(_customItemName);
            Rating = item.GetPropertyValue<float?>(_itemRating);
            SavedDurability = item.GetPropertyValue<float?>(_savedDurability);
            QualityIndex = item.GetPropertyValue<sbyte?>(_itemQualityIndex);
            CreationTime = item.GetPropertyValue<double?>(_creationTime);
            {
                var statValues = new short?[_itemStatValues.Length];
                var found = 0;
                for (var i = 0; i < statValues.Length; i++)
                {
                    var statValue = item.GetPropertyValue<short?>(_itemStatValues[i]);
                    if (statValue == null) continue;

                    found++;
                    statValues[i] = statValue.Value;
                }
                if (found > 0) StatValues = statValues;
            }

            EggDinoAncestors = ArkTamedCreatureAncestor.FromPropertyValue(item.GetPropertyValue<ArkArrayStruct>(_eggDinoAncestors));
            EggDinoAncestorsMale = ArkTamedCreatureAncestor.FromPropertyValue(item.GetPropertyValue<ArkArrayStruct>(_eggDinoAncestorsMale));

            {
                var colors = new sbyte[_eggColorSetIndices.Length];
                var found = 0;
                for (var i = 0; i < colors.Length; i++)
                {
                    var color = item.GetPropertyValue<sbyte?>(_eggColorSetIndices[i]);
                    if (color == null) continue;

                    found++;
                    colors[i] = color.Value;
                }
                if (found > 0) EggColors = colors;
            }
            {
                var basestats = new sbyte[_eggNumberOfLevelUpPointsApplied.Length];
                var found = 0;
                for (var i = 0; i < basestats.Length; i++)
                {
                    var basestat = item.GetPropertyValue<sbyte?>(_eggNumberOfLevelUpPointsApplied[i]);
                    if (basestat == null) continue;

                    found++;
                    basestats[i] = basestat.Value;
                }
                if (found > 0) EggBaseStats = basestats;
            }
            EggRandomMutationsMale = item.GetPropertyValue<int?>(_eggRandomMutationsMale);
            EggRandomMutationsFemale = item.GetPropertyValue<int?>(_eggRandomMutationsFemale);

            //todo: LocationData should be replaced since drop-locations do not have pitch, roll and yaw
            var dropLocation = item.GetPropertyValue<StructVector>(_originalItemDropLocation);
            if (dropLocation != null) Location = new ArkLocation(new LocationData { X = dropLocation.X, Y = dropLocation.Y, Z = dropLocation.Z }, saveState);
        }

        //public int Id { get; set; }
        public uint Id1 { get; set; }
        public uint Id2 { get; set; }
        public ulong Id => ((ulong)Id1 << 32) | Id2;
        public string ClassName { get; set; }
        public int? OwnerInventoryId { get; set; }
        public int? OwnerContainerId { get; set; }
        public bool IsBlueprint { get; set; }
        public bool IsEngram { get; set; }
        internal bool HideFromInventoryDisplay { get; set; }
        public uint Quantity { get; set; }
        public string CustomDescription { get; set; }
        public string CustomName { get; set; }
        public float? Rating { get; set; }
        public float? SavedDurability { get; set; }
        public sbyte? QualityIndex { get; set; }
        public double? CreationTime { get; set; }
        [JsonIgnore]
        public TimeSpan? ExistedForApprox => _saveState?.GetApproxTimeElapsedSince(CreationTime);
        public short?[] StatValues { get; set; }
        public ArkLocation Location { get; set; }

        public ArkTamedCreatureAncestor[] EggDinoAncestors { get; set; }
        public ArkTamedCreatureAncestor[] EggDinoAncestorsMale { get; set; }
        public sbyte[] EggColors { get; set; }
        public sbyte[] EggBaseStats { get; set; }
        public int? EggRandomMutationsFemale { get; set; }
        public int? EggRandomMutationsMale { get; set; }
        public int? EggRandomMutationTotal =>
            EggRandomMutationsMale.HasValue || EggRandomMutationsFemale.HasValue ?
            ((EggRandomMutationsMale ?? 0) + (EggRandomMutationsFemale ?? 0))
            : (int?)null;
    }
}