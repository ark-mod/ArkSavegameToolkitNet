using ArkSavegameToolkitNet.Arrays;
using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkCloudInventoryDino : ArkTamedCreature
    {
        //private static readonly ArkName _dinoClass = ArkName.Create("DinoClass");
        //private static readonly ArkName _dinoExperiencePoints = ArkName.Create("DinoExperiencePoints");
        //private static readonly ArkName _dinoId1 = ArkName.Create("DinoID1");
        //private static readonly ArkName _dinoId2 = ArkName.Create("DinoID2");
        //private static readonly ArkName _dinoName = ArkName.Create("DinoName");
        //private static readonly ArkName _dinoData = ArkName.Create("DinoData"); 
        //private static readonly ArkName[] _dinoStats = new[]
        //{
        //    ArkName.Create("DinoStats", 0), //health
        //    ArkName.Create("DinoStats", 1), //stamina
        //    ArkName.Create("DinoStats", 2), //torpor
        //    ArkName.Create("DinoStats", 3), //oxygen
        //    ArkName.Create("DinoStats", 4), //food
        //    ArkName.Create("DinoStats", 5), //water
        //    ArkName.Create("DinoStats", 6), //temperature
        //    ArkName.Create("DinoStats", 7), //weight
        //    ArkName.Create("DinoStats", 8), //melee damage
        //    ArkName.Create("DinoStats", 9), //movement speed
        //    ArkName.Create("DinoStats", 10), //fortitude
        //    ArkName.Create("DinoStats", 11) //crafting speed
        //};

        internal static readonly new ArkNameTree _dependencies = new ArkNameTree
        {
        };

        //private Regex _r_nameLevelSpecies = new Regex(@"^(?<name>.+) - Lvl (?<level>\d+) \((?<species>.+)\)$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        internal IPropertyContainer _dino;

        internal new void Decouple()
        {
            _dino = null;
            base.Decouple();
        }

        public ArkCloudInventoryDino() : base()
        {
            //Stats = new string[_dinoStats.Length];
        }

        public ArkCloudInventoryDino(IPropertyContainer dino, IGameObject creature, IGameObject status, IGameObject inventory, ISaveState saveState) : base(creature, status, saveState)
        {
            _dino = dino;

            //Id1 = dino.GetPropertyValue<uint>(_dinoId1);
            //Id2 = dino.GetPropertyValue<uint>(_dinoId2);
            //CloudName = dino.GetPropertyValue<string>(_dinoName);
            //if (CloudName != null)
            //{
            //    var m = _r_nameLevelSpecies.Match(CloudName);
            //    if (m.Success)
            //    {
            //        Name = m.Groups["name"].Value;
            //        Level = int.Parse(m.Groups["level"].Value);
            //        Species = m.Groups["species"].Value;
            //    }
            //}
            //ExperiencePoints = dino.GetPropertyValue<float?>(_dinoExperiencePoints);
            //CloudClassName = dino.GetPropertyValue<ObjectReference>(_dinoClass)?.ObjectString?.Name;
            //if (CloudClassName != null)
            //{
            //    var index = CloudClassName.LastIndexOf('.');
            //    if (index != -1 && index < CloudClassName.Length - 1)
            //    {
            //        ClassName = CloudClassName.Substring(index + 1);
            //    }
            //}
            //for (var i = 0; i < Stats.Length; i++) Stats[i] = dino.GetPropertyValue<string>(_dinoStats[i]);
        }

        //public uint Id1 { get; set; }
        //public uint Id2 { get; set; }
        //public ulong Id => ((ulong)Id1 << 32) | Id2;
        //public string CloudName { get; set; }
        //public string CloudClassName { get; set; }
        //public string Name { get; set; }
        //public int? Level { get; set; }
        //public string Species { get; set; }
        //public string ClassName { get; set; }
        //public float? ExperiencePoints { get; set; }
        //public string[] Stats { get; set; }
    }
}