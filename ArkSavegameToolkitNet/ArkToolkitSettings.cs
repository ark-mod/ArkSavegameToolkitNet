using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ArkSavegameToolkitNet
{
    public enum ObjectType
    {
        ItemElectricGeneratorGasoline = 0_000_002,
        StructureCropPlot = 1_000_001,
        StructureElectricGenerator = 1_000_002
    }

    public class ArkToolkitSettings
    {
        public static ArkToolkitSettings Instance => _instance ?? (_instance = new ArkToolkitSettings());
        private static ArkToolkitSettings _instance;

        public ArkToolkitSettings()
        {
        }

        [JsonConstructor]
        public ArkToolkitSettings(Dictionary<string, MapDefinition> maps, Dictionary<ObjectType, List<string>> objectTypes)
        {
            Maps = maps ?? Maps;
            ObjectTypes = objectTypes ?? ObjectTypes;
        }

        /// <summary>
        /// Import settings from another instance
        /// </summary>
        /// <param name="settings"></param>
        public void Setup(ArkToolkitSettings settings)
        {
            Maps = settings.Maps;
            ObjectTypes = settings.ObjectTypes;
        }

        // defaults

        // settings
        public Dictionary<string, MapDefinition> Maps { get; private set; } = new Dictionary<string, MapDefinition>();
        public Dictionary<ObjectType, List<string>> ObjectTypes { get; private set; } = new Dictionary<ObjectType, List<string>>();
    }

    /// <summary>
    /// Used to translate unreal engine world coordinates (x, y) into gps coordinates (latitude and longitude) and gps coordinates into pixel offsets on individual map images.
    /// </summary>
    /// <example>
    /// X = (longitude - longitude shift) * longitude divisor
    /// Y = (latitude - latitude shift) * latitude divisor
    /// Longitude = x / longitude divisor + longitude shift
    /// Latitude = y / latitude divisor + latitude shift
    /// </example>
    /// <remarks>
    /// Available in the devkit, from map authors, codebehind for wiki resource maps
    /// </remarks>
    public class MapDefinition
    {
        [JsonConstructor]
        public MapDefinition(
            float latitudeShift,
            float latitudeDivisor,
            float longitudeShift,
            float longitudeDivisor,
            IEnumerable<MapImageDefinition> images = null)
        {
            LatitudeShift = latitudeShift;
            LatitudeDivisor = latitudeDivisor;
            LongitudeShift = longitudeShift;
            LongitudeDivisor = longitudeDivisor;
            Images = images?.ToList() ?? new List<MapImageDefinition>();
        }

        /// <summary>
        /// Latitude shift
        /// </summary>
        public float LatitudeShift { get; set; }
        /// <summary>
        /// Latitude divisor
        /// </summary>
        public float LatitudeDivisor { get; set; }
        /// <summary>
        /// Longitude shift
        /// </summary>
        public float LongitudeShift { get; set; }
        /// <summary>
        /// Longitude divisor
        /// </summary>
        public float LongitudeDivisor { get; set; }
        /// <summary>
        /// Map images
        /// </summary>
        public List<MapImageDefinition> Images { get; set; }
    }

    /// <summary>
    ///  Used to translate gps coordinates (latitude and longitude) into pixel offsets on individual map images.
    /// </summary>
    /// <remarks>
    /// topo maps are an accurate representation of the in-game world and thus the coordinates for individual
    /// edges can be calculated using two or more known and easily identifiable points
    ///
    /// hand-painted maps are not an accurate representation of the in-game world but contain a coordinate-grid
    /// that should be used to calculate the coordinates for individual edges so that points align with the grid
    ///
    /// step 1. Find the in-game gps coordinates for two points that are: easily identifiable, small and as far
    ///         away from eachother as possible (on both x-/y-axis)
    ///         point1, point2 = [x1, y1], [x2, y2]
    ///
    /// step 2. Find the pixel offsets for both points on the map bitmap
    ///         point1, point2 = [px1, py1], [px2, py2]
    ///
    /// step 3. Calculate the delta distance between both points
    ///         dx = abs(x1 - x2)                        -> example: dx = abs(13.683 - 83.02) -> dx = 69.337
    ///         dy = abs(y1 - y2)                        -> example: dy = abs(60.261 - 15.132) -> dy = 45.129
    ///         dpx = abs(px1 - px2)                     -> example: dpx = abs(84 - 906) -> dpx = 822
    ///         dpy = abs(py1 - py2)                     -> example: dpy = abs(624 - 97) -> dpy = 527
    ///
    /// step 4. Calculate pixels per degree latitude and longitude
    ///         pfx = dpx / dx                           -> example: pfx = 822 / 69.337 -> pfx = 11.85514227612962776007
    ///         pfy = dpy / dy                           -> example: pfy = 527 / 45.129 -> pfy = 11.67763522347049569013
    ///
    /// step 5. Calculate the gps coordinate for each of the four edges of the map bitmap
    ///         xMin, yMin, pxMin, pyMin = min(x1, x2), min(y1, y2), min(px1, px2), min(py1, py2)
    ///         xMax, yMax, pxMax, pyMax = max(x1, x2), max(y1, y2), max(px1, px2), max(py1, py2)
    ///
    ///         top = yMin - pyMin / pfy                   -> example: top = 15.132 - 97 / 11.67763522347049569013 -> top = 6.82552371916508538899
    ///         left = xMin - pxMin / pfx                  -> example: left = 13.683 - 84 / 11.85514227612962776007 -> left = 6.59746715328467153285
    ///         right = xMax + (width - pxMax) / pfx         -> example: right = 83.02 + (1024 - 906) / 11.85514227612962776007 -> right = 92.97348661800486618005
    ///         bottom = yMax + (height - pyMax) / pfy       -> example: bottom = 60.261 + (1024 - 624) / 11.67763522347049569013 -> bottom = 94.51451043643263757117
    /// </remarks>
    public class MapImageDefinition
    {
        [JsonConstructor]
        public MapImageDefinition(
            string filePath,
            float topEdgeLatitude,
            float leftEdgeLongitude,
            float rightEdgeLongitude,
            float bottomEdgeLatitude)
            : this(() => (Bitmap)Bitmap.FromFile(filePath), topEdgeLatitude, leftEdgeLongitude, rightEdgeLongitude, bottomEdgeLatitude)
        {
            FilePath = filePath;
        }

        public MapImageDefinition(
            Func<Bitmap> imageProvider,
            float topEdgeLatitude,
            float leftEdgeLongitude,
            float rightEdgeLongitude,
            float bottomEdgeLatitude)
        {
            ImageProvider = imageProvider ?? throw new ArgumentNullException($"MapImageDefinition argument '{nameof(imageProvider)}' is null.");

            using (var image = imageProvider())
            {
                Width = image.Width;
                Height = image.Height;
            }

            TopEdgeLatitude = topEdgeLatitude;
            LeftEdgeLongitude = leftEdgeLongitude;
            RightEdgeLongitude = rightEdgeLongitude;
            BottomEdgeLatitude = bottomEdgeLatitude;
        }

        public string FilePath { get; private set; }

        /// <summary>
        /// Provider for map image
        /// </summary>
        /// <example>
        /// () => (Bitmap)Bitmap.FromFile(@"<path>")
        /// </example>
        [JsonIgnore]
        public Func<Bitmap> ImageProvider { get; private set; }

        /// <summary>
        /// Map image width
        /// </summary>
        [JsonIgnore]
        public int Width { get; private set; }

        /// <summary>
        /// Map image height
        /// </summary>
        [JsonIgnore]
        public int Height { get; private set; }

        /// <summary>
        /// Latitude coordinate on the top edge of the map image
        /// </summary>
        /// <remarks>
        /// This is the latitude you see in game if you stand in a location anywhere along the top edge of what you see in the map image.
        /// If the map image is a perfect representation of the game world this value would be 0.0.
        /// </remarks>
        /// <example>0.0</example>
        public float TopEdgeLatitude { get; private set; }

        /// <summary>
        /// Longitude coordinate on the left edge of the map image
        /// </summary>
        /// <remarks>
        /// This is the longitude you see in game if you stand in a location anywhere along the left edge of what you see in the map image.
        /// If the map image is a perfect representation of the game world this value would be 0.0.
        /// </remarks>
        /// <example>0.0</example>
        public float LeftEdgeLongitude { get; private set; }

        /// <summary>
        /// Longitude coordinate on the right edge of the map image
        /// </summary>
        /// <remarks>
        /// This is the longitude you see in game if you stand in a location anywhere along the right edge of what you see in the map image.
        /// If the map image is a perfect representation of the game world this value would be 100.0.
        /// </remarks>
        /// <example>100.0</example>
        public float RightEdgeLongitude { get; private set; }

        /// <summary>
        /// Latitude coordinate on the bottom edge of the map image
        /// </summary>
        /// <remarks>
        /// This is the latitude you see in game if you stand in a location anywhere along the bottom edge of what you see in the map image.
        /// If the map image is a perfect representation of the game world this value would be 100.0.
        /// </remarks>
        /// <example>100.0</example>
        public float BottomEdgeLatitude { get; private set; }
    }
}
