using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAircraftTemplate
{
    class AircraftAPI
    {
        public static GameObject SEAT_ADJUST_POSE_BOUNDS;

        public static void VehicleListUpdate()
        {
            // this all executes after PilotSaveManager loads all the vehicles from base game
            Main aircraftMod = Main.Instance;

            // private static field = Traverse babyyyyy
            Traverse trav = Traverse.Create<PilotSaveManager>();
            // yoink the existing vars that PilotSaveManager uses
            var vehicles = trav.Field("vehicles").GetValue<Dictionary<string, PlayerVehicle>>();
            var vehicleList = trav.Field("vehicleList").GetValue<List<PlayerVehicle>>();
            if (vehicles.ContainsKey(aircraftMod.customAircraftPV.vehicleName))
            {
                return;
            }
            // then add our vehicle to the list
            vehicles.Add(aircraftMod.customAircraftPV.vehicleName, aircraftMod.customAircraftPV);
            vehicleList.Add(aircraftMod.customAircraftPV);

            aircraftMod.playerVehicleList = vehicleList;

            // and set them back with our fancy updated data structures
            trav.Field("vehicles").SetValue(vehicles);
            trav.Field("vehicleList").SetValue(vehicleList);

            VTResources.LoadPlayerVehicles();
            PilotSaveManager.LoadPilotsFromFile();
        }

        public static GameObject GetChildWithName(GameObject obj, string name, bool check)
        {
            Transform[] children = obj.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.name == name || child.name == (name + "(Clone)"))
                {
                    return child.gameObject;
                }
            }
            return null;
        }
    }
}
