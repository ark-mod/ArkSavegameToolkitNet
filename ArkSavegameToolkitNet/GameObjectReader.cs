using ArkSavegameToolkitNet.Data;
using ArkSavegameToolkitNet.Exceptions;
using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Types;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public class GameObjectReader
    {
        private static ILog _logger = LogManager.GetLogger(typeof(GameObject));
        //protected internal int _propertiesOffset;

        public int Index { get; set; }
        //public Guid Uuid { get; set; }
        //public ArkName ClassName { get; set; }
        //public bool IsItem { get; set; }
        //public IList<ArkName> Names { get; set; }
        //public bool UnkBool { get; set; }
        //public int UnkIndex { get; set; }
        //public LocationData Location { get; set; }
        //public IExtraData ExtraData { get; set; }

        ////query helper fields
        //public bool IsCreature { get; set; }
        //public bool IsTamedCreature { get; set; }
        //public bool IsWildCreature { get; set; }
        //public bool IsRaftCreature { get; set; }
        //public bool IsStructure { get; set; }
        //public bool IsInventory { get; set; }
        //public bool IsTamedCreatureInventory { get; set; }
        //public bool IsWildCreatureInventory { get; set; }
        //public bool IsStructureInventory { get; set; }
        //public bool IsPlayerCharacterInventory { get; set; }
        //public bool IsStatusComponent { get; set; }
        //public bool IsDinoStatusComponent { get; set; }
        //public bool IsPlayerCharacterStatusComponent { get; set; }
        //public bool IsDroppedItem { get; set; }
        //public bool IsPlayerCharacter { get; set; }
        //public bool IsStructurePaintingComponent { get; set; }
        //public bool IsSomethingElse { get; set; }

        public ArkName ClassName => _className ?? (_className = _archive.GetName(Offset + _classNameOffset));
        private ArkName _className;

        public ArkName[] Names => _names ?? (_names = _archive.GetNames(Offset + _namesOffset));
        private ArkName[] _names;

        public int PropertiesOffset => _propertiesOffset ?? (_propertiesOffset = _archive.GetInt(Offset + _propertiesoffsetOffset)).Value;
        private int? _propertiesOffset;

        private const int _uuidOffset = 0;
        private const int _classNameOffset = 16;
        private int _isitemOffset;
        private int _namesOffset;
        private int _unkboolOffset;
        private int _unkindexOffset;
        private int _locationsOffset;
        private int _propertiesoffsetOffset;
        private int _shouldbezeroOffset;

        public long Offset { get; set; }
        public int Size { get; set; }

        private ArkNameCache _arkNameCache;
        private ArkArchive _archive;

        public GameObjectReader(ArkArchive archive, ArkNameCache arkNameCache = null)
        {
            _archive = archive;
            _arkNameCache = arkNameCache ?? new ArkNameCache();
            Offset = archive.Position;
            Size = getSize(archive);
        }

        //public IDictionary<ArkName, IProperty> Properties
        //{
        //    get
        //    {
        //        return properties;
        //    }
        //    set
        //    {
        //        if (value == null) throw new NullReferenceException("Null pointer exception from java");
        //        properties = value;
        //    }
        //}
        //protected internal IDictionary<ArkName, IProperty> properties = new Dictionary<ArkName, IProperty>();

        public int getSize(ArkArchive archive)
        {
            _isitemOffset = _classNameOffset + archive.GetNameLength(Offset + _classNameOffset);
            _namesOffset = _isitemOffset + 4;
            var names = archive.GetInt(Offset + _namesOffset);
            _unkboolOffset = _namesOffset + 4;
            for (var i = 0; i < names; i++) _unkboolOffset += archive.GetNameLength(Offset + _unkboolOffset);
            _unkindexOffset = _unkboolOffset + 4;
            _locationsOffset = _unkindexOffset + 4;
            var locations = archive.GetInt(Offset + _locationsOffset);
            if (locations > 1) _logger.Warn($"countLocationData > 1 at {Offset + _locationsOffset:X}");
            _propertiesoffsetOffset = _locationsOffset + 4 + locations * 6 * 4;
            _shouldbezeroOffset = _propertiesoffsetOffset + 4;

            return _shouldbezeroOffset + 4;

            //var size = /* uuid */ 16 + archive.GetNameLength(archive.Position + 16) + /* isitem */ 4;
            //var names = archive.GetInt(archive.Position + size);
            //size += /* names count */ 4;
            //for (var i = 0; i < names; i++) size += archive.GetNameLength(archive.Position + size);
            //size += /* unkbool */ 4 + /* unkindex */ 4;
            //var locations = archive.GetInt(archive.Position + size);
            //if (locations > 1) _logger.Warn($"countLocationData > 1 at {archive.Position + size:X}");
            //size += /* locations */ 4 + /* location */ locations * 6 * 4 + /* properties offset */ 4 + /* shouldbezero */ 4;

            //return size;
        }

        //public int getPropertiesSize(bool nameTable)
        //{
        //    int size = ArkArchive.getNameLength(qowyn.ark.properties.Property_Fields.NONE_NAME, nameTable);

        //    size += properties.Select(p => p.calculateSize(nameTable)).Sum();

        //    if (extraData != null)
        //    {
        //        size += extraData.calculateSize(nameTable);
        //    }

        //    return size;
        //}

        //public void read(ArkArchive archive)
        //{
        //    var uuidMostSig = archive.GetLong();
        //    var uuidLeastSig = archive.GetLong();
        //    var key = Tuple.Create(uuidMostSig, uuidLeastSig);

        //    Guid uuid = Guid.Empty;
        //    if (!_uuidCache.TryGetValue(key, out uuid))
        //    {
        //        var bytes = new byte[16];
        //        Array.Copy(BitConverter.GetBytes(uuidMostSig), bytes, 8);
        //        Array.Copy(BitConverter.GetBytes(uuidLeastSig), 0, bytes, 8, 8);
        //        uuid = new Guid(bytes);
        //        _uuidCache.Add(key, uuid);
        //    }
        //    Uuid = uuid;

        //    ClassName = archive.GetName();

        //    IsItem = archive.GetBoolean();

        //    var countNames = archive.GetInt();
        //    Names = new List<ArkName>();

        //    for (int nameIndex = 0; nameIndex < countNames; nameIndex++)
        //    {
        //        Names.Add(archive.GetName());
        //    }

        //    UnkBool = archive.GetBoolean();
        //    UnkIndex = archive.GetInt();

        //    var countLocationData = archive.GetInt();

        //    if (countLocationData > 1) _logger.Warn($"countLocationData > 1 at {archive.Position - 4:X}");

        //    if (countLocationData != 0)
        //    {
        //        Location = new LocationData(archive);
        //    }

        //    _propertiesOffset = archive.GetInt();

        //    var shouldBeZero = archive.GetInt();
        //    if (shouldBeZero != 0) _logger.Warn($"Expected int after propertiesOffset to be 0 but found {shouldBeZero} at {archive.Position - 4:X}");
        //}
    }
}
