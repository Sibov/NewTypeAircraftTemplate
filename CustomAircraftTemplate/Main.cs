using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Unity;
using TMPro;
using UnityEngine.UI;

namespace CustomAircraftTemplateSU35
{
    public class Main : VTOLMOD
    {
        
        
        public static Main instance;

        //Stores a prefab of the aircraft in order to spawn it in whenever you want
        public static GameObject aircraftPrefab;
        public static GameObject magic2Prefab;
        public static PlayerVehicle customAircraftPV;
        public static BuiltInCampaigns customBICampaigns;
        public static GameObject aircraftLoadoutConfiguratorPrefab;
        public static GameObject DebugTools;
        public static GameObject aircraftCustom;
        public static GameObject BOQuad;
        public static Single currentGAlpha;
        public static GameObject customAircraftPVobject;
        public static bool checkPVListFull = false;
        public static MultiplayerSpawn.Vehicles aircraftMSVId;
        public static int unitListCount = 1;

        

        public static int i=0;

        public static GameObject playerGameObject;
       
        public static string pathToBundle;
        public static bool logging = true;
        public static List<Campaign> campaignslist;
        public static String unitList;
        internal static List<PlayerVehicle> playerVehicleList;
        public static bool aircraftLoaded = false;
        public static TextMeshPro radarcontactlist;
        public static GameObject miniicp;
        internal static Radar radar;
        internal static GameObject HMCSAltText;

        // This method is run once, when the Mod Loader is done initialising this game object
        public override void ModLoaded()
        {
            
            //Debug.unityLogger.logEnabled = Main.logging;
            instance = this;

            Debug.Log("SU35 ML3");

            pathToBundle = Path.Combine(instance.ModFolder, AircraftInfo.AircraftAssetbundleName);
            AssetBundle bundleLoad = FileLoader.GetAssetBundleAsGameObject(pathToBundle, AircraftInfo.AircraftAssetbundleName);
            aircraftPrefab = FileLoader.GetPrefabAsGameObject(bundleLoad, AircraftInfo.AircraftPrefabName);
            //magic2Prefab = FileLoader.GetPrefabAsGameObject(bundleLoad, "m2-srmx1.prefab");

            aircraftLoadoutConfiguratorPrefab = FileLoader.GetPrefabAsGameObject(bundleLoad, AircraftInfo.AircraftLoadoutConfigurator);
            customAircraftPV = FileLoader.GetPrefabAsPlayerVehicle(bundleLoad, AircraftInfo.customAircraftPV);
            customBICampaigns = FileLoader.GetPrefabAsBICampaigns(bundleLoad, "Campaigns.asset");
            Debug.Log("SU35 ML3.1");
            int count = Enum.GetValues(typeof(MultiplayerSpawn.Vehicles)).Length;
            Debug.Log("SU35 ML3.2 : " + count);
            aircraftMSVId = (MultiplayerSpawn.Vehicles)AircraftInfo.AircraftMPIdentifier;
            VTNetworking.VTNetworkManager.RegisterOverrideResource(customAircraftPV.resourcePath, aircraftPrefab);
          

            Debug.Log("SU35 ML1");



            HarmonyInstance harmonyInstance = HarmonyInstance.Create(AircraftInfo.HarmonyId);
            
            harmonyInstance.PatchAll();
           

            Debug.Log("SU35 ML2");
            

            Debug.Log("SU35 ML4");
            //Debug.Log(pathToBundle);
            
           
           
            Debug.Log("SU35 ML5");

            Debug.Log("SU35 Got " + aircraftPrefab.name);

           

            //This is an event the VTOLAPI calls when the game is done loading a scene
            VTOLAPI.SceneLoaded += SceneLoaded;
            base.ModLoaded();

            

            AircraftAPI.VehicleListUpdate();
            
        }

       


        //This method is called every frame by Unity. Here you'll probably put most of your code
        void Update()
        {
            
        }


        //This method is like update but it's framerate independent. This means it gets called at a set time interval instead of every frame. This is useful for physics calculations
        void FixedUpdate()
        {
            if (aircraftLoaded)
            {
               
                //Main.HMCSAltText.GetComponent<Text>().text = Main.aircraftCustom.GetComponent<FlightInfo>().radarAltitude.ToString();
            } 
        }

        //This function is called every time a scene is loaded. this behaviour is defined in Awake().
        private void SceneLoaded(VTOLScenes scene)
        {
            //If you want something to happen in only one (or more) scenes, this is where you define it.
            //Debug.unityLogger.logEnabled = Main.logging;
            //For example, lets say you're making a mod which only does something in the ready room and the loading scene. This is how your code could look:
            switch (scene)
            {
                
                case VTOLScenes.VehicleConfiguration:
                    Debug.Log("SU35 Reload the configurator");
                    StartCoroutine(InitWaiter());
                    

                    break;
                default:
                    Debug.Log("SU35 In scene: " + scene);

                    break;

            }


        }

        private IEnumerator InitWaiter()
        {
        //Debug.unityLogger.logEnabled = Main.logging;
        Debug.Log("SU35 InitWaiter Started");
            yield return new WaitForSeconds(3f);
          
            yield break;
        }

       
    }
    }
