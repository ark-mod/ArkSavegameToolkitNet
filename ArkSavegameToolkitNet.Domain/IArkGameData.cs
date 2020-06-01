namespace ArkSavegameToolkitNet.Domain
{
    public interface IArkGameData
    {
        ArkItem[] Items { get;}
        ArkPlayer[] Players { get; }
        SaveState SaveState { get; }
        ArkStructure[] Structures { get; }
        ArkTamedCreature[] TamedCreatures { get; }
        ArkTribe[] Tribes { get; }
        ArkWildCreature[] WildCreatures { get; }
    }
}