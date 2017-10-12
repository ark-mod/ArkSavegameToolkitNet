using ArkSavegameToolkitNet.Domain;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ArkSavegameToolkitNet.TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var savePath = @"C:\save\TheIsland.ark";
            var clusterPath = @"C:\save\cluster";
            var domainOnly = true; //true: optimize loading of the domain model, false: load everything and keep references in memory

            //prepare
            var cd = new ArkClusterData(clusterPath, loadOnlyPropertiesInDomain: domainOnly);
            var gd = new ArkGameData(savePath, cd, loadOnlyPropertiesInDomain: domainOnly);

            var st = Stopwatch.StartNew();
            //extract savegame
            if (gd.Update(CancellationToken.None, null, true)?.Success == true)
            {
                Console.WriteLine($@"Elapsed (gd) {st.ElapsedMilliseconds:N0} ms");
                st = Stopwatch.StartNew();

                //extract cluster data
                var clusterResult = cd.Update(CancellationToken.None);

                Console.WriteLine($@"Elapsed (cd) {st.ElapsedMilliseconds:N0} ms");
                st = Stopwatch.StartNew();

                //assign the new data to the domain model
                gd.ApplyPreviousUpdate(domainOnly);

                Console.WriteLine($@"Elapsed (gd-apply) {st.ElapsedMilliseconds:N0} ms");

                Console.WriteLine("Save data loaded!");
            }
            else
            {
                Console.WriteLine("Failed to load save data!");
            }

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
