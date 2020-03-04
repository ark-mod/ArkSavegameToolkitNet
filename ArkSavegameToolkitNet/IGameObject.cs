using System;
using System.Collections.Generic;
using ArkSavegameToolkitNet.Types;

namespace ArkSavegameToolkitNet
{
    public interface IGameObject : IPropertyContainer
    {
        /// <summary>
        /// Is the index of this object in the container
        /// </summary>
        int ObjectId { get; set; }
        ArkName ClassName { get; set; }
        bool IsItem { get; set; }
        LocationData Location { get; set; }
        IList<ArkName> Names { get; set; }
        Guid Uuid { get; set; }

        //query helper fields
        bool IsCryo { get; set; }
        bool IsCreature { get; }
        bool IsTamedCreature { get; }
        bool IsWildCreature { get; }
        bool IsRaftCreature { get; }
        bool IsStructure { get; }
        bool IsInventory { get; }
        bool IsTamedCreatureInventory { get; }
        bool IsWildCreatureInventory { get; }
        bool IsStructureInventory { get; }
        bool IsPlayerCharacterInventory { get; }
        bool IsStatusComponent { get; }
        bool IsDinoStatusComponent { get; }
        bool IsPlayerCharacterStatusComponent { get; }
        bool IsDroppedItem { get; }
        bool IsPlayerCharacter { get; }
        bool IsStructurePaintingComponent { get; }
        bool IsSomethingElse { get; }

        //bool IsCreature { get; set; }
        //bool IsTamedCreature { get; set; }
        //bool IsWildCreature { get; set; }
        //bool IsRaftCreature { get; set; }
        //bool IsStructure { get; set; }
        //bool IsInventory { get; set; }
        //bool IsTamedCreatureInventory { get; set; }
        //bool IsWildCreatureInventory { get; set; }
        //bool IsStructureInventory { get; set; }
        //bool IsPlayerCharacterInventory { get; set; }
        //bool IsStatusComponent { get; set; }
        //bool IsDinoStatusComponent { get; set; }
        //bool IsPlayerCharacterStatusComponent { get; set; }
        //bool IsDroppedItem { get; set; }
        //bool IsPlayerCharacter { get; set; }
        //bool IsStructurePaintingComponent { get; set; }
        //bool IsSomethingElse { get; set; }
    }
}