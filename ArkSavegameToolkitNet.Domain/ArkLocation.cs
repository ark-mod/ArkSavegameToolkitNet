using ArkSavegameToolkitNet.Arrays;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkLocation
    {
        //latitude shift, latitude divisor, longitude shift, longitude divisor
        private static Dictionary<string, Tuple<float, float, float, float>> _latlonCalcs = new Dictionary<string, Tuple<float, float, float, float>>
        {
            { "TheIsland", Tuple.Create(50.0f, 8000.0f, 50.0f, 8000.0f) },
            { "TheCenter", Tuple.Create(30.34223747253418f, 9584.0f, 55.10416793823242f, 9600.0f) },
            { "ScorchedEarth_P", Tuple.Create(50.0f, 8000.0f, 50.0f, 8000.0f) },
            { "Aberration_P", Tuple.Create(50.0f, 8000.0f, 50.0f, 8000.0f) },
            { "Valhalla", Tuple.Create(48.813560485839844f, 14750.0f, 48.813560485839844f, 14750.0f) },
            { "MortemTupiu", Tuple.Create(32.479148864746094f, 20000.0f, 40.59893798828125f, 16000.0f) },
            { "ShigoIslands", Tuple.Create(50.001777870738339260f, 9562.0f, 50.001777870738339260f, 9562.0f) },
            { "Ragnarok", Tuple.Create(50.009388f, 13100f, 50.009388f, 13100f) },
            { "TheVolcano", Tuple.Create(50.0f, 9181.0f, 50.0f, 9181.0f) },
            { "PGARK", Tuple.Create(0.0f, 6080.0f, 0.0f, 6080.0f) },
            { "CrystalIsles" , Tuple.Create(50.0f, 13718.0f, 50.0f, 13718.0f) },
            { "Valguero_P" , Tuple.Create(49.958116419834943061f, 8161.6209476309226933f, 49.958116419834943061f, 8161.6209476309226933f) }
            
        };

        //width, height, latitude-top, longitude-left, longitude-right, latitude-bottom
        // { "ShigoIslands", Tuple.Create(50.0f, 8128.0f, 50.0f, 8128.0f) },
        private static Dictionary<string, Tuple<int, int, float, float, float, float>> _topoMapCalcs;

        static ArkLocation()
        {
            System.Drawing.Bitmap island = null, center = null, scorched = null, ragnarok = null, aberration = null, crystal = null, shigo = null, volcano = null, valguero = null;
            try
            {
                island = MapResources.topo_map_TheIsland;
                center = MapResources.topo_map_TheCenter;
                scorched = MapResources.topo_map_ScorchedEarth_P;
                ragnarok = MapResources.topo_map_Ragnarok;
                aberration = MapResources.topo_map_Aberration_P;
                crystal = MapResources.topo_map_CrystalIsles;
                shigo = MapResources.topo_map_ShigoIslands;
                volcano = MapResources.topo_map_TheVolcano;
                valguero = MapResources.topo_map_Valguero_P;

                //painted-maps are divided into a 10x10 grid, lacking precise offsets and should instead align with the grid (0.0f, 0.0f, 100.0f, 100.0f)
                //topo-maps offsets are calculated using two easily identifiable points on the map and reversing the formula for TopoMapX/TopoMapY val-2.95f, 0.0f, 86.3f, 89.0f
                _topoMapCalcs = new Dictionary<string, Tuple<int, int, float, float, float, float>>
                {
                    { "TheIsland", Tuple.Create(island.Width, island.Height, 7.2f, 7.2f, 92.8f, 92.8f) },
                    { "TheCenter", Tuple.Create(center.Width, center.Height, -2.5f, 1f, 104.5f, 101f) },
                    { "ScorchedEarth_P", Tuple.Create(scorched.Width, scorched.Height, 7.2f, 7.2f, 92.8f, 92.8f) },
                    { "Aberration_P", Tuple.Create(aberration.Width, aberration.Height, 0.0f, 0.0f, 100.0f, 100.0f) },
                    { "Ragnarok", Tuple.Create(ragnarok.Width, ragnarok.Height, 0.0f, 0.0f, 100.0f, 100.0f) },
                    { "CrystalIsles", Tuple.Create(crystal.Width, crystal.Height, -1.7f, -1.5f, 99.3f, 101.0f) },
                    { "ShigoIslands", Tuple.Create(shigo.Width, shigo.Height, -2.0f, -1.6f, 99.8f, 101.0f) },
                    { "TheVolcano", Tuple.Create(volcano.Width, volcano.Height, -1.95f, -1.3f, 99.5f, 100.7f) },
                    { "Valguero_P", Tuple.Create(valguero.Width, valguero.Height, -10.0f, -10.0f, 110.0f, 110.0f) }
                };
            }
            finally
            {
                island?.Dispose();
                center?.Dispose();
                scorched?.Dispose();
                ragnarok?.Dispose();
                aberration?.Dispose();
                crystal?.Dispose();
                shigo?.Dispose();
                volcano?.Dispose();
                valguero?.Dispose();
            }
        }

        public ArkLocation() { }

        public ArkLocation(LocationData locationData, ISaveState savedState)
        {
            X = locationData.X;
            Y = locationData.Y;
            Z = locationData.Z;

            if (savedState?.MapName != null)
            {
                MapName = savedState.MapName;

                Tuple<float, float, float, float> vals = null;
                if (_latlonCalcs.TryGetValue(savedState.MapName, out vals))
                {
                    Latitude = vals.Item1 + Y / vals.Item2;
                    Longitude = vals.Item3 + X / vals.Item4;

                    Tuple<int, int, float, float, float, float> mapvals = null;
                    if (_topoMapCalcs.TryGetValue(savedState.MapName, out mapvals))
                    {
                        TopoMapX = (Longitude - mapvals.Item4) * mapvals.Item1 / (mapvals.Item5 - mapvals.Item4);
                        TopoMapY = (Latitude - mapvals.Item3) * mapvals.Item2 / (mapvals.Item6 - mapvals.Item3);
                    }
                }
            }
        }

        public string MapName { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public float? TopoMapX { get; set; }
        public float? TopoMapY { get; set; }
    }
}
