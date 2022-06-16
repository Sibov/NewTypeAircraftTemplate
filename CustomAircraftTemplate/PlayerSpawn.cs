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
using ModLoader;
using VTOLVR.Multiplayer;
using VTNetworking;
using Steamworks;

namespace CustomAircraftTemplate
{


    [HarmonyPatch(typeof(MultiplayerSpawn), "SetupSpawnedVehicle")]
    public class SU35_MS_SetupSpawnedVehicle_Prefix
    {
        private static EjectionSeat prefabES;

        public static void Prefix(MultiplayerSpawn __instance, GameObject vehicleObj)
        {
            Main.i = 0;
            //Debug.Log("MSSSV 1.0");
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
            {
                return;
            }

            Main.aircraftLoaded = true;
            Main.aircraftCustom = vehicleObj;
            //Debug.Log("MSSSV 1.1");

            GameObject BONew = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutEffectNew", false);
            //Debug.Log("MSSSV 1.2");

            Main.BOQuad = AircraftAPI.GetChildWithName(BONew, "Quad", false);
            //Debug.Log("MSSSV 1.3");

            Rigidbody componentRB = Main.aircraftCustom.GetComponent<Rigidbody>();
            //Debug.Log("MSSSV 1.4");
            Vector3 position2 = new Vector3(200, 200.66f, 200);
            GameObject vehiclePrefab2 = VTResources.GetPlayerVehicle("F/A-26B").vehiclePrefab;

            GameObject f26seatholder = AircraftAPI.GetChildWithName(vehiclePrefab2, "EjectorSeat", false);
            //Debug.Log("MSSSV 1.5");

            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(f26seatholder, position2, Main.aircraftCustom.transform.rotation);
            //Debug.Log("MSSSV 1.6");

            gameObject2.transform.localScale = new Vector3(0.92f, 0.92f, 0.92f);
            gameObject2.transform.SetParent(Main.aircraftCustom.transform);
            //Debug.Log("MSSSV 1.6");
            gameObject2.SetActive(true);
            //Debug.Log("MSSSV 1.7");
            GameObject aircraftSeat = AircraftAPI.GetChildWithName(Main.aircraftCustom, "EjectorSeatLocation", false);
            gameObject2.transform.SetParent(aircraftSeat.transform);
            gameObject2.transform.SetParent(aircraftSeat.transform);
            //Debug.Log("MSSSV 1.8");

            gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
            //Debug.Log("MSSSV 1.9");
            Main.aircraftCustom.SetActive(false);


            //FloatingOriginShifter floatingOriginShifter = Main.aircraftCustom.GetComponent<FloatingOriginShifter>();

            ////Debug.Log("MSSSV 1.10");
            //floatingOriginShifter.enabled = false;
            //floatingOriginShifter.enabled = true;

            GameObject cameraEye = AircraftAPI.GetChildWithName(Main.aircraftCustom, "Camera (eye)", false);
            //Debug.Log("MSSSV 1.2.2");
            Camera cameraEyeCamera = cameraEye.GetComponent<Camera>();
            GameObject hudCanvas = AircraftAPI.GetChildWithName(Main.aircraftCustom, "HUDCanvas", false);
            //Debug.Log("MSSSV 1.2.3");
            Canvas hudCanvasCanvasComp = hudCanvas.GetComponent<Canvas>();
            hudCanvasCanvasComp.worldCamera = cameraEyeCamera;
            //Debug.Log("MSSSV 1.2.4");
            Transform cameraEyeTf = cameraEye.GetComponent<Transform>();
            GameObject elevationLadder = AircraftAPI.GetChildWithName(hudCanvas, "ElevationLadder", false);
            //Debug.Log("MSSSV 1.2.4.1");
            HUDElevationLadder elevationLadderComp = elevationLadder.GetComponent<HUDElevationLadder>();
            elevationLadderComp.headTransform = cameraEyeTf;
            //Debug.Log("MSSSV 1.2.4.2");
            GameObject sideStickObject = AircraftAPI.GetChildWithName(Main.aircraftCustom, "SideStickObjects", false);
            GameObject autoAdjust = AircraftAPI.GetChildWithName(sideStickObject, "AutoAdjust", false);
            GameObject autoAdjustCanvas = AircraftAPI.GetChildWithName(autoAdjust, "Canvas", false);
            //Debug.Log("MSSSV 1.2.4.3");

            Canvas autoAdjustCanvasCompCanvas = autoAdjustCanvas.GetComponent<Canvas>();
            autoAdjustCanvasCompCanvas.worldCamera = cameraEyeCamera;
            //Debug.Log("MSSSV 1.2.4.4");

            GameObject dashCanvas = AircraftAPI.GetChildWithName(Main.aircraftCustom, "DashCanvas", false);
            Canvas dashCanvasCompCanvas = dashCanvas.GetComponent<Canvas>();
            //Debug.Log("MSSSV 1.2.4.5");

            dashCanvasCompCanvas.worldCamera = cameraEyeCamera;
            GameObject spectatorCam = AircraftAPI.GetChildWithName(Main.aircraftCustom, "SpectatorCam", false);
            //Debug.Log("MSSSV 1.2.4.5");

            FlybyCameraMFDPage flybyCameraMFDPage = spectatorCam.GetComponent<FlybyCameraMFDPage>();
            AudioListener cameraEyeAL = cameraEye.GetComponent<AudioListener>();
            flybyCameraMFDPage.playerAudioListener = cameraEyeAL;
            //Debug.Log("MSSSV 1.2.4.5.1");

            AudioListenerPosition cameraEyeALP = cameraEye.GetComponent<AudioListenerPosition>();

            cameraEyeALP.SetParentRigidbody(Main.aircraftCustom.GetComponent<Rigidbody>());
            cameraEyeALP.rb = Main.aircraftCustom.GetComponent<Rigidbody>();


            //Debug.Log("MSSSV 1.2.4.6");


            //cameraEye.GetComponent<AudioListenerPosition>().rb = Main.aircraftCustom.GetComponent<Rigidbody>();
            TargetingMFDPage componentInChildren8 = Main.aircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);
            //Debug.Log("MSSSV 1.2.4.6");


            GameObject hqhGO = AircraftAPI.GetChildWithName(cameraEye, "hqh", false);
            HelmetController helmetController = hqhGO.GetComponent<HelmetController>();

            //Debug.Log("MSSSV 1.2.4.6");
            componentInChildren8.helmet = helmetController;
            GameObject glassMask = AircraftAPI.GetChildWithName(hudCanvas, "GlassMask", false);
            //Debug.Log("MSSSV 1.2.4.6.1");

            helmetController.hudPowerObject = glassMask;
            helmetController.hudMaskToggler = hudCanvas.GetComponent<HUDMaskToggler>();
            //Debug.Log("MSSSV 1.2.4.6.2");

            helmetController.battery = AircraftAPI.GetChildWithName(Main.aircraftCustom, "battery", false).GetComponent<Battery>();
            GameObject hmcsPowerInteractable = AircraftAPI.GetChildWithName(Main.aircraftCustom, "hmcsPowerInteractable", false);
            //Debug.Log("MSSSV 1.2.4.7");

            HUDWeaponInfo hudWeaponInfo = AircraftAPI.GetChildWithName(hudCanvas, "WeaponInfo", false).GetComponent<HUDWeaponInfo>();
            HMDWeaponInfo hmdWeaponInfo = AircraftAPI.GetChildWithName(cameraEye, "WeaponInfo", false).GetComponent<HMDWeaponInfo>();
            hmdWeaponInfo.weaponNameText = hudWeaponInfo.weaponNameText;
            hmdWeaponInfo.weaponCountText = hudWeaponInfo.ammoCountText;


            //Debug.Log("MSSSV 1.2.4.7.1");

            VRLever hmcsPowerInteractableVRL = hmcsPowerInteractable.GetComponent<VRLever>();
            hmcsPowerInteractableVRL.OnSetState.AddListener(helmetController.SetPower);

            GameObject visorButtonInteractable = AircraftAPI.GetChildWithName(Main.aircraftCustom, "visorButtonInteractable", false);
            //Debug.Log("MSSSV 1.2.4.8");

            VRInteractable visorButtonInteractableVRI = visorButtonInteractable.GetComponent<VRInteractable>();
            visorButtonInteractableVRI.OnInteract.AddListener(helmetController.ToggleVisor);
            //Debug.Log("MSSSV 1.2.4.9");



            //Debug.Log("MSSSV 1.2.5");
            WeaponManager wm = Main.aircraftCustom.GetComponent<WeaponManager>();
            //wm.SetOpticalTargeter(Main.aircraftCustom.GetComponentInChildren<OpticalTargeter>(true));
            //Debug.Log("MSSSV 1.2.6");
            Main.aircraftCustom.SetActive(true);
            //componentInChildren8.SetOpticalTargeter();
            GameObject blackOutParent = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutParent", true);
            blackOutParent.SetActive(false);
            GameObject blackOutEffect = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutEffect", true);
            blackOutParent.SetActive(false);
            blackOutEffect.SetActive(false);
            GameObject blackOutEffectNew = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutEffectNewParent", true);
            blackOutEffectNew.transform.SetParent(cameraEye.transform);



            //Debug.Log("MSSSV 1.3");

            GameObject screenFader = AircraftAPI.GetChildWithName(Main.aircraftCustom, "ScreenFader", false);
            screenFader.SetActive(false);

            Rigidbody ejectionSeatRB = gameObject2.GetComponent<Rigidbody>();
            EjectionSeat ejectionSeatES = gameObject2.GetComponent<EjectionSeat>();
            SeatAdjuster ejectionSeatSA = gameObject2.GetComponent<SeatAdjuster>();
            //Debug.Log("MSSSV 1.4");

            flybyCameraMFDPage.seatRb = ejectionSeatRB;
            //Debug.Log("MSSSV 1.5");

            ShipController CustomAircraftSC = Main.aircraftCustom.GetComponent<ShipController>();
            CustomAircraftSC.ejectionSeat = ejectionSeatES;
            //Debug.Log("MSSSV 1.6");

            //VRInteractable customLowerSeatInter = AircraftAPI.GetChildWithName(dashCanvas, "lowerSeatInter", false).GetComponent<VRInteractable>();
            //customLowerSeatInter.OnInteract.AddListener(ejectionSeatSA.StartLowerSeat);
            //customLowerSeatInter.OnStopInteract.AddListener(ejectionSeatSA.Stop);

            ////Debug.Log("MSSSV 1.7");

            //VRInteractable customRaiseSeatInter = AircraftAPI.GetChildWithName(dashCanvas, "raiseSeatInter", false).GetComponent<VRInteractable>();
            //customRaiseSeatInter.OnInteract.AddListener(ejectionSeatSA.StartRaiseSeat);
            //customRaiseSeatInter.OnStopInteract.AddListener(ejectionSeatSA.Stop);
            ////Debug.Log("MSSSV 1.8");

            GameObject intFixedCamSeat = AircraftAPI.GetChildWithName(gameObject2, "int_fixedCam_Cockpit3", false);
            flybyCameraMFDPage.fixedTransforms[5] = intFixedCamSeat.transform;
            //Debug.Log("MSSSV 1.9");

            GameObject riggedSuit2 = AircraftAPI.GetChildWithName(gameObject2, "RiggedSuit (2)", false);
            TempPilotDetacher tempPilotDetacherComp = AircraftAPI.GetChildWithName(Main.aircraftCustom, "TempPilotDetacher", false).GetComponent<TempPilotDetacher>();
            tempPilotDetacherComp.pilotModel = riggedSuit2;
            //Debug.Log("MSSSV 1.10");

            PlayerVehicleSetup customPlayerVS = Main.aircraftCustom.GetComponent<PlayerVehicleSetup>();
            customPlayerVS.hideObjectsOnConfig[2] = riggedSuit2;
            //Debug.Log("MSSSV 1.11");

            VehicleInputManager customPlayerVI = Main.aircraftCustom.GetComponent<VehicleInputManager>();
            customPlayerVI.pyrOutputs[1] = riggedSuit2.GetComponent<RudderFootAnimator>();
            //Debug.Log("MSSSV 1.12");

            PilotColorSetup pilotColorSetupComp = AircraftAPI.GetChildWithName(Main.aircraftCustom, "PilotColorApplier", false).GetComponent<PilotColorSetup>();
            SkinnedMeshRenderer riggedSuit001SMComp = AircraftAPI.GetChildWithName(gameObject2, "RiggedSuit.001", false).GetComponent<SkinnedMeshRenderer>();
            pilotColorSetupComp.pilotRenderers[3] = riggedSuit001SMComp;
            //Debug.Log("MSSSV 1.13");

            GameObject cameraRigParent = AircraftAPI.GetChildWithName(gameObject2, "CameraRigParent", false);
            customPlayerVS.hideObjectsOnConfig[0] = cameraRigParent;
            //Debug.Log("MSSSV 1.14");

            GameObject cameraRigGO = AircraftAPI.GetChildWithName(gameObject2, "[CameraRig]", false);
            tempPilotDetacherComp.cameraRig = cameraRigGO;
            //Debug.Log("MSSSV 1.15");

            CustomAircraftSC.cameraRig = cameraRigGO;
            //Debug.Log("MSSSV 1.16");

            GameObject playerGTrans = AircraftAPI.GetChildWithName(cameraRigGO, "PlayerGTransform", false);
            //Debug.Log("MSSSV 1.16.1");

            FlightInfo customFlightInfo = Main.aircraftCustom.GetComponent<FlightInfo>();
            //Debug.Log("MSSSV 1.16.2");

            //customFlightInfo.playerGTransform = playerGTrans.transform;
            //Debug.Log("MSSSV 1.17");

            GameObject controllerLeft = AircraftAPI.GetChildWithName(cameraRigParent, "Controller (left)", false);
            GameObject swatglowerLeft = AircraftAPI.GetChildWithName(controllerLeft, "SWAT_glower_pivot.002", false);
            GameObject postDeathLookTgt = AircraftAPI.GetChildWithName(controllerLeft, "postDeathLookTgt", false);
            LookRotationReference leftForearmLRTf = AircraftAPI.GetChildWithName(controllerLeft, "forearmLook", false).GetComponent<LookRotationReference>();


            pilotColorSetupComp.pilotRenderers[1] = swatglowerLeft.GetComponent<SkinnedMeshRenderer>();
            //Debug.Log("MSSSV 1.18");

            GameObject controllerRight = AircraftAPI.GetChildWithName(cameraRigParent, "Controller (right)", false);
            GameObject swatglowerRight = AircraftAPI.GetChildWithName(controllerRight, "SWAT_glower_pivot.002", false);

            pilotColorSetupComp.pilotRenderers[2] = swatglowerRight.GetComponent<SkinnedMeshRenderer>();
            //Debug.Log("MSSSV 1.19");

            MaterialSwitcher matSwitchLeft = swatglowerLeft.GetComponent<MaterialSwitcher>();
            tempPilotDetacherComp.OnDetachPilot.AddListener(matSwitchLeft.SwitchToB);
            MaterialSwitcher matSwitchRight = swatglowerRight.GetComponent<MaterialSwitcher>();
            tempPilotDetacherComp.OnDetachPilot.AddListener(matSwitchRight.SwitchToB);




            //Debug.Log("MSSSV 1.20");

            customPlayerVS.OnBeginRearming.AddListener(matSwitchLeft.SwitchToB);
            customPlayerVS.OnBeginRearming.AddListener(matSwitchRight.SwitchToB);
            customPlayerVS.OnEndRearming.AddListener(matSwitchLeft.SwitchToB);
            customPlayerVS.OnEndRearming.AddListener(matSwitchRight.SwitchToB);
            //Debug.Log("MSSSV 1.21");

            GameObject cameraEyeHelmet = AircraftAPI.GetChildWithName(cameraRigParent, "Camera (eye) Helmet", false);
            //Debug.Log("MSSSV 1.21.1");

            ScreenMaskedColorRamp screenMaskedClrRamp = cameraEyeHelmet.GetComponent<ScreenMaskedColorRamp>();
            //Debug.Log("MSSSV 1.21.2");

            flybyCameraMFDPage.playerNVG = screenMaskedClrRamp;
            //Debug.Log("MSSSV 1.22");

            GameObject headSphere = AircraftAPI.GetChildWithName(cameraRigParent, "headSphere", false);
            pilotColorSetupComp.pilotRenderers[0] = headSphere.GetComponent<MeshRenderer>();
            //Debug.Log("MSSSV 1.23");

            GameObject hqh = AircraftAPI.GetChildWithName(cameraRigParent, "hqh", false);
            HelmetController hqhHCComp = hqh.GetComponent<HelmetController>();
            componentInChildren8.helmet = hqhHCComp;
            //Debug.Log("MSSSV 1.24");

            GameObject nvgBIVL = AircraftAPI.GetChildWithName(Main.aircraftCustom, "nvgButtonInteractable", false);
            VRInteractable nvgBIVLVRL = nvgBIVL.GetComponent<VRInteractable>();
            nvgBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleNVG);
            //Debug.Log("MSSSV 1.25");

            GameObject visorBIVL = AircraftAPI.GetChildWithName(Main.aircraftCustom, "visorButtonInteractable", false);
            VRInteractable visorBIVLVRL = visorBIVL.GetComponent<VRInteractable>();
            visorBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleVisor);
            //Debug.Log("MSSSV 1.26");

            GameObject cockpitWindNoise = AircraftAPI.GetChildWithName(gameObject2, "cockpitWindNoise", false);
            CockpitWindAudioController cockpitWindNoiseAC = cockpitWindNoise.GetComponent<CockpitWindAudioController>();
            cockpitWindNoiseAC.flightInfo = customFlightInfo;

            GameObject HMCSDisplays = AircraftAPI.GetChildWithName(cameraEye, "HMCSDisplays", true);

            WeaponManager wem = Main.aircraftCustom.GetComponent<WeaponManager>();
            //Debug.Log("MSSSV 1.26.1 = " + wem.name);

            GameObject WeaponInfo = AircraftAPI.GetChildWithName(HMCSDisplays, "WeaponInfo", true);
            Main.HMCSAltText = AircraftAPI.GetChildWithName(HMCSDisplays, "Alt", true);


            WeaponInfo.GetComponent<HMDWeaponInfo>().wm = wem;

            Debug.Log("MSSSV 1.27");
            // MirageElements.SetUpGauges();
            //  MirageElements.IdentifiedRadarTargetsSetup();
            prefabES = aircraftSeat.GetComponent<EjectionSeat>();
            UnityEngine.Component.Destroy(prefabES);
             GameObject MPSyncs = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MPSyncs", false);
            Debug.Log("MSSSV 1.28");
            EjectSync ejectSyncs = MPSyncs.GetComponent<EjectSync>();
            Debug.Log("MSSSV 1.29");
            EjectionSeat ejectionSeat = f26seatholder.GetComponent<EjectionSeat>();
            Debug.Log("MSSSV 1.30");
            ejectSyncs.localEjector = ejectionSeat;
            return;
        }

    }

  

    [HarmonyPatch(typeof(WeaponManager), "ReattachWeapons")]
    public class SU35_ReattachWeapons_post
    {
        public static void Postfix(WeaponManager __instance)
        {
            //Debug.Log("MSSSV 1.27");
           // MirageElements.SetUpGauges();
            //Debug.Log("MSSSV 1.28");
          //  MirageElements.SetupArmingText();
            //Debug.Log("MSSSV 1.29");
        AircraftSetup.SetUpEOTS();
        }
    }
   


        [HarmonyPatch(typeof(MFDRadarUI), "Awake")]
    public class SU35_MFDUIAwakePatch
    {
        public static bool Prefix(MFDRadarUI __instance)
        {
          
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            //Debug.Log("Starting MDUIAwake 0.0");
            //Debug.unityLogger.logEnabled = Main.logging;
            //Debug.Log("Starting MDUIAwake 1.0");
            Traverse traversemui = Traverse.Create(__instance);
            traversemui.Field("isMultiCrew").SetValue(false);
            
            __instance.SetupPools();
            __instance.SetupDisplay();
            __instance.softLocks = new MFDRadarUI.UIRadarContact[__instance.softLockCount];
            __instance.radarCtrlr.OnElevationAdjusted += __instance.UpdateElevationText;
            if (__instance.playerRadar)
            {
                traversemui.Field("origScanRate").SetValue(__instance.playerRadar.rotationSpeed);
                __instance.lockingRadar.OnUnlocked += __instance.LockingRadar_OnUnlocked;
            }
            return false;
        }

    }

    [HarmonyPatch(typeof(MFDRadarUI), nameof(MFDRadarUI.SetPower))]
    public class SU35_MFDUISetPowerPatch
    {
        public static bool Prefix(MFDRadarUI __instance)
        {
            
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            //Debug.unityLogger.logEnabled = Main.logging;
            //Debug.Log("Starting MDUISP 1.0");
            return true;
        }

    }

    [HarmonyPatch(typeof(BlackoutEffect), nameof(BlackoutEffect.LateUpdate))]
    public class SU35_BlackoutPFPatch
    {
        public static void Postfix(BlackoutEffect __instance)
        {
           
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            //Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("Starting BOFPatch 1.0");
            Traverse traverse1 = Traverse.Create(__instance);
            Single newgAccum = (Single)traverse1.Field("gAccum").GetValue();
            float num = Mathf.Abs(newgAccum) * __instance.aFactor;
            Debug.Log("Starting BOFPatch 1.0.1");

            Single newAlpha = Main.currentGAlpha;
            Debug.Log("Starting BOFPatch 1.0.2");

            NightVisionGoggles newNVG = (NightVisionGoggles)traverse1.Field("nvg").GetValue();
            Debug.Log("Starting BOFPatch 1.0.3");

            newAlpha = Mathf.Lerp(newAlpha, num, 20f * Time.deltaTime);
            Debug.Log("Starting BOFPatch 1.0.4");
            Main.currentGAlpha = newAlpha;

            Color color = (newNVG && newNVG.IsNVGVisible()) ? __instance.nvgRedoutColor : __instance.redoutColor;
            Debug.Log("Starting BOFPatch 1.0.5");

            color *= RenderSettings.ambientIntensity;
            Debug.Log("Starting BOFPatch 1.0.6");

            Color color2 = (newgAccum >= 0f) ? Color.black : color;
            Debug.Log("Starting BOFPatch 1.0.7");

            color2.a = newAlpha * newAlpha;
            GameObject BONew = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutEffectNew", false);
            Debug.Log("Starting BOFPatch 1.0.7.1");
            Main.BOQuad = AircraftAPI.GetChildWithName(BONew, "Quad", false);

            Debug.Log("Starting BOFPatch 1.0.8");
            MeshRenderer BOMesh = Main.BOQuad.GetComponent<MeshRenderer>();
            Debug.Log("Starting BOFPatch 1.0.8.1");
            Material BOMat = BOMesh.material;
            Debug.Log("Starting BOFPatch 1.0.9");
            Color BOMatColor = BOMat.color;
            Debug.Log("Starting BOFPatch 1.0.0: Alpha = " + Main.currentGAlpha + "GAccum: " + newgAccum);

            BOMatColor.a = newAlpha;
            BOMat.color = new Color(0,0,0,newAlpha);
        }

    }

    [HarmonyPatch(typeof(BlackoutEffect), "OnDestroy")]
    class SU35_BOFCheck
    {
        public static bool Prefix(BlackoutEffect __instance)
        {
            if (!Main.aircraftLoaded)
            { return true; }
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            if (!__instance.audioMixer) { return false; }

            //Debug.Log("BOFDestroy 1.0");
            __instance.audioMixer.SetFloat("ConsciousVolume", 0f);

            //Debug.Log("BOFDestroy 1.1");

            __instance.audioMixer.SetFloat("BlackoutVolume", -80f);
            //Debug.Log("BOFDestroy 1.2");

            Traverse trav = Traverse.Create(__instance);
            trav.Field("destroyed").SetValue(true);

            return false;
        }
    }

    [HarmonyPatch(typeof(BlackoutEffect), "OnDisable")]
    class SU35_BOFDisCheck
    {
        public static bool Prefix(BlackoutEffect __instance)
        {
            if (!Main.aircraftLoaded)
            { return true; }
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            if (!__instance.audioMixer) { return false; }
            //Debug.Log("BOFDisable 1.0");
            __instance.audioMixer.SetFloat("ConsciousVolume", 0f);

            //Debug.Log("BOFDisable 1.1");

            __instance.audioMixer.SetFloat("BlackoutVolume", -80f);
            //Debug.Log("BOFDisable 1.2");
            return false;
        }
    }


    [HarmonyPatch(typeof(MFDRadarUI), nameof(MFDRadarUI.ClearSoftLocks))]
    public class SU35_ClearSoftLocksPatch
    {
        public static bool Prefix(MFDRadarUI __instance)
        {
            if (!Main.aircraftLoaded)
            { return true; }
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            //Debug.unityLogger.logEnabled = Main.logging;
            //Debug.Log("Starting ClrSftLk 1.0");

            for (int i = 0; i < __instance.softLockCount; i++)
            {
                //Debug.Log("Starting ClrSftLk 1.1: i=" + i );
                if (__instance.softLocks[i] != null)
                {
                    //Debug.Log("Starting ClrSftLk 1.2: i=" + i);
                    if (__instance.softLocks[i].actor && __instance.lockingRadar)
                    {
                        //Debug.Log("Starting ClrSftLk 1.3: i=" + i);
                        __instance.lockingRadar.RemoveTWSLock(__instance.softLocks[i].actor);
                    }
                    Traverse traversescl = Traverse.Create(__instance);
                    MFDRadarUI.UIRadarContact hardlockcheck = (MFDRadarUI.UIRadarContact)traversescl.Field("hardLock").GetValue();
                    //Debug.Log("Starting ClrSftLk 1.4: i=" + i);
                    if (hardlockcheck == null || __instance.softLocks[i].actorID != hardlockcheck.actorID)
                    {
                        //Debug.Log("Starting ClrSftLk 1.5: i=" + i);
                        __instance.softLocks[i] = null;
                    }
                }
            }
            //Debug.Log("Starting ClrSftLk 1.6: i=");
            __instance.UpdateLocks();
            return false;
        }
        
    }


    [HarmonyPatch(typeof(PlayerSpawn), "OnPreSpawnUnit")]
    public class SU35_OPSStartPatch
    {
        public static bool Prefix(PlayerSpawn __instance)
        {
           
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            Debug.Log("Starting OPS 1");
            Debug.unityLogger.logEnabled = Main.logging;
            // __instance.OnPreSpawnUnit();
           
            Debug.Log("Starting OPS");


            GameObject vehiclePrefab = Main.aircraftPrefab;
            if (vehiclePrefab)
            {
                Debug.Log("Starting OPS Inst");

                Debug.Log("AircraftSelected = " + AircraftInfo.AircraftSelected);


                Debug.Log("OPS 0.00.0.1");
                Main.i = 0;

                Vector3 position = new Vector3(0, 1.1f, 0);
                Quaternion rotation = new Quaternion(0, 0, 0, 0);

                Debug.Log("OPS 0.00.0.2");
                Main.aircraftCustom = UnityEngine.Object.Instantiate<GameObject>(vehiclePrefab, position, rotation);
                Main.aircraftLoaded = true;
                
                Debug.Log("OPS 0.00.0.3");

                GameObject BONew = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutEffectNew", false);
                Debug.Log("OPS 0.00.0.4");

                Main.BOQuad = AircraftAPI.GetChildWithName(BONew, "Quad", false);
                Debug.Log("OPS 0.00.0.5");

                Rigidbody component = Main.aircraftCustom.GetComponent<Rigidbody>();
                Debug.Log("OPS 0.00.0.6");

                Traverse traverse = Traverse.Create(__instance);
                traverse.Field("vehicleRb").SetValue(Main.aircraftCustom.GetComponent<Rigidbody>());
                Debug.Log("OPS 0.00.0.7");

                traverse.Field("playerVm").SetValue(Main.aircraftCustom.GetComponent<VehicleMaster>());
                traverse.Field("vehicleRb").Field("interpolation").SetValue(RigidbodyInterpolation.None);


                Vector3 position2 = new Vector3(200, 200.66f, 200);
                GameObject vehiclePrefab2 = VTResources.GetPlayerVehicle("F/A-26B").vehiclePrefab;

                GameObject f26seatholder = AircraftAPI.GetChildWithName(vehiclePrefab2, "EjectorSeat", false);
                Debug.Log("OPS 0.00.0.8");

                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(f26seatholder, position2, Main.aircraftCustom.transform.rotation);
                Debug.Log("OPS 0.00.0.9");

                
                Actor actor = __instance.actor = (FlightSceneManager.instance.playerActor = Main.aircraftCustom.GetComponent<Actor>());

                Debug.Log("OPS 0.00.0.10");

                actor.actorName = PilotSaveManager.current.pilotName;
                Debug.Log("OPS 0.00.0.11");

                actor.unitSpawn = __instance;
                Debug.Log("OPS 0.00.0.12");

                

                Debug.Log("OPSStartPatch 00");
                GameObject f26EjectorSeat = AircraftAPI.GetChildWithName(gameObject2, "EjectorSeat", false);
                f26EjectorSeat.transform.localScale = new Vector3(0.92f, 0.92f, 0.92f);
                f26EjectorSeat.transform.SetParent(Main.aircraftCustom.transform);
                Debug.Log("OPSStartPatch 00.1");

                Vector3 PlaneLocation = gameObject2.transform.position;
                Quaternion PlaneRotation = gameObject2.transform.rotation;
                Debug.Log("OPSStartPatch 00.2");
                Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);
               
                Debug.Log("OPSStartPatch 00.7");
                //GameObject f26Heartbeat2 = AircraftAPI.GetChildWithName(gameObject2, "HeartbeatAudio", true);
                f26EjectorSeat.SetActive(true);


                Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);

                Debug.Log("OPSStartPatch 0");

                Main.aircraftCustom.transform.position = PlaneLocation;
                Main.aircraftCustom.transform.rotation = PlaneRotation;
                Debug.Log("OPSStartPatch 01");

                Debug.Log("OPSStartPatch 1");
                GameObject aircraftSeat = AircraftAPI.GetChildWithName(Main.aircraftCustom, "EjectorSeatLocation", false);
                f26EjectorSeat.transform.SetParent(aircraftSeat.transform);
                Debug.Log("OPSStartPatch 1.1");

                f26EjectorSeat.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                f26EjectorSeat.transform.localPosition = new Vector3(0f, 0f, 0f);
                Debug.Log("OPSStartPatch 1.2");
                Debug.Log("FOS threshold: " + Main.aircraftCustom.GetComponent<FloatingOriginShifter>().threshold);



                Main.aircraftCustom.SetActive(false);

                //FloatingOriginShifter floatingOriginShifter = Main.aircraftCustom.AddComponent<FloatingOriginShifter>();
                //floatingOriginShifter.rb = component;
                //floatingOriginShifter.threshold = 600f;
               // FloatingOriginShifter floatingOriginShifter = Main.aircraftCustom.GetComponent<FloatingOriginShifter>();
               // floatingOriginShifter.enabled = false;
             //   floatingOriginShifter.enabled = true;
                Debug.Log("OPSStartPatch 1.2.1");


                GameObject cameraEye = AircraftAPI.GetChildWithName(Main.aircraftCustom, "Camera (eye)", false);
                Debug.Log("OPSStartPatch 1.2.2");
                Camera cameraEyeCamera = cameraEye.GetComponent<Camera>();
                GameObject hudCanvas = AircraftAPI.GetChildWithName(Main.aircraftCustom, "HUDCanvas", false);
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
                GameObject sideStickObject = AircraftAPI.GetChildWithName(Main.aircraftCustom, "SideStickObjects", false);
                GameObject autoAdjust = AircraftAPI.GetChildWithName(sideStickObject, "AutoAdjust", false);
                GameObject autoAdjustCanvas = AircraftAPI.GetChildWithName(autoAdjust, "Canvas", false);
                Debug.Log("OPSStartPatch 1.2.4.3");

                Canvas autoAdjustCanvasCompCanvas = autoAdjustCanvas.GetComponent<Canvas>();
                autoAdjustCanvasCompCanvas.worldCamera = cameraEyeCamera;
                Debug.Log("OPSStartPatch 1.2.4.4");

                GameObject dashCanvas = AircraftAPI.GetChildWithName(Main.aircraftCustom, "DashCanvas", false);
                Canvas dashCanvasCompCanvas = dashCanvas.GetComponent<Canvas>();
                Debug.Log("OPSStartPatch 1.2.4.5");

                dashCanvasCompCanvas.worldCamera = cameraEyeCamera;
                GameObject spectatorCam = AircraftAPI.GetChildWithName(Main.aircraftCustom, "SpectatorCam", false);
                Debug.Log("OPSStartPatch 1.2.4.5");

                FlybyCameraMFDPage flybyCameraMFDPage = spectatorCam.GetComponent<FlybyCameraMFDPage>();
                AudioListener cameraEyeAL = cameraEye.GetComponent<AudioListener>();
                flybyCameraMFDPage.playerAudioListener = cameraEyeAL;
                Debug.Log("OPSStartPatch 1.2.4.5.1");

                AudioListenerPosition cameraEyeALP = cameraEye.GetComponent<AudioListenerPosition>();
                
                cameraEyeALP.SetParentRigidbody(Main.aircraftCustom.GetComponent<Rigidbody>());
                cameraEyeALP.rb = Main.aircraftCustom.GetComponent<Rigidbody>();


                Debug.Log("OPSStartPatch 1.2.4.6");


                //cameraEye.GetComponent<AudioListenerPosition>().rb = Main.aircraftCustom.GetComponent<Rigidbody>();
                TargetingMFDPage componentInChildren8 = Main.aircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);
                Debug.Log("OPSStartPatch 1.2.4.6");


                GameObject hqhGO = AircraftAPI.GetChildWithName(cameraEye, "hqh", false);
                HelmetController helmetController = hqhGO.GetComponent<HelmetController>();

                Debug.Log("OPSStartPatch 1.2.4.6");
                componentInChildren8.helmet = helmetController;
                GameObject glassMask = AircraftAPI.GetChildWithName(hudCanvas, "GlassMask", false);
                Debug.Log("OPSStartPatch 1.2.4.6.1");

                //helmetController.hudPowerObject = glassMask;
                helmetController.hudMaskToggler = hudCanvas.GetComponent<HUDMaskToggler>();
                Debug.Log("OPSStartPatch 1.2.4.6.2");

                helmetController.battery = AircraftAPI.GetChildWithName(Main.aircraftCustom, "battery", false).GetComponent<Battery>();
                GameObject hmcsPowerInteractable = AircraftAPI.GetChildWithName(Main.aircraftCustom, "hmcsPowerInteractable", false);
                Debug.Log("OPSStartPatch 1.2.4.7");

                


                Debug.Log("OPSStartPatch 1.2.4.7.1");

                VRLever hmcsPowerInteractableVRL = hmcsPowerInteractable.GetComponent<VRLever>();
                hmcsPowerInteractableVRL.OnSetState.AddListener(helmetController.SetPower);

                GameObject visorButtonInteractable = AircraftAPI.GetChildWithName(Main.aircraftCustom, "visorButtonInteractable", false);
                Debug.Log("OPSStartPatch 1.2.4.8");

                VRInteractable visorButtonInteractableVRI = visorButtonInteractable.GetComponent<VRInteractable>();
                visorButtonInteractableVRI.OnInteract.AddListener(helmetController.ToggleVisor);
                Debug.Log("OPSStartPatch 1.2.4.9");



                Debug.Log("OPSStartPatch 1.2.5");
                Debug.Log("OPSStartPatch 1.2.6");
                Main.aircraftCustom.SetActive(true);
                //componentInChildren8.SetOpticalTargeter();
                GameObject blackOutParent = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutParent", true);
                blackOutParent.SetActive(false);
                GameObject blackOutEffect = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutEffect", true);
               
                blackOutEffect.SetActive(false);
                GameObject blackOutEffectNew = AircraftAPI.GetChildWithName(Main.aircraftCustom, "blackoutEffectNewParent", true);
                blackOutEffectNew.transform.SetParent(cameraEye.transform);
                


                Debug.Log("OPSStartPatch 1.3");

                GameObject screenFader = AircraftAPI.GetChildWithName(Main.aircraftCustom, "ScreenFader", false);
                screenFader.SetActive(false);

                Rigidbody ejectionSeatRB = f26EjectorSeat.GetComponent<Rigidbody>();
                EjectionSeat ejectionSeatES = f26EjectorSeat.GetComponent<EjectionSeat>();
                SeatAdjuster ejectionSeatSA = f26EjectorSeat.GetComponent<SeatAdjuster>();
                Debug.Log("OPSStartPatch 1.4");

                flybyCameraMFDPage.seatRb = ejectionSeatRB;
                Debug.Log("OPSStartPatch 1.5");

                ShipController CustomAircraftSC = Main.aircraftCustom.GetComponent<ShipController>();
                CustomAircraftSC.ejectionSeat = ejectionSeatES;
                Debug.Log("OPSStartPatch 1.6");


                GameObject intFixedCamSeat = AircraftAPI.GetChildWithName(f26EjectorSeat, "int_fixedCam_Cockpit3", false);
                flybyCameraMFDPage.fixedTransforms[5] = intFixedCamSeat.transform;
                Debug.Log("OPSStartPatch 1.9");

                GameObject riggedSuit2 = AircraftAPI.GetChildWithName(f26EjectorSeat, "RiggedSuit (2)", false);
                TempPilotDetacher tempPilotDetacherComp = AircraftAPI.GetChildWithName(Main.aircraftCustom, "TempPilotDetacher", false).GetComponent<TempPilotDetacher>();
                tempPilotDetacherComp.pilotModel = riggedSuit2;
                Debug.Log("OPSStartPatch 1.10");

                PlayerVehicleSetup customPlayerVS = Main.aircraftCustom.GetComponent<PlayerVehicleSetup>();
                customPlayerVS.hideObjectsOnConfig[2] = riggedSuit2;
                Debug.Log("OPSStartPatch 1.11");

                VehicleInputManager customPlayerVI = Main.aircraftCustom.GetComponent<VehicleInputManager>();
                customPlayerVI.pyrOutputs[1] = riggedSuit2.GetComponent<RudderFootAnimator>();
                Debug.Log("OPSStartPatch 1.12");

                PilotColorSetup pilotColorSetupComp = AircraftAPI.GetChildWithName(Main.aircraftCustom, "PilotColorApplier", false).GetComponent<PilotColorSetup>();
                SkinnedMeshRenderer riggedSuit001SMComp = AircraftAPI.GetChildWithName(f26EjectorSeat, "RiggedSuit.001", false).GetComponent<SkinnedMeshRenderer>();
                pilotColorSetupComp.pilotRenderers[3] = riggedSuit001SMComp;
                Debug.Log("OPSStartPatch 1.13");

                GameObject cameraRigParent = AircraftAPI.GetChildWithName(f26EjectorSeat, "CameraRigParent", false);
                customPlayerVS.hideObjectsOnConfig[0] = cameraRigParent;
                Debug.Log("OPSStartPatch 1.14");

                GameObject cameraRigGO = AircraftAPI.GetChildWithName(f26EjectorSeat, "[CameraRig]", false);
                tempPilotDetacherComp.cameraRig = cameraRigGO;
                Debug.Log("OPSStartPatch 1.15");

                CustomAircraftSC.cameraRig = cameraRigGO;
                Debug.Log("OPSStartPatch 1.16");

                GameObject playerGTrans = AircraftAPI.GetChildWithName(cameraRigGO, "PlayerGTransform", false);
                Debug.Log("OPSStartPatch 1.16.1");

                FlightInfo customFlightInfo = Main.aircraftCustom.GetComponent<FlightInfo>();
                Debug.Log("OPSStartPatch 1.16.2");

                //customFlightInfo.playerGTransform = playerGTrans.transform;
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

                customPlayerVS.OnBeginRearming.AddListener(matSwitchLeft.SwitchToB);
                customPlayerVS.OnBeginRearming.AddListener(matSwitchRight.SwitchToB);
                customPlayerVS.OnEndRearming.AddListener(matSwitchLeft.SwitchToB);
                customPlayerVS.OnEndRearming.AddListener(matSwitchRight.SwitchToB);
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

                GameObject nvgBIVL = AircraftAPI.GetChildWithName(Main.aircraftCustom, "nvgButtonInteractable", false);
                VRInteractable nvgBIVLVRL = nvgBIVL.GetComponent<VRInteractable>();
                nvgBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleNVG);
                Debug.Log("OPSStartPatch 1.25");

                GameObject visorBIVL = AircraftAPI.GetChildWithName(Main.aircraftCustom, "visorButtonInteractable", false);
                VRInteractable visorBIVLVRL = visorBIVL.GetComponent<VRInteractable>();
                visorBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleVisor);
                Debug.Log("OPSStartPatch 1.26");

                GameObject cockpitWindNoise = AircraftAPI.GetChildWithName(f26EjectorSeat, "cockpitWindNoise", false);
                CockpitWindAudioController cockpitWindNoiseAC = cockpitWindNoise.GetComponent<CockpitWindAudioController>();
                cockpitWindNoiseAC.flightInfo = customFlightInfo;

                GameObject HMCSDisplays = AircraftAPI.GetChildWithName(cameraEye, "HMCSDisplays", true);
                
                WeaponManager wem = Main.aircraftCustom.GetComponent<WeaponManager>();
                Debug.Log("OPSStartPatch 1.26.1 = " + wem.name);

                GameObject WeaponInfo = AircraftAPI.GetChildWithName(HMCSDisplays, "WeaponInfo", true);
                Main.HMCSAltText = AircraftAPI.GetChildWithName(HMCSDisplays, "Alt", true);

                PlayerVehicleNetSync componentNetSync = Main.aircraftCustom.GetComponent<PlayerVehicleNetSync>();
                if (componentNetSync)
                {
                    componentNetSync.Initialize();
                }

                WeaponInfo.GetComponent<HMDWeaponInfo>().wm = wem;

                Debug.Log("OPSStartPatch 1.27");
               // MirageElements.SetUpGauges();
             // MirageElements.IdentifiedRadarTargetsSetup();
            }
            return false;
        }
    }
    
    
    [HarmonyPatch(typeof(PlayerSpawn), "OnSpawnUnit")]
    public class SU35_OSStartPatch
    {
        public static bool Prefix(PlayerSpawn __instance)
        {
            //Debug.Log("Starting OSU 0.0");
            return true;
        }
            public static void Postfix(PlayerSpawn __instance)
        {
            //Debug.Log("Starting OSU 1.0");
            if (!Main.aircraftLoaded)
            { return ; }
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            //MirageElements.SetupArmingText();
            AircraftSetup.SetUpEOTS();
        }
    }


    [HarmonyPatch(typeof(TargetingMFDPage), "CloseOut")]
    public class SU35_TargetingMFDPagePatch
    {
        public static bool Prefix(TargetingMFDPage __instance)
        {
            if (!Main.aircraftLoaded)
            { return true; }
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            //Debug.unityLogger.logEnabled = Main.logging;
            HelmetController helmet = Main.aircraftCustom.GetComponentInChildren<HelmetController>(true);
            __instance.helmet = helmet;
            if (__instance.targetingCamera)
            {
                //Debug.Log("Part1TMFD");
                __instance.targetingCamera.enabled = false;
                //Debug.Log("Part1TMFD2");

            }
            if (__instance.helmet.tgpDisplayEnabled)
            {
                //Debug.Log("Part2TMFD");
                __instance.ToggleHelmetDisplay();
                //Debug.Log("Part2TMFD2");

            }
            if (__instance.tgpMode == TargetingMFDPage.TGPModes.HEAD)
            {
                //Debug.Log("Part3TMFD");
                __instance.MFDHeadButton();
                //Debug.Log("Part3TMFD2");

            }
            return false;
        }
    }

    
    [HarmonyPatch(typeof(WeaponManager), nameof(WeaponManager.Awake))]
    public class SU35_PlayerSpawnAwakePatch
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
            
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            //Debug.unityLogger.logEnabled = Main.logging;
            //Debug.Log("Awake prefix ran in wm!");
            

            //Debug.Log("Weapon Manager for : " + __instance.vm);


           

            if (__instance.gameObject.GetComponentInChildren<PlayerFlightLogger>())
            {

                Main.playerGameObject = __instance.gameObject;
                FA26Aircraft = AircraftAPI.GetChildWithName(Main.playerGameObject, "FA-26B", false);

                //Debug.Log("0");
                if (!FA26Aircraft) { return; }
                
                //Debug.Log("001");
              
                //Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);

                UnityEngine.Object.Destroy(FA26Aircraft);

                
                

            }
        }

        public static void Postfix(WeaponManager __instance)
        {
            
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            //Debug.unityLogger.logEnabled = Main.logging;
            //Debug.Log("pf1");
            Traverse traverse = Traverse.Create(__instance);
            //Debug.Log("pf1.1");
            HPEquippable[] hpEquippables = (HPEquippable[])traverse.Field("equips").GetValue();
            //Debug.Log("pf1.2");


        }
    }

    

    [HarmonyPatch(typeof(TargetingMFDPage), "Setup")]
    public class SU35_TMFDStartPatch
    {
        public static bool Prefix(TargetingMFDPage __instance)
        {
            
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            //Debug.unityLogger.logEnabled = Main.logging;
            //Debug.Log("TargetingMFDPage Setup 1.0 : " + __instance.gameObject.transform.parent.transform.parent.transform.parent);

            Traverse traverseT1 = Traverse.Create(__instance);
            bool startedTMFD = (bool)traverseT1.Field("started").GetValue();

            //Debug.Log("TargetingMFDPage Setup 1.1");

            if (startedTMFD)
            {
                //Debug.Log("TargetingMFDPage Setup 1.2");

                return false;
            }
            //Debug.Log("TargetingMFDPage Setup 1.3");

            startedTMFD = true;
            traverseT1.Field("started").SetValue(true);


            WeaponManager TMFDwm = (WeaponManager)traverseT1.Field("wm").GetValue();


            //Debug.Log("TargetingMFDPage Setup 1.4 +" + TMFDwm);

            if (!TMFDwm) { return false; }

            if (TMFDwm.opticalTargeter)
            {
                //Debug.Log("TargetingMFDPage Setup 1.5");

                if (!__instance.targetingCamera)
                {
                    //Debug.Log("TargetingMFDPage Setup 1.6");

                    __instance.targetingCamera = TMFDwm.opticalTargeter.cameraTransform.GetComponent<Camera>();
                }
                if (!__instance.opticalTargeter)
                {
                    //Debug.Log("TargetingMFDPage Setup 1.7");

                    __instance.opticalTargeter = TMFDwm.opticalTargeter;
                }
                if (__instance.targetingCamera)
                {
                    //Debug.Log("TargetingMFDPage Setup 1.8");

                    LODManager.instance.tcam = __instance.targetingCamera;
                }
                
            }
            else
            {
                return false;
            }
            string[] tgpModeLabelsT1 = (string[])traverseT1.Field("tgpModeLabels").GetValue();
            //Debug.Log("TargetingMFDPage Setup 1.9");

            if (__instance.mfdPage)
            {
                //Debug.Log("TargetingMFDPage Setup 1.10");


                __instance.mfdPage.SetText("tgpMode", tgpModeLabelsT1[(int)__instance.tgpMode]);
            }
            else if (__instance.portalPage)
            {
                //Debug.Log("TargetingMFDPage Setup 1.11");

                __instance.portalPage.SetText("tgpMode", tgpModeLabelsT1[(int)__instance.tgpMode]);
            }
            traverseT1.Field("gpsSystem").SetValue(TMFDwm.gpsSystem);
            if (__instance.limitLineRenderer)
            {
                //Debug.Log("TargetingMFDPage Setup 1.12");

                __instance.SetupLimitLine();
            }
            //Debug.Log("TargetingMFDPage Setup 1.13");

            __instance.UpdateLimLineVisibility();
            //Debug.Log("TargetingMFDPage Setup");
            return false;
        }
    }


    [HarmonyPatch(typeof(FlybyCameraMFDPage), "EnableCamera")]
    public static class SU35_SCamPatch
    {
        public static void Prefix(FlybyCameraMFDPage __instance)
        {
            
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            //Debug.unityLogger.logEnabled = Main.logging;
            Traverse traverseF1 = Traverse.Create(__instance);
            //Debug.Log("Scamepatch 1.0");
            bool flyCamEnabledFBCMFD = (bool)traverseF1.Field("flyCamEnabled").GetValue();
            bool previewEnabledFBCMFD = (bool)traverseF1.Field("previewEnabled").GetValue();
            //Debug.Log("Scamepatch 1.1");

            if (flyCamEnabledFBCMFD)
            {
                //Debug.Log("Scamepatch 1.2");
                __instance.SetupFlybyPosition((FlybyCameraMFDPage.SpectatorBehaviors)(-1));
                return;
            }
            //Debug.Log("Scamepatch 1.3");
            if (previewEnabledFBCMFD)
            {
                //Debug.Log("Scamepatch 1.3.1");
                __instance.previewObject.SetActive(true);
            }
            //Debug.Log("Scamepatch 1.4");
            traverseF1.Field("flyCamEnabled").SetValue(true);

            flyCamEnabledFBCMFD = true;
            //Debug.Log("Scamepatch 1.5");
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
    public static class SU35_PatchNVGPreCull
    {
        public static bool Prefix(FlybyCameraMFDPage.SCamNVGController __instance)
        {
           
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            //Debug.unityLogger.logEnabled = Main.logging;
            Traverse traverse1 = Traverse.Create(__instance);
            //Debug.Log("PatchNVGPreCull 1.0");

            if (!__instance.specCamNVG) { return false; }
            //Debug.Log("PatchNVGPreCull 1.0.1");
            if (__instance.specCamNVG.enabled)
            {
                //Debug.Log("PatchNVGPreCull 1.1");
                if (__instance.doIllum)
                {
                    //Debug.Log("PatchNVGPreCull 1.2");
                    traverse1.Field("illumEnabled").SetValue(true);
                    //__instance.illumEnabled = true;
                    //Debug.Log("PatchNVGPreCull 1.3");
                    __instance.nvg.EnableIlluminator();
                    //Debug.Log("PatchNVGPreCull 1.4");

                    return false;
                }
                return false;
            }
            else
            {
                //Debug.Log("PatchNVGPreCull 1.5");

                traverse1.Field("hidNvg").SetValue(true);
                //__instance.hidNvg = true;
                //Debug.Log("PatchNVGPreCull 1.6");

                object nvgScaleIDPull = traverse1.Field("nvgScaleID").GetValue();
                Shader.SetGlobalFloat(nvgScaleIDPull.ToString(), 0f);
                //Debug.Log("PatchNVGPreCull 1.7");
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(HPEquipGun), "Shake")]
    public static class SU35_PatchHPEquipGun
    {
        public static bool Prefix(HPEquipGun __instance)
        {

            //__instance.shaker.Shake(UnityEngine.Random.onUnitSphere * __instance.shakeMagnitude);
            //FlybyCameraMFDPage.ShakeSpectatorCamera(30f * __instance.shakeMagnitude / (FlybyCameraMFDPage.instance.flybyCam.transform.position - __instance.GetFireTransform().position).sqrMagnitude);
            return false;

        }
    }
    
  /* 
    [HarmonyPatch(typeof(HUDMaskToggler), "SetMask")]
    public static class SU35_HUDMaskTogglePatch
    {

        public static bool Prefix(bool maskEnabled, HUDMaskToggler __instance)
        {
            
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            //Debug.unityLogger.logEnabled = Main.logging;
            //Debug.Log("SetMask 1.0");
            if (__instance.alwaysFPVOnly && maskEnabled)
            {
                //Debug.Log("SetMask 1.1");
                return false;
            }
            //Debug.Log("SetMask 1.2");
            Traverse traverse = Traverse.Create(__instance);
            Transform[] displayObjectsP = (Transform[])traverse.Field("displayObjects").GetValue();
            GameObject hudCanvas = CustomAircraftTemplate.AircraftAPI.GetChildWithName(Main.aircraftCustom, "HUDCanvas", true);
            Transform[] displayObjectsHUD = hudCanvas.GetComponentsInChildren<Transform>(true);


            //Debug.Log("SetMask 1.3");

            for (int i = 0; i < displayObjectsHUD.Length; i++)
            {
                //Debug.Log("SetMask 1.4");
                if (displayObjectsHUD[i])
                {
                    //Debug.Log("SetMask 1.5");
                    if (__instance.alwaysFPVOnly)
                    {
                        //Debug.Log("SetMask 1.6");
                        displayObjectsHUD[i].gameObject.layer = 28;
                    }
                    else
                    {
                        //Debug.Log("SetMask 1.7");
                        displayObjectsHUD[i].gameObject.layer = (maskEnabled ? 5 : 28);
                    }
                }
            }
            traverse.Field("displayObjects").SetValue(displayObjectsP);
            //Debug.Log("SetMask 1.8");
            for (int j = 0; j < __instance.masks.Length; j++)
            {
                //Debug.Log("SetMask 1.9");
                if (__instance.masks[j])
                {
                    //Debug.Log("SetMask 1.10");
                    __instance.masks[j].enabled = maskEnabled;
                }
            }
            for (int k = 0; k < __instance.images.Length; k++)
            {
                //Debug.Log("SetMask 1.11");
                if (__instance.images[k])
                {
                    //Debug.Log("SetMask 1.12");
                    __instance.images[k].enabled = maskEnabled;
                }
            }
            if (__instance.alwaysFPVOnly)
            {
                //Debug.Log("SetMask 1.13");
                __instance.canvasObject.layer = 28;
            }
            else
            {
                //Debug.Log("SetMask 1.14");
               hudCanvas.layer = (maskEnabled ? 5 : 28);
            }
            //__instance.isMasked = maskEnabled;
            traverse.Field("isMasked").SetValue("maskEnabled");
            //Debug.Log("SetMask 1.15");
            return false;
        }
    }
    */
   


        }