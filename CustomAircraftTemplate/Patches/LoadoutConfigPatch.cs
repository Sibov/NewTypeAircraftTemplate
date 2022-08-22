using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModLoader;
using VTOLVR.Multiplayer;
using System.Reflection;

namespace CustomAircraftTemplateGAV25B
{


    [HarmonyPatch(typeof(VehicleConfigSceneSetup), "Start")]
    public class GAV25B_VehicleConfigStartPatch
    {


        public static void Postfix(VehicleConfigSceneSetup __instance)
        {
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
            { return; }
            PlayerVehicle currentVehicle = PilotSaveManager.currentVehicle;
            Debug.Log("GAV25B VCS1.0");
            Main.aircraftCustom = GameObject.Find(currentVehicle.vehicleName+"(Clone)");
            Debug.Log("GAV25B VCS1.1");
        }
    }


   

    [HarmonyPatch(typeof(LoadoutConfigurator), "Initialize")]
    public static class GAV25B_LCInit
    {
        public static void Postfix(LoadoutConfigurator __instance)
        {
            Traverse traverse2 = Traverse.Create(__instance);


            Dictionary<string, EqInfo> allWeaponPrefabsOutput = new Dictionary<string, EqInfo>();
            Debug.Log("GAV25B LCI: 1.0 : ");
            allWeaponPrefabsOutput = (Dictionary<string, EqInfo>)traverse2.Field("allWeaponPrefabs").GetValue();
            Debug.Log("GAV25B LCI: 1.1");
            traverse2.Field("unlockedWeaponPrefabs").SetValue(allWeaponPrefabsOutput);

        }

    }


            
    [HarmonyPatch(typeof(LoadoutConfigurator), "EquipCompatibilityMask")]
    public static class GAV25B_EquipCompatibilityPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, HPEquippable equip)
        {
            // if (!AircraftInfo.AircraftSelected)
            // { return true; }
            //Debug.unityLogger.logEnabled = Main.logging;
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;

            Debug.Log("GAV25B Name:" + Main.customAircraftPV.vehicleName);

            
            if (true) // fuck you c ; work on manners you ape
            {
                Debug.Log("GAV25B Section 11 New");


                // this creates a dictionary of all the wepaons and where they can be mounted, just alter the second string per weapon according to the wepaon you want.
                Dictionary<string, string> allowedhardpointbyweapon = new Dictionary<string, string>();
                allowedhardpointbyweapon.Add("asf-srmx1", "9,10");
                //allowedhardpointbyweapon.Add("GIAT30Gun", "0");
                //allowedhardpointbyweapon.Add("asf30_gun", "0");
                //allowedhardpointbyweapon.Add("fa26-cft", "");
                //allowedhardpointbyweapon.Add("fa26_agm89x1", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_agm161", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_aim9x2", "");
                //allowedhardpointbyweapon.Add("fa26_aim9x3", "");
                
                //allowedhardpointbyweapon.Add("fa26_cbu97x1", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_droptank", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_droptankXL", "");
                //allowedhardpointbyweapon.Add("fa26_gbu12x1", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_gbu12x2", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_gbu12x3", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_gbu38x1", "1,4,5,6,7,8,9");
                //allowedhardpointbyweapon.Add("fa26_gbu38x2", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_gbu38x3", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_gbu39x4uFront", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_gbu39x4uRear", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_harmx1", "1,4,5,6,7,8,9");
                //allowedhardpointbyweapon.Add("fa26_harmx1dpMount", "1,4,5,6,7,8,9");
                //allowedhardpointbyweapon.Add("fa26_iris-t-x1", "2,3,4,5");
                //allowedhardpointbyweapon.Add("fa26_iris-t-x2", "4,5");
                //allowedhardpointbyweapon.Add("fa26_iris-t-x3", "4,5");
                //allowedhardpointbyweapon.Add("fa26_maverickx1", "4,5");
                //allowedhardpointbyweapon.Add("fa26_maverickx3", "4,5");
                //allowedhardpointbyweapon.Add("fa26_mk82HDx1", "1,4,5,6,7,8,9");
                //allowedhardpointbyweapon.Add("fa26_mk82HDx2", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_mk82HDx3", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_mk82x2", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_mk82x3", "4,5,9");
                //allowedhardpointbyweapon.Add("fa26_mk83x1", "1,4,5,6,7,8,9");
                //allowedhardpointbyweapon.Add("fa26_sidearmx1", "1,4,5,6,7,8,9");
                //allowedhardpointbyweapon.Add("fa26_sidearmx2", "");
                //allowedhardpointbyweapon.Add("fa26_sidearmx3", "");
                //allowedhardpointbyweapon.Add("fa26_tgp", "1,6");
                //allowedhardpointbyweapon.Add("h70-4x4", "4, 5");
                //allowedhardpointbyweapon.Add("h70-x7", "4, 5");
                //allowedhardpointbyweapon.Add("h70-x19", "4, 5");
                //allowedhardpointbyweapon.Add("hellfirex4", "4, 5");

                //allowedhardpointbyweapon.Add("sidewinderx1", "2, 3");

                //allowedhardpointbyweapon.Add("af_aim9", "2, 3");
                //allowedhardpointbyweapon.Add("af_amraam", "1,4,5,6,7,8");

                //allowedhardpointbyweapon.Add("h70-x7ld", "4, 5");
                //allowedhardpointbyweapon.Add("h70-x7ld-under", "");
                //allowedhardpointbyweapon.Add("h70-x14ld-under", "");
                //allowedhardpointbyweapon.Add("h70-x14ld", "4,5");
                

                Debug.Log("GAV25B Before Equipment: " + equip.name + ", Allowed on" + equip.allowedHardpoints);

                

                if (allowedhardpointbyweapon.ContainsKey(equip.name))
                {
                    equip.allowedHardpoints = (string)allowedhardpointbyweapon[equip.name];
                    Debug.Log("GAV25B Equipment: " + equip.name + ", Allowed on" + equip.allowedHardpoints);
                }
                else
                {
                    Debug.Log("GAV25B Equipment: " + equip.name + ", not in dictionary");
                }
               


            }
            //equip.allowedHardpoints = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15";
            


            return true;
        }
    }


    
    [HarmonyPatch(typeof(ReArmingPoint), "FinalBeginReArm")]
    public static class GAV25B_RAPFBAPatch
    {
        public static bool Prefix(ReArmingPoint __instance)
        {
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;

            Debug.Log("GAV25B RAPFBAP 1.0");
            AudioController.instance.SetExteriorOpening("rearming", 1f);
            PlayerVehicle currentVehicle = PilotSaveManager.currentVehicle;
            Debug.Log("GAV25B RAPFBAP 1.1");
            Transform transform = __instance.transform;
            GameObject gameObject = Main.aircraftCustom;
            Vector3 b = Vector3.zero;
            Debug.Log("GAV25B RAPFBAP 1.2");
            RaycastHit raycastHit;
            Debug.Log("GAV25B RAPFBAP 1.3");
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out raycastHit, 100f, 1))
            {
                Debug.Log("GAV25B RAPFBAP 1.4");
                b = transform.InverseTransformPoint(raycastHit.point);
            }
            Debug.Log("GAV25B RAPFBAP 1.5");
            FlightSceneManager.instance.playerActor.flightInfo.PauseGCalculations();
            gameObject.transform.position = transform.TransformPoint(currentVehicle.playerSpawnOffset + b);
            gameObject.transform.rotation = Quaternion.AngleAxis(currentVehicle.spawnPitch, transform.right) * transform.rotation;
            Debug.Log("GAV25B RAPFBAP 1.6");
            gameObject.GetComponent<PlayerVehicleSetup>().LandVehicle(transform);
            WeaponManager component = gameObject.GetComponent<WeaponManager>();
            Debug.Log("GAV25B RAPFBAP 1.7");
            Traverse trav2 = Traverse.Create(__instance);
            HPEquippable[] mpOriginEquips = (HPEquippable[])trav2.Field("mp_OrigEquips").GetValue();
            Debug.Log("GAV25B RAPFBAP 1.8");
            if (VTOLMPUtils.IsMultiplayer())
            {
                Debug.Log("GAV25B RAPFBAP 1.9");
                if (mpOriginEquips == null || mpOriginEquips.Length != component.equipCount)
                {
                    Debug.Log("GAV25B RAPFBAP 1.10");
                    mpOriginEquips = new HPEquippable[component.equipCount];
                }
                for (int i = 0; i < component.equipCount; i++)
                {
                    Debug.Log("GAV25B RAPFBAP 1.11");
                    mpOriginEquips[i] = component.GetEquip(i);
                }
                Debug.Log("GAV25B RAPFBAP 1.12");
                trav2.Field("mp_OrigEquips").SetValue(mpOriginEquips);

            }
            Debug.Log("GAV25B RAPFBAP 1.13");
            //Transform TcamRigParent = (Transform)trav2.Field("camRigParent").GetValue();

            Debug.Log("GAV25B RAPFBAP 1.14" );
            Transform TcamRigParent = VRHead.instance.transform.parent.parent;
            trav2.Field("camRigParent").SetValue(TcamRigParent);
            Debug.Log("GAV25B RAPFBAP 1.15: x=" + TcamRigParent.position.x + ", y=" + TcamRigParent.position.y + ", z = " + TcamRigParent.position.z);
            gameObject.SetActive(true);
            EjectionSeat componentInChildren = AircraftAPI.GetChildWithName(Main.aircraftCustom, "EjectorSeat", false).GetComponent<EjectionSeat>();
            Debug.Log("GAV25B RAPFBAP 1.16");
            if (componentInChildren)
            {
                Debug.Log("GAV25B RAPFBAP 1.17");
                if (componentInChildren.pilotModel)
                {
                    componentInChildren.pilotModel.SetActive(false);
                }
                }
            Debug.Log("GAV25B RAPFBAP 1.18");
            VTOLQuickStart componentInChildren2 = gameObject.GetComponentInChildren<VTOLQuickStart>();
            Debug.Log("GAV25B RAPFBAP 1.18.1");
            if (componentInChildren2.throttle)
            {
                Debug.Log("GAV25B RAPFBAP 1.19");
                componentInChildren2.throttle.RemoteSetThrottle(0f);
            }
            Debug.Log("GAV25B RAPFBAP 1.20");
            componentInChildren2.quickStopComponents.ApplySettings();
            Debug.Log("GAV25B RAPFBAP 1.21");
            PlayerVehicleSetup TvehicleSetup = (PlayerVehicleSetup)trav2.Field("vehicleSetup").GetValue();
            Debug.Log("GAV25B RAPFBAP 1.22");
            TvehicleSetup = gameObject.GetComponentInChildren<PlayerVehicleSetup>();
            if (TvehicleSetup && TvehicleSetup.OnBeginRearming != null)
            {
                Debug.Log("GAV25B RAPFBAP 1.23");
                TvehicleSetup.OnBeginRearming.Invoke();
            }
            Debug.Log("GAV25B RAPFBAP 1.24");
            GameObject TconfigObject = (GameObject)trav2.Field("configObject").GetValue();

            Debug.Log("GAV25B RAPFBAP 1.25");
            TconfigObject = UnityEngine.Object.Instantiate<GameObject>(currentVehicle.loadoutConfiguratorPrefab);
            TconfigObject.transform.parent = transform;
            Debug.Log("GAV25B RAPFBAP 1.26");
            TconfigObject.transform.position = transform.position;
            TconfigObject.transform.rotation = transform.rotation;
            Debug.Log("GAV25B RAPFBAP 1.27");
            TconfigObject.SetActive(true);
            trav2.Field("configObject").SetValue(TconfigObject);
            Debug.Log("GAV25B RAPFBAP 1.28");
            LoadoutConfigurator Tconfig = (LoadoutConfigurator)trav2.Field("config").GetValue();

            Tconfig = TconfigObject.GetComponent<LoadoutConfigurator>();
            Debug.Log("GAV25B RAPFBAP 1.29");
            Tconfig.wm = component;
            Tconfig.canRefuel = __instance.canRefuel;
            Debug.Log("GAV25B RAPFBAP 1.30");
            Tconfig.canArm = __instance.canArm;
            trav2.Field("config").SetValue(Tconfig);
            Debug.Log("GAV25B RAPFBAP 1.31");
            if (Tconfig.equipRigTf)
            {
                Debug.Log("GAV25B RAPFBAP 1.32");
                float z = currentVehicle.playerSpawnOffset.z - currentVehicle.loadoutSpawnOffset.z;
                Debug.Log("GAV25B RAPFBAP 1.33");
                Vector3 vector = Tconfig.equipRigTf.localPosition;
                Debug.Log("GAV25B RAPFBAP 1.34");
                vector += new Vector3(0f, 0f, z);
                vector.y = 0f;
                Debug.Log("GAV25B RAPFBAP 1.35");
                Tconfig.equipRigTf.localPosition = vector;
            }
            trav2.Field("config").SetValue(Tconfig);

            if (TvehicleSetup)
            {
                Debug.Log("GAV25B RAPFBAP 1.36");
                TvehicleSetup.StartUsingConfigurator(Tconfig);
            }
            trav2.Field("vehicleSetup").SetValue(TvehicleSetup);

            foreach (VRHandController vrhandController in VRHandController.controllers)
            {
                Debug.Log("GAV25B RAPFBAP 1.37");
                if (vrhandController.activeInteractable)
                {
                    Debug.Log("GAV25B RAPFBAP 1.38");
                    vrhandController.ReleaseFromInteractable();
                }
            }
            Debug.Log("GAV25B RAPFBAP 1.39");
            VRHead.instance.transform.parent.parent = Tconfig.seatTransform;
            Debug.Log("GAV25B RAPFBAP 1.40");
            VRHead.instance.transform.parent.localPosition = VRHead.playAreaPosition;
            Debug.Log("GAV25B RAPFBAP 1.41");
            VRHead.instance.transform.parent.localRotation = VRHead.playAreaRotation;
            Debug.Log("GAV25B RAPFBAP 1.42");
            CampaignSave campaignSave = PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName).GetCampaignSave(PilotSaveManager.currentCampaign.campaignID);

            Debug.Log("GAV25B RAPFBAP 1.43");
            Tconfig.availableEquipStrings = new List<string>();
            List<string> availableWeapons = campaignSave.availableWeapons;
            Debug.Log("GAV25B RAPFBAP 1.44");
            if (VTOLMPUtils.IsMultiplayer())
            {
                Debug.Log("GAV25B RAPFBAP 1.45");
                PlayerInfo localPlayer = VTOLMPSceneManager.instance.localPlayer;
                List<string> equipment = VTOLMPSceneManager.instance.GetMPSpawn(localPlayer.team, localPlayer.selectedSlot).equipment.equipment;
                Debug.Log("GAV25B RAPFBAP 1.46");
                using (List<GameObject>.Enumerator enumerator2 = PilotSaveManager.currentVehicle.allEquipPrefabs.GetEnumerator())
                {
                    Debug.Log("GAV25B RAPFBAP 1.47");

                    while (enumerator2.MoveNext())
                    {
                        Debug.Log("GAV25B RAPFBAP 1.48");
                        GameObject gameObject2 = enumerator2.Current;
                        if (!equipment.Contains(gameObject2.gameObject.name))
                        {
                            Debug.Log("GAV25B RAPFBAP 1.49");
                            Tconfig.availableEquipStrings.Add(gameObject2.gameObject.name);
                        }
                    }
                    goto IL_4F0;
                }
            }
            trav2.Field("config").SetValue(Tconfig);
            Debug.Log("GAV25B RAPFBAP 1.50");
            foreach (string item in campaignSave.availableWeapons)
            {
                if (!(VTScenario.current.gameVersion > new GameVersion(1, 3, 0, 30, GameVersion.ReleaseTypes.Testing)) || VTScenario.current.allowedEquips.Contains(item))
                {
                    Debug.Log("GAV25B RAPFBAP 1.51");
                    Tconfig.availableEquipStrings.Add(item);
                }
            }
            trav2.Field("config").SetValue(Tconfig);
        IL_4F0:
            Tconfig.Initialize(campaignSave, true);
            if (PilotSaveManager.currentScenario.forcedEquips != null)
            {
                Debug.Log("GAV25B RAPFBAP 1.52");
                foreach (CampaignScenario.ForcedEquip forcedEquip in PilotSaveManager.currentScenario.forcedEquips)
                {
                    Debug.Log("GAV25B RAPFBAP 1.53");
                    Tconfig.Attach(forcedEquip.weaponName, forcedEquip.hardpointIdx);
                    Tconfig.lockedHardpoints.Add(forcedEquip.hardpointIdx);
                }
            }
            Debug.Log("GAV25B RAPFBAP 1.54");
            trav2.Field("config").SetValue(Tconfig);
            Tconfig.UpdateNodes();
            Debug.Log("GAV25B RAPFBAP 1.5");
            trav2.Field("config").SetValue(Tconfig);
            __instance.StartCoroutine(__instance.SetRearmAnchorDelayed());
            return false;
        }
    }

    [HarmonyPatch(typeof(ReArmingPoint), "FinalEndReArm")]
    public static class GAV25B_RAPFERPatch
    {
        public static bool Prefix(ReArmingPoint __instance)
        {
            Debug.Log("GAV25B RAPFFER 1.0");
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            Traverse trav3 = Traverse.Create(__instance);
            Traverse trav4 = Traverse.Create(typeof(ReArmingPoint));

            LoadoutConfigurator trav3config = (LoadoutConfigurator)trav3.Field("config").GetValue();
            Transform trav3camrigParent = (Transform)trav3.Field("camRigParent").GetValue();
            Transform trav4camrigParent = (Transform)trav4.Field("camRigParent").GetValue();

            GameObject trav3configObject = (GameObject)trav3.Field("configObject").GetValue();
            UnityEngine.Object trav3active = (UnityEngine.Object)trav3.Field("active").GetValue();
            PlayerVehicleSetup trav3pvSetup = (PlayerVehicleSetup)trav3.Field("vehicleSetup").GetValue();
            HPEquippable[] trav3mp_OrigEquips = (HPEquippable[])trav3.Field("mp_OrigEquips").GetValue();
            Debug.Log("GAV25B RAPFFER 1.1");
            float totalFlightCost = trav3config.GetTotalFlightCost();
            AudioController.instance.SetExteriorOpening("rearming", 0f);
            Debug.Log("GAV25B RAPFFER 1.2");
            PilotSaveManager.currentScenario.inFlightSpending += totalFlightCost;
            Debug.Log("GAV25B RAPFFER 1.2.1:");
            Debug.Log("GAV25B x =" + trav3camrigParent.position.x + ", y=" + trav3camrigParent.position.y + ", z = " + trav3camrigParent.position.z);
            VRHead.instance.transform.parent.parent = trav3camrigParent;
            VRHead.instance.transform.parent.localPosition = VRHead.playAreaPosition;
            VRHead.instance.transform.parent.localRotation = VRHead.playAreaRotation;
            Debug.Log("GAV25B RAPFFER 1.3");
            GameObject gameObject = FlightSceneManager.instance.playerActor.gameObject;
            //GameObject gameObject = Main.aircraftCustom;
            Debug.Log("GAV25B RAPFFER 1.3.1 : " + gameObject.name);


            EjectionSeat componentInChildren = AircraftAPI.GetChildWithName(Main.aircraftCustom, "EjectorSeat", false).GetComponent<EjectionSeat>();
            if (componentInChildren)
            {
                Debug.Log("GAV25B RAPFFER 1.3.2 ");
                componentInChildren.pilotModel.SetActive(true);
            }
            Debug.Log("GAV25B RAPFFER 1.4");
            CommRadioSource componentInChildren2 = gameObject.GetComponentInChildren<CommRadioSource>();
            if (componentInChildren2)
            {
                Debug.Log("GAV25B RAPFFER 1.4.1 : ");
                componentInChildren2.SetAsRadioSource();
            }
            Debug.Log("GAV25B RAPFFER 1.5");
            foreach (VRHandController vrhandController in VRHandController.controllers)
            {
                Debug.Log("GAV25B RAPFFER 1.5.1");
                if (vrhandController.activeInteractable)
                {
                    Debug.Log("GAV25B RAPFFER 1.5.2");
                    vrhandController.ReleaseFromInteractable();
                }
            }
            Debug.Log("GAV25B RAPFFER 1.6");
            UnityEngine.Object.Destroy(trav3configObject);
            trav4.Field("active").SetValue(null);
            Debug.Log("GAV25B RAPFFER 1.7");
            if (FlightSceneManager.instance)
            {
                FlightSceneManager.instance.OnExitScene -= __instance.Instance_OnExitScene;
            }
            if (trav3pvSetup)
            {
                if (trav3pvSetup.OnEndRearming != null)
                {
                    trav3pvSetup.OnEndRearming.Invoke();
                }
                trav3pvSetup.EndUsingConfigurator(trav3config);
            }
            Debug.Log("GAV25B RAPFFER 1.8");
            //if (__instance.OnEndRearm != null)
            //{
            //    __instance.OnEndRearm();
            
            //var eventDelegate = (MulticastDelegate)__instance.GetType()
            //.GetField("OnEndRearm", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            //.GetValue(__instance);
            //if (eventDelegate == null) return true; // or print error then return, etc
            //foreach (var handler in eventDelegate.GetInvocationList())
            //{
            //    handler.Method.Invoke(handler.Target, new object[] { __instance, EventArgs.Empty });
            //}
                

                if (FlightSceneManager.instance.playerActor)
            {
                Debug.Log("GAV25B RAPFFER 1.8.1");
                __instance.voiceProfile.PlayMessage(GroundCrewVoiceProfile.GroundCrewMessages.ReturnedToVehicle);
                Debug.Log("GAV25B RAPFFER 1.8.1");
                FlightSceneManager.instance.playerActor.flightInfo.UnpauseGCalculations();
            }
            Debug.Log("GAV25B RAPFFER 1.9");
            if (VTOLMPUtils.IsMultiplayer() && gameObject)
            {
                WeaponManagerSync componentInChildren3 = gameObject.GetComponentInChildren<WeaponManagerSync>();
                Loadout loadout = new Loadout();
                loadout.cmLoadout = new int[]
                {
                9999,
                9999
                };
                loadout.normalizedFuel = gameObject.GetComponent<FuelTank>().fuelFraction;
                loadout.hpLoadout = new string[componentInChildren3.wm.equipCount];
                for (int i = 0; i < componentInChildren3.wm.equipCount; i++)
                {
                    if (componentInChildren3.wm.GetEquip(i) != trav3mp_OrigEquips[i])
                    {
                        loadout.hpLoadout[i] = componentInChildren3.wm.GetEquip(i).gameObject.name;
                    }
                }
                componentInChildren3.NetEquipWeapons(loadout, true);
            }
            return false;
        }

    }
    
}
    

    //
        

