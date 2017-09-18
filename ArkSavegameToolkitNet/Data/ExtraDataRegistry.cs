using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Data
{
    public class ExtraDataRegistry
    {

        /// <summary>
        /// Contains ExtraDataHandler in reverse Order
        /// </summary>
        public static readonly IList<IExtraDataHandler> EXTRA_DATA_HANDLERS = new List<IExtraDataHandler>();

        static ExtraDataRegistry()
        {
            EXTRA_DATA_HANDLERS.Add(new ExtraDataFallbackHandler());
            EXTRA_DATA_HANDLERS.Add(new ExtraDataZeroHandler());
            EXTRA_DATA_HANDLERS.Add(new ExtraDataCharacterHandler());
            EXTRA_DATA_HANDLERS.Add(new ExtraDataFoliageHandler());
        }

        /// <summary>
        /// Searches <seealso cref="#EXTRA_DATA_HANDLERS"/> in reverse Order and terminates on the first
        /// <seealso cref="ExtraDataHandler"/> which can handle given <seealso cref="GameObject"/> {@code object}
        /// </summary>
        /// <param name="obj"> The GameObject </param>
        /// <param name="archive"> The source archive of object </param>
        /// <param name="length"> Amount of bytes of extra data
        /// @return </param>
        public static IExtraData getExtraData(GameObject obj, ArkArchive archive, long length)
        {
            for (int i = EXTRA_DATA_HANDLERS.Count - 1; i >= 0; i--)
            {
                IExtraDataHandler handler = EXTRA_DATA_HANDLERS[i];
                if (handler.CanHandle(obj, length))
                {
                    return handler.Read(obj, archive, length);
                }
            }

            return null;
        }
    }
}
