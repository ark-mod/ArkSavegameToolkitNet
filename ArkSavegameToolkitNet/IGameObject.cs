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
        bool IsCreature { get; set; }
        bool IsTamedCreature { get; set; }
        bool IsWildCreature { get; set; }
        bool IsRaftCreature { get; set; }
        bool IsStructure { get; set; }
        bool IsInventory { get; set; }
        bool IsTamedCreatureInventory { get; set; }
        bool IsWildCreatureInventory { get; set; }
        bool IsStructureInventory { get; set; }
        bool IsPlayerCharacterInventory { get; set; }
        bool IsStatusComponent { get; set; }
        bool IsDinoStatusComponent { get; set; }
        bool IsPlayerCharacterStatusComponent { get; set; }
        bool IsDroppedItem { get; set; }
        bool IsPlayerCharacter { get; set; }
        bool IsStructurePaintingComponent { get; set; }
        bool IsSomethingElse { get; set; }
    }
}