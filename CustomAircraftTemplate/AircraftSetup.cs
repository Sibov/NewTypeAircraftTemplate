using UnityEngine;
using UnityEngine.UI;

namespace CustomAircraftTemplate
{
    class AircraftSetup
    {
        public static GameObject Fa26;
        public static GameObject customAircraft;
        private static GameObject TGP2;
        private static GameObject tgp;
        private static GameObject targetCamera;
        private static Camera tgCam;
        private static OpticalTargeter tgOptTargeter;
        private static TargetingMFDPage TMFD;
        private static RenderTexture rt;

        public static void SetUpEOTS()
        {
            Main aircraftMod = Main.Instance;

            if (aircraftMod.hasInitEots)
                return;
            try
            {
                tgp = AircraftAPI.GetChildWithName(aircraftMod.aircraftCustom, "fa26_tgp", true);
                targetCamera = AircraftAPI.GetChildWithName(tgp, "TargetingCam", true);
                tgCam = targetCamera.GetComponent<Camera>();
                tgOptTargeter = tgp.GetComponent<OpticalTargeter>();
                TMFD = aircraftMod.aircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);

                rt = tgCam.targetTexture;
            }
            catch
            {
                try
                {
                    tgp = AircraftAPI.GetChildWithName(aircraftMod.aircraftCustom, "EOTS", true);
                    targetCamera = AircraftAPI.GetChildWithName(tgp, "TargetingCam", true);
                    tgCam = targetCamera.GetComponent<Camera>();
                    tgOptTargeter = tgp.GetComponent<OpticalTargeter>();
                    TMFD = aircraftMod.aircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);
                }
                catch
                {
                    return;
                }
            }
            GameObject MFDTGPPage = AircraftAPI.GetChildWithName(aircraftMod.aircraftCustom, "MFDTargetingRT", false);

            MFDTGPPage.GetComponent<RawImage>().texture = rt;
            aircraftMod.hasInitEots = true;
        }
    }
}
