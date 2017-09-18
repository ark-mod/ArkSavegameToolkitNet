namespace ArkSavegameToolkitNet.Domain
{
    public interface IArkClusterData
    {
        ArkCloudInventory[] Inventories { get; }
    }
}