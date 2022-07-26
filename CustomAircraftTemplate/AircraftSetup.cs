using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Harmony;
using UnityEngine.UI;
using VTOLVR.Multiplayer;

namespace CustomAircraftTemplateSU35
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
        private static GameObject eotstf;
        private static GameObject mpSyncsObj;
        private static TGPSync mpSyncsTGPSync;
        private static GameObject radarUIC;
        private static GameObject TPC;
        private static GameObject TSC;
        private static TacticalSituationController TSCComp;
        private static GameObject MFDTRT;
        private static GameObject TC;

        public static void SetUpEOTS()
        {
            Debug.Log("SU35 SetEOT0");
            
            if (Main.i == 1) { return; };

            Debug.Log("SU35 SetEOT1");



            try
            {
                Debug.Log("SU35 SetEOT2.1");
                tgp = AircraftAPI.GetChildWithName(Main.aircraftCustom, "fa26_tgp", true);
                Debug.Log("SU35 SetEOT2.1.1");
                targetCamera = AircraftAPI.GetChildWithName(tgp, "TargetingCam", true);
                Debug.Log("SU35 SetEOT2.1.2");
                tgCam = targetCamera.GetComponent<Camera>();
                Debug.Log("SU35 SetEOT2.1.3");
                tgOptTargeter = tgp.GetComponent<OpticalTargeter>();
                Debug.Log("SU35 SetEOT2.1.4");
                TMFD = Main.aircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);
                Debug.Log("SU35 SetEOT2.1.1");

                rt = tgCam.targetTexture;
            }


            catch
            {
                try
                {
                    eotstf = AircraftAPI.GetChildWithName(Main.aircraftCustom, "EOTSHolder", true);
                    Debug.Log("SU35 SetEOT3.1.1");
                    GameObject sevtf = VTResources.GetPlayerVehicle("F-45A").vehiclePrefab;

                    GameObject sevtfeots = AircraftAPI.GetChildWithName(sevtf, "EOTS", false);
                    Debug.Log("SU35 SetEOT3.1.2");

                    GameObject EOTSObj = UnityEngine.Object.Instantiate<GameObject>(sevtfeots, eotstf.transform.position, eotstf.transform.rotation);
                    Debug.Log("SU35 SetEOT3.1.3");
                    EOTSObj.transform.SetParent(eotstf.transform);
                    EOTSObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                    EOTSObj.transform.localPosition = new Vector3(0f, 0f, 0f);


                    mpSyncsObj = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MPSyncs", true);
                    Debug.Log("SU35 SetEOT3.1.4");
                    mpSyncsTGPSync = mpSyncsObj.GetComponent<TGPSync>();
                    Debug.Log("SU35 SetEOT3.1.5");
                    mpSyncsTGPSync.targeter = EOTSObj.GetComponent<OpticalTargeter>();
                    Debug.Log("SU35 SetEOT3.1.5.1");
                    EOTSObj.GetComponent<OpticalTargeter>().actor = Main.aircraftCustom.GetComponent<Actor>();
                    EOTSObj.GetComponent<OpticalTargeter>().wm = Main.aircraftCustom.GetComponent<WeaponManager>();
                    Debug.Log("SU35 SetEOT3.1.6");
                    Main.aircraftCustom.GetComponent<WeaponManager>().opticalTargeter = EOTSObj.GetComponent<OpticalTargeter>();
                    Debug.Log("SU35 SetEOT3.1.7");
                    radarUIC = AircraftAPI.GetChildWithName(Main.aircraftCustom, "RadarUIController", true);

                    Debug.Log("SU35 SetEOT3.1.8");
                    radarUIC.GetComponent<MFDRadarUI>().tgp = EOTSObj.GetComponent<OpticalTargeter>();
                    Debug.Log("SU35 SetEOT3.1.9");
                    TC = AircraftAPI.GetChildWithName(EOTSObj, "TargetingCam", true);

                    TPC = AircraftAPI.GetChildWithName(Main.aircraftCustom, "TargetingPageController", true);

                    Debug.Log("SU35 SetEOT3.1.10");
                    TPC.GetComponent<TargetingMFDPage>().opticalTargeter = EOTSObj.GetComponent<OpticalTargeter>();
                    TPC.GetComponent<TargetingMFDPage>().targetingCamera = TC.GetComponent<Camera>();
                    TPC.GetComponent<TargetingMFDPage>().cameraFog = TC.GetComponent<CameraFogSettings>();
                    TPC.GetComponent<TargetingMFDPage>().targetIlluminator = TC.GetComponent<IlluminateVesselsOnRender>();
                    Debug.Log("SU35 SetEOT3.1.11");
                    TSC = AircraftAPI.GetChildWithName(Main.aircraftCustom, "TacticalSituationController", true);

                    Debug.Log("SU35 SetEOT3.1.12");
                    TSC.GetComponent<TacticalSituationController>().opticalTargeter = EOTSObj.GetComponent<OpticalTargeter>();
                    Debug.Log("SU35 SetEOT3.1.13");
                    TSCComp = TSC.GetComponent<TacticalSituationController>();
                    TSCComp.CollectFromTGP();
                    MFDTRT = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MFDTargetingRT", true);


                    MFDTRT.GetComponent<RawImage>().texture = TC.GetComponent<Camera>().targetTexture;

                    return;
                }
                catch
                {
                    return;
                }
            }



            Debug.Log("SU35 SetEOT3");




            Debug.Log("SU35 SetEOT4");

            GameObject MFDTGPPage = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MFDTargetingRT", false);
            Debug.Log("SU35 SetEOT5");

            MFDTGPPage.GetComponent<RawImage>().texture = rt;


            
            Main.i = 1;




            Debug.Log("SU35 SetEOT7");
        }


        

            

        
        

        
    }
}
