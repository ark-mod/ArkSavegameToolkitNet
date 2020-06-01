using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Domain.Utils.Serialization;
using ArkSavegameToolkitNet.Utils.Extensions;
using Newtonsoft.Json;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkCloudInventoryItem : ArkClusterDataContainerBase
    {
        private static readonly ArkName _myArkData = ArkName.Create("MyArkData");
        private static readonly ArkName _arkItems = ArkName.Create("ArkItems");

        private static readonly ArkName _arkTributeItem = ArkName.Create("ArkTributeItem"); 
        private static readonly ArkName _itemId = ArkName.Create("ItemId");
        private static readonly ArkName _itemId1 = ArkName.Create("ItemID1");
        private static readonly ArkName _itemId2 = ArkName.Create("ItemID2");
        private static readonly ArkName _itemArchetype = ArkName.Create("ItemArchetype");
        private static readonly ArkName _bIsBlueprint = ArkName.Create("bIsBlueprint");
        private static readonly ArkName _itemQuantity = ArkName.Create("ItemQuantity");
        private static readonly ArkName _customItemName = ArkName.Create("CustomItemName");
        private static readonly ArkName _customItemDescription = ArkName.Create("CustomItemDescription");
        private static readonly ArkName _itemDurability = ArkName.Create("ItemDurability");
        private static readonly ArkName _itemRating = ArkName.Create("ItemRating");
        private static readonly ArkName _itemQualityIndex = ArkName.Create("ItemQualityIndex");

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

        internal IPropertyContainer _item;

        internal void Decouple()
        {
            _item = null;
        }

        public ArkCloudInventoryItem()
        {
        }

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
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
                                        _itemId,
                                        new ArkNameTree
                                        {
                                            { _itemId1, null },
                                            { _itemId2, null }
                                        }
                                    },
                                    { _itemArchetype, null },
                                    { _bIsBlueprint, null },
                                    { _customItemDescription, null },
                                    { _customItemName, null },
                                    { _itemQuantity, null },
                                    { _itemRating, null },
                                    { _itemDurability, null },
                                    { _itemQualityIndex, null },
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
                                    { _eggRandomMutationsFemale, null }
                                }
                            }
                        }
                    }
                }
            }
        };

        public ArkCloudInventoryItem(IPropertyContainer item) : this()
        {
            _item = item;

            var tribute = item.GetProperty<PropertyListStructProperty>(_arkTributeItem);
            var itemid = tribute.GetProperty<PropertyListStructProperty>(_itemId);
            Id1 = itemid.GetPropertyValue<uint>(_itemId1);
            Id2 = itemid.GetPropertyValue<uint>(_itemId2);
            ClassName = tribute.GetProperty<ObjectReferencePathProperty>(_itemArchetype)?.path?.Name;
            IsBlueprint = tribute.GetPropertyValue<bool?>(_bIsBlueprint) ?? false;
            CustomDescription = tribute.GetPropertyValue<string>(_customItemDescription);
            CustomName = tribute.GetPropertyValue<string>(_customItemName);
            Quantity = tribute.GetPropertyValue<uint?>(_itemQuantity) ?? 1;
            Rating = tribute.GetPropertyValue<float?>(_itemRating);
            Durability = tribute.GetPropertyValue<float?>(_itemDurability);
            QualityIndex = tribute.GetPropertyValue<sbyte?>(_itemQualityIndex);
            {
                var statValues = new ushort?[_itemStatValues.Length];
                var found = 0;
                for (var i = 0; i < statValues.Length; i++)
                {
                    var statValue = tribute.GetPropertyValue<ushort?>(_itemStatValues[i]);
                    if (statValue == null) continue;

                    found++;
                    statValues[i] = statValue.Value;
                }
                if (found > 0) StatValues = statValues;
            }

            EggDinoAncestors = ArkTamedCreatureAncestor.FromPropertyValue(tribute.GetProperty<StructArrayProperty>(_eggDinoAncestors));
            EggDinoAncestorsMale = ArkTamedCreatureAncestor.FromPropertyValue(tribute.GetProperty<StructArrayProperty>(_eggDinoAncestorsMale));

            {
                var colors = new byte[_eggColorSetIndices.Length];
                var found = 0;
                for (var i = 0; i < colors.Length; i++)
                {
                    var color = tribute.GetPropertyValue<byte?>(_eggColorSetIndices[i]);
                    if (color == null) continue;

                    found++;
                    colors[i] = color.Value;
                }
                if (found > 0) EggColors = colors;
            }
            {
                var basestats = new byte[_eggNumberOfLevelUpPointsApplied.Length];
                var found = 0;
                for (var i = 0; i < basestats.Length; i++)
                {
                    var basestat = tribute.GetPropertyValue<byte?>(_eggNumberOfLevelUpPointsApplied[i]);
                    if (basestat == null) continue;

                    found++;
                    basestats[i] = basestat.Value;
                }
                if (found > 0) EggBaseStats = basestats;
            }
            EggRandomMutationsMale = tribute.GetPropertyValue<int?>(_eggRandomMutationsMale);
            EggRandomMutationsFemale = tribute.GetPropertyValue<int?>(_eggRandomMutationsFemale);
        }

        public uint Id1 { get; set; }
        public uint Id2 { get; set; }
        public ulong Id => ((ulong)Id1 << 32) | Id2;
        public string ClassName { get; set; }
        public bool IsBlueprint { get; set; }
        public uint Quantity { get; set; }
        public string CustomDescription { get; set; }
        public string CustomName { get; set; }
        public float? Durability { get; set; }
        public float? Rating { get; set; }
        public sbyte? QualityIndex { get; set; }
        public ushort?[] StatValues { get; set; }
         
        public ArkTamedCreatureAncestor[] EggDinoAncestors { get; set; }
        public ArkTamedCreatureAncestor[] EggDinoAncestorsMale { get; set; }
        [JsonConverter(typeof(ByteArrayConverter))]
        public byte[] EggColors { get; set; }
        [JsonConverter(typeof(ByteArrayConverter))]
        public byte[] EggBaseStats { get; set; }
        public int? EggRandomMutationsFemale { get; set; }
        public int? EggRandomMutationsMale { get; set; }
        public int? EggRandomMutationTotal =>
            EggRandomMutationsMale.HasValue || EggRandomMutationsFemale.HasValue ?
            ((EggRandomMutationsMale ?? 0) + (EggRandomMutationsFemale ?? 0))
            : (int?)null;
    }
}

//"MyArkData->ArkItems->ArkTributeItem->bAllowRemovalFromInventory (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bAllowRemovalFromSteamInventory (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bFromSteamInventory (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bHideFromInventoryDisplay (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsBlueprint (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsCustomRecipe (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsEngram (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsEquipped (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsFoodRecipe (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsFromAllClustersInventory (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsInitialItem (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsRepairing (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->bIsSlot (Boolean)",
//"MyArkData->ArkItems->ArkTributeItem->ClusterSpoilingTimeUTC (Double)",
//"MyArkData->ArkItems->ArkTributeItem->CraftingSkill (Single)",
//"MyArkData->ArkItems->ArkTributeItem->CraftQueue (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->CreationTime (Double)",
//"MyArkData->ArkItems->ArkTributeItem->CustomItemColors (StructArrayProperty)",
//"MyArkData->ArkItems->ArkTributeItem->CustomItemDatas (StructArrayProperty)",
//"MyArkData->ArkItems->ArkTributeItem->CustomItemDescription (String)",
//"MyArkData->ArkItems->ArkTributeItem->CustomItemID (Int32)",
//"MyArkData->ArkItems->ArkTributeItem->CustomItemName (String)",
//"MyArkData->ArkItems->ArkTributeItem->CustomResourceRequirements (StructArrayProperty)",
//"MyArkData->ArkItems->ArkTributeItem->EggColorSetIndices (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggColorSetIndices_1 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggColorSetIndices_2 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggColorSetIndices_3 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggColorSetIndices_4 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggColorSetIndices_5 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggDinoAncestors (StructArrayProperty)",
//"MyArkData->ArkItems->ArkTributeItem->EggDinoAncestorsMale (StructArrayProperty)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_1 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_10 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_11 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_2 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_3 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_4 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_5 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_6 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_7 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_8 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggNumberOfLevelUpPointsApplied_9 (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->EggRandomMutationsFemale (Int32)",
//"MyArkData->ArkItems->ArkTributeItem->EggRandomMutationsMale (Int32)",
//"MyArkData->ArkItems->ArkTributeItem->EggTamedIneffectivenessModifier (Single)",
//"MyArkData->ArkItems->ArkTributeItem->ExpirationTimeUTC (Int32)",
//"MyArkData->ArkItems->ArkTributeItem->ItemArchetype (ObjectReference)",
//"MyArkData->ArkItems->ArkTributeItem->ItemColorID (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemColorID_1 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemColorID_2 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemColorID_3 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemColorID_4 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemColorID_5 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemCustomClass (ObjectReference)",
//"MyArkData->ArkItems->ArkTributeItem->ItemDurability (Single)",
//"MyArkData->ArkItems->ArkTributeItem->ItemId->ItemID1 (Int32)",
//"MyArkData->ArkItems->ArkTributeItem->ItemId->ItemID2 (Int32)",
//"MyArkData->ArkItems->ArkTributeItem->ItemProfileVersion (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->ItemQualityIndex (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->ItemQuantity (Int32)",
//"MyArkData->ArkItems->ArkTributeItem->ItemRating (Single)",
//"MyArkData->ArkItems->ArkTributeItem->ItemSkinTemplate (ObjectReference)",
//"MyArkData->ArkItems->ArkTributeItem->ItemStatValues (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemStatValues_1 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemStatValues_2 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemStatValues_3 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemStatValues_4 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemStatValues_5 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemStatValues_6 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemStatValues_7 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->ItemVersion (ArkByteValue)",
//"MyArkData->ArkItems->ArkTributeItem->LastAutoDurabilityDecreaseTime (Double)",
//"MyArkData->ArkItems->ArkTributeItem->LastOwnerPlayer (ObjectReference)",
//"MyArkData->ArkItems->ArkTributeItem->LastSpoilingTime (Double)",
//"MyArkData->ArkItems->ArkTributeItem->NextCraftCompletionTime (Double)",
//"MyArkData->ArkItems->ArkTributeItem->NextSpoilingTime (Double)",
//"MyArkData->ArkItems->ArkTributeItem->OriginalItemDropLocation (VectorStructProperty)",
//"MyArkData->ArkItems->ArkTributeItem->OwnerPlayerDataID (Int64)",
//"MyArkData->ArkItems->ArkTributeItem->PreSkinItemColorID (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->PreSkinItemColorID_1 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->PreSkinItemColorID_2 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->PreSkinItemColorID_3 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->PreSkinItemColorID_4 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->PreSkinItemColorID_5 (Int16)",
//"MyArkData->ArkItems->ArkTributeItem->SlotIndex (Int32)",
//"MyArkData->ArkItems->ArkTributeItem->SteamUserItemID (ArkArrayLong)",
//"MyArkData->ArkItems->ArkTributeItem->WeaponClipAmmo (Int32)",
//"MyArkData->ArkItems->UploadTime (Int32)",
//"MyArkData->ArkItems->Version (Single)",