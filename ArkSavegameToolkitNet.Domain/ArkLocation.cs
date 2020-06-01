using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Extras;
using System.Linq;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkLocation
    {
        public ArkLocation() { }

        public ArkLocation(LocationData locationData, ArkSaveData savedState)
        {
            X = locationData.x;
            Y = locationData.y;
            Z = locationData.z;

            if (savedState?.mapName == null) return;
            MapName = savedState.mapName;

            if (ArkToolkitSettings.Instance.Maps == null) return;
            if (ArkToolkitSettings.Instance.Maps.TryGetValue(savedState.mapName, out var c) != true) return;
            Latitude = Y / c.LatitudeDivisor + c.LatitudeShift;
            Longitude = X / c.LongitudeDivisor + c.LongitudeShift;

            if (!(c.Images?.Count >= 1)) return;
            var m = c.Images.First();
            TopoMapX = (Longitude - m.LeftEdgeLongitude) * m.Width / (m.RightEdgeLongitude - m.LeftEdgeLongitude);
            TopoMapY = (Latitude - m.TopEdgeLatitude) * m.Height / (m.BottomEdgeLatitude - m.TopEdgeLatitude);
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