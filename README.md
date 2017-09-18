# ARK Savegame Toolkit .NET

## Introduction

A library for reading ARK Survival Evolved savegame files in C#. The library is used extensively by ArkBot (https://github.com/tsebring/ArkBot). 

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
            //prepare
            var cd = new ArkClusterData(@"C:\save\cluster");
            var gd = new ArkGameData(@"C:\save\TheIsland.ark", cd);

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

This library is ported from Qowyn's immense work on ark-savegame-toolkit (https://github.com/Qowyn/ark-savegame-toolkit) in Java. The code was partially translated from Java to C#. It then deviated from the main project to fulfill the requirements of ArkBot. Save editing was scrapped, extraction speed was prioritized and a full domain model was built on top.

## Links

ArkBot (https://github.com/tsebring/ArkBot)

ark-savegame-toolkit by Qowyn (https://github.com/Qowyn/ark-savegame-toolkit)

ark-tools by Qowyn (https://github.com/Qowyn/ark-tools)