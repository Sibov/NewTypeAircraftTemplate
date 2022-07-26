using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomAircraftTemplateF35
{
    class AircraftAPI
    {
        public static GameObject SEAT_ADJUST_POSE_BOUNDS;
        private static Texture2D MenuTexture;

       
        

        public static void VehicleListUpdate()
        {
            
            //Debug.unityLogger.logEnabled = Main.logging;
            // this all executes after PilotSaveManager loads all the vehicles from base game

            // private static field = Traverse babyyyyy
            Debug.Log("F35 VLU1.0");
            Traverse trav = Traverse.Create<PilotSaveManager>();
            // yoink the existing vars that PilotSaveManager uses
            Debug.Log("F35 VLU1.1");
            var vehicles = trav.Field("vehicles").GetValue<Dictionary<string, PlayerVehicle>>();
            Debug.Log("F35 VLU1.1.1");
            var vehicleList = trav.Field("vehicleList").GetValue<List<PlayerVehicle>>();
            Debug.Log("F35 VLU1.1.2");
            if (vehicles.ContainsKey(Main.customAircraftPV.vehicleName))
            {
                Debug.Log("F35 VLU1.1.3");
                return;
            }
            // then add our vehicle to the list
            Debug.Log("F35 VLU1.3");

            Debug.Log("F35 VLU2.0");


            
            vehicles.Add(Main.customAircraftPV.vehicleName, Main.customAircraftPV);
            vehicleList.Add(Main.customAircraftPV);

            Main.playerVehicleList = vehicleList;

            foreach (PlayerVehicle vehicle in vehicleList)
            {
                Debug.Log("F35 VLU2.1 : " + vehicle.vehicleName);
            }

            Debug.Log("F35 VLU3.0");
            // and set them back with our fancy updated data structures
            trav.Field("vehicles").SetValue(vehicles);
            trav.Field("vehicleList").SetValue(vehicleList);
            Debug.Log("F35 VLU3.1");
            VTResources.LoadPlayerVehicles();
            PilotSaveManager.LoadPilotsFromFile();


            Main.checkPVListFull = true;
        }


      
        public static GameObject GetChildWithName(GameObject obj, string name, bool check)
        {

            //Debug.unityLogger.logEnabled = Main.logging;
            Transform[] children = obj.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (check) {
                    Debug.Log("F35 Looking for:" + name + ", Found:" + child.name); 
                }
                if (child.name == name || child.name == (name + "(Clone)"))
                {
                    return child.gameObject;
                }
            }


            return null;

        }

       
        
       



       
    }




}
