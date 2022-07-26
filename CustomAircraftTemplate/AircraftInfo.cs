﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAircraftTemplateF35
{
    public class AircraftInfo
    {

        //READ ME, IMPORTANT!!!!!!!!
        //You must change HarmonyId in order for your custom aircraft mod to be compatable with other aircraft mods
        public const string HarmonyId = "Bovine.F35";

        //Stores if your custom aircraft is selected.
        //This is what prevents your aircraft from constantly replacing the FA-26
        public static bool AircraftSelected = false;

        //Info about your aircraft
        public const float maxInternalFuel = 7600;
        //You need a number for this below to identify your aircraft, see modding community to discuss next number
        public const Int32 AircraftMPIdentifier = 9;
        public const string customAircraftPV = "F-35.asset";
        public const string pilottype = "F-45A";




        //Names of the various files you need to put in your builds folder

        public const string AircraftAssetbundleName = "f35";
        

        //Name of the prefab of your aircraft from the assetbundle
        public const string AircraftPrefabName = "F35.prefab";
        public const string AircraftLoadoutConfigurator = "F35-LoadoutConfigurator.prefab";



    }
}
