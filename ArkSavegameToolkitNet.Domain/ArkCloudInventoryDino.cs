using ArkSavegameToolkitNet.DataTypes;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkCloudInventoryDino : ArkTamedCreature
    {
        internal static readonly new ArkNameTree _dependencies = new ArkNameTree
        {
        };

        internal IPropertyContainer _dino;

        internal new void Decouple()
        {
            _dino = null;
            base.Decouple();
        }

        public ArkCloudInventoryDino() : base()
        {
        }

        public ArkCloudInventoryDino(IPropertyContainer dino, GameObject creature, GameObject status, GameObject inventory, ClusterSaveGameData saveState) : base(creature, status, null)
        {
            _dino = dino;
        }
    }
}