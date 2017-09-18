using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Data
{
    public class ExtraDataCharacterHandler : IExtraDataHandler
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ExtraDataCharacterHandler));
        private static readonly ExtraDataCharacter INSTANCE = new ExtraDataCharacter();

        public bool CanHandle(GameObject obj, long length)
        {
            return obj.ClassName.Token.Contains("_Character_");
        }

        public IExtraData Read(GameObject obj, ArkArchive archive, long length)
        {
            var shouldBeZero = archive.GetInt();
            if (shouldBeZero != 0)
            {
                _logger.Warn($"Expected int after properties to be 0 but found {shouldBeZero} at {archive.Position - 4}");
            }

            var shouldBeOne = archive.GetInt();
            if (shouldBeOne != 1)
            {
                _logger.Warn($"Expected int after properties to be 1 but found {shouldBeOne} at {archive.Position - 4}");
            }

            return INSTANCE;
        }
    }
}
