using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkItemTypeGroup
    {
        public ArkItemTypeGroup(ArkItem[] all)
        {
            All = all ?? new ArkItem[] { };
            _items = new Lazy<ArkItem[]>(() => All.Where(x => !x.IsBlueprint).ToArray());
            _blueprints = new Lazy<ArkItem[]>(() => All.Where(x => x.IsBlueprint).ToArray());
            _countItems = new Lazy<int>(() => Items.Any() ? (int)Items.Sum(x => x.Quantity) : 0);
            _countBlueprints = new Lazy<int>(() => Blueprints.Any() ? (int)Blueprints.Sum(x => x.Quantity) : 0);
            _count = new Lazy<int>(() => CountItems + CountBlueprints);
        }

        public ArkItem[] All { get; private set; }

        public ArkItem[] Items => _items.Value;
        private Lazy<ArkItem[]> _items;
        public ArkItem[] Blueprints => _blueprints.Value;
        private Lazy<ArkItem[]> _blueprints;
        public int Count => _count.Value;
        private Lazy<int> _count;
        public int CountItems => _countItems.Value;
        private Lazy<int> _countItems;
        public int CountBlueprints => _countBlueprints.Value;
        private Lazy<int> _countBlueprints;
    }
}
