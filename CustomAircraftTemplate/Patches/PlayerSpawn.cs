using Harmony;
using UnityEngine;
using UnityEngine.UI;
using VTOLVR.Multiplayer;

/*
List of places where a CAT mod needs to hook:

- MultiplayerSpawn/
    SetupSpawnedVehicle()

- HUDAoAMeter/
    OnEnable()

- HMDAltitude/
    Awake()

- WeaponManager/
    Awake()
    ReattachWeapons()

- MFDRadarUI/
    Awake()
    ClearSoftLocks()

- BlackoutEffect/
    LateUpdate()
    OnDestroy()
    OnDisable()

- PlayerSpawn/
    OnPreSpawnUnit()
    OnSpawnUnit()

- TargetingMFDPage/
    Setup()
    CloseOut()

- FlybyCameraMFDPage/
    EnableCamera()
    SCamNVGController/
        OnPreCull()

- HPEquipGun/
    Shake()

- HUDMaskToggler/
    SetMask()

 */

//namespace CAT
//{
//    [HarmonyPatch(typeof(MultiplayerSpawn), "SetupSpawnedVehicle")]
//    public class CAT_MS_SetupSpawnedVehicle_Prefix
//    {
//        private static EjectionSeat prefabES;

//        public static void Prefix(MultiplayerSpawn __instance, GameObject vehicleObj)
//        {
//            Main.IsEotsSetUp = 0;
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName) return;

//            Main.AircraftLoaded = true;
//            Main.AircraftCustom = vehicleObj;

//            var BONew = AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutEffectNew", false);

//            Main.BoQuad = AircraftApi.GetChildWithName(BONew, "Quad", false);

//            var componentRB = Main.AircraftCustom.GetComponent<Rigidbody>();
//            var position2 = new Vector3(200, 200.66f, 200);
//            var vehiclePrefab2 = VTResources.GetPlayerVehicle(AircraftInfo.PilotRootType).vehiclePrefab;

//            var f26seatholder = AircraftApi.GetChildWithName(vehiclePrefab2, "EjectorSeat", false);

//            var gameObject2 =
//                Object.Instantiate(f26seatholder, position2,
//                    Main.AircraftCustom.transform.rotation);

//            gameObject2.transform.localScale = new Vector3(0.92f, 0.92f, 0.92f);
//            gameObject2.transform.SetParent(Main.AircraftCustom.transform);
//            gameObject2.SetActive(true);
//            var aircraftSeat = AircraftApi.GetChildWithName(Main.AircraftCustom, "EjectorSeatLocation", false);
//            gameObject2.transform.SetParent(aircraftSeat.transform);
//            gameObject2.transform.SetParent(aircraftSeat.transform);

//            gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
//            gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
//            Main.AircraftCustom.SetActive(false);


//            //FloatingOriginShifter floatingOriginShifter = Main.aircraftCustom.GetComponent<FloatingOriginShifter>();

//            //floatingOriginShifter.enabled = false;
//            //floatingOriginShifter.enabled = true;

//            var cameraEye = AircraftApi.GetChildWithName(Main.AircraftCustom, "Camera (eye)", false);
//            var cameraEyeCamera = cameraEye.GetComponent<Camera>();
//            var hudCanvas = AircraftApi.GetChildWithName(Main.AircraftCustom, "HUDCanvas", false);
//            var hudCanvasCanvasComp = hudCanvas.GetComponent<Canvas>();
//            hudCanvasCanvasComp.worldCamera = cameraEyeCamera;

//            var cameraEyeTf = cameraEye.GetComponent<Transform>();
//            var elevationLadder = AircraftApi.GetChildWithName(hudCanvas, "ElevationLadder", false);
//            var elevationLadderComp = elevationLadder.GetComponent<HUDElevationLadder>();
//            elevationLadderComp.headTransform = cameraEyeTf;

//            var sideStickObject = AircraftApi.GetChildWithName(Main.AircraftCustom, "SideStickObjects", false);
//            var autoAdjust = AircraftApi.GetChildWithName(sideStickObject, "AutoAdjust", false);
//            var autoAdjustCanvas = AircraftApi.GetChildWithName(autoAdjust, "Canvas", false);

//            var autoAdjustCanvasCompCanvas = autoAdjustCanvas.GetComponent<Canvas>();
//            autoAdjustCanvasCompCanvas.worldCamera = cameraEyeCamera;

//            var dashCanvas = AircraftApi.GetChildWithName(Main.AircraftCustom, "DashCanvas", false);
//            var dashCanvasCompCanvas = dashCanvas.GetComponent<Canvas>();

//            dashCanvasCompCanvas.worldCamera = cameraEyeCamera;
//            var spectatorCam = AircraftApi.GetChildWithName(Main.AircraftCustom, "SpectatorCam", false);

//            var flybyCameraMFDPage = spectatorCam.GetComponent<FlybyCameraMFDPage>();
//            var cameraEyeAL = cameraEye.GetComponent<AudioListener>();
//            flybyCameraMFDPage.playerAudioListener = cameraEyeAL;

//            var cameraEyeALP = cameraEye.GetComponent<AudioListenerPosition>();

//            cameraEyeALP.SetParentRigidbody(Main.AircraftCustom.GetComponent<Rigidbody>());
//            cameraEyeALP.rb = Main.AircraftCustom.GetComponent<Rigidbody>();


//            //cameraEye.GetComponent<AudioListenerPosition>().rb = Main.aircraftCustom.GetComponent<Rigidbody>();
//            var componentInChildren8 = Main.AircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);

//            var hqhGO = AircraftApi.GetChildWithName(cameraEye, "hqh", false);
//            var helmetController = hqhGO.GetComponent<HelmetController>();

//            componentInChildren8.helmet = helmetController;
//            var glassMask = AircraftApi.GetChildWithName(hudCanvas, "GlassMask", false);

//            helmetController.hudPowerObject = glassMask;
//            helmetController.hudMaskToggler = hudCanvas.GetComponent<HUDMaskToggler>();

//            helmetController.battery = AircraftApi.GetChildWithName(Main.AircraftCustom, "battery", false)
//                .GetComponent<Battery>();
//            var hmcsPowerInteractable =
//                AircraftApi.GetChildWithName(Main.AircraftCustom, "hmcsPowerInteractable", false);

//            if (!hmcsPowerInteractable)
//            {
//                hmcsPowerInteractable =
//                    AircraftApi.GetChildWithName(Main.AircraftCustom, "HMCSPowerInteractable", false);
//                var hmcsPowerInteractableVRL = hmcsPowerInteractable.GetComponent<VRTwistKnobInt>();
//                hmcsPowerInteractableVRL.OnSetState.AddListener(helmetController.SetPower);
//            }
//            else
//            {
//                var hmcsPowerInteractableVRL = hmcsPowerInteractable.GetComponent<VRLever>();
//                hmcsPowerInteractableVRL.OnSetState.AddListener(helmetController.SetPower);
//            }

//            var hudWeaponInfo = AircraftApi.GetChildWithName(hudCanvas, "WeaponInfo", false)
//                .GetComponent<HUDWeaponInfo>();
//            var hmdWeaponInfo = AircraftApi.GetChildWithName(cameraEye, "WeaponInfo", false)
//                .GetComponent<HMDWeaponInfo>();
//            hmdWeaponInfo.weaponNameText = hudWeaponInfo.weaponNameText;
//            hmdWeaponInfo.weaponCountText = hudWeaponInfo.ammoCountText;

//            var visorButtonInteractable =
//                AircraftApi.GetChildWithName(Main.AircraftCustom, "visorButtonInteractable", false);

//            var visorButtonInteractableVRI = visorButtonInteractable.GetComponent<VRInteractable>();
//            visorButtonInteractableVRI.OnInteract.AddListener(helmetController.ToggleVisor);

//            var wm = Main.AircraftCustom.GetComponent<WeaponManager>();
//            //wm.SetOpticalTargeter(Main.aircraftCustom.GetComponentInChildren<OpticalTargeter>(true));
//            Main.AircraftCustom.SetActive(true);
//            //componentInChildren8.SetOpticalTargeter();
//            var blackOutParent = AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutParent", true);
//            blackOutParent.SetActive(false);
//            var blackOutEffect = AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutEffect", true);
//            blackOutParent.SetActive(false);
//            blackOutEffect.SetActive(false);
//            var blackOutEffectNew = AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutEffectNewParent", true);
//            blackOutEffectNew.transform.SetParent(cameraEye.transform);

//            var screenFader = AircraftApi.GetChildWithName(Main.AircraftCustom, "ScreenFader", false);
//            screenFader.SetActive(false);

//            var ejectionSeatRB = gameObject2.GetComponent<Rigidbody>();
//            var ejectionSeatES = gameObject2.GetComponent<EjectionSeat>();
//            var ejectionSeatSA = gameObject2.GetComponent<SeatAdjuster>();

//            flybyCameraMFDPage.seatRb = ejectionSeatRB;

//            var CustomAircraftSC = Main.AircraftCustom.GetComponent<ShipController>();
//            CustomAircraftSC.ejectionSeat = ejectionSeatES;

//            //VRInteractable customLowerSeatInter = AircraftAPI.GetChildWithName(dashCanvas, "lowerSeatInter", false).GetComponent<VRInteractable>();
//            //customLowerSeatInter.OnInteract.AddListener(ejectionSeatSA.StartLowerSeat);
//            //customLowerSeatInter.OnStopInteract.AddListener(ejectionSeatSA.Stop);

//            //VRInteractable customRaiseSeatInter = AircraftAPI.GetChildWithName(dashCanvas, "raiseSeatInter", false).GetComponent<VRInteractable>();
//            //customRaiseSeatInter.OnInteract.AddListener(ejectionSeatSA.StartRaiseSeat);
//            //customRaiseSeatInter.OnStopInteract.AddListener(ejectionSeatSA.Stop);

//            var intFixedCamSeat = AircraftApi.GetChildWithName(gameObject2, "int_fixedCam_Cockpit3", false);
//            if (intFixedCamSeat) flybyCameraMFDPage.fixedTransforms[5] = intFixedCamSeat.transform;

//            var riggedSuit2 = AircraftApi.GetChildWithName(gameObject2, "RiggedSuit (2)", false);
//            if (!riggedSuit2) riggedSuit2 = AircraftApi.GetChildWithName(gameObject2, "RiggedSuit (1)", false);
//            var tempPilotDetacherComp = AircraftApi.GetChildWithName(Main.AircraftCustom, "TempPilotDetacher", false)
//                .GetComponent<TempPilotDetacher>();
//            tempPilotDetacherComp.pilotModel = riggedSuit2;

//            var customPlayerVS = Main.AircraftCustom.GetComponent<PlayerVehicleSetup>();
//            customPlayerVS.hideObjectsOnConfig[2] = riggedSuit2;

//            var customPlayerVI = Main.AircraftCustom.GetComponent<VehicleInputManager>();
//            customPlayerVI.pyrOutputs[1] = riggedSuit2.GetComponent<RudderFootAnimator>();

//            var pilotColorSetupComp = AircraftApi.GetChildWithName(Main.AircraftCustom, "PilotColorApplier", false)
//                .GetComponent<PilotColorSetup>();
//            var riggedSuit001SMComp = AircraftApi.GetChildWithName(gameObject2, "RiggedSuit.001", false)
//                .GetComponent<SkinnedMeshRenderer>();
//            pilotColorSetupComp.pilotRenderers[3] = riggedSuit001SMComp;

//            var cameraRigParent = AircraftApi.GetChildWithName(gameObject2, "CameraRigParent", false);
//            customPlayerVS.hideObjectsOnConfig[0] = cameraRigParent;

//            var cameraRigGO = AircraftApi.GetChildWithName(gameObject2, "[CameraRig]", false);
//            tempPilotDetacherComp.cameraRig = cameraRigGO;

//            CustomAircraftSC.cameraRig = cameraRigGO;

//            var playerGTrans = AircraftApi.GetChildWithName(cameraRigGO, "PlayerGTransform", false);

//            var customFlightInfo = Main.AircraftCustom.GetComponent<FlightInfo>();

//            //customFlightInfo.playerGTransform = playerGTrans.transform;

//            var controllerLeft = AircraftApi.GetChildWithName(cameraRigParent, "Controller (left)", false);
//            var swatglowerLeft = AircraftApi.GetChildWithName(controllerLeft, "SWAT_glower_pivot.002", false);
//            var postDeathLookTgt = AircraftApi.GetChildWithName(controllerLeft, "postDeathLookTgt", false);
//            var leftForearmLRTf = AircraftApi.GetChildWithName(controllerLeft, "forearmLook", false)
//                .GetComponent<LookRotationReference>();

//            pilotColorSetupComp.pilotRenderers[1] = swatglowerLeft.GetComponent<SkinnedMeshRenderer>();

//            var controllerRight = AircraftApi.GetChildWithName(cameraRigParent, "Controller (right)", false);
//            var swatglowerRight = AircraftApi.GetChildWithName(controllerRight, "SWAT_glower_pivot.002", false);

//            pilotColorSetupComp.pilotRenderers[2] = swatglowerRight.GetComponent<SkinnedMeshRenderer>();

//            var matSwitchLeft = swatglowerLeft.GetComponent<MaterialSwitcher>();
//            tempPilotDetacherComp.OnDetachPilot.AddListener(matSwitchLeft.SwitchToB);
//            var matSwitchRight = swatglowerRight.GetComponent<MaterialSwitcher>();
//            tempPilotDetacherComp.OnDetachPilot.AddListener(matSwitchRight.SwitchToB);

//            customPlayerVS.OnBeginRearming.AddListener(matSwitchLeft.SwitchToB);
//            customPlayerVS.OnBeginRearming.AddListener(matSwitchRight.SwitchToB);
//            customPlayerVS.OnEndRearming.AddListener(matSwitchLeft.SwitchToB);
//            customPlayerVS.OnEndRearming.AddListener(matSwitchRight.SwitchToB);

//            var cameraEyeHelmet = AircraftApi.GetChildWithName(cameraRigParent, "Camera (eye) Helmet", false);

//            var screenMaskedClrRamp = cameraEyeHelmet.GetComponent<ScreenMaskedColorRamp>();

//            flybyCameraMFDPage.playerNVG = screenMaskedClrRamp;

//            var headSphere = AircraftApi.GetChildWithName(cameraRigParent, "headSphere", false);
//            pilotColorSetupComp.pilotRenderers[0] = headSphere.GetComponent<MeshRenderer>();

//            var hqh = AircraftApi.GetChildWithName(cameraRigParent, "hqh", false);
//            var hqhHCComp = hqh.GetComponent<HelmetController>();
//            componentInChildren8.helmet = hqhHCComp;

//            var nvgBIVL = AircraftApi.GetChildWithName(Main.AircraftCustom, "nvgButtonInteractable", false);
//            var nvgBIVLVRL = nvgBIVL.GetComponent<VRInteractable>();
//            nvgBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleNVG);

//            var visorBIVL = AircraftApi.GetChildWithName(Main.AircraftCustom, "visorButtonInteractable", false);
//            var visorBIVLVRL = visorBIVL.GetComponent<VRInteractable>();
//            visorBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleVisor);

//            var cockpitWindNoise = AircraftApi.GetChildWithName(gameObject2, "cockpitWindNoise", false);
//            var cockpitWindNoiseAc = cockpitWindNoise.GetComponent<CockpitWindAudioController>();
//            cockpitWindNoiseAc.flightInfo = customFlightInfo;

//            var hmcsDisplays = AircraftApi.GetChildWithName(cameraEye, "HMCSDisplays", true);

//            var wem = Main.AircraftCustom.GetComponent<WeaponManager>();

//            var weaponInfo = AircraftApi.GetChildWithName(hmcsDisplays, "WeaponInfo", true);
//            weaponInfo.GetComponent<HMDWeaponInfo>().wm = wem;
//            Main.HmcsAltText = AircraftApi.GetChildWithName(hmcsDisplays, "Alt", true);
//            var aoahmcsInfo = AircraftApi.GetChildWithName(hmcsDisplays, "Atext", true);
//            var hmcsaoa = aoahmcsInfo.GetComponent<HUDAoAMeter>();
//            hmcsaoa.flightInfo = Main.AircraftCustom.GetComponent<FlightInfo>();

//            var gmhmcsInfo = AircraftApi.GetChildWithName(hmcsDisplays, "Gtext", true);
//            var hmcsgm = gmhmcsInfo.GetComponent<HUDGMeter>();
//            hmcsgm.flightInfo = Main.AircraftCustom.GetComponent<FlightInfo>();

//            var mmhmcsInfo = AircraftApi.GetChildWithName(hmcsDisplays, "MText", true);
//            var HMCSMM = mmhmcsInfo.GetComponent<HUDVelocity>();

//            // MirageElements.SetUpGauges();
//            //  MirageElements.IdentifiedRadarTargetsSetup();
//            prefabES = aircraftSeat.GetComponent<EjectionSeat>();
//            Object.Destroy(prefabES);
//            var MPSyncs = AircraftApi.GetChildWithName(Main.AircraftCustom, "MPSyncs", false);
//            var ejectSyncs = MPSyncs.GetComponent<EjectSync>();
//            var ejectionSeat = f26seatholder.GetComponent<EjectionSeat>();
//            ejectSyncs.localEjector = ejectionSeat;
//        }
//    }

//    [HarmonyPatch(typeof(HUDAoAMeter), "OnEnable")]
//    public class SU35_HUDAOA_Postfix
//    {
//        public static void Prefix(HUDAoAMeter __instance)
//        {
//            __instance.flightInfo = Main.AircraftCustom.GetComponent<FlightInfo>();
//        }
//    }

//    [HarmonyPatch(typeof(HMDAltitude), "Awake")]
//    public class SU35_HMDAlt_Postfix
//    {
//        public static void Postfix(HMDAltitude __instance)
//        {
//            var trav1 = Traverse.Create(__instance);

//            trav1.Field("flightInfo").SetValue(Main.AircraftCustom.GetComponent<FlightInfo>());
//            trav1.Field("vm").SetValue(Main.AircraftCustom.GetComponent<VehicleMaster>());
//            trav1.Field("measurements").SetValue(Main.AircraftCustom.GetComponent<MeasurementManager>());
//            var altRadarMode = (bool) trav1.Field("vm").Field("_useRadarAlt").GetValue();
//            Main.AircraftCustom.GetComponent<VehicleMaster>().useRadarAlt = altRadarMode;
//        }
//    }
    
//    [HarmonyPatch(typeof(WeaponManager), "ReattachWeapons")]
//    public class SU35_ReattachWeapons_Post
//    {
//        public static void Postfix(WeaponManager __instance)
//        {
//            // MirageElements.SetUpGauges();
//            //  MirageElements.SetupArmingText();
//            AircraftSetup.SetUpEots();
//        }
//    }

//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(MFDRadarUI), "Awake")]
//    public class CAT_MFDUIAwakePatch
//    {
//        public static bool Prefix(MFDRadarUI __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;

//            var traverseMui = Traverse.Create(__instance);
//            traverseMui.Field("isMultiCrew").SetValue(false);

//            __instance.SetupPools();
//            __instance.SetupDisplay();
//            __instance.softLocks = new MFDRadarUI.UIRadarContact[__instance.softLockCount];
//            __instance.radarCtrlr.OnElevationAdjusted += __instance.UpdateElevationText;
//            if (!__instance.playerRadar) return false;

//            traverseMui.Field("origScanRate").SetValue(__instance.playerRadar.rotationSpeed);
//            __instance.lockingRadar.OnUnlocked += __instance.LockingRadar_OnUnlocked;

//            return false;
//        }
//    }
    
//    //[HarmonyPatch(typeof(MFDRadarUI), nameof(MFDRadarUI.SetPower))]
//    //public class CAT_MFDUISetPowerPatch
//    //{
//    //    public static bool Prefix(MFDRadarUI __instance)
//    //    {
//    //        if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
//    //            return true;
//    //        //Debug.unityLogger.logEnabled = Main.logging;
//    //        Debug.Log("SU35 Starting MDUISP 1.0");
//    //        return true;
//    //    }
//    //}
    
//    [HarmonyPatch(typeof(BlackoutEffect), nameof(BlackoutEffect.LateUpdate))]
//    public class CAT_BlackoutPFPatch
//    {
//        public static void Postfix(BlackoutEffect __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return;

//            var traverse1 = Traverse.Create(__instance);
//            var newgAccum = (float) traverse1.Field("gAccum").GetValue();
//            var num = Mathf.Abs(newgAccum) * __instance.aFactor;

//            var newAlpha = Main.CurrentGAlpha;

//            var newNVG = (NightVisionGoggles) traverse1.Field("nvg").GetValue();

//            newAlpha = Mathf.Lerp(newAlpha, num, 20f * Time.deltaTime);
//            Main.CurrentGAlpha = newAlpha;

//            var color = newNVG && newNVG.IsNVGVisible() ? __instance.nvgRedoutColor : __instance.redoutColor;

//            color *= RenderSettings.ambientIntensity;

//            var color2 = newgAccum >= 0f ? Color.black : color;

//            color2.a = newAlpha * newAlpha;
//            var BONew = AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutEffectNew", false);
//            Main.BoQuad = AircraftApi.GetChildWithName(BONew, "Quad", false);
            
//            var BOMesh = Main.BoQuad.GetComponent<MeshRenderer>();
//            var BOMat = BOMesh.material;
//            var BOMatColor = BOMat.color;

//            BOMatColor.a = newAlpha;
//            BOMat.color = new Color(0, 0, 0, newAlpha);
//        }
//    }

//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(BlackoutEffect), "OnDestroy")]
//    internal class CAT_BOFCheck
//    {
//        public static bool Prefix(BlackoutEffect __instance)
//        {
//            Debug.Log("SU35 BOFDestroy 1.0.0");
//            if (!Main.AircraftLoaded) return true;

//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;

//            if (!__instance.audioMixer) return false;
            
//            __instance.audioMixer.SetFloat("ConsciousVolume", 0f);
            
//            __instance.audioMixer.SetFloat("BlackoutVolume", -80f);

//            var trav = Traverse.Create(__instance);
//            trav.Field("destroyed").SetValue(true);

//            return false;
//        }
//    }

//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(BlackoutEffect), "OnDisable")]
//    internal class CAT_BOFDisCheck
//    {
//        public static bool Prefix(BlackoutEffect __instance)
//        {
//            if (!Main.AircraftLoaded) return true;

//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;

//            if (!__instance.audioMixer) return false;
//            __instance.audioMixer.SetFloat("ConsciousVolume", 0f);
            
//            __instance.audioMixer.SetFloat("BlackoutVolume", -80f);
//            return false;
//        }
//    }
    
//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(MFDRadarUI), nameof(MFDRadarUI.ClearSoftLocks))]
//    public class CAT_ClearSoftLocksPatch
//    {
//        public static bool Prefix(MFDRadarUI __instance)
//        {
//            if (!Main.AircraftLoaded) return true;
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;

//            for (var i = 0; i < __instance.softLockCount; i++)
//            {
//                if (__instance.softLocks[i] == null) continue;

//                if (__instance.softLocks[i].actor && __instance.lockingRadar)
//                {
//                    __instance.lockingRadar.RemoveTWSLock(__instance.softLocks[i].actor);
//                }

//                var traversescl = Traverse.Create(__instance);
//                var hardLockCheck = (MFDRadarUI.UIRadarContact) traversescl.Field("hardLock").GetValue();

//                if (hardLockCheck != null && __instance.softLocks[i].actorID == hardLockCheck.actorID) continue;

//                __instance.softLocks[i] = null;
//            }
            
//            __instance.UpdateLocks();
//            return false;
//        }
//    }

//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(PlayerSpawn), "OnPreSpawnUnit")]
//    public class CAT_OPSStartPatch
//    {
//        private static GameObject _hmcsPowerInteractable;
//        private static VRLever _hmcsPowerInteractableVrl;
//        private static VRTwistKnobInt _hmcsPowerInteractableVrt;

//        public static bool Prefix(PlayerSpawn __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;

//            Debug.unityLogger.logEnabled = Main.Logging;
//            // __instance.OnPreSpawnUnit();

//            var vehiclePrefab = Main.AircraftPrefab;
//            if (!vehiclePrefab) return false;
            
//            Main.IsEotsSetUp = 0;

//            var position = new Vector3(0, 1.1f, 0);
//            var rotation = new Quaternion(0, 0, 0, 0);
            
//            Main.AircraftCustom = Object.Instantiate(vehiclePrefab, position, rotation);
//            Main.AircraftLoaded = true;

//            var boNew = AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutEffectNew", false);

//            Main.BoQuad = AircraftApi.GetChildWithName(boNew, "Quad", false);

//            var component = Main.AircraftCustom.GetComponent<Rigidbody>();

//            var traverse = Traverse.Create(__instance);
//            traverse.Field("vehicleRb").SetValue(Main.AircraftCustom.GetComponent<Rigidbody>());

//            traverse.Field("playerVm").SetValue(Main.AircraftCustom.GetComponent<VehicleMaster>());
//            traverse.Field("vehicleRb").Field("interpolation").SetValue(RigidbodyInterpolation.None);


//            var position2 = new Vector3(200, 200.66f, 200);
//            var vehiclePrefab2 = VTResources.GetPlayerVehicle(AircraftInfo.PilotRootType).vehiclePrefab;

//            var f26seatholder = AircraftApi.GetChildWithName(vehiclePrefab2, "EjectorSeat", false);

//            var gameObject2 = Object.Instantiate(f26seatholder, position2,
//                Main.AircraftCustom.transform.rotation);

//            var actor = __instance.actor =
//                FlightSceneManager.instance.playerActor = Main.AircraftCustom.GetComponent<Actor>();

//            actor.actorName = PilotSaveManager.current.pilotName;

//            actor.unitSpawn = __instance;
            
//            var f26EjectorSeat = AircraftApi.GetChildWithName(gameObject2, "EjectorSeat", false);
//            f26EjectorSeat.transform.localScale = new Vector3(0.92f, 0.92f, 0.92f);
//            f26EjectorSeat.transform.SetParent(Main.AircraftCustom.transform);

//            var PlaneLocation = gameObject2.transform.position;
//            var PlaneRotation = gameObject2.transform.rotation;
            
//            //GameObject f26Heartbeat2 = AircraftAPI.GetChildWithName(gameObject2, "HeartbeatAudio", true);
//            f26EjectorSeat.SetActive(true);
            
//            Main.AircraftCustom.transform.position = PlaneLocation;
//            Main.AircraftCustom.transform.rotation = PlaneRotation;
            
//            var aircraftSeat = AircraftApi.GetChildWithName(Main.AircraftCustom, "EjectorSeatLocation", false);
//            f26EjectorSeat.transform.SetParent(aircraftSeat.transform);

//            f26EjectorSeat.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
//            f26EjectorSeat.transform.localPosition = new Vector3(0f, 0f, 0f);

//            Main.AircraftCustom.SetActive(false);

//            //FloatingOriginShifter floatingOriginShifter = Main.aircraftCustom.AddComponent<FloatingOriginShifter>();
//            //floatingOriginShifter.rb = component;
//            //floatingOriginShifter.threshold = 600f;
//            // FloatingOriginShifter floatingOriginShifter = Main.aircraftCustom.GetComponent<FloatingOriginShifter>();
//            // floatingOriginShifter.enabled = false;
//            //   floatingOriginShifter.enabled = true;

//            var cameraEye = AircraftApi.GetChildWithName(Main.AircraftCustom, "Camera (eye)", false);
//            var cameraEyeCamera = cameraEye.GetComponent<Camera>();
//            var hudCanvas = AircraftApi.GetChildWithName(Main.AircraftCustom, "HUDCanvas", false);
//            var hudCanvasCanvasComp = hudCanvas.GetComponent<Canvas>();
//            hudCanvasCanvasComp.worldCamera = cameraEyeCamera;

//            var cameraEyeTf = cameraEye.GetComponent<Transform>();
//            var elevationLadder = AircraftApi.GetChildWithName(hudCanvas, "ElevationLadder", false);
//            var elevationLadderComp = elevationLadder.GetComponent<HUDElevationLadder>();
//            elevationLadderComp.headTransform = cameraEyeTf;
            
//            var sideStickObject = AircraftApi.GetChildWithName(Main.AircraftCustom, "SideStickObjects", false);
//            var autoAdjust = AircraftApi.GetChildWithName(sideStickObject, "AutoAdjust", false);
//            var autoAdjustCanvas = AircraftApi.GetChildWithName(autoAdjust, "Canvas", false);
            
//            var autoAdjustCanvasCompCanvas = autoAdjustCanvas.GetComponent<Canvas>();
//            autoAdjustCanvasCompCanvas.worldCamera = cameraEyeCamera;

//            var dashCanvas = AircraftApi.GetChildWithName(Main.AircraftCustom, "DashCanvas", false);
//            var dashCanvasCompCanvas = dashCanvas.GetComponent<Canvas>();

//            dashCanvasCompCanvas.worldCamera = cameraEyeCamera;
//            var spectatorCam = AircraftApi.GetChildWithName(Main.AircraftCustom, "SpectatorCam", false);

//            var flybyCameraMFDPage = spectatorCam.GetComponent<FlybyCameraMFDPage>();
//            var cameraEyeAL = cameraEye.GetComponent<AudioListener>();
//            flybyCameraMFDPage.playerAudioListener = cameraEyeAL;

//            var cameraEyeALP = cameraEye.GetComponent<AudioListenerPosition>();

//            cameraEyeALP.SetParentRigidbody(Main.AircraftCustom.GetComponent<Rigidbody>());
//            cameraEyeALP.rb = Main.AircraftCustom.GetComponent<Rigidbody>();
            
//            //cameraEye.GetComponent<AudioListenerPosition>().rb = Main.aircraftCustom.GetComponent<Rigidbody>();
//            var componentInChildren8 = Main.AircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);
            
//            var hqhGO = AircraftApi.GetChildWithName(cameraEye, "hqh", false);
//            var helmetController = hqhGO.GetComponent<HelmetController>();
            
//            componentInChildren8.helmet = helmetController;
//            var glassMask = AircraftApi.GetChildWithName(hudCanvas, "GlassMask", false);

//            //helmetController.hudPowerObject = glassMask;
//            helmetController.hudMaskToggler = hudCanvas.GetComponent<HUDMaskToggler>();

//            helmetController.battery = AircraftApi.GetChildWithName(Main.AircraftCustom, "battery", false)
//                .GetComponent<Battery>();
//            _hmcsPowerInteractable =
//                AircraftApi.GetChildWithName(Main.AircraftCustom, "hmcsPowerInteractable", true);
//            if (!_hmcsPowerInteractable)
//                _hmcsPowerInteractable =
//                    AircraftApi.GetChildWithName(Main.AircraftCustom, "HMCSPowerInteractable", true);
            
//            _hmcsPowerInteractableVrl = _hmcsPowerInteractable.GetComponent<VRLever>();
//            if (!_hmcsPowerInteractableVrl)
//                _hmcsPowerInteractableVrt = _hmcsPowerInteractable.GetComponent<VRTwistKnobInt>();
            
//            if (!_hmcsPowerInteractableVrl)
//                _hmcsPowerInteractableVrt.OnSetState.AddListener(helmetController.SetPower);
//            else
//                _hmcsPowerInteractableVrl.OnSetState.AddListener(helmetController.SetPower);

//            var visorButtonInteractable =
//                AircraftApi.GetChildWithName(Main.AircraftCustom, "visorButtonInteractable", true);

//            var visorButtonInteractableVRI = visorButtonInteractable.GetComponent<VRInteractable>();
//            visorButtonInteractableVRI.OnInteract.AddListener(helmetController.ToggleVisor);

//            Main.AircraftCustom.SetActive(true);
//            //componentInChildren8.SetOpticalTargeter();
//            var blackOutParent = AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutParent", true);
//            blackOutParent.SetActive(false);
//            var blackOutEffect = AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutEffect", true);

//            blackOutEffect.SetActive(false);
//            var blackOutEffectNew =
//                AircraftApi.GetChildWithName(Main.AircraftCustom, "blackoutEffectNewParent", true);
//            blackOutEffectNew.transform.SetParent(cameraEye.transform);
            
//            var screenFader = AircraftApi.GetChildWithName(Main.AircraftCustom, "ScreenFader", false);
//            screenFader.SetActive(false);

//            var ejectionSeatRB = f26EjectorSeat.GetComponent<Rigidbody>();
//            var ejectionSeatES = f26EjectorSeat.GetComponent<EjectionSeat>();
//            var ejectionSeatSA = f26EjectorSeat.GetComponent<SeatAdjuster>();

//            flybyCameraMFDPage.seatRb = ejectionSeatRB;

//            var CustomAircraftSC = Main.AircraftCustom.GetComponent<ShipController>();
//            CustomAircraftSC.ejectionSeat = ejectionSeatES;

//            var intFixedCamSeat = AircraftApi.GetChildWithName(f26EjectorSeat, "int_fixedCam_Cockpit3", false);
//            if (intFixedCamSeat) flybyCameraMFDPage.fixedTransforms[5] = intFixedCamSeat.transform;
            
//            var riggedSuit2 = AircraftApi.GetChildWithName(f26EjectorSeat, "RiggedSuit (2)", false);
//            if (!riggedSuit2) riggedSuit2 = AircraftApi.GetChildWithName(f26EjectorSeat, "RiggedSuit (1)", false);
//            var tempPilotDetacherComp = AircraftApi
//                .GetChildWithName(Main.AircraftCustom, "TempPilotDetacher", false)
//                .GetComponent<TempPilotDetacher>();
//            tempPilotDetacherComp.pilotModel = riggedSuit2;

//            var customPlayerVS = Main.AircraftCustom.GetComponent<PlayerVehicleSetup>();
//            customPlayerVS.hideObjectsOnConfig[2] = riggedSuit2;

//            var customPlayerVI = Main.AircraftCustom.GetComponent<VehicleInputManager>();
//            customPlayerVI.pyrOutputs[1] = riggedSuit2.GetComponent<RudderFootAnimator>();

//            var pilotColorSetupComp = AircraftApi.GetChildWithName(Main.AircraftCustom, "PilotColorApplier", false)
//                .GetComponent<PilotColorSetup>();
//            var riggedSuit001SMComp = AircraftApi.GetChildWithName(f26EjectorSeat, "RiggedSuit.001", false)
//                .GetComponent<SkinnedMeshRenderer>();
//            pilotColorSetupComp.pilotRenderers[3] = riggedSuit001SMComp;

//            var cameraRigParent = AircraftApi.GetChildWithName(f26EjectorSeat, "CameraRigParent", false);
//            customPlayerVS.hideObjectsOnConfig[0] = cameraRigParent;

//            var cameraRigGO = AircraftApi.GetChildWithName(f26EjectorSeat, "[CameraRig]", false);
//            tempPilotDetacherComp.cameraRig = cameraRigGO;

//            CustomAircraftSC.cameraRig = cameraRigGO;

//            var playerGTrans = AircraftApi.GetChildWithName(cameraRigGO, "PlayerGTransform", false);

//            var customFlightInfo = Main.AircraftCustom.GetComponent<FlightInfo>();

//            //customFlightInfo.playerGTransform = playerGTrans.transform;

//            var controllerLeft = AircraftApi.GetChildWithName(cameraRigParent, "Controller (left)", false);
//            var swatglowerLeft = AircraftApi.GetChildWithName(controllerLeft, "SWAT_glower_pivot.002", false);
//            var postDeathLookTgt = AircraftApi.GetChildWithName(controllerLeft, "postDeathLookTgt", false);
//            var leftForearmLRTf = AircraftApi.GetChildWithName(controllerLeft, "forearmLook", false)
//                .GetComponent<LookRotationReference>();
            
//            pilotColorSetupComp.pilotRenderers[1] = swatglowerLeft.GetComponent<SkinnedMeshRenderer>();

//            var controllerRight = AircraftApi.GetChildWithName(cameraRigParent, "Controller (right)", false);
//            var swatglowerRight = AircraftApi.GetChildWithName(controllerRight, "SWAT_glower_pivot.002", false);

//            pilotColorSetupComp.pilotRenderers[2] = swatglowerRight.GetComponent<SkinnedMeshRenderer>();

//            var matSwitchLeft = swatglowerLeft.GetComponent<MaterialSwitcher>();
//            tempPilotDetacherComp.OnDetachPilot.AddListener(matSwitchLeft.SwitchToB);
//            var matSwitchRight = swatglowerRight.GetComponent<MaterialSwitcher>();
//            tempPilotDetacherComp.OnDetachPilot.AddListener(matSwitchRight.SwitchToB);
            
//            customPlayerVS.OnBeginRearming.AddListener(matSwitchLeft.SwitchToB);
//            customPlayerVS.OnBeginRearming.AddListener(matSwitchRight.SwitchToB);
//            customPlayerVS.OnEndRearming.AddListener(matSwitchLeft.SwitchToB);
//            customPlayerVS.OnEndRearming.AddListener(matSwitchRight.SwitchToB);

//            var cameraEyeHelmet = AircraftApi.GetChildWithName(cameraRigParent, "Camera (eye) Helmet", false);

//            var screenMaskedClrRamp = cameraEyeHelmet.GetComponent<ScreenMaskedColorRamp>();

//            flybyCameraMFDPage.playerNVG = screenMaskedClrRamp;

//            var headSphere = AircraftApi.GetChildWithName(cameraRigParent, "headSphere", false);
//            pilotColorSetupComp.pilotRenderers[0] = headSphere.GetComponent<MeshRenderer>();

//            var hqh = AircraftApi.GetChildWithName(cameraRigParent, "hqh", false);
//            var hqhHCComp = hqh.GetComponent<HelmetController>();
//            componentInChildren8.helmet = hqhHCComp;

//            var nvgBIVL = AircraftApi.GetChildWithName(Main.AircraftCustom, "nvgButtonInteractable", false);
//            var nvgBIVLVRL = nvgBIVL.GetComponent<VRInteractable>();
//            nvgBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleNVG);

//            var visorBIVL = AircraftApi.GetChildWithName(Main.AircraftCustom, "visorButtonInteractable", false);
//            var visorBIVLVRL = visorBIVL.GetComponent<VRInteractable>();
//            visorBIVLVRL.OnInteract.AddListener(hqhHCComp.ToggleVisor);

//            var cockpitWindNoise = AircraftApi.GetChildWithName(f26EjectorSeat, "cockpitWindNoise", false);
//            var cockpitWindNoiseAC = cockpitWindNoise.GetComponent<CockpitWindAudioController>();
//            cockpitWindNoiseAC.flightInfo = customFlightInfo;

//            var HMCSDisplays = AircraftApi.GetChildWithName(cameraEye, "HMCSDisplays", true);

//            var wem = Main.AircraftCustom.GetComponent<WeaponManager>();

//            var WeaponInfo = AircraftApi.GetChildWithName(HMCSDisplays, "WeaponInfo", true);
//            WeaponInfo.GetComponent<HMDWeaponInfo>().wm = wem;

//            Main.HmcsAltText = AircraftApi.GetChildWithName(HMCSDisplays, "Alt", true);

//            var componentNetSync = Main.AircraftCustom.GetComponent<PlayerVehicleNetSync>();
//            if (componentNetSync) componentNetSync.Initialize();


//            Main.HmcsAltText = AircraftApi.GetChildWithName(HMCSDisplays, "Alt", true);
//            var AOAHMCSInfo = AircraftApi.GetChildWithName(HMCSDisplays, "Atext", true);
//            var HMCSAOA = AOAHMCSInfo.GetComponent<HUDAoAMeter>();
//            HMCSAOA.flightInfo = Main.AircraftCustom.GetComponent<FlightInfo>();

//            var GMHMCSInfo = AircraftApi.GetChildWithName(HMCSDisplays, "Gtext", true);
//            var HMCSGM = GMHMCSInfo.GetComponent<HUDGMeter>();
//            HMCSGM.flightInfo = Main.AircraftCustom.GetComponent<FlightInfo>();

//            var MMHMCSInfo = AircraftApi.GetChildWithName(HMCSDisplays, "MText", true);
//            var HMCSMM = MMHMCSInfo.GetComponent<HUDVelocity>();

//            // MirageElements.SetUpGauges();
//            // MirageElements.IdentifiedRadarTargetsSetup();

//            return false;
//        }
//    }


//    [HarmonyPatch(typeof(PlayerSpawn), "OnSpawnUnit")]
//    public class CAT_OSStartPatch
//    {
//        public static bool Prefix(PlayerSpawn __instance)
//        {
//            Debug.Log("SU35 Starting OSU 0.0");
//            return true;
//        }

//        public static void Postfix(PlayerSpawn __instance)
//        {
//            Debug.Log("SU35 Starting OSU 1.0");
//            if (!Main.AircraftLoaded) return;
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return;
//            //MirageElements.SetupArmingText();
//            AircraftSetup.SetUpEots();
//        }
//    }


//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(TargetingMFDPage), "CloseOut")]
//    public class CAT_TargetingMFDPagePatch
//    {
//        public static bool Prefix(TargetingMFDPage __instance)
//        {
//            if (!Main.AircraftLoaded) return true;
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;
//            //Debug.unityLogger.logEnabled = Main.logging;
//            var helmet = Main.AircraftCustom.GetComponentInChildren<HelmetController>(true);
//            __instance.helmet = helmet;
//            if (__instance.targetingCamera)
//            {
//                __instance.targetingCamera.enabled = false;
//            }

//            if (__instance.helmet.tgpDisplayEnabled)
//            {
//                __instance.ToggleHelmetDisplay();
//            }

//            if (__instance.tgpMode == TargetingMFDPage.TGPModes.HEAD)
//            {
//                __instance.MFDHeadButton();
//            }

//            return false;
//        }
//    }


//    [HarmonyPatch(typeof(WeaponManager), nameof(WeaponManager.Awake))]
//    public class CAT_PlayerSpawnAwakePatch
//    {
//        private static GameObject SeatLocation;
//        private static GameObject FA26Aircraft;
//        private static Vector3 PlaneLocation;
//        private static GameObject Newposition;
//        private static GameObject aircraft;
//        private static GameObject aircraftSeat;
//        private static VTOLScenes scene;

//        public static void Prefix(WeaponManager __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return;

//            if (!__instance.gameObject.GetComponentInChildren<PlayerFlightLogger>()) return;

//            Main.PlayerGameObject = __instance.gameObject;
//            FA26Aircraft = AircraftApi.GetChildWithName(Main.PlayerGameObject, "FA-26B", false);
                
//            if (!FA26Aircraft) return;
                
//            Object.Destroy(FA26Aircraft);
//        }

//        public static void Postfix(WeaponManager __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return;

//            var traverse = Traverse.Create(__instance);
//            var hpEquippables = (HPEquippable[]) traverse.Field("equips").GetValue();
//        }
//    }


//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(TargetingMFDPage), "Setup")]
//    public class CAT_TMFDStartPatch
//    {
//        public static bool Prefix(TargetingMFDPage __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;

//            var traverseT1 = Traverse.Create(__instance);
//            var startedTMFD = (bool) traverseT1.Field("started").GetValue();
            
//            if (startedTMFD)
//            {
//                return false;
//            }
            
//            startedTMFD = true;
//            traverseT1.Field("started").SetValue(true);
            
//            var TMFDwm = (WeaponManager) traverseT1.Field("wm").GetValue();

//            if (!TMFDwm) return false;

//            if (TMFDwm.opticalTargeter)
//            {
//                if (!__instance.targetingCamera)
//                {
//                    __instance.targetingCamera = TMFDwm.opticalTargeter.cameraTransform.GetComponent<Camera>();
//                }

//                if (!__instance.opticalTargeter)
//                {
//                    __instance.opticalTargeter = TMFDwm.opticalTargeter;
//                }

//                if (__instance.targetingCamera)
//                {
//                    LODManager.instance.tcam = __instance.targetingCamera;
//                }
//            }
//            else
//            {
//                return false;
//            }

//            var tgpModeLabelsT1 = (string[]) traverseT1.Field("tgpModeLabels").GetValue();

//            if (__instance.mfdPage)
//            {
//                __instance.mfdPage.SetText("tgpMode", tgpModeLabelsT1[(int) __instance.tgpMode]);
//            }
//            else if (__instance.portalPage)
//            {
//                __instance.portalPage.SetText("tgpMode", tgpModeLabelsT1[(int) __instance.tgpMode]);
//            }

//            traverseT1.Field("gpsSystem").SetValue(TMFDwm.gpsSystem);
//            if (__instance.limitLineRenderer)
//            {
//                __instance.SetupLimitLine();
//            }
            
//            __instance.UpdateLimLineVisibility();
//            return false;
//        }
//    }


//    [HarmonyPatch(typeof(FlybyCameraMFDPage), "EnableCamera")]
//    public static class CAT_SCamPatch
//    {
//        public static void Prefix(FlybyCameraMFDPage __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return;
//            //Debug.unityLogger.logEnabled = Main.logging;
//            var traverseF1 = Traverse.Create(__instance);

//            var flyCamEnabledFBCMFD = (bool) traverseF1.Field("flyCamEnabled").GetValue();
//            var previewEnabledFBCMFD = (bool) traverseF1.Field("previewEnabled").GetValue();
            
//            if (flyCamEnabledFBCMFD)
//            {
//                __instance.SetupFlybyPosition((FlybyCameraMFDPage.SpectatorBehaviors) (-1));
//                return;
//            }
            
//            if (previewEnabledFBCMFD)
//            {
//                __instance.previewObject.SetActive(true);
//            }
            
//            traverseF1.Field("flyCamEnabled").SetValue(true);

//            flyCamEnabledFBCMFD = true;

//            traverseF1.Field("cameraStartTime").SetValue(Time.time);
//            __instance.flybyCam.gameObject.SetActive(true);
//            __instance.flybyCam.transform.parent = null;

//            __instance.playerAudioListener.enabled = !__instance.cameraAudio;
//            __instance.cameraAudioListener.enabled = __instance.cameraAudio;
//            FloatingOrigin.instance.OnOriginShift += __instance.OnCamOriginShift;
//            __instance.SetupFlybyPosition();
//        }
//    }


//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(FlybyCameraMFDPage.SCamNVGController), "OnPreCull")]
//    public static class SU35_PatchNVGPreCull
//    {
//        public static bool Prefix(FlybyCameraMFDPage.SCamNVGController __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;
//            //Debug.unityLogger.logEnabled = Main.logging;
//            var traverse1 = Traverse.Create(__instance);

//            if (!__instance.specCamNVG) return false;

//            if (__instance.specCamNVG.enabled)
//            {
//                if (!__instance.doIllum) return false;
//                traverse1.Field("illumEnabled").SetValue(true);
//                //__instance.illumEnabled = true;
//                __instance.nvg.EnableIlluminator();

//                return false;
//            }
            
//            traverse1.Field("hidNvg").SetValue(true);
//            //__instance.hidNvg = true;

//            var nvgScaleIDPull = traverse1.Field("nvgScaleID").GetValue();
//            Shader.SetGlobalFloat(nvgScaleIDPull.ToString(), 0f);
//            return false;
//        }
//    }


//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(HPEquipGun), "Shake")]
//    public static class CAT_PatchHPEquipGun
//    {
//        public static bool Prefix(HPEquipGun __instance)
//        {
//            //__instance.shaker.Shake(UnityEngine.Random.onUnitSphere * __instance.shakeMagnitude);
//            //FlybyCameraMFDPage.ShakeSpectatorCamera(30f * __instance.shakeMagnitude / (FlybyCameraMFDPage.instance.flybyCam.transform.position - __instance.GetFireTransform().position).sqrMagnitude);
//            return false;
//        }
//    }


//    // TODO: false-prefix, need to replace
//    [HarmonyPatch(typeof(HUDMaskToggler), "SetMask")]
//    public static class CAT_HUDMaskTogglePatch
//    {
//        public static bool Prefix(bool maskEnabled, HUDMaskToggler __instance)
//        {
//            if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
//                return true;

//            if (__instance.alwaysFPVOnly && maskEnabled)
//            {
//                return false;
//            }
            
//            var traverse = Traverse.Create(__instance);
//            var displayObjectsP = (Transform[]) traverse.Field("displayObjects").GetValue();
//            var hudCanvas = AircraftApi.GetChildWithName(Main.AircraftCustom, "HUDCanvas", true);
//            var displayObjectsHud = hudCanvas.GetComponentsInChildren<Transform>(true);

//            foreach (var t in displayObjectsHud)
//            {
//                if (!t) continue;
//                if (__instance.alwaysFPVOnly)
//                {
//                    t.gameObject.layer = 28;
//                }
//                else
//                {
//                    t.gameObject.layer = maskEnabled ? 5 : 28;
//                }
//            }

//            traverse.Field("displayObjects").SetValue(displayObjectsP);

//            foreach (var mask in __instance.masks)
//            {
//                if (mask)
//                {
//                    mask.enabled = maskEnabled;
//                }
//            }

//            foreach (var image in __instance.images)
//            {
//                if (image)
//                {
//                    image.enabled = maskEnabled;
//                }
//            }

//            if (__instance.alwaysFPVOnly)
//            {
//                __instance.canvasObject.layer = 28;
//            }
//            else
//            {
//                hudCanvas.layer = maskEnabled ? 5 : 28;
//            }

//            //__instance.isMasked = maskEnabled;
//            traverse.Field("isMasked").SetValue("maskEnabled");
//            return false;
//        }
//    }
//}