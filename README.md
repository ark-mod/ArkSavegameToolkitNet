# ARK Savegame Toolkit .NET Core

## Introduction

A library for reading ARK Survival Evolved savegame files in C#. The library is used extensively by ArkBot (https://github.com/ark-mod/ArkBot). 

## How to use

```csharp
using System;
using System.Threading;
using ArkSavegameToolkitNet.Domain;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize default settings (maps etc.)
            ArkToolkitDomain.Initialize();

            //prepare
            var cd = new ArkClusterData(@"C:\save\cluster", loadOnlyPropertiesInDomain: true);
            var gd = new ArkGameData(@"C:\save\TheIsland.ark", cd, loadOnlyPropertiesInDomain: true);

            //extract savegame
            if (gd.Update(CancellationToken.None, deferApplyNewData: true)?.Success == true)
            {
                //extract cluster data
                var cr = cd.Update(CancellationToken.None);

                //assign the new data to the domain model
                gd.ApplyPreviousUpdate();

                //query the domain model
                var rexes = gd.TamedCreatures.Where(x => x.ClassName?.Equals("Rex_Character_BP_C") == true).ToArray();
            }
        }
    }
}
```

## Acknowledgements

This library builds on Qowyn's work in reading the ARK Save Game format implemented in ark-savegame-toolkit (https://github.com/Qowyn/ark-savegame-toolkit) in Java.

## Links

ArkBot (https://github.com/ark-mod/ArkBot)