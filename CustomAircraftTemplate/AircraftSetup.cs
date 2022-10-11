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

namespace CAT
{
    //internal class AircraftSetup
    //{
    //    private static GameObject _tgp;
    //    private static GameObject _targetCamera;
    //    private static Camera _tgCam;
    //    private static RenderTexture _rt;
    //    private static GameObject _eotstf;
    //    private static GameObject _mpSyncsObj;
    //    private static TGPSync _mpSyncsTgpSync;
    //    private static GameObject _radarUic;
    //    private static GameObject _tpc;
    //    private static GameObject _tsc;
    //    private static TacticalSituationController _tscComp;
    //    private static GameObject _mfdtrt;
    //    private static GameObject _tc;

    //    public static void SetUpEots()
    //    {
    //        if (Main.IsEotsSetUp == 1) { return; };
            
    //        try
    //        {
    //            _tgp = AircraftApi.GetChildWithName(Main.AircraftCustom, "fa26_tgp", true);
    //            _targetCamera = AircraftApi.GetChildWithName(_tgp, "TargetingCam", true);
    //            _tgCam = _targetCamera.GetComponent<Camera>();
    //            _tgp.GetComponent<OpticalTargeter>();
    //            Main.AircraftCustom.GetComponentInChildren<TargetingMFDPage>(true);

    //            _rt = _tgCam.targetTexture;
    //        }
    //        catch
    //        {
    //            _eotstf = AircraftApi.GetChildWithName(Main.AircraftCustom, "EOTSHolder", true);
    //            var sevTf = VTResources.GetPlayerVehicle("F-45A").vehiclePrefab;
    //            var sevTfEots = AircraftApi.GetChildWithName(sevTf, "EOTS", false);

    //            var eotsObj = UnityEngine.Object.Instantiate(sevTfEots, _eotstf.transform.position, _eotstf.transform.rotation);

    //            eotsObj.transform.SetParent(_eotstf.transform);
    //            eotsObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
    //            eotsObj.transform.localPosition = new Vector3(0f, 0f, 0f);

    //            _mpSyncsObj = AircraftApi.GetChildWithName(Main.AircraftCustom, "MPSyncs", true);
    //            _mpSyncsTgpSync = _mpSyncsObj.GetComponent<TGPSync>();
    //            _mpSyncsTgpSync.targeter = eotsObj.GetComponent<OpticalTargeter>();

    //            eotsObj.GetComponent<OpticalTargeter>().actor = Main.AircraftCustom.GetComponent<Actor>();
    //            eotsObj.GetComponent<OpticalTargeter>().wm = Main.AircraftCustom.GetComponent<WeaponManager>();

    //            Main.AircraftCustom.GetComponent<WeaponManager>().opticalTargeter = eotsObj.GetComponent<OpticalTargeter>();

    //            _radarUic = AircraftApi.GetChildWithName(Main.AircraftCustom, "RadarUIController", true);
                
    //            _radarUic.GetComponent<MFDRadarUI>().tgp = eotsObj.GetComponent<OpticalTargeter>();
    //            _tc = AircraftApi.GetChildWithName(eotsObj, "TargetingCam", true);
    //            _tpc = AircraftApi.GetChildWithName(Main.AircraftCustom, "TargetingPageController", true);
                
    //            _tpc.GetComponent<TargetingMFDPage>().opticalTargeter = eotsObj.GetComponent<OpticalTargeter>();
    //            _tpc.GetComponent<TargetingMFDPage>().targetingCamera = _tc.GetComponent<Camera>();
    //            _tpc.GetComponent<TargetingMFDPage>().cameraFog = _tc.GetComponent<CameraFogSettings>();
    //            _tpc.GetComponent<TargetingMFDPage>().targetIlluminator = _tc.GetComponent<IlluminateVesselsOnRender>();

    //            _tsc = AircraftApi.GetChildWithName(Main.AircraftCustom, "TacticalSituationController", true);
    //            _tsc.GetComponent<TacticalSituationController>().opticalTargeter = eotsObj.GetComponent<OpticalTargeter>();

    //            _tscComp = _tsc.GetComponent<TacticalSituationController>();
    //            _tscComp.CollectFromTGP();
    //            _mfdtrt = AircraftApi.GetChildWithName(Main.AircraftCustom, "MFDTargetingRT", true);
    //            _mfdtrt.GetComponent<RawImage>().texture = _tc.GetComponent<Camera>().targetTexture;
    //            return;
    //        }
    //        var mfdtgpPage = AircraftApi.GetChildWithName(Main.AircraftCustom, "MFDTargetingRT", false);
    //        mfdtgpPage.GetComponent<RawImage>().texture = _rt;

    //        Main.IsEotsSetUp = 1;
    //    }
    //}
}
