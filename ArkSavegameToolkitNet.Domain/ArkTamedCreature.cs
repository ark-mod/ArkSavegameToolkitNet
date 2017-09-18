using ArkSavegameToolkitNet.Arrays;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkTamedCreature : ArkCreature
    {
        private static readonly ArkName _owningPlayerID = ArkName.Create("OwningPlayerID");
        private static readonly ArkName _owningPlayerName = ArkName.Create("OwningPlayerName");
        private static readonly ArkName _randomMutationsFemale = ArkName.Create("RandomMutationsFemale");
        private static readonly ArkName _randomMutationsMale = ArkName.Create("RandomMutationsMale");
        private static readonly ArkName _tamedAtTime = ArkName.Create("TamedAtTime");
        private static readonly ArkName _tamedName = ArkName.Create("TamedName");
        private static readonly ArkName _tamedOnServerName = ArkName.Create("TamedOnServerName");
        //private static readonly ArkName _tameIneffectivenessModifier = ArkName.Create("TameIneffectivenessModifier");
        private static readonly ArkName _tamerString = ArkName.Create("TamerString");
        private static readonly ArkName _targetingTeam = ArkName.Create("TargetingTeam");
        private static readonly ArkName _tribeName = ArkName.Create("TribeName");
        private static readonly ArkName _lastUpdatedGestationAtTime = ArkName.Create("LastUpdatedGestationAtTime");
        private static readonly ArkName _lastUpdatedMatingAtTime = ArkName.Create("_lastUpdatedMatingAtTime");

        private static readonly ArkName _babyGestationProgress = ArkName.Create("BabyGestationProgress");
        private static readonly ArkName _babyNextCuddleTime = ArkName.Create("BabyNextCuddleTime");
        private static readonly ArkName _bNeutered = ArkName.Create("bNeutered");
        private static readonly ArkName _dinoAncestors = ArkName.Create("DinoAncestors");
        private static readonly ArkName _dinoAncestorsMale = ArkName.Create("DinoAncestorsMale");
        private static readonly ArkName[] _gestationEggColorSetIndices = new[]
        {
            ArkName.Create("GestationEggColorSetIndices", 0),
            ArkName.Create("GestationEggColorSetIndices", 1),
            ArkName.Create("GestationEggColorSetIndices", 2),
            ArkName.Create("GestationEggColorSetIndices", 3),
            ArkName.Create("GestationEggColorSetIndices", 4),
            ArkName.Create("GestationEggColorSetIndices", 5)
        };
        private static readonly ArkName[] _gestationEggNumberOfLevelUpPointsApplied = new[]
        {
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 0), //health
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 1), //stamina
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 2), //torpor
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 3), //oxygen
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 4), //food
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 5), //water
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 6), //temperature
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 7), //weight
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 8), //melee damage
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 9), //movement speed
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 10), //fortitude
            ArkName.Create("GestationEggNumberOfLevelUpPointsApplied", 11) //crafting speed
        };
        private static readonly ArkName _gestationEggRandomMutationsFemale = ArkName.Create("GestationEggRandomMutationsFemale");
        private static readonly ArkName _gestationEggRandomMutationsMale = ArkName.Create("GestationEggRandomMutationsMale");
        private static readonly ArkName _imprinterName = ArkName.Create("ImprinterName");
        private static readonly ArkName _imprinterPlayerDataID = ArkName.Create("ImprinterPlayerDataID");
        private static readonly ArkName _nextAllowedMatingTime = ArkName.Create("NextAllowedMatingTime");
        private static readonly ArkName _nextBabyDinoAncestors = ArkName.Create("NextBabyDinoAncestors");
        private static readonly ArkName _nextBabyDinoAncestorsMale = ArkName.Create("NextBabyDinoAncestorsMale");

        //status
        private static readonly ArkName[] _numberOfLevelUpPointsAppliedTamed = new[]
        {
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 0), //health
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 1), //stamina
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 2), //torpor
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 3), //oxygen
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 4), //food
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 5), //water
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 6), //temperature
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 7), //weight
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 8), //melee damage
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 9), //movement speed
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 10), //fortitude
            ArkName.Create("NumberOfLevelUpPointsAppliedTamed", 11) //crafting speed
        };
        private static readonly ArkName _dinoImprintingQuality = ArkName.Create("DinoImprintingQuality");
        private static readonly ArkName _experiencePoints = ArkName.Create("ExperiencePoints");
        private static readonly ArkName _extraCharacterLevel = ArkName.Create("ExtraCharacterLevel");
        private static readonly ArkName _tamedIneffectivenessModifier = ArkName.Create("TamedIneffectivenessModifier");
        private static readonly ArkName[] _currentStatusValues = new[]
        {
            ArkName.Create("CurrentStatusValues", 0), //health
            ArkName.Create("CurrentStatusValues", 1), //stamina
            ArkName.Create("CurrentStatusValues", 2), //torpor
            ArkName.Create("CurrentStatusValues", 3), //oxygen
            ArkName.Create("CurrentStatusValues", 4), //food
            ArkName.Create("CurrentStatusValues", 5), //water
            ArkName.Create("CurrentStatusValues", 6), //temperature
            ArkName.Create("CurrentStatusValues", 7), //weight
            ArkName.Create("CurrentStatusValues", 8), //melee damage
            ArkName.Create("CurrentStatusValues", 9), //movement speed
            ArkName.Create("CurrentStatusValues", 10), //fortitude
            ArkName.Create("CurrentStatusValues", 11) //crafting speed
        };
        private static readonly ArkName _myInventoryComponent = ArkName.Create("MyInventoryComponent");

        private ISaveState _saveState;

        internal new void Decouple()
        {
            base.Decouple();
        }

        public ArkTamedCreature() : base()
        {
            Construct();
        }

        public ArkTamedCreature(IGameObject creature, IGameObject status, ISaveState saveState) : base(creature, status, saveState)
        {
            Construct();

            _saveState = saveState;

            OwningPlayerId = creature.GetPropertyValue<int?>(_owningPlayerID);
            OwningPlayerName = creature.GetPropertyValue<string>(_owningPlayerName);
            Name = creature.GetPropertyValue<string>(_tamedName);
            TamedOnServerName = creature.GetPropertyValue<string>(_tamedOnServerName);
            TamerName = creature.GetPropertyValue<string>(_tamerString);
            TargetingTeam = creature.GetPropertyValue<int>(_targetingTeam);
            TribeName = creature.GetPropertyValue<string>(_tribeName);
            RandomMutationsMale = creature.GetPropertyValue<int?>(_randomMutationsMale) ?? 0;
            RandomMutationsFemale = creature.GetPropertyValue<int?>(_randomMutationsFemale) ?? 0;
            TamedAtTime = creature.GetPropertyValue<double?>(_tamedAtTime);
            LastUpdatedGestationAtTime = creature.GetPropertyValue<double?>(_lastUpdatedGestationAtTime);
            LastUpdatedMatingAtTime = creature.GetPropertyValue<double?>(_lastUpdatedMatingAtTime);

            BabyGestationProgress = creature.GetPropertyValue<float?>(_babyGestationProgress);
            BabyNextCuddleTime = creature.GetPropertyValue<double?>(_babyNextCuddleTime);
            IsNeutered = creature.GetPropertyValue<bool?>(_bNeutered) ?? false;
            DinoAncestors = ArkTamedCreatureAncestor.FromPropertyValue(creature.GetPropertyValue<ArkArrayStruct>(_dinoAncestors));
            DinoAncestorsMale = ArkTamedCreatureAncestor.FromPropertyValue(creature.GetPropertyValue<ArkArrayStruct>(_dinoAncestorsMale));

            {
                var colors = new sbyte[_gestationEggColorSetIndices.Length];
                var found = 0;
                for (var i = 0; i < colors.Length; i++)
                {
                    var color = creature.GetPropertyValue<sbyte?>(_gestationEggColorSetIndices[i]);
                    if (color == null) continue;

                    found++;
                    colors[i] = color.Value;
                }
                if (found > 0) GestationEggColors = colors;
            }
            {
                var basestats = new sbyte[_gestationEggNumberOfLevelUpPointsApplied.Length];
                var found = 0;
                for (var i = 0; i < basestats.Length; i++)
                {
                    var basestat = creature.GetPropertyValue<sbyte?>(_gestationEggNumberOfLevelUpPointsApplied[i]);
                    if (basestat == null) continue;

                    found++;
                    basestats[i] = basestat.Value;
                }
                if (found > 0) GestationEggBaseStats = basestats;
            }
            GestationEggRandomMutationsMale = creature.GetPropertyValue<int?>(_gestationEggRandomMutationsMale);
            GestationEggRandomMutationsFemale = creature.GetPropertyValue<int?>(_gestationEggRandomMutationsFemale);
            ImprinterName = creature.GetPropertyValue<string>(_imprinterName);
            ImprinterPlayerDataId = creature.GetPropertyValue<long?>(_imprinterPlayerDataID);
            NextAllowedMatingTime = creature.GetPropertyValue<double?>(_nextAllowedMatingTime);
            NextBabyDinoAncestors = ArkTamedCreatureAncestor.FromPropertyValue(creature.GetPropertyValue<ArkArrayStruct>(_nextBabyDinoAncestors));
            NextBabyDinoAncestorsMale = ArkTamedCreatureAncestor.FromPropertyValue(creature.GetPropertyValue<ArkArrayStruct>(_nextBabyDinoAncestorsMale));
            InventoryId = creature.GetPropertyValue<ObjectReference>(_myInventoryComponent)?.ObjectId;

            TamedStats = new sbyte[_numberOfLevelUpPointsAppliedTamed.Length];
            if (status != null)
            {
                for (var i = 0; i < TamedStats.Length; i++) TamedStats[i] = status.GetPropertyValue<sbyte?>(_numberOfLevelUpPointsAppliedTamed[i]) ?? 0;
                DinoImprintingQuality = status.GetPropertyValue<float?>(_dinoImprintingQuality);
                ExperiencePoints = status.GetPropertyValue<float?>(_experiencePoints);
                ExtraCharacterLevel = status.GetPropertyValue<short?>(_extraCharacterLevel) ?? 0;
                TamedIneffectivenessModifier = status.GetPropertyValue<float?>(_tamedIneffectivenessModifier);
                CurrentStatusValues = new float?[_currentStatusValues.Length];
                for (var i = 0; i < CurrentStatusValues.Length; i++) CurrentStatusValues[i] = status.GetPropertyValue<float?>(_currentStatusValues[i]);
            }
        }

        private void Construct()
        {
            TamedStats = new sbyte[_numberOfLevelUpPointsAppliedTamed.Length];
            CurrentStatusValues = new float?[_currentStatusValues.Length];

            // Relations
            _inventory = new Lazy<ArkItem[]>(() => {
                if (!InventoryId.HasValue) return new ArkItem[] { };

                ArkItem[] items = null;
                return _gameData._inventoryItems.TryGetValue(InventoryId.Value, out items) ? items.Where(ArkItem.Filter_RealItems).ToArray() : new ArkItem[] { };
            });
        }

        public int? OwningPlayerId { get; set; }
        public string OwningPlayerName { get; set; }
        public string Name { get; set; }
        public string TamedOnServerName { get; set; }
        public string TamerName { get; set; }
        /// <summary>
        /// TribeId or PlayerId to which the tamed creature belongs.
        /// </summary>
        public int TargetingTeam { get; set; }
        public string TribeName { get; set; }
        public double? TamedAtTime { get; set; }
        public TimeSpan? TamedForApprox => _saveState?.GetApproxTimeElapsedSince(TamedAtTime);
        public double? LastUpdatedGestationAtTime { get; set; }
        public TimeSpan? LastUpdatedGestationTimeAgoApprox => _saveState?.GetApproxTimeElapsedSince(LastUpdatedGestationAtTime);
        public double? LastUpdatedMatingAtTime { get; set; }
        public TimeSpan? LastUpdatedMatingTimeAgoApprox => _saveState?.GetApproxTimeElapsedSince(LastUpdatedMatingAtTime);
        public int RandomMutationsFemale { get; set; }
        public int RandomMutationsMale { get; set; }
        public int RandomMutationTotal => RandomMutationsMale + RandomMutationsFemale;
        public float? BabyGestationProgress { get; set; }
        public double? BabyNextCuddleTime { get; set; }
        public DateTime? BabyNextCuddleTimeApprox => _saveState?.GetApproxDateTimeOf(BabyNextCuddleTime);
        public bool IsNeutered { get; set; }
        public ArkTamedCreatureAncestor[] DinoAncestors { get; set; }
        public ArkTamedCreatureAncestor[] DinoAncestorsMale { get; set; }
        public sbyte[] GestationEggColors { get; set; }
        public sbyte[] GestationEggBaseStats { get; set; }
        public int? GestationEggRandomMutationsFemale { get; set; }
        public int? GestationEggRandomMutationsMale { get; set; }
        public int? GestationEggRandomMutationTotal => 
            GestationEggRandomMutationsMale.HasValue || GestationEggRandomMutationsFemale.HasValue ? 
            ((GestationEggRandomMutationsMale ?? 0) + (GestationEggRandomMutationsFemale ?? 0)) 
            : (int?)null;
        public string ImprinterName { get; set; }
        public long? ImprinterPlayerDataId { get; set; }
        public double? NextAllowedMatingTime { get; set; }
        public DateTime? NextAllowedMatingTimeApprox => _saveState?.GetApproxDateTimeOf(NextAllowedMatingTime);
        public ArkTamedCreatureAncestor[] NextBabyDinoAncestors { get; set; }
        public ArkTamedCreatureAncestor[] NextBabyDinoAncestorsMale { get; set; }
        public sbyte[] TamedStats { get; set; }
        public float? DinoImprintingQuality { get; set; }
        public float? ExperiencePoints { get; set; }
        public short ExtraCharacterLevel { get; set; }
        public int Level => BaseLevel + ExtraCharacterLevel;
        public float? TamedIneffectivenessModifier { get; set; }
        public float?[] CurrentStatusValues { get; set; }
        public int? InventoryId { get; set; }

        // Relations
        [JsonIgnore]
        public ArkItem[] Inventory => _inventory.Value;
        private Lazy<ArkItem[]> _inventory;
    }
}
// STATUS COMPONENT PROPERTIES
//"bAllowLevelUps (Boolean) [*]",
//"BaseCharacterLevel (Int32) [*]",
//"bInitializedBaseLevelMaxStatusValues (Boolean) [*]",
//"bReplicateGlobalStatusValues (Boolean) [*]",
//"bServerFirstInitialized (Boolean)",
//"CurrentStatusStates_1 (SByte) [*]",
//"CurrentStatusStates_3 (SByte) [*]",
//"CurrentStatusStates_4 (SByte) [*]",
//"CurrentStatusStates_7 (SByte) [*]",
//"CurrentStatusValues (Single)",
//"CurrentStatusValues_1 (Single) [*]",
//"CurrentStatusValues_2 (Single) [*]",
//"CurrentStatusValues_3 (Single) [*]",
//"CurrentStatusValues_4 (Single) [*]",
//"CurrentStatusValues_5 (Single) [*]",
//"CurrentStatusValues_7 (Single) [*]",
//"CurrentStatusValues_8 (Single) [*]",
//"CurrentStatusValues_9 (Single) [*]",
//"DinoImprintingQuality (Single) [*]",
//"ExperiencePoints (Single) [*]",
//"ExtraCharacterLevel (Int16) [*]",
//"NumberOfLevelUpPointsApplied (ArkByteValue) [*]",
//"NumberOfLevelUpPointsApplied_1 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsApplied_3 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsApplied_4 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsApplied_7 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsApplied_8 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsApplied_9 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsAppliedTamed (ArkByteValue) [*]",
//"NumberOfLevelUpPointsAppliedTamed_1 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsAppliedTamed_3 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsAppliedTamed_4 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsAppliedTamed_7 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsAppliedTamed_8 (ArkByteValue) [*]",
//"NumberOfLevelUpPointsAppliedTamed_9 (ArkByteValue) [*]",
//"TamedIneffectivenessModifier (Single) [*]"

//DINO PROPERTIES
//"AIRangeMultiplier (Single) [*]",
//"BabyCuddleFood (ObjectReference) [*]",
//"BabyCuddleType (ArkByteValue) [*]",
//"BabyCuddleWalkStartingLocation (StructVector) [*]",
//"BabyGestationProgress (Single) [*]",
//"BabyNextCuddleTime (Double) [*]",
//"bAllowPublicSeating (Boolean) [*]",
//"bBonesHidden (Boolean) [*]",
//"bCanBeDamaged (Boolean) [*]",
//"bCheatForceTameRide (Boolean) [*]",
//"bCollectVictimItems (Boolean) [*]",
//"bDisableHarvesting (Boolean) [*]",
//"bDontWander (Boolean) [*]",
//"bEnableTamedWandering (Boolean) [*]",
//"bIgnoreAllWhistles (Boolean) [*]",
//"bIsFemale (Boolean) [*]",
//"bIsFlying (Boolean) [*]",
//"bIsNaturallySleeping (Boolean) [*]",
//"bNeutered (Boolean) [*]",
//"bSeekFishIfLowHealth (Boolean) [*]",
//"bServerInitializedDino (Boolean)",
//"bTargetingIgnoredByWildDinos (Boolean) [*]",
//"bTargetingIgnoreWildDinos (Boolean) [*]",
//"BuryAttackRange (Int32) [*]",
//"ColorSetIndices (ArkByteValue) [*]",
//"ColorSetIndices_1 (ArkByteValue) [*]",
//"ColorSetIndices_2 (ArkByteValue) [*]",
//"ColorSetIndices_3 (ArkByteValue) [*]",
//"ColorSetIndices_4 (ArkByteValue) [*]",
//"ColorSetIndices_5 (ArkByteValue) [*]",
//"currentHuntingMode (Int32) [*]",
//"CurrentTameAffinity (Single) [*]",
//"DinoAncestors (ArkArrayStruct) [*]",
//"DinoAncestorsMale (ArkArrayStruct) [*]",
//"DinoDownloadedAtTime (Double) [*]",
//"DinoID1 (Int32)",
//"DinoID2 (Int32)",
//"FollowStoppingDistance (ArkByteValue) [*]",
//"GestationEggColorSetIndices (ArkByteValue) [*]",
//"GestationEggColorSetIndices_1 (ArkByteValue) [*]",
//"GestationEggColorSetIndices_2 (ArkByteValue) [*]",
//"GestationEggColorSetIndices_3 (ArkByteValue) [*]",
//"GestationEggColorSetIndices_4 (ArkByteValue) [*]",
//"GestationEggColorSetIndices_5 (ArkByteValue) [*]",
//"GestationEggNumberOfLevelUpPointsApplied (ArkByteValue) [*]",
//"GestationEggNumberOfLevelUpPointsApplied_1 (ArkByteValue) [*]",
//"GestationEggNumberOfLevelUpPointsApplied_3 (ArkByteValue) [*]",
//"GestationEggNumberOfLevelUpPointsApplied_4 (ArkByteValue) [*]",
//"GestationEggNumberOfLevelUpPointsApplied_7 (ArkByteValue) [*]",
//"GestationEggNumberOfLevelUpPointsApplied_8 (ArkByteValue) [*]",
//"GestationEggNumberOfLevelUpPointsApplied_9 (ArkByteValue) [*]",
//"GestationEggRandomMutationsFemale (Int32) [*]",
//"GestationEggRandomMutationsMale (Int32) [*]",
//"GestationEggTamedIneffectivenessModifier (Single) [*]",
//"HardLimitWildDinoToVolume (ObjectReference) [*]",
//"HarvestResourceLevels (ArkArrayInteger) [*]",
//"ImprinterName (String) [*]",
//"ImprinterPlayerDataID (Int64) [*]",
//"Instigator (ObjectReference)",
//"isBuried (Boolean) [*]",
//"isForcedBury (Boolean) [*]",
//"LastBeaverDamSpawn (Double) [*]",
//"LastEggSpawnChanceTime (Double) [*]",
//"LastEnterStasisTime (Double)",
//"LastInAllyRangeTime (Double)",
//"LastTimeSheared (Double) [*]",
//"LastUnstasisStructureTime (Double) [*]",
//"LastUpdatedBabyAgeAtTime (Double) [*]",
//"LastUpdatedGestationAtTime (Double) [*]",
//"LastUpdatedMatingAtTime (Double) [*]",
//"MoschLevelUpsAvailable (Int32) [*]",
//"MyCharacterStatusComponent (ObjectReference)",
//"MyInventoryComponent (ObjectReference) [*]",
//"NextAllowedMatingTime (Double) [*]",
//"NextBabyDinoAncestors (ArkArrayStruct) [*]",
//"NextBabyDinoAncestorsMale (ArkArrayStruct) [*]",
//"OriginalCreationTime (Double)",
//"OriginalNPCVolumeName (ArkName) [*]",
//"OwningPlayerID (Int32) [*]",
//"OwningPlayerName (String) [*]",
//"PaintingComponent (ObjectReference) [*]",
//"PickPocketingMode (Int32) [*]",
//"RandomMutationsFemale (Int32) [*]",
//"RandomMutationsMale (Int32) [*]",
//"RequiredTameAffinity (Single) [*]",
//"RequiredTamingFoodIndex (Int32) [*]",
//"SaddleStructures (ArkArrayStruct) [*]",
//"SaveDestroyWildDinosUnderVersion (Int32) [*]",
//"TamedAggressionLevel (Int32) [*]",
//"TamedAITargetingRange (ArkByteValue) [*]",
//"TamedAtTime (Double) [*]",
//"TamedFollowTarget (ObjectReference) [*]",
//"TamedName (String) [*]",
//"TamedOnServerName (String) [*]",
//"TameIneffectivenessModifier (Single) [*]",
//"TamerString (String) [*]",
//"TamingLastFoodConsumptionTime (Double) [*]",
//"TamingTeamID (Int32) [*]",
//"TargetingTeam (Int32)",
//"TherizinoLevelUpsAvailable (Int32) [*]",
//"TribeGroupInventoryRank (ArkByteValue) [*]",
//"TribeGroupPetRidingRank (ArkByteValue) [*]",
//"TribeName (String) [*]",
//"UntamedPoopTimeCache (Single) [*]",
//"UploadedFromServerName (String) [*]"
