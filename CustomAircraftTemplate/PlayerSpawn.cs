using Harmony;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Reflection;
using System.IO;
using Valve.Newtonsoft.Json;
using Harmony;
using TMPro;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;

namespace CustomAircraftTemplate
{

    [HarmonyPatch(typeof(Actor), "Awake")]
    public class UnitSpawnRCSPatch
    {
        public static void Postfix(Actor __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("RCS Patch: " + __instance.name);
            Debug.Log("RCS Patch: 1.1 ");


            //List<Actor> allUnitList = TargetManager.instance.allActors;
            //Debug.Log("no of units =" + allUnitList.Count);
            Debug.Log("RCS Patch: 1.3 ");

            /*
            foreach (Actor Unit in allUnitList)
            {
                try
                {
                    Debug.Log("RCS Patch: 1.4 ");

                    RadarCrossSection unitsRCSComponent = Unit.GetComponentInChildren<RadarCrossSection>(true);
                    Debug.Log(Unit.name + " : " + unitsRCSComponent);
                }
                catch
                {
                */

            Debug.Log("No RCS");
            GameObject UnitGO = __instance.gameObject;
            Debug.Log("No RCS 1");
            RadarCrossSection RCSforUnit = UnitGO.GetComponent<RadarCrossSection>();
            if (RCSforUnit == null)
            {
                RadarCrossSection UnitRCS = UnitGO.AddComponent<RadarCrossSection>();
                Debug.Log("No RCS 2");
                UnitRCS.weaponManager = UnitGO.GetComponent<WeaponManager>();

                Debug.Log("No RCS 3");
                UnitRCS.size = 7f;
                Debug.Log("No RCS 4");
                UnitRCS.overrideMultiplier = 1f;
                Debug.Log("No RCS 5");

                List<RadarCrossSection.RadarReturn> UnitRCSReturns = new List<RadarCrossSection.RadarReturn>();
                Debug.Log("No RCS 6");
                UnitRCSReturns = Main.aircraftMirage.GetComponent<RadarCrossSection>().returns;
                UnitRCS.enabled = true;
            }

            return;
        }

    }





    [HarmonyPatch(typeof(PlayerSpawn), "OnPreSpawnUnit")]
    public class OPSStartPatch
    {
        public static bool Prefix(PlayerSpawn __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            // __instance.OnPreSpawnUnit();
            PlayerVehicle vehicle = VTScenario.current.vehicle;
            Debug.Log("Starting OPS");


            GameObject vehiclePrefab = Main.aircraftPrefab;
            if (vehiclePrefab)
            {
                Debug.Log("Starting OPS Inst");

                Main.i = 0;

                Vector3 position = new Vector3(0, 2.66f, 0);
                Quaternion rotation = new Quaternion(0, 0, 0, 0);
                Main.aircraftMirage = UnityEngine.Object.Instantiate<GameObject>(vehiclePrefab, position, rotation);
                Rigidbody component = Main.aircraftMirage.GetComponent<Rigidbody>();
                Traverse traverse = Traverse.Create(__instance);
                traverse.Field("vehicleRb").SetValue(Main.aircraftMirage.GetComponent<Rigidbody>());
                traverse.Field("playerVm").SetValue(Main.aircraftMirage.GetComponent<VehicleMaster>());
                traverse.Field("vehicleRb").Field("interpolation").SetValue(RigidbodyInterpolation.None);


                Vector3 position2 = new Vector3(2, 2.66f, 2);
                GameObject vehiclePrefab2 = vehicle.vehiclePrefab;
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(vehiclePrefab2, Main.aircraftMirage.transform.position, Main.aircraftMirage.transform.rotation);
                vehicle.vehiclePrefab = Main.aircraftPrefab;
                Actor actor = __instance.actor = (FlightSceneManager.instance.playerActor = Main.aircraftMirage.GetComponent<Actor>());

                actor.actorName = PilotSaveManager.current.pilotName;

                actor.unitSpawn = __instance;

                GameObject FA26Aircraft = AircraftAPI.GetChildWithName(gameObject2, "FA-26B", false);

                Debug.Log("OPSStartPatch 000");
                if (!FA26Aircraft) { return true; }

                Debug.Log("OPSStartPatch 00");
                GameObject f26EjectorSeat = AircraftAPI.GetChildWithName(gameObject2, "EjectorSeat", false);
                f26EjectorSeat.transform.SetParent(Main.aircraftMirage.transform);

                Vector3 PlaneLocation = gameObject2.transform.position;
                Quaternion PlaneRotation = gameObject2.transform.rotation;

                Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);

                UnityEngine.Object.Destroy(FA26Aircraft);
                f26EjectorSeat.SetActive(true);


                Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);

                Debug.Log("OPSStartPatch 0");

                Main.aircraftMirage.transform.position = PlaneLocation;
                Main.aircraftMirage.transform.rotation = PlaneRotation;
                Debug.Log("OPSStartPatch 01");

                Debug.Log("OPSStartPatch 1");
                GameObject aircraftSeat = AircraftAPI.GetChildWithName(Main.aircraftMirage, "EjectorSeatLocation", false);
                f26EjectorSeat.transform.SetParent(aircraftSeat.transform);
                Debug.Log("OPSStartPatch 1.1");

                f26EjectorSeat.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                f26EjectorSeat.transform.localPosition = new Vector3(0f, 0f, 0f);
                Debug.Log("OPSStartPatch 1.2");
                Debug.Log("FOS threshold: " + Main.aircraftMirage.GetComponent<FloatingOriginShifter>().threshold);



                Main.aircraftMirage.SetActive(false);

                //FloatingOriginShifter floatingOriginShifter = Main.aircraftMirage.AddComponent<FloatingOriginShifter>();
                //floatingOriginShifter.rb = component;
                //floatingOriginShifter.threshold = 600f;
                FloatingOriginShifter floatingOriginShifter = Main.aircraftMirage.GetComponent<FloatingOriginShifter>();
                floatingOriginShifter.enabled = false;
                floatingOriginShifter.enabled = true;
                Debug.Log("OPSStartPatch 1.2.1");


                GameObject cameraEye = AircraftAPI.GetChildWithName(Main.aircraftMirage, "Camera (eye)", false);
                Debug.Log("OPSStartPatch 1.2.2");
                Camera cameraEyeCamera = cameraEye.GetComponent<Camera>();
                GameObject hudCanvas = AircraftAPI.GetChildWithName(Main.aircraftMirage, "HUDCanvas", false);
                Debug.Log("OPSStartPatch 1.2.3");
                Canvas hudCanvasCanvasComp = hudCanvas.GetComponent<Canvas>();
                hudCanvasCanvasComp.worldCamera = cameraEyeCamera;
                Debug.Log("OPSStartPatch 1.2.4");
                Transform cameraEyeTf = cameraEye.GetComponent<Transform>();
                GameObject elevationLadder = AircraftAPI.GetChildWithName(hudCanvas, "ElevationLadder", false);
                Debug.Log("OPSStartPatch 1.2.4.1");
                HUDElevationLadder elevationLadderComp = elevationLadder.GetComponent<HUDElevationLadder>();
                elevationLadderComp.headTransform = cameraEyeTf;
                Debug.Log("OPSStartPatch 1.2.4.2");
                GameObject sideStickObject = AircraftAPI.GetChildWithName(Main.aircraftMirage, "SideStickObjects", false);
                GameObject autoAdjust = AircraftAPI.GetChildWithName(sideStickObject, "AutoAdjust", false);
                GameObject autoAdjustCanvas = AircraftAPI.GetChildWithName(autoAdjust, "Canvas", false);
                Debug.Log("OPSStartPatch 1.2.4.3");

                Canvas autoAdjustCanvasCompCanvas = autoAdjustCanvas.GetComponent<Canvas>();
                autoAdjustCanvasCompCanvas.worldCamera = cameraEyeCamera;
                Debug.Log("OPSStartPatch 1.2.4.4");

                GameObject dashCanvas = AircraftAPI.GetChildWithName(Main.aircraftMirage, "DashCanvas", false);
                Canvas dashCanvasCompCanvas = dashCanvas.GetComponent<Canvas>();
                Debug.Log("OPSStartPatch 1.2.4.5");

                dashCanvasCompCanvas.worldCamera = cameraEyeCamera;
                GameObject spectatorCam = AircraftAPI.GetChildWithName(Main.aircraftMirage, "SpectatorCam", false);
                Debug.Log("OPSStartPatch 1.2.4.5");

                FlybyCameraMFDPage flybyCameraMFDPage = spectatorCam.GetComponent<FlybyCameraMFDPage>();
                AudioListener cameraEyeAL = cameraEye.GetComponent<AudioListener>();
                flybyCameraMFDPage.playerAudioListener = cameraEyeAL;
                Debug.Log("OPSStartPatch 1.2.4.5.1");

                AudioListenerPosition cameraEyeALP = cameraEye.GetComponent<AudioListenerPosition>();
                cameraEyeALP.rb = Main.aircraftMirage.GetComponent<Rigidbody>();


                Debug.Log("OPSStartPatch 1.2.4.6");


                cameraEye.GetComponent<AudioListenerPosition>().rb = Main.aircraftMirage.GetComponent<Rigidbody>();
                TargetingMFDPage componentInChildren8 = Main.aircraftMirage.GetComponentInChildren<TargetingMFDPage>(true);
                Debug.Log("OPSStartPatch 1.2.4.6");


                GameObject hqhGO = AircraftAPI.GetChildWithName(cameraEye, "hqh", false);
                HelmetController helmetController = hqhGO.GetComponent<HelmetController>();

                Debug.Log("OPSStartPatch 1.2.4.6");
                componentInChildren8.helmet = helmetController;
                GameObject glassMask = AircraftAPI.GetChildWithName(hudCanvas, "GlassMask", false);
                Debug.Log("OPSStartPatch 1.2.4.6.1");

                helmetController.hudPowerObject = glassMask;
                helmetController.hudMaskToggler = hudCanvas.GetComponent<HUDMaskToggler>();
                Debug.Log("OPSStartPatch 1.2.4.6.2");

                helmetController.battery = AircraftAPI.GetChildWithName(Main.aircraftMirage, "battery", false).GetComponent<Battery>();
                GameObject hmcsPowerInteractable = AircraftAPI.GetChildWithName(Main.aircraftMirage, "hmcsPowerInteractable", false);
                Debug.Log("OPSStartPatch 1.2.4.7");

                HUDWeaponInfo hudWeaponInfo = AircraftAPI.GetChildWithName(hudCanvas, "WeaponInfo", false).GetComponent<HUDWeaponInfo>();
                HMDWeaponInfo hmdWeaponInfo = AircraftAPI.GetChildWithName(cameraEye, "WeaponInfo", false).GetComponent<HMDWeaponInfo>();
                hmdWeaponInfo.weaponInfo = hudWeaponInfo;
                
                Debug.Log("OPSStartPatch 1.2.4.7.1");

                VRLever hmcsPowerInteractableVRL = hmcsPowerInteractable.GetComponent<VRLever>();
                hmcsPowerInteractableVRL.OnSetState.AddListener(helmetController.SetPower);

                GameObject visorButtonInteractable = AircraftAPI.GetChildWithName(Main.aircraftMirage, "visorButtonInteractable", false);
                Debug.Log("OPSStartPatch 1.2.4.8");

                VRInteractable visorButtonInteractableVRI = visorButtonInteractable.GetComponent<VRInteractable>();
                visorButtonInteractableVRI.OnInteract.AddListener(helmetController.ToggleVisor);
                Debug.Log("OPSStartPatch 1.2.4.9");



                Debug.Log("OPSStartPatch 1.2.5");
                WeaponManager wm = Main.aircraftMirage.GetComponent<WeaponManager>();
                //wm.SetOpticalTargeter(Main.aircraftMirage.GetComponentInChildren<OpticalTargeter>(true));
                Debug.Log("OPSStartPatch 1.2.6");
                Main.aircraftMirage.SetActive(true);
                //componentInChildren8.SetOpticalTargeter();
                GameObject blackOutParent = AircraftAPI.GetChildWithName(Main.aircraftMirage, "blackoutParent", false);
                blackOutParent.SetActive(false);
                Debug.Log("OPSStartPatch 1.3");

                GameObject screenFader = AircraftAPI.GetChildWithName(Main.aircraftMirage, "ScreenFader", false);
                screenFader.SetActive(false);

                Rigidbody ejectionSeatRB = f26EjectorSeat.GetComponent<Rigidbody>();
                EjectionSeat ejectionSeatES = f26EjectorSeat.GetComponent<EjectionSeat>();
                SeatAdjuster ejectionSeatSA = f26EjectorSeat.GetComponent<SeatAdjuster>();
                Debug.Log("OPSStartPatch 1.4");

                flybyCameraMFDPage.seatRb = ejectionSeatRB;
                Debug.Log("OPSStartPatch 1.5");

                ShipController mirageSC = Main.aircraftMirage.GetComponent<ShipController>();
                mirageSC.ejectionSeat = ejectionSeatES;
                Debug.Log("OPSStartPatch 1.6");

                VRInteractable mirageLowerSeatInter = AircraftAPI.GetChildWithName(dashCanvas, "lowerSeatInter", false).GetComponent<VRInteractable>();
                mirageLowerSeatInter.OnInteract.AddListener(ejectionSeatSA.StartLowerSeat);
                mirageLowerSeatInter.OnStopInteract.AddListener(ejectionSeatSA.Stop);

                Debug.Log("OPSStartPatch 1.7");

                VRInteractable mirageRaiseSeatInter = AircraftAPI.GetChildWithName(dashCanvas, "raiseSeatInter", false).GetComponent<VRInteractable>();
                mirageRaiseSeatInter.OnInteract.AddListener(ejectionSeatSA.StartRaiseSeat);
                mirageRaiseSeatInter.OnStopInteract.AddListener(ejectionSeatSA.Stop);
                Debug.Log("OPSStartPatch 1.8");

                GameObject intFixedCamSeat = AircraftAPI.GetChildWithName(f26EjectorSeat, "int_fixedCam_Cockpit3", false);
                flybyCameraMFDPage.fixedTransforms[5] = intFixedCamSeat.transform;
                Debug.Log("OPSStartPatch 1.9");

                GameObject riggedSuit2 = AircraftAPI.GetChildWithName(f26EjectorSeat, "RiggedSuit (2)", false);
                TempPilotDetacher tempPilotDetacherComp = AircraftAPI.GetChildWithName(Main.aircraftMirage, "TempPilotDetacher", false).GetComponent<TempPilotDetacher>();
                tempPilotDetacherComp.pilotModel = riggedSuit2;
                Debug.Log("OPSStartPatch 1.10");

                PlayerVehicleSetup miragePlayerVS = Main.aircraftMirage.GetComponent<PlayerVehicleSetup>();
                miragePlayerVS.hideObjectsOnConfig[2] = riggedSuit2;
                Debug.Log("OPSStartPatch 1.11");

                VehicleInputManager miragePlayerVI = Main.aircraftMirage.GetComponent<VehicleInputManager>();
                miragePlayerVI.pyrOutputs[2] = riggedSuit2.GetComponent<RudderFootAnimator>();
                Debug.Log("OPSStartPatch 1.12");

                PilotColorSetup pilotColorSetupComp = AircraftAPI.GetChildWithName(Main.aircraftMirage, "PilotColorApplier", false).GetComponent<PilotColorSetup>();
                SkinnedMeshRenderer riggedSuit001SMComp = AircraftAPI.GetChildWithName(f26EjectorSeat, "RiggedSuit.001", false).GetComponent<SkinnedMeshRenderer>();
                pilotColorSetupComp.pilotRenderers[3] = riggedSuit001SMComp;
                Debug.Log("OPSStartPatch 1.13");

                GameObject cameraRigParent = AircraftAPI.GetChildWithName(f26EjectorSeat, "CameraRigParent", false);
                miragePlayerVS.hideObjectsOnConfig[0] = cameraRigParent;
                Debug.Log("OPSStartPatch 1.14");

                GameObject cameraRigGO = AircraftAPI.GetChildWithName(f26EjectorSeat, "[CameraRig]", false);
                tempPilotDetacherComp.cameraRig = cameraRigGO;
                Debug.Log("OPSStartPatch 1.15");

                mirageSC.cameraRig = cameraRigGO;
                Debug.Log("OPSStartPatch 1.16");

                GameObject playerGTrans = AircraftAPI.GetChildWithName(cameraRigGO, "PlayerGTransform", false);
                FlightInfo mirageFlightInfo = Main.aircraftMirage.GetComponent<FlightInfo>();
                mirageFlightInfo.playerGTransform = playerGTrans.transform;
                Debug.Log("OPSStartPatch 1.17");

                GameObject controllerLeft = AircraftAPI.GetChildWithName(cameraRigParent, "Controller (left)", false);
                GameObject swatglowerLeft = AircraftAPI.GetChildWithName(controllerLeft, "SWAT_glower_pivot.002", false);
                GameObject postDeathLookTgt = AircraftAPI.GetChildWithName(controllerLeft, "postDeathLookTgt", false);
                LookRotationReference leftForearmLRTf = AircraftAPI.GetChildWithName(controllerLeft, "forearmLook", false).GetComponent<LookRotationReference>();


                pilotColorSetupComp.pilotRenderers[1] = swatglowerLeft.GetComponent<SkinnedMeshRenderer>();
                Debug.Log("OPSStartPatch 1.18");

                GameObject controllerRight = AircraftAPI.GetChildWithName(cameraRigParent, "Controller (right)", false);
                GameObject swatglowerRight = AircraftAPI.GetChildWithName(controllerRight, "SWAT_glower_pivot.002", false);

                pilotColorSetupComp.pilotRenderers[2] = swatglowerRight.GetComponent<SkinnedMeshRenderer>();
                Debug.Log("OPSStartPatch 1.19");

                MaterialSwitcher matSwitchLeft = swatglowerLeft.GetComponent<MaterialSwitcher>();
                tempPilotDetacherComp.OnDetachPilot.AddListener(matSwitchLeft.SwitchToB);
                MaterialSwitcher matSwitchRight = swatglowerRight.GetComponent<MaterialSwitcher>();
                tempPilotDetacherComp.OnDetachPilot.AddListener(matSwitchRight.SwitchToB);




                Debug.Log("OPSStartPatch 1.20");

                miragePlayerVS.OnBeginRearming.AddListener(matSwitchLeft.SwitchToB);
                miragePlayerVS.OnBeginRearming.AddListener(matSwitchRight.SwitchToB);
                miragePlayerVS.OnEndRearming.AddListener(matSwitchLeft.SwitchToB);
                miragePlayerVS.OnEndRearming.AddListener(matSwitchRight.SwitchToB);
                Debug.Log("OPSStartPatch 1.21");

                GameObject cameraEyeHelmet = AircraftAPI.GetChildWithName(cameraRigParent, "Camera (eye) Helmet", false);
                Debug.Log("OPSStartPatch 1.21.1");

                ScreenMaskedColorRamp screenMaskedClrRamp = cameraEyeHelmet.GetComponent<ScreenMaskedColorRamp>();
                Debug.Log("OPSStartPatch 1.21.2");

                flybyCameraMFDPage.playerNVG = screenMaskedClrRamp;
                Debug.Log("OPSStartPatch 1.22");

                GameObject headSphere = AircraftAPI.GetChildWithName(cameraRigParent, "headSphere", false);
                pilotColorSetupComp.pilotRenderers[0] = headSphere.GetComponent<MeshRenderer>();
                Debug.Log("OPSStartPatch 1.23");

                GameObject hqh = AircraftAPI.GetChildWithName(cameraRigParent, "hqh", false);
                HelmetController hqhHCComp = hqh.GetComponent<HelmetController>();
                componentInChildren8.helmet = hqhHCComp;
                Debug.Log("OPSStartPatch 1.24");

                GameObject nvgBIVL = AircraftAPI.GetChildWithName(Main.aircraftMirage, "nvgButtonInteractable", false);
                VRInteractable nvgBIVLVRL = nvgBIVL.GetComponent<VRInteractable>();
                nvgBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleNVG);
                Debug.Log("OPSStartPatch 1.25");

                GameObject visorBIVL = AircraftAPI.GetChildWithName(Main.aircraftMirage, "visorButtonInteractable", false);
                VRInteractable visorBIVLVRL = visorBIVL.GetComponent<VRInteractable>();
                visorBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleVisor);
                Debug.Log("OPSStartPatch 1.26");

                GameObject cockpitWindNoise = AircraftAPI.GetChildWithName(f26EjectorSeat, "cockpitWindNoise", false);
                CockpitWindAudioController cockpitWindNoiseAC = cockpitWindNoise.GetComponent<CockpitWindAudioController>();
                cockpitWindNoiseAC.flightInfo = mirageFlightInfo;

            }
            return false;
        }
    }


    [HarmonyPatch(typeof(TargetingMFDPage), "CloseOut")]
    public class TargetingMFDPagePatch
    {
        public static bool Prefix(TargetingMFDPage __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            HelmetController helmet = Main.aircraftMirage.GetComponentInChildren<HelmetController>(true);
            __instance.helmet = helmet;
            if (__instance.targetingCamera)
            {
                Debug.Log("Part1TMFD");
                __instance.targetingCamera.enabled = false;
                Debug.Log("Part1TMFD2");

            }
            if (__instance.helmet.tgpDisplayEnabled)
            {
                Debug.Log("Part2TMFD");
                __instance.ToggleHelmetDisplay();
                Debug.Log("Part2TMFD2");

            }
            if (__instance.tgpMode == TargetingMFDPage.TGPModes.HEAD)
            {
                Debug.Log("Part3TMFD");
                __instance.MFDHeadButton();
                Debug.Log("Part3TMFD2");

            }
            return false;
        }
    }

    [HarmonyPatch(typeof(WeaponManager), nameof(WeaponManager.Awake))]
    class PlayerSpawnAwakePatch
    {
        private static GameObject SeatLocation;
        private static GameObject FA26Aircraft;
        private static Vector3 PlaneLocation;
        private static GameObject Newposition;
        private static GameObject aircraft;
        private static GameObject aircraftSeat;
        private static VTOLScenes scene;

        public static void Prefix(WeaponManager __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            FlightLogger.Log("Awake prefix ran in wm!");
            bool mpCheck = true;

            Debug.Log("Weapon Manager for : " + __instance.vm);


            if (MpPlugin.MPActive)
            {
                mpCheck = Main.instance.plugin.CheckPlaneSelected();

            }

            if (mpCheck && __instance.gameObject.GetComponentInChildren<PlayerFlightLogger>() && VTOLAPI.GetPlayersVehicleEnum() == VTOLVehicles.FA26B && AircraftInfo.AircraftSelected)
            {

                Main.playerGameObject = __instance.gameObject;
                FA26Aircraft = AircraftAPI.GetChildWithName(Main.playerGameObject, "FA-26B", false);

                Debug.Log("0");
                if (!FA26Aircraft) { return; }
                /*
                Debug.Log("01");
                GameObject f26EjectorSeat = AircraftAPI.GetChildWithName(Main.playerGameObject, "EjectorSeat", false);
                f26EjectorSeat.transform.SetParent(Main.playerGameObject.transform);
*/
                Debug.Log("001");
                //PlaneLocation = Main.playerGameObject.transform.position;
                //Quaternion PlaneRotation = Main.playerGameObject.transform.rotation;

                Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);

                UnityEngine.Object.Destroy(FA26Aircraft);

                __instance.SetOpticalTargeter(Main.aircraftMirage.GetComponentInChildren<OpticalTargeter>(true));
                /*  f26EjectorSeat.SetActive(true);
                 

                  Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);
                  if (Main.aircraftMirage) { Debug.Log("Already here"); }
                  else
                  {
                      Debug.Log("New Spawn");
                      //Main.aircraftMirage = GameObject.Instantiate(Main.aircraftPrefab, PlaneLocation, PlaneRotation);
                  }
                  //Main.aircraftMirage = Main.aircraftPrefab;

                  Debug.Log("00");

                  Main.aircraftMirage.transform.position = PlaneLocation;
                  Main.aircraftMirage.transform.localPosition = new Vector3(0, -2.66f, 0);
                  Main.aircraftMirage.transform.rotation = PlaneRotation;
                  Debug.Log("01");
                  /*
                  Main.playerGameObject = Main.aircraftMirage;



                  Debug.Log("1");
                  aircraftSeat = AircraftAPI.GetChildWithName(Main.aircraftMirage, "EjectorSeatLocation", false);
                  f26EjectorSeat.transform.SetParent(aircraftSeat.transform);
                  f26EjectorSeat.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                  f26EjectorSeat.transform.localPosition = new Vector3(0f, 0f, 0f);
                  Debug.Log("2");




                  ///REMOVE THIS LINE ITS TO TEST WHAT IS UNDER THE PLAYEROBJECT


                  AircraftSetup.Fa26 = Main.playerGameObject;
                  AircraftSetup.customAircraft = Main.aircraftMirage;



                  //Changes depth and scale of the hud to make it legible
                  //               AircraftSetup.SetUpHud();


                  //Fixes the weird shifting nav map bug. Must be called after unity mover
                  //             AircraftSetup.ScaleNavMap();





                  Debug.Log("Disabling mesh");

              */

            }
        }

        public static void Postfix(WeaponManager __instance)
        {
            Debug.Log("pf1");



        }
    }



    [HarmonyPatch(typeof(TargetingMFDPage), "Setup")]
    public class TMFDStartPatch
    {
        public static bool Prefix(TargetingMFDPage __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("TargetingMFDPage Setup 1.0 : " + __instance.gameObject.transform.parent.transform.parent.transform.parent);

            Traverse traverseT1 = Traverse.Create(__instance);
            bool startedTMFD = (bool)traverseT1.Field("started").GetValue();

            Debug.Log("TargetingMFDPage Setup 1.1");

            if (startedTMFD)
            {
                Debug.Log("TargetingMFDPage Setup 1.2");

                return false;
            }
            Debug.Log("TargetingMFDPage Setup 1.3");

            startedTMFD = true;
            traverseT1.Field("started").SetValue(true);


            WeaponManager TMFDwm = (WeaponManager)traverseT1.Field("wm").GetValue();


            Debug.Log("TargetingMFDPage Setup 1.4 +" + TMFDwm);

            if (TMFDwm.opticalTargeter)
            {
                Debug.Log("TargetingMFDPage Setup 1.5");

                if (!__instance.targetingCamera)
                {
                    Debug.Log("TargetingMFDPage Setup 1.6");

                    __instance.targetingCamera = TMFDwm.opticalTargeter.cameraTransform.GetComponent<Camera>();
                }
                if (!__instance.opticalTargeter)
                {
                    Debug.Log("TargetingMFDPage Setup 1.7");

                    __instance.opticalTargeter = TMFDwm.opticalTargeter;
                }
                if (__instance.targetingCamera)
                {
                    Debug.Log("TargetingMFDPage Setup 1.8");

                    LODManager.instance.tcam = __instance.targetingCamera;
                }
            }
            string[] tgpModeLabelsT1 = (string[])traverseT1.Field("tgpModeLabels").GetValue();
            Debug.Log("TargetingMFDPage Setup 1.9");

            if (__instance.mfdPage)
            {
                Debug.Log("TargetingMFDPage Setup 1.10");


                __instance.mfdPage.SetText("tgpMode", tgpModeLabelsT1[(int)__instance.tgpMode]);
            }
            else if (__instance.portalPage)
            {
                Debug.Log("TargetingMFDPage Setup 1.11");

                __instance.portalPage.SetText("tgpMode", tgpModeLabelsT1[(int)__instance.tgpMode]);
            }
            traverseT1.Field("gpsSystem").SetValue(TMFDwm.gpsSystem);
            if (__instance.limitLineRenderer)
            {
                Debug.Log("TargetingMFDPage Setup 1.12");

                __instance.SetupLimitLine();
            }
            Debug.Log("TargetingMFDPage Setup 1.13");

            __instance.UpdateLimLineVisibility();
            Debug.Log("TargetingMFDPage Setup");
            return false;
        }
    }


    [HarmonyPatch(typeof(FlybyCameraMFDPage), "EnableCamera")]
    public static class SCamPatch
    {
        public static void Prefix(FlybyCameraMFDPage __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Traverse traverseF1 = Traverse.Create(__instance);
            Debug.Log("Scamepatch 1.0");
            bool flyCamEnabledFBCMFD = (bool)traverseF1.Field("flyCamEnabled").GetValue();
            bool previewEnabledFBCMFD = (bool)traverseF1.Field("previewEnabled").GetValue();
            Debug.Log("Scamepatch 1.1");

            if (flyCamEnabledFBCMFD)
            {
                Debug.Log("Scamepatch 1.2");
                __instance.SetupFlybyPosition((FlybyCameraMFDPage.SpectatorBehaviors)(-1));
                return;
            }
            Debug.Log("Scamepatch 1.3");
            if (previewEnabledFBCMFD)
            {
                Debug.Log("Scamepatch 1.3.1");
                __instance.previewObject.SetActive(true);
            }
            Debug.Log("Scamepatch 1.4");
            traverseF1.Field("flyCamEnabled").SetValue(true);

            flyCamEnabledFBCMFD = true;
            Debug.Log("Scamepatch 1.5");
            traverseF1.Field("cameraStartTime").SetValue(Time.time);
            __instance.flybyCam.gameObject.SetActive(true);
            __instance.flybyCam.transform.parent = null;
            Debug.Log("Scamepatch 1.6");
            __instance.playerAudioListener.enabled = !__instance.cameraAudio;
            __instance.cameraAudioListener.enabled = __instance.cameraAudio;
            FloatingOrigin.instance.OnOriginShift += __instance.OnCamOriginShift;
            __instance.SetupFlybyPosition((FlybyCameraMFDPage.SpectatorBehaviors)(-1));
        }
    }

    [HarmonyPatch(typeof(FlybyCameraMFDPage.SCamNVGController), "OnPreCull")]
    public static class PatchNVGPreCull
    {
        public static bool Prefix(FlybyCameraMFDPage.SCamNVGController __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Traverse traverse1 = Traverse.Create(__instance);
            Debug.Log("PatchNVGPreCull 1.0");

            if (!__instance.specCamNVG) { return false; }
            Debug.Log("PatchNVGPreCull 1.0.1");
            if (__instance.specCamNVG.enabled)
            {
                Debug.Log("PatchNVGPreCull 1.1");
                if (__instance.doIllum)
                {
                    Debug.Log("PatchNVGPreCull 1.2");
                    traverse1.Field("illumEnabled").SetValue(true);
                    //__instance.illumEnabled = true;
                    Debug.Log("PatchNVGPreCull 1.3");
                    __instance.nvg.EnableIlluminator();
                    Debug.Log("PatchNVGPreCull 1.4");

                    return false;
                }
                return false;
            }
            else
            {
                Debug.Log("PatchNVGPreCull 1.5");

                traverse1.Field("hidNvg").SetValue(true);
                //__instance.hidNvg = true;
                Debug.Log("PatchNVGPreCull 1.6");

                object nvgScaleIDPull = traverse1.Field("nvgScaleID").GetValue();
                Shader.SetGlobalFloat(nvgScaleIDPull.ToString(), 0f);
                Debug.Log("PatchNVGPreCull 1.7");
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(HPEquipGun), "Shake")]
    public static class PatchHPEquipGun
    {
        public static bool Prefix(HPEquipGun __instance)
        {

            //__instance.shaker.Shake(UnityEngine.Random.onUnitSphere * __instance.shakeMagnitude);
            //FlybyCameraMFDPage.ShakeSpectatorCamera(30f * __instance.shakeMagnitude / (FlybyCameraMFDPage.instance.flybyCam.transform.position - __instance.GetFireTransform().position).sqrMagnitude);
            return false;

        }
    }

    [HarmonyPatch(typeof(Radar), "ProcessUnit")]
    public static class PatchRadarProcessingForGroundAttack
    {
        public static RaycastHit raycastHit;

        public static bool Prefix(Radar __instance, Actor a, float dotThresh, bool hasMapGen)
        {


            if (!a || !a.gameObject.activeSelf || a.name == "Enemy Infantry MANPADS" || a.name == "Enemy Infantry" || a.name == "Allied Infantry MANPADS" || a.name == "Allied Infantry")
            {
                return false;
            }
            if (a.finalCombatRole == Actor.Roles.Air || a.role == Actor.Roles.GroundArmor || a.role == Actor.Roles.Ground)
            {
                Debug.Log("Simon found: " + a.actorName);
                if (!__instance.detectAircraft)
                {
                    return false;

                }

            }
            else if (a.role == Actor.Roles.Missile)
            {
                if (!__instance.detectMissiles)
                {
                    return false;
                }
            }
            else
            {
                if (a.finalCombatRole != Actor.Roles.Ship)
                {
                    return false;
                }
                if (!__instance.detectShips)
                {
                    return false;
                }
            }
            if (!a.alive)
            {
                return false;
            }
            Vector3 position = a.position;
            float sqrMagnitude = (position - __instance.rotationTransform.position).sqrMagnitude;



            if (sqrMagnitude >= 150000 && !Radar.ADV_RADAR)
            {
                return false;
            }
            Vector3 vector = __instance.rotationTransform.InverseTransformPoint(position);
            vector.y = 0f;
            if (Vector3.Dot(vector.normalized, Vector3.forward) < dotThresh)
            {
                return false;
            }
            Quaternion localRotation = __instance.rotationTransform.localRotation;
            float y = VectorUtils.SignedAngle(__instance.rotationTransform.parent.forward, Vector3.ProjectOnPlane(position - __instance.rotationTransform.position, __instance.rotationTransform.parent.up), __instance.rotationTransform.right);
            __instance.rotationTransform.localRotation = Quaternion.Euler(0f, y, 0f);
            if (Vector3.Dot((position - __instance.radarTransform.position).normalized, __instance.radarTransform.forward) > 0.32)
            {
                Traverse traverseT1 = Traverse.Create(__instance);
                bool myChunkColliderEnabledPatched = (bool)traverseT1.Field("myChunkColliderEnabled").GetValue();


                bool flag = !hasMapGen || VTMapGenerator.fetch.IsChunkColliderEnabled(a.position);
                //RaycastHit raycastHit;
                if (myChunkColliderEnabledPatched && Physics.Linecast(__instance.radarTransform.position, position, out raycastHit, 1) && (raycastHit.point - position).sqrMagnitude > 10000f)
                {
                    __instance.rotationTransform.localRotation = localRotation;
                    return false;
                }
                if (flag && Physics.Linecast(position, __instance.radarTransform.position, out raycastHit, 1) && (raycastHit.point - __instance.radarTransform.position).sqrMagnitude > 10000f)
                {
                    Hitbox component = raycastHit.collider.GetComponent<Hitbox>();
                    if (!component || component.actor != a)
                    {
                        __instance.rotationTransform.localRotation = localRotation;
                        return false;
                    }


                }
                if (hasMapGen && (!myChunkColliderEnabledPatched || !flag))
                {
                    __instance.StartCoroutine(__instance.HeightmapOccludeCheck(a));
                    __instance.rotationTransform.localRotation = localRotation;
                    return false;
                }
                Radar.SendRadarDetectEvent(a, __instance.myActor, __instance.radarSymbol, __instance.detectionPersistanceTime, __instance.rotationTransform.position, __instance.transmissionStrength);
                if (Radar.ADV_RADAR)
                {
                    float radarSignalStrength = Radar.GetRadarSignalStrength(__instance.radarTransform.position, a);
                    float num = __instance.transmissionStrength * radarSignalStrength / sqrMagnitude;
                    if (num < 1f / __instance.receiverSensitivity)
                    {

                        __instance.rotationTransform.localRotation = localRotation;
                        return false;
                    }


                }
                __instance.DetectActor(a);
            }
            __instance.rotationTransform.localRotation = localRotation;
            return false;

        }
    }
    [HarmonyPatch(typeof(HUDMaskToggler), "SetMask")]
    public static class HUDMaskTogglePatch
    {

        public static bool Prefix(bool maskEnabled, HUDMaskToggler __instance)
        {
            Debug.Log("SetMask 1.0");
            if (__instance.alwaysFPVOnly && maskEnabled)
            {
                Debug.Log("SetMask 1.1");
                return false;
            }
            Debug.Log("SetMask 1.2");
            Traverse traverse = Traverse.Create(__instance);
            Transform[] displayObjectsP = (Transform[])traverse.Field("displayObjects").GetValue();
            GameObject hudCanvas = CustomAircraftTemplate.AircraftAPI.GetChildWithName(Main.aircraftMirage, "HUDCanvas", true);
            Transform[] displayObjectsHUD = hudCanvas.GetComponentsInChildren<Transform>(true);


            Debug.Log("SetMask 1.3");

            for (int i = 0; i < displayObjectsHUD.Length; i++)
            {
                Debug.Log("SetMask 1.4");
                if (displayObjectsHUD[i])
                {
                    Debug.Log("SetMask 1.5");
                    if (__instance.alwaysFPVOnly)
                    {
                        Debug.Log("SetMask 1.6");
                        displayObjectsHUD[i].gameObject.layer = 28;
                    }
                    else
                    {
                        Debug.Log("SetMask 1.7");
                        displayObjectsHUD[i].gameObject.layer = (maskEnabled ? 5 : 28);
                    }
                }
            }
            traverse.Field("displayObjects").SetValue(displayObjectsP);
            Debug.Log("SetMask 1.8");
            for (int j = 0; j < __instance.masks.Length; j++)
            {
                Debug.Log("SetMask 1.9");
                if (__instance.masks[j])
                {
                    Debug.Log("SetMask 1.10");
                    __instance.masks[j].enabled = maskEnabled;
                }
            }
            for (int k = 0; k < __instance.images.Length; k++)
            {
                Debug.Log("SetMask 1.11");
                if (__instance.images[k])
                {
                    Debug.Log("SetMask 1.12");
                    __instance.images[k].enabled = maskEnabled;
                }
            }
            if (__instance.alwaysFPVOnly)
            {
                Debug.Log("SetMask 1.13");
                __instance.canvasObject.layer = 28;
            }
            else
            {
                Debug.Log("SetMask 1.14");
               hudCanvas.layer = (maskEnabled ? 5 : 28);
            }
            //__instance.isMasked = maskEnabled;
            traverse.Field("isMasked").SetValue("maskEnabled");
            Debug.Log("SetMask 1.15");
            return false;
        }
    }



    /*   [HarmonyPatch(typeof(PlayerVehicleSetup), "SetupOCCam")]
       public static class CloudOCPatch
       {


           public static void Postfix(PlayerVehicleSetup __instance, Camera cam)
           {
               OC.OverCloudCamera overCloudCamera = cam.gameObject.GetComponent<OC.OverCloudCamera>();
               overCloudCamera.downsampleFactor = OC.DownSampleFactor.Eight;
               overCloudCamera.renderAtmosphere = false;
               overCloudCamera.lightSampleCount = OC.SampleCount.Low;
               overCloudCamera.scatteringMaskSamples = OC.SampleCount.Low;
               overCloudCamera.renderScatteringMask = false;
               overCloudCamera.includeCascadedShadows = false;
           }
       }*/
}