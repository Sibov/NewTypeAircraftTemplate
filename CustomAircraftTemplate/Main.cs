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

namespace CustomAircraftTemplate
{
    public class Main : VTOLMOD
    {
        
        
        public static Main instance;

        //Stores a prefab of the aircraft in order to spawn it in whenever you want
        public static GameObject aircraftPrefab;
        public static GameObject debugTools;
        public static GameObject aircraftMirage;
        public static int i=0;

        public static GameObject playerGameObject;
        public MpPlugin plugin = null;
        public static string pathToBundle;
        public static bool logging = true;

        // This method is run once, when the Mod Loader is done initialising this game object
        public override void ModLoaded()
        {
            Debug.unityLogger.logEnabled = Main.logging;
            instance = this;

            Debug.Log("ML3");

            pathToBundle = Path.Combine(instance.ModFolder, AircraftInfo.AircraftAssetbundleName);
            aircraftPrefab = FileLoader.GetAssetBundleAsGameObject(pathToBundle, AircraftInfo.AircraftPrefabName);
            Debug.Log("ML1");
            HarmonyInstance harmonyInstance = HarmonyInstance.Create(AircraftInfo.HarmonyId);
            harmonyInstance.PatchAll();

            Debug.Log("ML2");
            ;

            Debug.Log("ML4");
            Debug.Log(pathToBundle);
            
           
           
            Debug.Log("ML5");

            Debug.Log("Got le " + aircraftPrefab.name);

            //Adds the custom plane to the main menu
            StartCoroutine(AircraftAPI.CreatePlaneMenuItem());
   
           

            //This is an event the VTOLAPI calls when the game is done loading a scene
            VTOLAPI.SceneLoaded += SceneLoaded;
            base.ModLoaded();

            this.checkMPloaded();


        }

        public void checkMPloaded()
        {
            Debug.unityLogger.logEnabled = Main.logging;
            FlightLogger.Log("checking Multiplayer is installed");
            List<Mod> list = new List<Mod>();
            list = VTOLAPI.GetUsersMods();
            foreach (Mod mod in list)
            {
                bool flag = mod.isLoaded && mod.name.Contains("ultiplayer");
                if (flag)
                {
                    this.plugin = new MpPlugin();
                    this.plugin.MPlock();
                }
            }
        }



        //This method is called every frame by Unity. Here you'll probably put most of your code
        void Update()
        {
           //StartCoroutine(Aeroinfo());
            
        }

        private IEnumerator Aeroinfo()
        {
            
            yield break;
        }

        //This method is like update but it's framerate independent. This means it gets called at a set time interval instead of every frame. This is useful for physics calculations
        void FixedUpdate()
        {
            Debug.unityLogger.logEnabled = Main.logging;
            AircraftSetup.SetUpEOTS();
        }

        //This function is called every time a scene is loaded. this behaviour is defined in Awake().
        private void SceneLoaded(VTOLScenes scene)
        {
            //If you want something to happen in only one (or more) scenes, this is where you define it.

            //For example, lets say you're making a mod which only does something in the ready room and the loading scene. This is how your code could look:
             switch (scene)
            {
                case VTOLScenes.VehicleConfiguration:
                    Debug.Log("Reload the configurator");
                    StartCoroutine(InitWaiter());
                    

                    break;
                default:
                    Debug.Log("In scene: " + scene);

                    break;

            }


        }

        private IEnumerator InitWaiter()
        {
            Debug.Log("InitWaiter Started");
            yield return new WaitForSeconds(3f);
           // TriggerLCRefresh();
            yield break;
        }

        private void TriggerLCRefresh()
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("Reload the configurator2");
            GameObject f26loadoutscreen = GameObject.Find("FA26-LoadoutConfigurator(Clone)");
            if (f26loadoutscreen != null) { Debug.Log("found lc"); } else { Debug.Log("notfound lc"); }
            CampaignSave campaignSave = PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID);
            Debug.Log("Reload the configurator3");
            f26loadoutscreen.GetComponent<LoadoutConfigurator>().Initialize(campaignSave);
            Debug.Log("Reload the configurator4");
            Debug.Log("hpnode size: " + f26loadoutscreen.GetComponent<LoadoutConfigurator>().hpNodes.Length);
            VRLever mirageBL = AircraftAPI.GetChildWithName(Main.playerGameObject, "BrakeLockInteractable", false).GetComponent<VRLever>();
            mirageBL.SetState(0);

        }
    }
    }
