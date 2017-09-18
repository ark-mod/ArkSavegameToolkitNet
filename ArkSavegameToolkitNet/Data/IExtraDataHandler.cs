using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Data
{
    public interface IExtraDataHandler
    {
        bool CanHandle(GameObject obj, long length);
        IExtraData Read(GameObject obj, ArkArchive archive, long length);
    }
}
