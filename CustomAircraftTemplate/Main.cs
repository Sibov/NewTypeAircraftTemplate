using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;

namespace CAT
{
    public class Main : VTOLMOD
    {
        private const string ConfigFileName = "cat.json";

        public static Main Instance;
        public static AircraftConfig Config;
        
        public static string PathToBundle;

        // This method is run once, when the Mod Loader is done initializing this game object
        public override void ModLoaded()
        {
            Instance = this;

            var configPath = Path.Combine(Instance.ModFolder, ConfigFileName);
            try
            {
                Config = AircraftInfo.LoadFromFile(configPath);
            }
            catch (Exception exc)
            {
                Debug.LogError("[CAT] Could not load CAT config file (invalid cat.json?):");
                Debug.LogException(exc);
                enabled = false;
                return;
            }

            PathToBundle = Path.Combine(Instance.ModFolder, Config.AssetBundleName);

            VTResources.OnLoadingPlayerVehicles += () =>
                VTResources.LoadExternalVehicle(PathToBundle, Config.PrefabName);
            
            var harmonyInstance = HarmonyInstance.Create(Config.HarmonyId);
            harmonyInstance.PatchAll();

            VTOLAPI.SceneLoaded += SceneLoaded;
            base.ModLoaded();
        }

        /// <summary>
        /// This function is called every time a scene is loaded. This behaviour is defined in the <c>Awake()</c> call time step.
        /// </summary>
        private static void SceneLoaded(VTOLScenes scene)
        {
            switch (scene)
            {
                case VTOLScenes.VehicleConfiguration:
                case VTOLScenes.SplashScene:
                case VTOLScenes.SamplerScene:
                case VTOLScenes.ReadyRoom:
                case VTOLScenes.MeshTerrain:
                case VTOLScenes.LoadingScene:
                case VTOLScenes.OpenWater:
                case VTOLScenes.Akutan:
                case VTOLScenes.VTEditMenu:
                case VTOLScenes.VTEditLoadingScene:
                case VTOLScenes.VTMapEditMenu:
                case VTOLScenes.CustomMapBase:
                case VTOLScenes.CommRadioTest:
                case VTOLScenes.ShaderVariantsScene:
                case VTOLScenes.CustomMapBase_OverCloud:
                case VTOLScenes.LocalizationScene:
                default:
                    break;
            }
        }
    }

    //public class Main : VTOLMOD
    //{
    //    public static Main Instance;

    //    //Stores a prefab of the aircraft in order to spawn it in whenever you want
    //    public static GameObject AircraftPrefab;
    //    public static PlayerVehicle CustomAircraftPv;
    //    public static BuiltInCampaigns CustomBiCampaigns;
    //    public static GameObject AircraftLoadoutConfiguratorPrefab;

    //    public static GameObject AircraftCustom;
    //    public static GameObject BoQuad;
    //    public static float CurrentGAlpha;
        
    //    public static int IsEotsSetUp=0;

    //    public static GameObject PlayerGameObject;
       
    //    public static string PathToBundle;
    //    public static bool Logging = true;

    //    internal static List<PlayerVehicle> PlayerVehicleList;
    //    public static bool AircraftLoaded = false;
    //    public static TextMeshPro RadarContactList;
    //    public static GameObject MiniIcp;
    //    internal static Radar RadarRef;
    //    internal static GameObject HmcsAltText;

    //    // This method is run once, when the Mod Loader is done initialising this game object
    //    public override void ModLoaded()
    //    {
    //        Instance = this;
            
    //        PathToBundle = Path.Combine(Instance.ModFolder, AircraftInfo.AircraftAssetBundleName);
    //        var bundleLoad = FileLoader.GetAssetBundleAsGameObject(PathToBundle, AircraftInfo.AircraftAssetBundleName);
    //        AircraftPrefab = FileLoader.GetPrefabAsGameObject(bundleLoad, AircraftInfo.AircraftPrefabName);

    //        AircraftLoadoutConfiguratorPrefab = FileLoader.GetPrefabAsGameObject(bundleLoad, AircraftInfo.AircraftLoadoutConfigurator);
    //        CustomAircraftPv = FileLoader.GetPrefabAsPlayerVehicle(bundleLoad, AircraftInfo.CustomAircraftPv);
    //        CustomBiCampaigns = FileLoader.GetPrefabAsBICampaigns(bundleLoad, "Campaigns.asset");
    //        VTNetworking.VTNetworkManager.RegisterOverrideResource(CustomAircraftPv.resourcePath, AircraftPrefab);
          
    //        var harmonyInstance = HarmonyInstance.Create(AircraftInfo.HarmonyId);
            
    //        harmonyInstance.PatchAll();
            
    //        VTOLAPI.SceneLoaded += SceneLoaded;
    //        base.ModLoaded();
    //        AircraftApi.VehicleListUpdate();
    //    }

    //    /// <summary>
    //    /// This function is called every time a scene is loaded. This behaviour is defined in the <c>Awake()</c> call time step.
    //    /// </summary>
    //    private void SceneLoaded(VTOLScenes scene)
    //    {
    //        switch (scene)
    //        {
    //            case VTOLScenes.VehicleConfiguration:
    //            {
    //                StartCoroutine(InitWaiter());
    //            }
    //                break;
    //            case VTOLScenes.SplashScene:
    //            case VTOLScenes.SamplerScene:
    //            case VTOLScenes.ReadyRoom:
    //            case VTOLScenes.MeshTerrain:
    //            case VTOLScenes.LoadingScene:
    //            case VTOLScenes.OpenWater:
    //            case VTOLScenes.Akutan:
    //            case VTOLScenes.VTEditMenu:
    //            case VTOLScenes.VTEditLoadingScene:
    //            case VTOLScenes.VTMapEditMenu:
    //            case VTOLScenes.CustomMapBase:
    //            case VTOLScenes.CommRadioTest:
    //            case VTOLScenes.ShaderVariantsScene:
    //            case VTOLScenes.CustomMapBase_OverCloud:
    //            case VTOLScenes.LocalizationScene:
    //            default:
    //                break;
    //        }
    //    }

    //    private static IEnumerator InitWaiter()
    //    {
    //        yield return new WaitForSeconds(3f);
    //    }
    //}
}
