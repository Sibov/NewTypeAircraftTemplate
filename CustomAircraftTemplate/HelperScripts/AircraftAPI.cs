using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CAT
{
    //internal class AircraftApi
    //{
    //    public static GameObject SeatAdjustPoseBounds;
        
    //    /// <summary>
    //    /// This method is executed after <see cref="PilotSaveManager"/> loads all vehicles for the base game.
    //    /// </summary>
    //    public static void VehicleListUpdate()
    //    {
    //        // private static field = Traverse babyyyyy
    //        var t1 = Traverse.Create<PilotSaveManager>();
    //        // yoink the existing vars that PilotSaveManager uses
    //        var vehicles = t1.Field("vehicles").GetValue<Dictionary<string, PlayerVehicle>>();
    //        var vehicleList = t1.Field("vehicleList").GetValue<List<PlayerVehicle>>();
    //        if (vehicles.ContainsKey(Main.CustomAircraftPv.vehicleName))
    //            return;

    //        // then add our vehicle to the list
    //        vehicles.Add(Main.CustomAircraftPv.vehicleName, Main.CustomAircraftPv);
    //        vehicleList.Add(Main.CustomAircraftPv);

    //        Main.PlayerVehicleList = vehicleList;
            
    //        // and set them back with our fancy updated data structures
    //        t1.Field("vehicles").SetValue(vehicles);
    //        t1.Field("vehicleList").SetValue(vehicleList);

    //        VTResources.LoadPlayerVehicles();
    //        PilotSaveManager.LoadPilotsFromFile();
    //    }
        
    //    /// <summary>
    //    /// Returns the first child <see cref="GameObject"/> with the given name from the parent, else <c>null</c>.
    //    /// This search includes names with any number of "<c>(Clone)</c>" strings bolted to the end.
    //    /// </summary>
    //    /// <param name="parent">The parent game object to search through</param>
    //    /// <param name="name">The name of the child game object to locate</param>
    //    /// <param name="check">Deprecated parameter previously used for logging</param>
    //    public static GameObject GetChildWithName(GameObject parent, string name, bool check = false, bool matchClone = true)
    //    {
    //        var children = parent.GetComponentsInChildren<Transform>(true);
    //        var nameRegex = @"^" + name + (matchClone ? @"(\s*\(Clone\))*$" : @"$");
    //        return (from child in children where Regex.IsMatch(child.name, nameRegex) select child.gameObject).FirstOrDefault();
    //    }
    //}
}
