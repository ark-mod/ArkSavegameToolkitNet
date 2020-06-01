namespace ArkSavegameToolkitNet.Domain
{
    public class ArkClusterDataResult<TData> : ArkClusterDataUpdateResult
    {
        public string FilePath { get; set; }
        public TData Data { get; set; }
    }
}
