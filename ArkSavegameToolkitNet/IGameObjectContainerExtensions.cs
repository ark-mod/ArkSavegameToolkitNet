using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet
{
    public static class IGameObjectContainerExtensions
    {
        public static GameObject GetObject(this IGameObjectContainer self, ObjectReference reference)
        {
            if (reference == null || reference.ObjectType != ObjectReference.TYPE_ID) return null;

            if (reference.ObjectId > -1 && reference.ObjectId < self.Objects.Count)
                return self.Objects.ElementAt(reference.ObjectId);
            else return null;
        }
    }
}
