using ArkSavegameToolkitNet.DataTypes;

namespace ArkSavegameToolkitNet.DataConsumers
{
    public class NullDataConsumer : IDataConsumer
    {
        public void Push<TDataType>(TDataType entry) where TDataType : IDataEntry
        {
            // null: do nothing
        }

        public void Completed()
        {
        }
    }
}
