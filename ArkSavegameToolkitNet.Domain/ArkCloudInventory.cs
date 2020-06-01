using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Utils.Extensions;
using System;
using System.Linq;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkCloudInventory : ArkClusterDataContainerBase
    {
        private static readonly ArkName _myArkData = ArkName.Create("MyArkData");
        private static readonly ArkName _arkItems = ArkName.Create("ArkItems");
        private static readonly ArkName _arkPlayerData = ArkName.Create("ArkPlayerData");
        private static readonly ArkName _arkTamedDinosData = ArkName.Create("ArkTamedDinosData");
        private static readonly ArkName _dinoData = ArkName.Create("DinoData");
        private static readonly ArkName _version = ArkName.Create("Version");

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            {
                _myArkData,
                new ArkNameTree
                {
                    { _arkItems, null },
                    { _arkPlayerData, null },
                    {
                        _arkTamedDinosData,
                        new ArkNameTree
                        {
                            { _version, null },
                            { _dinoData, null }
                        }
                    }
                }
            }
        };

        internal GameObject _cloudinv;

        internal void Decouple()
        {
            _cloudinv = null;
            foreach (var item in Items) item.Decouple();
            foreach (var character in Characters) character.Decouple();
            foreach (var dino in Dinos) dino.Decouple();
        }

        public ArkCloudInventory()
        {
            Items = new ArkCloudInventoryItem[] { };
            Characters = new ArkCloudInventoryCharacter[] { };
            Dinos = new ArkCloudInventoryDino[] { };
        }

        public ArkCloudInventory(string steamId, GameObject cloudinv, ClusterSaveGameData saveState) : this()
        {
            if (saveState == null) throw new ApplicationException("Save state must be set in ArkCloudInventory::ArkCloudInventory");

            _cloudinv = cloudinv;

            SteamId = steamId;
            var mydata = cloudinv.GetProperty<PropertyListStructProperty>(_myArkData);
            var items = mydata.GetProperty<StructArrayProperty>(_arkItems);
            var characters = mydata.GetProperty<StructArrayProperty>(_arkPlayerData);
            var dinos = mydata.GetProperty<StructArrayProperty>(_arkTamedDinosData);
            if (items?.values != null) Items = items.values.OfType<PropertyListStructProperty>().Select(x => new ArkCloudInventoryItem(x)).ToArray();
            if (characters?.values != null) Characters = characters.values.OfType<PropertyListStructProperty>().Select(x => new ArkCloudInventoryCharacter(x)).ToArray();

            //todo: add back support for loading creatures uploaded to cluster

            if (dinos != null)
            {
                Dinos = dinos.values.OfType<PropertyListStructProperty>().Select(x =>
                {
                    var version = x.GetPropertyValue<float?>(_version);
                    if (!version.HasValue) return null;

                    var data = x.GetProperty<ByteArrayProperty>(_dinoData);

                    if (data?.values == null) throw new NotSupportedException("DinoData was null or contained null-values.");

                    //todo: we don't support loading only domain properties for cluster dinos yet
                    var settings = ArkToolkit.DefaultSettings.Invoke();
                    settings.ExclusivePropertyNameTree = null;

                    var dinoData = ArkToolkitLoader.Create(settings).LoadClusterDino(data.values.ToArray(), version.Value);

                    var creature = dinoData.objects.ElementAtOrDefault(0);
                    var status = dinoData.objects.ElementAtOrDefault(1);
                    var inventory = dinoData.objects.ElementAtOrDefault(2);

                    return new ArkCloudInventoryDino(x, creature, status, inventory, saveState);
                }).ToArray();
            }

            SavedAt = saveState.savedAt;
        }

        public string SteamId { get; set; }
        public ArkCloudInventoryItem[] Items { get; set; }
        public ArkCloudInventoryCharacter[] Characters { get; set; }
        public ArkCloudInventoryDino[] Dinos { get; set; }
        public DateTime SavedAt { get; set; }
    }
}