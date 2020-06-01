using ArkSavegameToolkitNet.DataTypes;

namespace ArkSavegameToolkitNet.DataConsumers
{
    public interface IDataConsumer
    {
        void Push<TDataType>(TDataType entry) where TDataType: IDataEntry;

        void Completed();
    }
}
