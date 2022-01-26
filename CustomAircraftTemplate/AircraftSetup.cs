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
        private static TargetingMFDPage TMFD;
        private static RenderTexture rt;

        public static void SetUpHud()
        {
            CollimatedHUDUI hud = Fa26.GetComponentInChildren<CollimatedHUDUI>(true);
            hud.depth = 1000f;
            hud.UIscale = 1.5f;
        }

        public static void SetUpEjectionSeat()
        {
            Fa26.GetComponentInChildren<EjectionSeat>(true).canopyObject = AircraftAPI.GetChildWithName(customAircraft, "Canopy_Main",false) ;
        }

        public static void SetUpEOTS()
        {

            Debug.unityLogger.logEnabled = Main.logging;
            if (Main.i == 1) { return; };

            Debug.Log("SetEOT1");

            try
            {
                Debug.Log("SetEOT1.0.1");
                OpticalTargeter targeter = Main.aircraftMirage.GetComponentInChildren<OpticalTargeter>();
                
                    }
            catch
                {
                Debug.Log("SetEOT1.0");
                return; }

            Debug.Log("SetEOT1.1");
            try
            {
                Debug.Log("SetEOT1.1.1");
                TGP2 = GameObject.FindObjectOfType<OpticalTargeter>().gameObject;

            }
            catch
            {
                Debug.Log("SetEOT1.1.2");
                return;
            }
           
            if (TGP2 != null)
            {
                Debug.Log("TGP: " + TGP2.name);
                Debug.Log("TGP active: " + TGP2.active);
                
            }

            
            //Debug.Log("Targeter state: " + targeter.powered);
            //Debug.Log("Targeter object: " + targeter.gameObject);

            Debug.Log("SetEOT2");
            
            WeaponManager wm = Main.aircraftMirage.GetComponentInChildren<WeaponManager>(true);
            try
            {
                Debug.Log("SetEOT2.1");
                TMFD = Main.aircraftMirage.GetComponentInChildren<TargetingMFDPage>(true);
                rt = TMFD.targetingCamera.targetTexture;
            }
            catch
            {
                Debug.Log("SetEOT2.2");
                return;
            }

            Debug.Log("SetEOT3");


            
            //Debug.Log("WM actor: " + wm.actor);
            //Debug.Log("tgtr wm: " + targeter.wm);
            //Debug.Log("tgtr wm: " + targeter.actor);
            //Debug.Log("WM ui: " + wm.ui);
            //Debug.Log("WM vm: " + wm.vm);
            //Debug.Log("TMFD OT : " + TMFD.opticalTargeter);
            //Debug.Log("TMFD TC : " + TMFD.targetingCamera);
            //Debug.Log("TMFD TC A?: " + TMFD.targetingCamera.isActiveAndEnabled);
            //Debug.Log("TMFD TC A1?: " + TMFD.targetingCamera.enabled);


            //Debug.Log("TMFD OT PS: " + TMFD.opticalTargeter.powered);
            //Debug.Log("TMFD OT PS: " + TMFD.opticalTargeter);

            Debug.Log("SetEOT4");

            GameObject MFDTGPPage = AircraftAPI.GetChildWithName(Main.aircraftMirage, "MFDTargetingRT", false);
            Debug.Log("SetEOT5");

            MFDTGPPage.GetComponent<RawImage>().texture = rt;


            
            Main.i = 1;




            Debug.Log("SetEOT7");
        }


        public static void SetUpGauges()
        {
            Battery componentInChildren = Main.aircraftMirage.GetComponentInChildren<Battery>(true);
            FlightInfo componentInChildren2 = Main.aircraftMirage.GetComponentInChildren<FlightInfo>(true);
            GameObject childWithName3 = AircraftAPI.GetChildWithName(Main.aircraftMirage, "ClimbGauge",false);
            DashVertGauge dashVertGauge = childWithName3.AddComponent<DashVertGauge>();
            dashVertGauge.battery = componentInChildren;
            dashVertGauge.dialHand = AircraftAPI.GetChildWithName(childWithName3, "dialHand", false).transform;
            dashVertGauge.axis = new Vector3(0f, 1f, 0f);
            dashVertGauge.arcAngle = 360f;
            dashVertGauge.maxValue = 5f;
            dashVertGauge.lerpRate = 8f;
            dashVertGauge.loop = true;
            dashVertGauge.gizmoRadius = 0.02f;
            dashVertGauge.gizmoHeight = 0.005f;
            dashVertGauge.doCalibration = true;
            dashVertGauge.calibrationSpeed = 1f;
            dashVertGauge.info = componentInChildren2;
            dashVertGauge.measures = Main.aircraftMirage.GetComponent<MeasurementManager>();
        }

            public static void ScaleNavMap()
        {
            Transform mfd1 = AircraftAPI.GetChildWithName(Fa26, "MFD1",false).transform;
            Transform mapParent = AircraftAPI.GetChildWithName(Fa26, "MapParent", false).transform;
            Transform mapDisplay = AircraftAPI.GetChildWithName(Fa26, "MapDisplay", false).transform;
            Transform mapTest = AircraftAPI.GetChildWithName(Fa26, "MapTest", false).transform;
            Transform mapTransform = AircraftAPI.GetChildWithName(Fa26, "MapTransform", false).transform;

            float small = mfd1.transform.localScale.x / 99.73274f;
            float big = 99.73274f / mfd1.transform.localScale.x;

            Vector3 smallScale = Vector3.one * small;
            Vector3 bigScale = Vector3.one * big;

            mapParent.transform.localScale = smallScale;
            mapDisplay.transform.localScale = bigScale;
            mapTest.transform.localScale = bigScale;
            mapTransform.transform.localScale = bigScale;

        }

        
        public static void SetWingFold()
        {
            Main.instance.StartCoroutine(WingFoldRoutine());

        }

        
  


        public static IEnumerator WingFoldRoutine()
        {
            yield return new WaitForSeconds(1);

            Fa26.GetComponentInChildren<VehicleMaster>(true).SetWingFoldImmediate(false);
            Fa26.GetComponentInChildren<FlightWarnings>(true).RemoveCommonWarning(FlightWarnings.CommonWarnings.WingFold);

            VRLever wingLever = AircraftAPI.FindInteractable("Wing Fold").gameObject.GetComponent<VRLever>();
            wingLever.gameObject.GetComponent<AudioSource>().volume = 0;
            wingLever.RemoteSetState(0);
            WingFoldController[] wings = UnityEngine.Object.FindObjectsOfType<WingFoldController>();
            foreach (WingFoldController wing in wings)
            {
                wing.maxSpeed = float.MaxValue;
                wing.killSpeed = float.MaxValue;
                
            }
            WingFoldController[] array = null;

        }

        
    }
}
