using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace CAT
{
    [Serializable]
    public struct AircraftConfig
    {
        /// <summary>
        /// A unique identifier for this mod. It must not conflict with any other mod at all, or things will be break. Example: "<c>Bovine.SU35</c>"
        /// </summary>
        public string HarmonyId;
        /// <summary>
        /// The aircraft asset bundle name, as exported from Unity. Example: "<c>su35</c>"
        /// </summary>
        public string AssetBundleName;
        /// <summary>
        /// The aircraft prefab name, as set in Unity. Example: "<c>SU-35.prefab</c>"
        /// </summary>
        public string PrefabName;

        ///// <summary>
        ///// Static variable that prevents the aircraft from constantly replacing the F/A-26.
        ///// </summary>
        //public static bool AircraftSelected = false;
        ///// <summary>
        ///// The asset file identifier for the custom aircraft. Example: "<c>SU-35.asset</c>"
        ///// </summary>
        //public const string CustomAircraftPv = "SU-35.asset";
        ///// <summary>
        ///// Determines the pilot root type to use. Must be one of "<c>F/A-26B</c>", "<c>F-45A</c>", "<c>AV-42C</c>".
        ///// </summary>
        //public const string PilotRootType = "F/A-26B";
        ///// <summary>
        ///// The local file name of the preview image. Example: "<c>Mirage2000preview.png</c>"
        ///// </summary>
        //public const string PreviewPngFileName = "Mirage2000preview.png";
        ///// <summary>
        ///// The prefab for the aircraft loadout configurator, as set in Unity. Example: "<c>SU35-LoadoutConfigurator.prefab</c>"
        ///// </summary>
        //public const string AircraftLoadoutConfigurator = "SU35-LoadoutConfigurator.prefab";
    }

    public static class AircraftInfo
    {
        public static AircraftConfig LoadFromFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Could not load config file");
            
            return JsonConvert.DeserializeObject<AircraftConfig>(File.ReadAllText(path));
        }

        public static void SaveToFile(string path, AircraftConfig ai)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(ai));
        }
    }
}
