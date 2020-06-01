using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ArkSavegameToolkitNet.Domain
{
    public static class ArkToolkitDomain
    {
        internal static readonly Dictionary<string, MapDefinition> DefaultMaps = new Dictionary<string, MapDefinition>
        {
            {"TheIsland", new MapDefinition(50.0f, 8000.0f, 50.0f, 8000.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_TheIsland, 7.2f, 7.2f, 92.8f, 92.8f)
                })},
            {"TheCenter", new MapDefinition(30.34223747253418f, 9584.0f, 55.10416793823242f, 9600.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_TheCenter, -2.5f, 1f, 104.5f, 101f)
                })},
             {"ScorchedEarth_P", new MapDefinition(50.0f, 8000.0f, 50.0f, 8000.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_ScorchedEarth_P, 7.2f, 7.2f, 92.8f, 92.8f)
                })},
             {"Aberration_P", new MapDefinition(50.0f, 8000.0f, 50.0f, 8000.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_Aberration_P, 0.0f, 0.0f, 100.0f, 100.0f)
                })},
             {"Extinction", new MapDefinition(50.0f, 8000.0f, 50.0f, 8000.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_Extinction, 0.0f, 0.0f, 100.0f, 100.0f)
                })},
             {"ShigoIslands", new MapDefinition(50.001777870738339260f, 9562.0f, 50.001777870738339260f, 9562.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_ShigoIslands, -2.0f, -1.6f, 99.8f, 101.0f)
                })},
             {"Ragnarok", new MapDefinition(50.009388f, 13100f, 50.009388f, 13100f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_Ragnarok, 0.0f, 0.0f, 100.0f, 100.0f)
                })},
             {"TheVolcano", new MapDefinition(50.0f, 9181.0f, 50.0f, 9181.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_TheVolcano, -1.95f, -1.3f, 99.5f, 100.7f)
                })},
             {"CrystalIsles", new MapDefinition(50.0f, 13718.0f, 50.0f, 13718.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_CrystalIsles, -1.7f, -1.5f, 99.3f, 101.0f)
                })},
             {"Valguero_P", new MapDefinition(50.0f, 8161.0f, 50.0f, 8161.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_Valguero_P, -10.0f, -10.0f, 110.0f, 110.0f)
                })},
             {"Genesis", new MapDefinition(50.0f, 10500.0f, 50.0f, 10500.0f, new []
                {
                    new MapImageDefinition(() => MapResources.topo_map_Genesis, 4.19f, 0.51f, 96.69f, 97.47f)
                })},
             {"Valhalla", new MapDefinition(48.813560485839844f, 14750.0f, 48.813560485839844f, 14750.0f)},
             {"MortemTupiu", new MapDefinition(32.479148864746094f, 20000.0f, 40.59893798828125f, 16000.0f)},
             {"PGARK", new MapDefinition(0.0f, 6080.0f, 0.0f, 6080.0f)}
        };

        internal static readonly Dictionary<ObjectType, List<string>> DefaultObjectTypes = new Dictionary<ObjectType, List<string>>
        {
            { ObjectType.StructureCropPlot, new List<string>(new []
                {
                    "CropPlotSmall_SM_C",
                    "CropPlotMedium_SM_C",
                    "CropPlotLarge_SM_C",
                    "BP_CropPlot_Small_C",
                    "BP_CropPlot_Medium_C",
                    "BP_CropPlot_Large_C"
                })
            },
            { ObjectType.StructureElectricGenerator, new List<string>(new []
                {
                    "ElectricGenerator_C",
                    "BP_Generator_C"
                })
            },
            { ObjectType.ItemElectricGeneratorGasoline, new List<string>(new []
                {
                    "PrimalItemResource_Gasoline_C"
                })
            },
        };

        /// <summary>
        /// Initializes all settings with default values
        /// </summary>
        public static void Initialize()
        {
            foreach (var map in DefaultMaps) ArkToolkitSettings.Instance.Maps.TryAdd(map.Key, map.Value);
            foreach (var objectType in DefaultObjectTypes) ArkToolkitSettings.Instance.ObjectTypes.TryAdd(objectType.Key, objectType.Value);
        }
    }
}
