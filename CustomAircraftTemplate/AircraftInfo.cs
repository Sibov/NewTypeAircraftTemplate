using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAircraftTemplateSU35
{
    public class AircraftInfo
    {

        //READ ME, IMPORTANT!!!!!!!!
        //You must change HarmonyId in order for your custom aircraft mod to be compatable with other aircraft mods
        public const string HarmonyId = "Bovine.SU35";

        //Stores if your custom aircraft is selected.
        //This is what prevents your aircraft from constantly replacing the FA-26
        public static bool AircraftSelected = false;

        //Info about your aircraft
        public const float maxInternalFuel = 11500;
        public const Int32 AircraftMPIdentifier = 6;
        public const string customAircraftPV = "SU-35.asset";
        
        public const string pilottype = "F/A-26B";

        //Names of the various files you need to put in your builds folder
        public const string PreviewPngFileName = "Mirage2000preview.png";
        public const string AircraftAssetbundleName = "su35";
        


        //Name of the prefab of your aircraft from the assetbundle
        public const string AircraftPrefabName = "SU-35.prefab";
        public const string AircraftLoadoutConfigurator = "SU35-LoadoutConfigurator.prefab";



    }
}
