using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Data
{
    public class ExtraDataZeroHandler : IExtraDataHandler
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ExtraDataZeroHandler));
        private static readonly ExtraDataZero INSTANCE = new ExtraDataZero();

        public bool CanHandle(GameObject obj, long length)
        {
            return length == 4;
        }

        public IExtraData Read(GameObject obj, ArkArchive archive, long length)
        {
            var shouldBeZero = archive.GetInt();
            if (shouldBeZero != 0) _logger.Warn($"Expected int after properties to be 0 but found {shouldBeZero} at {archive.Position - 4:X}");

            return INSTANCE;
        }
    }
}
