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
        public const string HarmonyId = "Bovine.Mirage";

        //Stores if your custom aircraft is selected.
        //This is what prevents your aircraft from constantly replacing the FA-26
        public static bool AircraftSelected = false;

        //Info about your aircraft
        public const string AircraftName = "Mirage 2000C-X";
        public const string AircraftNickName = "Mirage";
        public const string AircraftDescription = "\"Mirage 2000\" A fourth generation multi-role fighter developed as an alternative to the American and Russian fourth generation strike fighters.";

        //Names of the various files you need to put in your builds folder
        public const string PreviewPngFileName = "Mirage2000preview.png";
        public const string AircraftAssetbundleName = "mirage2000";
        public const string UnityMoverFileName = "MiragePositions.surg";
        public const string AIUnityMoverFileName = "MiragePositionsAI.surg";

        //Name of the prefab of your aircraft from the assetbundle
        public const string AircraftPrefabName = "Mirage2000.prefab";




    }
}
