using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public interface ICloudInventoryDinoData
    {
        IGameObject Creature { get; }
        IGameObject Status { get; }
        IGameObject Inventory { get; }
    }
}
