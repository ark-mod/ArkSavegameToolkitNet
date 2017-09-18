using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public interface IGameObjectContainer
    {
        IList<GameObject> Objects { get; }
    }
}
