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
    
    [HarmonyPatch(typeof(LoadoutConfigurator), "UpdateNodes")]
    public class UpdateNodesPatch
    {
        private static int aircraftNodeLength;

        public static bool Prefix(LoadoutConfigurator __instance)
        {
            __instance.wm.gameObject.GetComponent<MassUpdater>().UpdateMassObjects();
            Debug.Log("HpNodes Length = " + __instance.hpNodes.Length);
            aircraftNodeLength = Main.aircraftMirage.GetComponent<WeaponManager>().hardpointTransforms.Length;
            Debug.Log("aircraft HpNodes Length = " + aircraftNodeLength);

            for (int i = 0; i< aircraftNodeLength; i++)
            {
                Debug.Log("HpNodes Count: " + i);
                __instance.hpNodes[i].configurator = __instance;
                __instance.hpNodes[i].UpdateInfo(__instance.equips[i], i);
            }
            __instance.fullInfo.UpdateUI();
            return false;
        }
    }


  

    [HarmonyPatch(typeof(VehicleConfigSceneSetup), "Start")]
    public class VehicleConfigStartPatch
    {
        private static GameObject f26LC;

        public static bool Prefix(VehicleConfigSceneSetup __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Traverse traverse = Traverse.Create(__instance);
            if (PilotSaveManager.currentVehicle == null)
            {
                LoadingSceneController.LoadSceneImmediate("ReadyRoom");
                return true;
            }
            Debug.Log("VCS1.0");
            PilotSaveManager.LoadPilotsFromFile();
            Debug.Log("VCS1.1");
            PlayerVehicle currentVehicle = PilotSaveManager.currentVehicle;
            Debug.Log("VCS1.1.1");
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Main.aircraftPrefab);
            Main.aircraftMirage = gameObject;
            Debug.Log("VCS1.1.2");
            gameObject.transform.position = __instance.loadoutSpawnTransform.TransformPoint(currentVehicle.loadoutSpawnOffset);
            Debug.Log("VCS1.1.3");
            gameObject.transform.rotation = __instance.loadoutSpawnTransform.rotation;
            Debug.Log("VCS1.2");
            PlayerVehicleSetup component = gameObject.GetComponent<PlayerVehicleSetup>();
            component.SetToConfigurationState();
            WheelsController component2 = gameObject.GetComponent<WheelsController>();
            Debug.Log("VCS1.3");
            if (component2)
            {
                component2.SetBrakeLock(1);
            }
            Debug.Log("VCS1.4");
            gameObject.SetActive(true);
            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Main.aircraftLoadoutConfiguratorPrefab);
            
            gameObject2.transform.position = __instance.loadoutSpawnTransform.position;
            gameObject2.transform.rotation = __instance.loadoutSpawnTransform.rotation;
            gameObject2.SetActive(true);
            Debug.Log("VCS1.5");
            traverse.Field("config").SetValue(gameObject2.GetComponent<LoadoutConfigurator>());
            traverse.Field("config").Field("wm").SetValue(gameObject.GetComponent<WeaponManager>());
            LoadoutConfigurator LC1 = gameObject2.GetComponent<LoadoutConfigurator>();
            Debug.Log("VCS1.6");
            component.StartUsingConfigurator(LC1);
            CampaignSave campaignSave = PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID);
            Debug.Log("VCS1.7");
            List<string> allAvailableEquipStrings = new List<string>();
            if (PilotSaveManager.currentCampaign.isCustomScenarios && PilotSaveManager.currentCampaign.isStandaloneScenarios)
            {
                Debug.Log("VCS1.8");
                List<string> allowedEquips = VTResources.GetScenario(PilotSaveManager.currentScenario.scenarioID, PilotSaveManager.currentCampaign).allowedEquips;
                foreach (string item in allowedEquips)
                {
                    allAvailableEquipStrings.Add(item);
                }
                if (campaignSave.currentWeapons != null)
                {
                    for (int i = 0; i < campaignSave.currentWeapons.Length; i++)
                    {
                        if (!allowedEquips.Contains(campaignSave.currentWeapons[i]))
                        {
                            campaignSave.currentWeapons[i] = string.Empty;
                        }
                    }
                }

            }
            else
            {
                Debug.Log("VCS1.9");
                foreach (string item2 in campaignSave.availableWeapons)
                {
                    allAvailableEquipStrings.Add(item2);
                }
            }
            Debug.Log("VCS1.10");
            traverse.Field("config").Field("availableEquipStrings").SetValue(allAvailableEquipStrings);
            Debug.Log("VCS1.10.1");
            PilotSaveManager.currentScenario.initialSpending = 0f;
            Debug.Log("VCS1.10.1" + " : " + LC1.name + ": " + campaignSave.campaignName);
            LC1.Initialize(campaignSave, false);
            Debug.Log("VCS1.11");
            if (PilotSaveManager.currentScenario.forcedEquips != null)
            {
                Debug.Log("VCS1.12");
                foreach (CampaignScenario.ForcedEquip forcedEquip in PilotSaveManager.currentScenario.forcedEquips)
                {
                    LC1.AttachImmediate(forcedEquip.weaponName, forcedEquip.hardpointIdx);
                    LC1.lockedHardpoints.Add(forcedEquip.hardpointIdx);
                }
            }
            if (campaignSave.currentWeapons != null)
            {
                for (int k = 0; k < campaignSave.currentWeapons.Length; k++)
                {
                    if (!LC1.lockedHardpoints.Contains(k) && !string.IsNullOrEmpty(campaignSave.currentWeapons[k]))
                    {
                        LC1.AttachImmediate(campaignSave.currentWeapons[k], k);
                    }
                }
            }
            Debug.Log("VCS1.13");
            ScreenFader.FadeIn(1f);
           return false;
        }

        public static void Postfix(VehicleConfigSceneSetup __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.activeInHierarchy)
                    Debug.Log(go + " is an active object");
            }

            f26LC = GameObject.Find("FA26-LoadoutConfigurator(Clone)");
            Debug.Log("foundlc: " + f26LC);
            UnityEngine.Object.Destroy(f26LC);
            Traverse traverse = Traverse.Create(__instance);
            Debug.Log("VCS patch");

            try
            {
                Debug.Log("VCS patch a");

                WeaponManager Comp = Main.aircraftMirage.GetComponent<WeaponManager>();
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("VCS patch b");

                Main.aircraftMirage = UnityEngine.Object.Instantiate<GameObject>(Main.aircraftPrefab);
                traverse.Field("config").Field("wm").SetValue(Main.aircraftMirage.GetComponent<WeaponManager>());
                
            }
            Debug.Log("VCS patch c"); 
        }
    }
    
    [HarmonyPatch(typeof(LoadoutConfigurator), "Initialize")]
    public class LoadoutConfigStartPatch
    {

        public static bool Prefix(LoadoutConfigurator __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            bool flag = !AircraftInfo.AircraftSelected || VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                Transform transform = AircraftAPI.GetChildWithName(__instance.gameObject, "vtImage", true).transform;
                
                for (int j = 0; j <= 9; j++)
                {
                    __instance.lockedHardpoints.Remove(j);
                }
                result = true;
            }
            return result;
        }

        public static void Postfix(LoadoutConfigurator __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            bool flag = !AircraftInfo.AircraftSelected || VTOLAPI.GetPlayersVehicleEnum() != VTOLVehicles.FA26B;
            if (!flag)
            {
                Debug.Log("fuel: " + __instance.fuel);
                WeaponManager component = Main.aircraftMirage.GetComponent<WeaponManager>();
                Debug.Log("wm: " + __instance.wm);
                Debug.Log("wmhp: " + __instance.wm.hardpointTransforms.Length);
                __instance.wm = component;
                Rigidbody component2 = Main.aircraftMirage.GetComponent<Rigidbody>();
                __instance.vehicleRb = component2;
                FuelTank component3 = Main.aircraftMirage.GetComponent<FuelTank>();
                __instance.SetNormFuel(component3.fuelFraction);
                Traverse traverse = Traverse.Create(__instance);
                traverse.Field("fuelTank").SetValue(component3);
                GameObject childWithName = AircraftAPI.GetChildWithName(__instance.gameObject, "left", false);
                GameObject childWithName2 = AircraftAPI.GetChildWithName(childWithName.gameObject, "title", false);
                Text component4 = childWithName2.GetComponent<Text>();
                component4.text = 
"Mirage";
                WheelsController component5 = Main.aircraftMirage.GetComponent<WheelsController>();
                bool flag2 = component5;
                if (flag2)
                {
                    component5.SetBrakeLock(0);
                }
                Main.aircraftMirage.SetActive(true);
                Debug.Log("fuel: " + __instance.fuel);
                Debug.Log("wm: " + __instance.wm);
                Debug.Log("wmhp: " + __instance.wm.hardpointTransforms.Length);
                int transformsSize = __instance.wm.hardpointTransforms.Length;
                Transform[] hardpointTransforms = __instance.wm.hardpointTransforms;
                Traverse traverse2 = Traverse.Create(__instance);
                traverse2.Field("hpTransforms").SetValue(hardpointTransforms);
                traverse2.Field("hpTransforms").Field("Size").SetValue(transformsSize);

                __instance.CalculateTotalThrust();
                Vector3 position = __instance.transform.position;
                Quaternion rotation = __instance.transform.rotation;
                Debug.Log(string.Concat(new object[]
                {
                    "PL x:",
                    position.x,
                    "PL y:",
                    position.y,
                    "PL z:",
                    position.z
                }));
                Debug.Log("Updating Location as in the Config Screen");
                position.x -= 1f;
                position.y -= 0f;
                position.z -= 0.6f;
                rotation.y += 0.5f;
                Debug.Log(string.Concat(new object[]
                {
                    "PL x:",
                    position.x,
                    "PL y:",
                    position.y,
                    "PL z:",
                    position.z
                }));
                Main.aircraftMirage.transform.position = position;
                Main.aircraftMirage.transform.rotation = rotation;
            }
        }

        //Coroutine that detaches all the weapons
        public static IEnumerator DetachRoutine(LoadoutConfigurator config)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            yield return new WaitForSeconds(1);

            Debug.Log("Hardpoint count: " + config.wm.hardpointTransforms.Length);
            for (int i = 0; i < config.wm.hardpointTransforms.Length; i++)
            {

                config.DetachImmediate(i);
            }
        }

    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "EquipCompatibilityMask")]
    public static class EquipCompatibilityPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, HPEquippable equip)
        {

            Debug.unityLogger.logEnabled = Main.logging;
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


                
            }
            //equip.allowedHardpoints = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15";



            return true;
        }
    }
    [HarmonyPatch(typeof(LoadoutConfigurator), "AttachRoutine")]
    public static class AttachRoutinePatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, int hpIdx, string weaponName)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("Running AttachRoutine");
            Debug.Log("Running AttachRoutine 1");
            AttachRoutinePatch.StartConfigRoutine(hpIdx, weaponName, __instance);
            return false;
        }

        public static void StartConfigRoutine(int hpIdx, string weaponName, LoadoutConfigurator __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            try
            {
                __instance.StartCoroutine(AttachRoutinePatch.attachRoutine(hpIdx, weaponName, __instance));
            }
            catch (NullReferenceException ex)
            {
                bool flag = weaponName != null && __instance != null;
                bool flag2 = flag;
                if (flag2)
                {
                    Debug.LogError("Wrong parenting!");
                }
                throw ex;
            }
        }

        public static IEnumerator attachRoutine(int hpIdx, string weaponName, LoadoutConfigurator __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("Running Attach Routine");
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: -6"
            }));
            Traverse traverse2 = Traverse.Create(__instance);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: -5"
            }));
            AttachRoutinePatch.detachRoutinesList = (Coroutine[])traverse2.Field("detachRoutines").GetValue();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: -4"
            }));
            Dictionary<string, EqInfo> allWeaponPrefabsOutput = new Dictionary<string, EqInfo>();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: -3"
            }));
            allWeaponPrefabsOutput = (Dictionary<string, EqInfo>)traverse2.Field("allWeaponPrefabs").GetValue();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: -2"
            }));
            Transform[] hpTransformsList = (Transform[])traverse2.Field("hpTransforms").GetValue();
            Debug.Log("HPT1 =" + hpTransformsList.Length);
            Transform[] hptransformsListNew = __instance.wm.hardpointTransforms;
            traverse2.Field("hpTransforms").SetValue(hptransformsListNew);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: -1.5"
            }));
            object iwbObject = traverse2.Field("iwbAttach").GetValue();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: -1"
            }));
            Coroutine[] attachRoutinesList = (Coroutine[])traverse2.Field("attachRoutines").GetValue();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: -0.5"
            }));
            List<AudioSource> hpAudioSourcesList = (List<AudioSource>)traverse2.Field("hpAudioSourcesList").GetValue();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 0"
            }));
            Debug.Log("HPT1 =" + hpTransformsList.Length);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 1"
            }));
            Coroutine[] detachRoutines = (Coroutine[])traverse2.Field("detachRoutines").GetValue();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 1.1"
            }));
            bool flag = detachRoutines[hpIdx] != null;
            bool flag2 = flag;
            if (flag2)
            {
                Debug.Log(string.Concat(new object[]
                {
                    "HP: ",
                    hpIdx,
                    " W:",
                    weaponName,
                    ", AR:: 1.2"
                }));
                yield return detachRoutines[hpIdx];
                Debug.Log(string.Concat(new object[]
                {
                    "HP: ",
                    hpIdx,
                    " W:",
                    weaponName,
                    ", AR:: 1.3"
                }));
                detachRoutines = (Coroutine[])traverse2.Field("detachRoutines").GetValue();
            }
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 2"
            }));
            bool flag3 = !allWeaponPrefabsOutput.ContainsKey(weaponName);
            if (flag3)
            {
                __instance.UpdateNodes();
                yield break;
            }
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 3"
            }));
            GameObject instantiated = allWeaponPrefabsOutput[weaponName].GetInstantiated();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 4"
            }));
            InternalWeaponBay iwb = __instance.GetWeaponBay(hpIdx);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 5"
            }));
            Transform weaponTf = instantiated.transform;
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 6"
            }));
            Transform[] allTfs = (Transform[])traverse2.Field("hpTransforms").GetValue();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 7"
            }));
            Transform hpTf = allTfs[hpIdx];
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 8"
            }));
            __instance.equips[hpIdx] = weaponTf.GetComponent<HPEquippable>();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 9"
            }));
            __instance.equips[hpIdx].OnConfigAttach(__instance);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 10"
            }));
            weaponTf.rotation = hpTf.rotation;
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 11"
            }));
            Vector3 localPos = new Vector3(0f, -4f, 0f);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 12"
            }));
            weaponTf.position = hpTf.TransformPoint(localPos);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 13"
            }));
            __instance.UpdateNodes();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 14"
            }));
            Vector3 tgt = new Vector3(0f, 0f, 0.5f);
            weaponTf.parent = hpTf;
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 19"
            }));
            weaponTf.localPosition = tgt;
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 20"
            }));
            weaponTf.localRotation = Quaternion.identity;
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 21"
            }));
            __instance.vehicleRb.AddForceAtPosition(Vector3.up * __instance.equipImpulse, __instance.wm.hardpointTransforms[hpIdx].position, ForceMode.Impulse);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 22"
            }));
            AudioSource[] allAudioTfs = (AudioSource[])traverse2.Field("hpAudioSources").GetValue();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 23"
            }));
            allAudioTfs[hpIdx].PlayOneShot(__instance.attachAudioClip);
            __instance.attachPs.transform.position = hpTf.position;
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 24"
            }));
            __instance.attachPs.FireBurst();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 25"
            }));
            yield return new WaitForSeconds(0.2f);
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 26"
            }));
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 12"
            }));
            bool flag4 = iwb;
            if (flag4)
            {
                iwb.UnregisterOpenReq(iwbObject);
            }
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 13"
            }));
            weaponTf.localPosition = Vector3.zero;
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 13.1"
            }));
            __instance.UpdateNodes();
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 13.2"
            }));
            Debug.Log(string.Concat(new object[]
            {
                "HP: ",
                hpIdx,
                " W:",
                weaponName,
                ", AR:: 13.3"
            }));
            yield break;
        }

        public static Coroutine[] detachRoutinesList;

        private static ExternalOptionalHardpoints extHardpoint;
    }
}

    [HarmonyPatch(typeof(LoadoutConfigurator), "SaveConfig")]
    public static class SaveConfigPatch
    {
        private static CampaignSave campaignSaveOutput;

        public static bool Prefix(LoadoutConfigurator __instance)
        {
        Debug.unityLogger.logEnabled = CustomAircraftTemplate.Main.logging;
        Loadout loadout = new Loadout();
            
            loadout.normalizedFuel = __instance.fuel / 3160;
            Debug.Log("Fuel: " + loadout.normalizedFuel);
            loadout.hpLoadout = new string[__instance.equips.Length];

            Debug.Log("Spot 2");
            Traverse traverse2 = Traverse.Create(__instance);
            Debug.Log("Spot 3.5");
            campaignSaveOutput = (CampaignSave)traverse2.Field("campaignSave").GetValue();
            Debug.Log("Spot 4");

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
                    Debug.Log("Spot 6");
                }
            }
            loadout.cmLoadout = new int[__instance.cms.Count];
            for (int j = 0; j < __instance.cms.Count; j++)
            {
                loadout.cmLoadout[j] = __instance.cms[j].count;
                Debug.Log("Spot 7");
            }
            if (campaignSaveOutput != null)
            {
                Debug.Log("Spot 8");
                campaignSaveOutput.currentFuel = __instance.fuel / 3160;
            }
            Debug.Log("Spot 9");
            traverse2.Field("campaignSave").Field("currentFuel").SetValue(campaignSaveOutput.currentFuel);
            traverse2.Field("campaignSave").Field("currentWeapons").SetValue(campaignSaveOutput.currentWeapons);

            VehicleEquipper.loadout = loadout;
            return false;
        }
    }

    //
        

