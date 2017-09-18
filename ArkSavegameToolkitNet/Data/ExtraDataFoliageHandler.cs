using ArkSavegameToolkitNet.Exceptions;
using ArkSavegameToolkitNet.Structs;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Data
{
    public class ExtraDataFoliageHandler : IExtraDataHandler
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ExtraDataFoliageHandler));

        public bool CanHandle(GameObject obj, long length)
        {
            return obj.ClassName.Token.Equals("InstancedFoliageActor");
        }

        public IExtraData Read(GameObject obj, ArkArchive archive, long length)
        {
            var shouldBeZero = archive.GetInt();
            if (shouldBeZero != 0) _logger.Warn($"Expected int after properties to be 0 but found {shouldBeZero} at {archive.Position - 4:X}");

            var structMapCount = archive.GetInt();

            IList<IDictionary<string, StructPropertyList>> structMapList = new List<IDictionary<string, StructPropertyList>>(structMapCount);

            try
            {
                for (int structMapIndex = 0; structMapIndex < structMapCount; structMapIndex++)
                {
                    var structCount = archive.GetInt();
                    IDictionary<string, StructPropertyList> structMap = new Dictionary<string, StructPropertyList>();

                    for (int structIndex = 0; structIndex < structCount; structIndex++)
                    {
                        var structName = archive.GetString();
                        StructPropertyList properties = new StructPropertyList(archive, null);

                        var shouldBeZero2 = archive.GetInt();
                        if (shouldBeZero2 != 0) _logger.Warn($"Expected int after properties to be 0 but found {shouldBeZero2} at {archive.Position - 4:X}");

                        structMap[structName] = properties;
                    }

                    structMapList.Add(structMap);
                }
            }
            catch (UnreadablePropertyException)
            {
                // Just stop reading and attach collected structs
            }

            ExtraDataFoliage extraDataFoliage = new ExtraDataFoliage();
            extraDataFoliage.StructMapList = structMapList;

            return extraDataFoliage;
        }
    }
}
