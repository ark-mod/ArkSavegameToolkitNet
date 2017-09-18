using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Types
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ObjectReference : INameContainer
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ObjectReference));

        public const int TYPE_ID = 0;
        public const int TYPE_PATH = 1;
        public const int TYPE_PATH_NO_TYPE = 2; // Temporary, to support path references in sav files

        public ObjectReference() { }

        public ObjectReference(int length, int objectId)
        {
            Length = length;
            ObjectId = objectId;
            ObjectType = TYPE_ID;
        }

        public ObjectReference(ArkName objectString)
        {
            ObjectString = objectString;
            ObjectType = TYPE_PATH;
        }

        public ObjectReference(ArkArchive archive, int length)
        {
            Length = length;
            Read(archive);
        }
        [JsonProperty]
        public int Length { get; set; }
        [JsonProperty]
        public int ObjectId { get; set; }
        [JsonProperty]
        public ArkName ObjectString { get; set; }
        [JsonProperty]
        public int ObjectType { get; set; }


        public override string ToString()
        {
            return "ObjectReference [objectType=" + ObjectType + ", objectId=" + ObjectId + ", objectString=" + ObjectString + ", length=" + Length + "]";
        }

        public GameObject GetObject(IGameObjectContainer objectContainer)
        {
            if (ObjectType == TYPE_ID && ObjectId > -1 && ObjectId < objectContainer.Objects.Count)
                return objectContainer.Objects[ObjectId];

            return null;
        }

        public void Read(ArkArchive archive)
        {
            if (Length >= 8)
            {
                ObjectType = archive.GetInt();
                if (ObjectType == TYPE_ID) ObjectId = archive.GetInt();
                else if (ObjectType == TYPE_PATH) ObjectString = archive.GetName();
                else
                {
                    //No longer used: ObjectReference with possibly unknown type {objectType} at {archive.Position:X};
                    archive.Position -= 4;
                    ObjectType = TYPE_PATH_NO_TYPE;
                    ObjectString = archive.GetName();
                }
            }
            else if (Length == 4)
            {
                // Only seems to happen in Version 5
                ObjectType = TYPE_ID;
                ObjectId = archive.GetInt();
            }
            else
            {
                _logger.Warn($"ObjectReference with length value {Length} at {archive.Position:X}");
                archive.Position += Length;
            }
        }

        public void CollectNames(ISet<string> nameTable)
        {
            if (ObjectType == TYPE_PATH) nameTable.Add(ObjectString.Name);
        }

        //public int getSize(bool nameTable)
        //{
        //    if (objectType == TYPE_ID)
        //    {
        //        return length;
        //    }
        //    else if (objectType == TYPE_PATH)
        //    {
        //        return Integer.BYTES + ArkArchive.getNameLength(objectString, nameTable);
        //    }
        //    else if (objectType == TYPE_PATH_NO_TYPE)
        //    {
        //        return ArkArchive.getNameLength(objectString, nameTable);
        //    }
        //    else
        //    {
        //        return length;
        //    }
        //}
    }
}
