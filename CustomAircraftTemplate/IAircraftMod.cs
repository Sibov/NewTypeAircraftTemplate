using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomAircraftTemplate
{
    public abstract class IAircraftMod<T> : VTOLMOD where T : IAircraftMod<T>, new()
    {
        /// <summary> The single instance of this aircraft mod. </summary>
        public static T Instance { get; private set; }
        public IAircraftInfo AircraftInfo { get; private set; }

        /// <summary> Reference to the aircraft prefab, used to spawn instances. </summary>
        public      GameObject aircraftPrefab;
        /// <summary> Reference to the aircraft prefab's PlayerVehicle data object. </summary>
        public      PlayerVehicle customAircraftPV;
        /// <summary> Reference to the aircraft prefab's built-in campaign asset. </summary>
        public      BuiltInCampaigns customBICampaigns;
        /// <summary> Reference to the aircraft loadout configurator prefab, used to spawn instances. </summary>
        public      GameObject aircraftLoadoutConfiguratorPrefab;

        /// <summary> Reference to the instantiated custom aircraft, set during patches. </summary>
        public      GameObject aircraftCustom;
        /// <summary> Unique identifier, set not to overlap with the game's range or other mods during injection. </summary>
        public      MultiplayerSpawn.Vehicles aircraftMSVId;

        public      GameObject BOQuad;
        public      float currentGAlpha;

        public      bool hasInitEots = false;

        public      GameObject playerGameObject;

        public      string pathToBundle;
        public      bool logging = true;
        internal    List<PlayerVehicle> playerVehicleList;
        public      bool aircraftLoaded = false;

        public      TextMeshPro radarContactListText;
        public      GameObject miniIcp;
        internal    Radar radar;
        internal    GameObject HMCSAltText;

        public void AircraftModLoaded(T instance, IAircraftInfo aircraftInfo)
        {
            Instance = instance;

            AircraftInfo = aircraftInfo;

            pathToBundle = Path.Combine(Instance.ModFolder, AircraftInfo.AircraftAssetbundleName);
            AssetBundle bundleLoad = FileLoader.GetAssetBundleAsGameObject(pathToBundle, AircraftInfo.AircraftAssetbundleName);
            aircraftPrefab = FileLoader.GetPrefabAsGameObject(bundleLoad, AircraftInfo.AircraftPrefabName);

            aircraftLoadoutConfiguratorPrefab = FileLoader.GetPrefabAsGameObject(bundleLoad, AircraftInfo.AircraftLoadoutConfigurator);
            customAircraftPV = FileLoader.GetPrefabAsPlayerVehicle(bundleLoad, AircraftInfo.CustomAircraftPV);
            customBICampaigns = FileLoader.GetPrefabAsBICampaigns(bundleLoad, "Campaigns.asset");

            int count = Enum.GetValues(typeof(MultiplayerSpawn.Vehicles)).Length;

            aircraftMSVId = (MultiplayerSpawn.Vehicles)AircraftInfo.AircraftMPIdentifier;
            VTNetworking.VTNetworkManager.RegisterOverrideResource(customAircraftPV.resourcePath, aircraftPrefab);

            // Set up Harmony patching
            HarmonyInstance harmonyInstance = HarmonyInstance.Create(AircraftInfo.HarmonyId);
            harmonyInstance.PatchAll();

            VTOLAPI.SceneLoaded += SceneLoaded;
            base.ModLoaded();

            // Lastly, force the vehicle list to update now that an aircraft has been added
            AircraftAPI.VehicleListUpdate();
        }
        
        private void FixedUpdate()
        {
            if (!aircraftLoaded)
                return;

            //MirageElements.ClockUpdate();
            HMCSAltText.GetComponent<Text>().text = aircraftCustom.GetComponent<FlightInfo>().radarAltitude.ToString();
        }

        // This function is called every time a scene is loaded. This behaviour executes in the Awake() stage.
        private void SceneLoaded(VTOLScenes scene)
        {
            switch (scene)
            {
                case VTOLScenes.VehicleConfiguration:
                    {
                        StartCoroutine(InitWaiter());
                    } 
                    break;
                default:
                    break;
            }
        }

        private IEnumerator InitWaiter()
        {
            yield return new WaitForSeconds(3f);

            yield break;
        }
    }
}
