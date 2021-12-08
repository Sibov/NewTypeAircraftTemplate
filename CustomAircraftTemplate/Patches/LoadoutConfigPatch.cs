using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomAircraftTemplate
{

    [HarmonyPatch(typeof(VehicleConfigSceneSetup), "Start")]
    public class VehicleConfigStartPatch
    {
        public static void Postfix(VehicleConfigSceneSetup __instance)
        {
            Traverse traverse = Traverse.Create(__instance);
            Debug.Log("VCS patch");

            Main.aircraftMirage = GameObject.Instantiate(Main.aircraftPrefab);

            object LC = traverse.Field("config").GetValue();

            Traverse traverse2 = Traverse.Create(LC);
            traverse2.Field("wm").SetValue(Main.aircraftMirage.GetComponent<WeaponManager>());
            Rigidbody mirageRB = Main.aircraftMirage.GetComponent<Rigidbody>();
            traverse2.Field("vehicleRB").SetValue(mirageRB);
            return;

        }
    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "Initialize")]
    public class LoadoutConfigStartPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance)
        {

            if (!AircraftInfo.AircraftSelected || VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B) return true;

            Transform parent = AircraftAPI.GetChildWithName(__instance.gameObject, "vtImage", false).transform;

            const string hpInfo = "HardpointInfo";
            for (int i = 10; i <= 15; i++)
            {
                parent.Find(hpInfo + " (" + i + ")").gameObject.SetActive(false);
                Debug.Log("Hardpoint cleared: " + i);

            }

            for (int i = 0; i <= 9; i++)
            {
                __instance.lockedHardpoints.Remove(i);
            }

            //HPConfiguratorNode[] nodearray = __instance.hpNodes;
            //Array.Resize(ref nodearray, nodearray.Length - 6);



            return true;
        }

        public static void Postfix(LoadoutConfigurator __instance)
        {

            if (!AircraftInfo.AircraftSelected || VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B) return;

            //Detaches the weapons from the aircraft
            //   Main.instance.StartCoroutine(DetachRoutine(__instance));
            Debug.Log("fuel: " + __instance.fuel);



            WeaponManager mirageWM = Main.aircraftMirage.GetComponent<WeaponManager>();
            Debug.Log("wm: " + __instance.wm);
            Debug.Log("wmhp: " + __instance.wm.hardpointTransforms.Length);


            __instance.wm = mirageWM;

            
            FuelTank mirageFT = Main.aircraftMirage.GetComponent<FuelTank>();
            __instance.SetNormFuel(mirageFT.fuelFraction);
            Traverse traverse2 = Traverse.Create(__instance);
            traverse2.Field("fuelTank").SetValue(mirageFT);
            
                
            GameObject lcleftpanel = AircraftAPI.GetChildWithName(__instance.gameObject, "left", false);
            GameObject lcleftpaneltitle = AircraftAPI.GetChildWithName(lcleftpanel.gameObject, "title", false);
            Text lcleftpaneltitletext = lcleftpaneltitle.GetComponent<Text>();
            lcleftpaneltitletext.text = AircraftInfo.AircraftNickName;
           // lcleftpaneltitletext.fontSize = 40;

            WheelsController component2 = Main.aircraftMirage.GetComponent<WheelsController>();
            if (component2)
            {
                component2.SetBrakeLock(0);
            }
            Main.aircraftMirage.SetActive(true);



            Debug.Log("fuel: " + __instance.fuel);
            Debug.Log("wm: " + __instance.wm);
            Debug.Log("wmhp: " + __instance.wm.hardpointTransforms.Length);
            Transform[] hptransformsList = __instance.wm.hardpointTransforms;

            Traverse traverse = Traverse.Create(__instance);
            traverse.Field("hpTransforms").SetValue(hptransformsList);




            __instance.CalculateTotalThrust();



            Vector3 PlaneLocation = __instance.transform.position;
            Quaternion PlaneRotation = __instance.transform.rotation;
            Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);


            Debug.Log("Updating Location as in the Config Screen");
            PlaneLocation.x = PlaneLocation.x - 1f;
            PlaneLocation.y = PlaneLocation.y - 0f;
            PlaneLocation.z = PlaneLocation.z - 0.6f;
            PlaneRotation.y = PlaneRotation.y + 0.5f;





            Debug.Log("PL x:" + PlaneLocation.x + "PL y:" + PlaneLocation.y + "PL z:" + PlaneLocation.z);

            Main.aircraftMirage.transform.position = PlaneLocation;
            Main.aircraftMirage.transform.rotation = PlaneRotation;


            return;
        }



    }
    [HarmonyPatch(typeof(LoadoutConfigurator), "EquipCompatibilityMask")]
    public static class EquipCompatibilityPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, HPEquippable equip)
        {


            if (!AircraftInfo.AircraftSelected) return true;

            Debug.Log("Name:" + AircraftInfo.AircraftName);

            if (true) // fuck you c ; work on manners you ape
            {
                Debug.Log("Section 11");


                // this creates a dictionary of all the wepaons and where they can be mounted, just alter the second string per weapon according to the wepaon you want.
                Dictionary<string, string> allowedhardpointbyweapon = new Dictionary<string, string>();

                allowedhardpointbyweapon.Add("fa26_gun", "0");
                allowedhardpointbyweapon.Add("fa26-cft", "");
                allowedhardpointbyweapon.Add("fa26_agm89x1", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_agm161", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_aim9x2", "");
                allowedhardpointbyweapon.Add("fa26_aim9x3", "");
                allowedhardpointbyweapon.Add("fa26_cagm-6", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_cbu97x1", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_droptank", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_droptankXL", "");
                allowedhardpointbyweapon.Add("fa26_gbu12x1", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_gbu12x2", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_gbu12x3", "");
                allowedhardpointbyweapon.Add("fa26_gbu38x1", "1,4,5,6,7,8,9");
                allowedhardpointbyweapon.Add("fa26_gbu38x2", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_gbu38x3", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_gbu39x4uFront", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_gbu39x4uRear", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_harmx1", "1,4,5,6,7,8,9");
                allowedhardpointbyweapon.Add("fa26_harmx1dpMount", "1,4,5,6,7,8,9");
                allowedhardpointbyweapon.Add("fa26_iris-t-x1", "2,3");
                allowedhardpointbyweapon.Add("fa26_iris-t-x2", "4,5");
                allowedhardpointbyweapon.Add("fa26_iris-t-x3", "");
                allowedhardpointbyweapon.Add("fa26_maverickx1", "4,5");
                allowedhardpointbyweapon.Add("fa26_maverickx3", "4,5");
                allowedhardpointbyweapon.Add("fa26_mk82HDx1", "1,4,5,6,7,8,9");
                allowedhardpointbyweapon.Add("fa26_mk82HDx2", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_mk82HDx3", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_mk82x2", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_mk82x3", "4,5,9");
                allowedhardpointbyweapon.Add("fa26_mk83x1", "1,4,5,6,7,8,9");
                allowedhardpointbyweapon.Add("fa26_sidearmx1", "1,4,5,6,7,8,9");
                allowedhardpointbyweapon.Add("fa26_sidearmx2", "");
                allowedhardpointbyweapon.Add("fa26_sidearmx3", "");
                allowedhardpointbyweapon.Add("fa26_tgp", "1,6");
                // allowedhardpointbyweapon.Add("cagm-6", "");
                //allowedhardpointbyweapon.Add("cbu97x1", "11, 12");
                //allowedhardpointbyweapon.Add("gbu38x1", "11, 12");
                //allowedhardpointbyweapon.Add("gbu38x2", "");
                //allowedhardpointbyweapon.Add("gbu38x3", "");
                //allowedhardpointbyweapon.Add("gbu39x3", "");
                //allowedhardpointbyweapon.Add("gbu39x4u", "");
                allowedhardpointbyweapon.Add("h70-4x4", "4, 5");
                allowedhardpointbyweapon.Add("h70-x7", "4, 5");
                allowedhardpointbyweapon.Add("h70-x19", "4, 5");
                allowedhardpointbyweapon.Add("hellfirex4", "4, 5");
                //allowedhardpointbyweapon.Add("iris-t-x1", "11, 12");
                //allowedhardpointbyweapon.Add("iris-t-x2", "");
                //allowedhardpointbyweapon.Add("iris-t-x3", "");
                //allowedhardpointbyweapon.Add("m230", "");
                //allowedhardpointbyweapon.Add("marmx1", "");
                //allowedhardpointbyweapon.Add("maverickx1", "11, 12");
                //allowedhardpointbyweapon.Add("maverickx3", "");
                //allowedhardpointbyweapon.Add("mk82HDx1", "11, 12");
                //allowedhardpointbyweapon.Add("mk82HDx2", "");
                //allowedhardpointbyweapon.Add("mk82HDx3", "");
                //allowedhardpointbyweapon.Add("mk82x1", "11, 12");
                //allowedhardpointbyweapon.Add("mk82x2", "");
                //allowedhardpointbyweapon.Add("mk82x3", "");
                //allowedhardpointbyweapon.Add("sidearmx1", "11, 12");
                //allowedhardpointbyweapon.Add("sidearmx2", "");
                //allowedhardpointbyweapon.Add("sidearmx3", "");
                allowedhardpointbyweapon.Add("sidewinderx1", "2, 3");
                //allowedhardpointbyweapon.Add("sidewinderx2", "");
                //allowedhardpointbyweapon.Add("sidewinderx3", "");
                allowedhardpointbyweapon.Add("af_aim9", "2, 3");
                allowedhardpointbyweapon.Add("af_amraam", "1,4,5,6,7,8");
                //allowedhardpointbyweapon.Add("af_amraamRail", "11, 12");
                //allowedhardpointbyweapon.Add("af_amraamRailx2", "");
                //allowedhardpointbyweapon.Add("af_dropTank", "");
                //allowedhardpointbyweapon.Add("af_maverickx1", "11, 12");
                //allowedhardpointbyweapon.Add("af_maverickx3", "11, 12");
                //allowedhardpointbyweapon.Add("af_mk82", "11, 12");
                //allowedhardpointbyweapon.Add("af_tgp", "14");
                allowedhardpointbyweapon.Add("h70-x7ld", "4, 5");
                allowedhardpointbyweapon.Add("h70-x7ld-under", "");
                allowedhardpointbyweapon.Add("h70-x14ld-under", "");
                allowedhardpointbyweapon.Add("h70-x14ld", "4,5");


                Dictionary<string, string> weaponnamechanges = new Dictionary<string, string>();
                weaponnamechanges.Add("af_aim9", "Magic II");

                Debug.Log("Before Equipment: " + equip.name + ", Allowed on" + equip.allowedHardpoints);



                if (allowedhardpointbyweapon.ContainsKey(equip.name))
                {
                    equip.allowedHardpoints = (string)allowedhardpointbyweapon[equip.name];
                    Debug.Log("Equipment: " + equip.name + ", Allowed on" + equip.allowedHardpoints);
                }
                else
                {
                    Debug.Log("Equipment: " + equip.name + ", not in dictionary");
                }


                if (weaponnamechanges.ContainsKey(equip.name))
                {
                    equip.shortName = (string)weaponnamechanges[equip.name];
                    equip.fullName = (string)weaponnamechanges[equip.name];


                    Debug.Log("Equipment: " + equip.name + ", Allowed on" + equip.allowedHardpoints);
                }
            }
            //equip.allowedHardpoints = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15";



            return true;
        }
    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "AttachRoutine")]
    public static class AttachRoutinePatch
    {
        public static Coroutine[] detachRoutinesList;
        private static ExternalOptionalHardpoints extHardpoint;

        public static bool Prefix(LoadoutConfigurator __instance, int hpIdx, string weaponName)
        {
            Debug.Log("Running AttachRoutine");

            //__instance.DetachRoutine(hpIdx);
            Debug.Log("Running AttachRoutine 1");

            StartConfigRoutine(hpIdx, weaponName, __instance);
            return false;

            // Simons Patch Code
            /*
            Debug.Log("Running Attach Routine");
            
            int i = 1;


            Debug.Log("Stop: -6");
            Traverse traverse2 = Traverse.Create(__instance);

            Debug.Log("Stop: -5");

            detachRoutinesList = (Coroutine[])traverse2.Field("detachRoutines").GetValue();
            Debug.Log("Stop: -4");
            Dictionary<string, EqInfo> allWeaponPrefabsOutput = new Dictionary<string, EqInfo>();
            Debug.Log("Stop: -3");
            allWeaponPrefabsOutput = (Dictionary<string, EqInfo>)traverse2.Field("allWeaponPrefabs").GetValue();
            Debug.Log("Stop: -2");
            Transform[] hpTransformsList = (Transform[])traverse2.Field("hpTransforms").GetValue();
            Debug.Log("HPT1 =" + hpTransformsList.Length);
            Transform[] hptransformsListNew = __instance.wm.hardpointTransforms;
            traverse2.Field("hpTransforms").SetValue(hptransformsListNew);
            Debug.Log("Stop: -1.5");
            object iwbObject = traverse2.Field("iwbAttach").GetValue();
            Debug.Log("Stop: -1");
            Coroutine[] attachRoutinesList = (Coroutine[])traverse2.Field("attachRoutines").GetValue();

            Debug.Log("Stop: -0.5");
            List<AudioSource> hpAudioSourcesList = (List<AudioSource>)traverse2.Field("hpAudioSourcesList").GetValue();
            Debug.Log("Stop: 0");

            Debug.Log("HPT1 =" + hpTransformsList.Length);
            Debug.Log("Stop: 1");

            if (detachRoutinesList[hpIdx] != null)
            {

               return true;
            }

            Debug.Log("Stop: 2"); 

            if (!allWeaponPrefabsOutput.ContainsKey(weaponName))
            {
                __instance.UpdateNodes();
                return false;
            }
            Debug.Log("Stop: 3");

            GameObject instantiated = allWeaponPrefabsOutput[weaponName].GetInstantiated();
            Transform weaponTf = instantiated.transform;
            Transform hpTf = hpTransformsList[hpIdx];
            Debug.Log("Stop: 3.5"); i++;
            InternalWeaponBay iwb = __instance.GetWeaponBay(hpIdx);
            if (iwb)
            {
                iwb.RegisterOpenReq(iwbObject);
            }
            Debug.Log("Stop: 4");
            __instance.equips[hpIdx] = weaponTf.GetComponent<HPEquippable>();
            __instance.equips[hpIdx].OnConfigAttach(__instance);


           Debug.Log("Stop: 5");
            //weaponTf.rotation = hpTf.rotation;
            Debug.Log("Stop: 5.1");
            Vector3 localPos = new Vector3(0f, -4f, 0f);
            Debug.Log("Stop: 5.2");
            //weaponTf.position = hpTf.TransformPoint(localPos);
            Debug.Log("Stop: 5.3");
            __instance.UpdateNodes();
            Debug.Log("Stop: 6");
            Vector3 tgt = new Vector3(0f, 0f, 0.5f);
            if (hpIdx == 0 || iwb)
            {
                tgt = Vector3.zero;
            }
            Debug.Log("Stop: 7");
            
            
            Debug.Log("Stop: 8");
            weaponTf.parent = hpTf;
            weaponTf.localPosition = tgt;
            Debug.Log("Stop: 9");

            weaponTf.localRotation = Quaternion.identity;

            __instance.vehicleRb.AddForceAtPosition(Vector3.up * __instance.equipImpulse, __instance.wm.hardpointTransforms[hpIdx].position, ForceMode.Impulse);
            Debug.Log("Stop: 10");

            //hpAudioSourcesList[hpIdx].PlayOneShot(__instance.attachAudioClip);
            Debug.Log("Stop: 10.1");

            __instance.attachPs.transform.position = hpTf.position;

            Debug.Log("Stop: 10.2");
            __instance.attachPs.FireBurst();
            Debug.Log("Stop: 11");

            //yield return new WaitForSeconds(0.2f);
            /*while (weaponTf.localPosition.sqrMagnitude > 0.001f)
            {
                weaponTf.localPosition = Vector3.MoveTowards(weaponTf.localPosition, Vector3.zero, 4f * Time.deltaTime);
                return false;
            }

            Debug.Log("Stop: 12");
            if (iwb)

            {
                iwb.UnregisterOpenReq(iwbObject);
            }

            Debug.Log("Stop: 13"); 
            weaponTf.localPosition = Vector3.zero;
            Debug.Log("Stop: 13.1");

            __instance.UpdateNodes();
            Debug.Log("Stop: 13.2");

            attachRoutinesList[hpIdx] = null;
            Debug.Log("Stop: 13.3");

            //traverse2.Field("attachRoutines").SetValue(attachRoutinesList);
    
            return true ;
            */
        }

        public static void StartConfigRoutine(int hpIdx, string weaponName, LoadoutConfigurator __instance)
        {
            try
            {
                __instance.StartCoroutine(attachRoutine(hpIdx, weaponName, __instance));
            }
            catch (NullReferenceException ex)
            {
                bool flag = weaponName != null && __instance != null;
                if (flag)
                {
                    Debug.LogError("wrong parent");
                }
                throw ex;
            }
        }

        public static IEnumerator attachRoutine(int hpIdx, string weaponName, LoadoutConfigurator __instance)
        {
            Debug.Log("Running Attach Routine");

            int i = 1;


            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: -6");
            Traverse traverse2 = Traverse.Create(__instance);

            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: -5");

            detachRoutinesList = (Coroutine[])traverse2.Field("detachRoutines").GetValue();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: -4");
            Dictionary<string, EqInfo> allWeaponPrefabsOutput = new Dictionary<string, EqInfo>();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: -3");
            allWeaponPrefabsOutput = (Dictionary<string, EqInfo>)traverse2.Field("allWeaponPrefabs").GetValue();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: -2");
            Transform[] hpTransformsList = (Transform[])traverse2.Field("hpTransforms").GetValue();
            Debug.Log("HPT1 =" + hpTransformsList.Length);
            Transform[] hptransformsListNew = __instance.wm.hardpointTransforms;
            traverse2.Field("hpTransforms").SetValue(hptransformsListNew);
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: -1.5");
            object iwbObject = traverse2.Field("iwbAttach").GetValue();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: -1");
            Coroutine[] attachRoutinesList = (Coroutine[])traverse2.Field("attachRoutines").GetValue();

            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: -0.5");
            List<AudioSource> hpAudioSourcesList = (List<AudioSource>)traverse2.Field("hpAudioSourcesList").GetValue();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 0");

            Debug.Log("HPT1 =" + hpTransformsList.Length);
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 1");
            Coroutine[] detachRoutines = (Coroutine[])traverse2.Field("detachRoutines").GetValue();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 1.1");
            bool flag = detachRoutines[hpIdx] != null;
            if (flag)
            {
                Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 1.2");
                yield return detachRoutines[hpIdx];
                Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 1.3");
                detachRoutines = (Coroutine[])traverse2.Field("detachRoutines").GetValue();
            }

            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 2");

            if (!allWeaponPrefabsOutput.ContainsKey(weaponName))
            {
                __instance.UpdateNodes();
                yield break;
            }
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 3");

            GameObject instantiated = allWeaponPrefabsOutput[weaponName].GetInstantiated();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 4");

            InternalWeaponBay iwb = __instance.GetWeaponBay(hpIdx);
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 5");

            Transform weaponTf = instantiated.transform;
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 6");

            Transform[] allTfs = (Transform[])traverse2.Field("hpTransforms").GetValue();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 7");
            Transform hpTf = allTfs[hpIdx];
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 8");
            __instance.equips[hpIdx] = weaponTf.GetComponent<HPEquippable>();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 9");
            __instance.equips[hpIdx].OnConfigAttach(__instance);
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 10");
            weaponTf.rotation = hpTf.rotation;
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 11");
            Vector3 localPos = new Vector3(0f, -4f, 0f);
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 12");
            weaponTf.position = hpTf.TransformPoint(localPos);
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 13");
            __instance.UpdateNodes();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 14");
            Vector3 tgt = new Vector3(0f, 0f, 0.5f);


            weaponTf.parent = hpTf;
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 19");

            weaponTf.localPosition = tgt;
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 20");

            weaponTf.localRotation = Quaternion.identity;
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 21");

            __instance.vehicleRb.AddForceAtPosition(Vector3.up * __instance.equipImpulse, __instance.wm.hardpointTransforms[hpIdx].position, ForceMode.Impulse);
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 22");

            AudioSource[] allAudioTfs = (AudioSource[])traverse2.Field("hpAudioSources").GetValue();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 23");
            allAudioTfs[hpIdx].PlayOneShot(__instance.attachAudioClip);
            __instance.attachPs.transform.position = hpTf.position;
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 24");
            __instance.attachPs.FireBurst();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 25");
            yield return new WaitForSeconds(0.2f);
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 26");

            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 12");
            if (iwb)

            {
                iwb.UnregisterOpenReq(iwbObject);
            }

            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 13");
            weaponTf.localPosition = Vector3.zero;
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 13.1");

            __instance.UpdateNodes();
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 13.2");

            //attachRoutinesList[hpIdx] = null;
            Debug.Log("HP: " + hpIdx + " W:" + weaponName + ", AR:: 13.3");

            //traverse2.Field("attachRoutines").SetValue(attachRoutinesList);

            yield break;

        }

    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "SaveConfig")]
    public static class SaveConfigPatch
    {
        private static CampaignSave campaignSaveOutput;

        public static bool Prefix(LoadoutConfigurator __instance)
        {
            Loadout loadout = new Loadout();
            FuelTank mirageFT = Main.aircraftMirage.GetComponent<FuelTank>();
            
            loadout.normalizedFuel = __instance.fuel / mirageFT.maxFuel;
            Debug.Log("Fuel: " + loadout.normalizedFuel);
            loadout.hpLoadout = new string[__instance.equips.Length];

            Debug.Log("savec 2");
            Traverse traverse2 = Traverse.Create(__instance);
            Debug.Log("savec 3.5");
            campaignSaveOutput = (CampaignSave)traverse2.Field("campaignSave").GetValue();
            Debug.Log("savec 4");

            Debug.Log("__instance.equips.Length = " + __instance.equips.Length);

            Debug.Log("campaignSaveOutput.currentWeapons.Length = " + campaignSaveOutput.currentWeapons.Length);

            for (int i = 0; i < __instance.equips.Length; i++)
            {
                if (__instance.equips[i] != null)
                {
                    string name = __instance.equips[i].gameObject.name;
                    loadout.hpLoadout[i] = name;
                    Debug.Log("i" + i + "name = " + name);
                    if (campaignSaveOutput != null)
                    {

                        campaignSaveOutput.currentWeapons[i] = name;
                        Debug.Log("CSO" + i + "name = " + name);

                    }
                }
                else if (campaignSaveOutput != null)
                {
                    campaignSaveOutput.currentWeapons[i] = string.Empty;
                    Debug.Log("savec 6");
                }
            }
            loadout.cmLoadout = new int[__instance.cms.Count];
            for (int j = 0; j < __instance.cms.Count; j++)
            {
                loadout.cmLoadout[j] = __instance.cms[j].count;
                Debug.Log("savec 7");
            }
            if (campaignSaveOutput != null)
            {
                Debug.Log("Spot savec 8");
                campaignSaveOutput.currentFuel = __instance.fuel / 3160;
            }
            Debug.Log("Spot savec 9");
            traverse2.Field("campaignSave").Field("currentFuel").SetValue(campaignSaveOutput.currentFuel);
            traverse2.Field("campaignSave").Field("currentWeapons").SetValue(campaignSaveOutput.currentWeapons);

            VehicleEquipper.loadout = loadout;
            return false;
        }
    }

    [HarmonyPatch(typeof(HPConfiguratorFullInfo), "AttachSymmetry")]
    public static class AttachSymmetryPatch
    {
        public static bool Prefix(string weaponName, int hpIdx, HPEquippable eqPrefab, HPConfiguratorFullInfo __instance)
        {
            if (__instance.configurator.wm.symmetryIndices != null && hpIdx < __instance.configurator.wm.symmetryIndices.Length)
            {
                Debug.Log("ASPatch 1.1 : " + weaponName + ", " + hpIdx + ", " + eqPrefab);
                int num = __instance.configurator.wm.symmetryIndices[hpIdx];
                Debug.Log("ASPatch 1.2");

                if (num < 0 || __instance.configurator.lockedHardpoints.Contains(num))
                {
                    Debug.Log("ASPatch 1.3");

                    return false;
                }
                if (__instance.configurator.TryGetEqInfo(weaponName, out EqInfo eqInfo) && eqInfo.IsCompatibleWithHardpoint(num))
                {
                    __instance.configurator.Attach(weaponName, num);
                    Debug.Log("ASPatch 1.4");

                }

            }
            return false;
        }
    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "Attach")]
    public static class LoadoutConfiguratorAttachPatch
    {
        public static bool Prefix(string weaponName, int hpIdx, LoadoutConfigurator __instance)
        {
            Debug.Log("AttPatch 1.0");

            Traverse traverse2 = Traverse.Create(__instance);
            Coroutine[] attachRoutinesList = (Coroutine[])traverse2.Field("attachRoutines").GetValue();
            Debug.Log("AttPatch 1.1");

            VehiclePart componentInParent = __instance.wm.hardpointTransforms[hpIdx].GetComponentInParent<VehiclePart>();
            Debug.Log("AttPatch 1.2");

            if ((componentInParent && componentInParent.hasDetached) || componentInParent.partDied)
            {
                Debug.Log("AttPatch 1.3");

                return false;
            }
            Debug.Log("AttPatch 1.4");

            __instance.Detach(hpIdx);
            Debug.Log("AttPatch 1.5");

            Coroutine ARinfo = __instance.StartCoroutine(__instance.AttachRoutine(hpIdx, weaponName));

            traverse2.Field("attachRoutines").SetValue(ARinfo);

            //__instance.attachRoutines[hpIdx] = ARinfo;
            attachRoutinesList[hpIdx] = null;

            Debug.Log("AttPatch 1.6");

            return false;
        }

        

    }

    [HarmonyPatch(typeof(HPConfiguratorFullInfo), "EquipButton")]
    public static class EquipButtonPatch
    {

        public static bool Prefix (HPConfiguratorFullInfo __instance)
        {
            Traverse traverse2 = Traverse.Create(__instance);
            Int32 hpIdxPull = (Int32)traverse2.Field("hpIdx").GetValue();
            Debug.Log("EBpatch 1.0");

            if (__instance.configurator.lockedHardpoints.Contains(hpIdxPull))
            {
                Debug.Log("EBpatch 1.1");


                return false;
            }

            HPEquippable[]  availableEquipsPull =(HPEquippable[])traverse2.Field("availableEquips").GetValue();
            Debug.Log("EBpatch 1.2");

            if (availableEquipsPull.Length < 1)
            {
                Debug.Log("EBpatch 1.3");

                return false;
            }

            string name = availableEquipsPull[__instance.currIdx].gameObject.name;
            Debug.Log("EBpatch 1.4");

            __instance.configurator.Attach(name, hpIdxPull);
            Debug.Log("EBpatch 1.5");

            traverse2.Field("equippedIdx").SetValue(__instance.currIdx);
                        if (__instance.configurator.symmetryMode)
            {
                Debug.Log("EBpatch 1.6");

                __instance.AttachSymmetry(name, hpIdxPull, availableEquipsPull[__instance.currIdx]);
            }
            Debug.Log("EBpatch 1.7");

            __instance.UpdateUI();
            return false;
        }
    }

}

    