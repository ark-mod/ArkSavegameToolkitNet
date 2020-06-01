using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.Utils.Extensions;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkWildCreature : ArkCreature
    {
        private static readonly ArkName _bForceDisablingTaming = ArkName.Create("bForceDisablingTaming");

        internal static readonly new ArkNameTree _dependencies = new ArkNameTree
        {
            { _bForceDisablingTaming, null }
        };

        internal new void Decouple()
        {
            base.Decouple();
        }

        public ArkWildCreature() : base() { }

        public ArkWildCreature(GameObject creature, GameObject status, ArkSaveData savedState) : base(creature, status, savedState)
        {
            IsTameable = !(creature.GetPropertyValue<bool?>(_bForceDisablingTaming) ?? false);
        }

        public bool IsTameable { get; set; }
    }
}
