using ArkSavegameToolkitNet.DataTypes;
using System;
using System.IO;
using System.Runtime.CompilerServices;

//[assembly: InternalsVisibleTo("ArkSavegameToolkitNet.Benchmark")]
namespace ArkSavegameToolkitNet
{
    public static class ArkToolkit
    {
        /// <summary>
        /// Gets or sets a function that creates default settings for ARK Savegame Toolkit .NET
        /// </summary>
        public static Func<ArkToolkitLoaderSettings> DefaultSettings { get; set; }

        /// <summary>
        /// Load .ARK save game from the filesystem
        /// </summary>
        public static ArkSaveData LoadArkSave(string fileName)
        {
            return ArkToolkitLoader.CreateDefault().LoadArkSave(fileName);
        }

        /// <summary>
        /// Load .ARK save game from a stream
        /// </summary>
        public static ArkSaveData LoadArkSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadArkSave(stream, madeUpFileName, savedAt);
        }

        /// <summary>
        /// Load .ARK save game from a byte array
        /// </summary>
        public static ArkSaveData LoadArkSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadArkSave(data, madeUpFileName, savedAt);
        }


        /// <summary>
        /// Load .arkprofile save from the filesystem
        /// </summary>
        public static ProfileSaveGameData LoadProfileSave(string fileName)
        {
            return ArkToolkitLoader.CreateDefault().LoadProfileSave(fileName);
        }

        /// <summary>
        /// Load .arkprofile save from a stream
        /// </summary>
        public static ProfileSaveGameData LoadProfileSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadProfileSave(stream, madeUpFileName, savedAt);
        }

        /// <summary>
        /// Load .arkprofile save from a byte array
        /// </summary>
        public static ProfileSaveGameData LoadProfileSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadProfileSave(data, madeUpFileName, savedAt);
        }


        /// <summary>
        /// Load PlayerLocalData.arkprofile save from the filesystem
        /// </summary>
        public static LocalProfileSaveGameData LoadLocalProfileSave(string fileName)
        {
            return ArkToolkitLoader.CreateDefault().LoadLocalProfileSave(fileName);
        }

        /// <summary>
        /// Load PlayerLocalData.arkprofile save from a stream
        /// </summary>
        public static LocalProfileSaveGameData LoadLocalProfileSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadLocalProfileSave(stream, madeUpFileName, savedAt);
        }

        /// <summary>
        /// Load PlayerLocalData.arkprofile save from a byte array
        /// </summary>
        public static LocalProfileSaveGameData LoadLocalProfileSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadLocalProfileSave(data, madeUpFileName, savedAt);
        }


        /// <summary>
        /// Load .arktribe save from the filesystem
        /// </summary>
        public static TribeSaveGameData LoadTribeSave(string fileName)
        {
            return ArkToolkitLoader.CreateDefault().LoadTribeSave(fileName);
        }

        /// <summary>
        /// Load .arktribe save from a stream
        /// </summary>
        public static TribeSaveGameData LoadTribeSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadTribeSave(stream, madeUpFileName, savedAt);
        }

        /// <summary>
        /// Load .arktribe save from a byte array
        /// </summary>
        public static TribeSaveGameData LoadTribeSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadTribeSave(data, madeUpFileName, savedAt);
        }


        /// <summary>
        /// Load cluster save from the filesystem
        /// </summary>
        public static ClusterSaveGameData LoadClusterSave(string fileName)
        {
            return ArkToolkitLoader.CreateDefault().LoadClusterSave(fileName);
        }

        /// <summary>
        /// Load cluster save from a stream
        /// </summary>
        public static ClusterSaveGameData LoadClusterSave(Stream stream, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadClusterSave(stream, madeUpFileName, savedAt);
        }

        /// <summary>
        /// Load cluster save from a byte array
        /// </summary>
        public static ClusterSaveGameData LoadClusterSave(byte[] data, string madeUpFileName, DateTime savedAt)
        {
            return ArkToolkitLoader.CreateDefault().LoadClusterSave(data, madeUpFileName, savedAt);
        }


        /// <summary>
        /// Load cluster dino from a byte array
        /// </summary>
        public static ClusterDinoData LoadClusterDino(byte[] data, float version)
        {
            return ArkToolkitLoader.CreateDefault().LoadClusterDino(data, version);
        }


        /// <summary>
        /// Load frozen dino from a byte array
        /// </summary>
        public static FrozenDinoData LoadFrozenDino(byte[] data)
        {
            return ArkToolkitLoader.CreateDefault().LoadFrozenDino(data);
        }
    }
}
