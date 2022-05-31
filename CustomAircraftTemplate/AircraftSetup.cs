using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Harmony;
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
            //Debug.Log("SetEOT0");
            
            if (Main.i == 1) { return; };

            //Debug.Log("SetEOT1");

            
            
            try
            {
                //Debug.Log("SetEOT2.1");
                tgp = AircraftAPI.GetChildWithName(Main.aircraftCustom, "fa26_tgp", true);
                //Debug.Log("SetEOT2.1.1");
                targetCamera = AircraftAPI.GetChildWithName(tgp, "TargetingCam", true);
                //Debug.Log("SetEOT2.1.2");
                tgCam = targetCamera.GetComponent<Camera>();
                //Debug.Log("SetEOT2.1.3");
                tgOptTargeter = tgp.GetComponent<OpticalTargeter>();
                //Debug.Log("SetEOT2.1.4");
                TMFD = Main.aircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);
                //Debug.Log("SetEOT2.1.1");

                rt = tgCam.targetTexture;
            }
            catch
            {
                //Debug.Log("SetEOT2.2");
                return;
            }

            //Debug.Log("SetEOT3");


            
            
            //Debug.Log("SetEOT4");

            GameObject MFDTGPPage = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MFDTargetingRT", false);
            //Debug.Log("SetEOT5");

            MFDTGPPage.GetComponent<RawImage>().texture = rt;


            
            Main.i = 1;




            //Debug.Log("SetEOT7");
        }


        

            

        
        

        
    }
}
