using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAircraftTemplate
{
    public class AircraftInfo
    {

        //READ ME, IMPORTANT!!!!!!!!
        //You must change HarmonyId in order for your custom aircraft mod to be compatable with other aircraft mods
        public const string HarmonyId = "Bovine.GAV25";

        //Stores if your custom aircraft is selected.
        //This is what prevents your aircraft from constantly replacing the FA-26
        public static bool AircraftSelected = false;

        //Info about your aircraft
        public const float maxInternalFuel = 5000;
        public const Int32 AircraftMPIdentifier = 7;
        public const string customAircraftPV = "GAV-25.asset";
        public const string AircraftName = "GAV-25 Bullshark";
        public const string AircraftNickName = "Bullshark";
        public const string AircraftDescription = "GAV-25 Bullshark";

        //Names of the various files you need to put in your builds folder
        public const string PreviewPngFileName = "Mirage2000preview.png";
        public const string AircraftAssetbundleName = "gav25";
        

        //Name of the prefab of your aircraft from the assetbundle
        public const string AircraftPrefabName = "GAV-25B.prefab";
        public const string AircraftLoadoutConfigurator = "GAV25-LoadoutConfigurator.prefab";



    }
}
