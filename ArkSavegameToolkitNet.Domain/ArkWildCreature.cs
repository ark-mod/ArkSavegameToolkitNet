using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkWildCreature : ArkCreature
    {
        private static readonly ArkName _bForceDisablingTaming = ArkName.Create("bForceDisablingTaming");

        internal new void Decouple()
        {
            base.Decouple();
        }

        public ArkWildCreature() : base() { }

        public ArkWildCreature(IGameObject creature, IGameObject status, ISaveState savedState) : base(creature, status, savedState)
        {
            IsTameable = !(creature.GetPropertyValue<bool?>(_bForceDisablingTaming) ?? false);
        }

        public bool IsTameable { get; set; }
    }
}
