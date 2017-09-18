using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Data
{
    public class ExtraDataFallbackHandler : IExtraDataHandler
    {
        public bool CanHandle(GameObject obj, long length)
        {
            return true;
        }

        public IExtraData Read(GameObject obj, ArkArchive archive, long length)
        {
            if (length > int.MaxValue) throw new ApplicationException("Attempted to read more than int.maxvalue bytes.");

            ExtraDataBlob extraData = new ExtraDataBlob();

            extraData.Data = archive.GetBytes((int)length);

            return extraData;
        }
    }
}
