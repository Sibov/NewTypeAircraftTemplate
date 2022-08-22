using Harmony;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace CustomAircraftTemplateGAV25B
{

    [HarmonyPatch(typeof(VTScenario), "LoadFromNode")]
    class GAV25B_GetLFNCheck
    {
        public static bool Prefix(VTScenario __instance, ConfigNode saveNode)
        {
            UnitCatalogue.UpdateCatalogue();
            Debug.Log("GAV25B LFN 1.0");
            ConfigNodeUtils.TryParseValue<GameVersion>(saveNode, "gameVersion", ref __instance.gameVersion);
            __instance.scenarioName = saveNode.GetValue("scenarioName");
            __instance.scenarioID = saveNode.GetValue("scenarioID");
            __instance.scenarioDescription = saveNode.GetValue("scenarioDescription");
            Debug.Log("GAV25B LFN 1.1");
            ConfigNodeUtils.TryParseValue<bool>(saveNode, "multiplayer", ref __instance.multiplayer);
            ConfigNodeUtils.TryParseValue<string>(saveNode, "campaignID", ref __instance.campaignID);
            ConfigNodeUtils.TryParseValue<int>(saveNode, "campaignOrderIdx", ref __instance.campaignOrderIdx);

            Debug.Log("GAV25B LFN 1.2");
            if (saveNode.HasValue("mapID"))
            {
                Debug.Log("GAV25B LFN 1.3");
                __instance.mapID = saveNode.GetValue("mapID");
            }
            else
            {
                Debug.Log("GAV25B LFN 1.4");
                __instance.mapID = VTMapManager.fetch.map.mapID;
            }
            Debug.Log("GAV25B LFN 1.5");
            __instance.globalValues.LoadFromScenarioNode(saveNode);
            __instance.units.LoadFromScenarioNode(saveNode);
            __instance.staticObjects.LoadFromScenarioNode(saveNode);
            __instance.paths.LoadFromScenarioNode(saveNode);
            __instance.waypoints.LoadFromScenarioNode(saveNode);
            Debug.Log("GAV25B LFN 1.6");
            __instance.groups.LoadFromScenarioNode(saveNode);
            __instance.conditionals.LoadFromScenarioNode(saveNode);
            __instance.timedEventGroups.LoadFromScenarioNode(saveNode);
            __instance.conditionalActions.LoadFromScenarioNode(saveNode);
            Debug.Log("GAV25B LFN 1.7");
            __instance.triggerEvents.LoadFromScenarioNode(saveNode);
            __instance.objectives.LoadFromScenarioNode(saveNode);
            __instance.bases.LoadFromScenarioNode(saveNode);
            Debug.Log("GAV25B LFN 1.8");
            __instance.sequencedEvents.LoadFromScenarioNode(saveNode);
            __instance.conditionals.GatherReferences();
            __instance.vehicle = VTResources.GetPlayerVehicle(saveNode.GetValue("vehicle"));
            Debug.Log("GAV25B LFN 1.9");
            if (saveNode.HasValue("allowedEquips"))
            {
                Debug.Log("GAV25B LFN 1.10");
                __instance.allowedEquips = ConfigNodeUtils.ParseList(saveNode.GetValue("allowedEquips"));
            }
            if (saveNode.HasValue("forcedEquips"))
            {
                Debug.Log("GAV25B LFN 1.11");
                __instance.forcedEquips = ConfigNodeUtils.ParseList(saveNode.GetValue("forcedEquips"));
            }
            if (saveNode.HasValue("equipsOnComplete"))
            {
                Debug.Log("GAV25B LFN 1.12");
                __instance.equipsOnComplete = ConfigNodeUtils.ParseList(saveNode.GetValue("equipsOnComplete"));
            }
            Debug.Log("GAV25B LFN 1.13");
            __instance.forceEquips = ConfigNodeUtils.ParseBool(saveNode.GetValue("forceEquips"));
            ConfigNodeUtils.TryParseValue<float>(saveNode, "normForcedFuel", ref __instance.normForcedFuel);
            Debug.Log("GAV25B LFN 1.14");
            __instance.equipsConfigurable = ConfigNodeUtils.ParseBool(saveNode.GetValue("equipsConfigurable"));
            __instance.isTraining = ConfigNodeUtils.ParseBool(saveNode.GetValue("isTraining"));
            __instance.baseBudget = ConfigNodeUtils.ParseFloat(saveNode.GetValue("baseBudget"));
            Debug.Log("GAV25B LFN 1.15");
            if (saveNode.HasValue("rtbWptID"))
            {
                Debug.Log("GAV25B LFN 1.16");
                __instance.rtbWptID = saveNode.GetValue("rtbWptID");
            }
            if (saveNode.HasValue("rtbWptID_B"))
            {
                Debug.Log("GAV25B LFN 1.17");
                __instance.rtbWptID_B = saveNode.GetValue("rtbWptID_B");
            }
            if (saveNode.HasValue("refuelWptID"))
            {
                Debug.Log("GAV25B LFN 1.18");
                __instance.refuelWptID = saveNode.GetValue("refuelWptID");
            }
            if (saveNode.HasValue("refuelWptID_B"))
            {
                Debug.Log("GAV25B LFN 1.19");
                __instance.refuelWptID_B = saveNode.GetValue("refuelWptID_B");
            }
            __instance.briefingNotes = ProtoBriefingNote.GetProtoBriefingsFromConfig(saveNode, false);
            if (__instance.multiplayer)
            {
                Debug.Log("GAV25B LFN 1.20");
                __instance.briefingNotesB = ProtoBriefingNote.GetProtoBriefingsFromConfig(saveNode, true);
                ConfigNodeUtils.TryParseValue<bool>(saveNode, "separateBriefings", ref __instance.separateBriefings);
            }

            Traverse trav = Traverse.Create(__instance);
            Debug.Log("GAV25B LFN 1.21");
            var resourceManifest2 = new List<string>();
            if (saveNode.HasNode("ResourceManifest"))
            {
                Debug.Log("GAV25B LFN 1.22");
                foreach (ConfigNode.ConfigValue configValue in saveNode.GetNode("ResourceManifest").GetValues())
                {
                    Debug.Log("GAV25B LFN 1.23");
                    resourceManifest2.Add(configValue.value);
                }
            }
            trav.Field("resourceManifest").SetValue(resourceManifest2);
            Debug.Log("GAV25B LFN 1.24");
            if (saveNode.HasValue("envName"))
            {
                Debug.Log("GAV25B LFN 1.25");
                __instance.envName = saveNode.GetValue("envName");
            }
            else
            {
                Debug.Log("GAV25B LFN 1.26");
                __instance.envName = "day";
            }
            Debug.Log("GAV25B LFN 1.27");
            ConfigNodeUtils.TryParseValue<QuicksaveManager.QSModes>(saveNode, "qsMode", ref __instance.qsMode);
            ConfigNodeUtils.TryParseValue<int>(saveNode, "qsLimit", ref __instance.qsLimit);
            Debug.Log("GAV25B LFN 1.28");
            ConfigNodeUtils.TryParseValue<bool>(saveNode, "selectableEnv", ref __instance.selectableEnv);
            __instance.ApplyBaseInfo();
            return false;
        }

    }

    [HarmonyPatch(typeof(VTResources), "LoadScenariosFromDir")]
    class GAV25B_VTR_LoadScenariosPatch
    {
        public static List<VTScenarioInfo> Helper(string parentDirectory, bool checkModified, bool enforceDirectoryName, bool decodeWSScenarios)
        {
            // replaced tail recursion with iteration here
            while (true)
            {
                var traverse = Traverse.Create(typeof(VTResources));

                var enforceDirInvalidFlag = false;
                var list = new List<VTScenarioInfo>();
                var skippedCount = 0;
                foreach (var path in Directory.GetDirectories(parentDirectory, "*", SearchOption.TopDirectoryOnly))
                {
                    var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                    var j = 0;
                    while (j < files.Length)
                    {
                        var text = files[j];
                        try
                        {
                            if (text.EndsWith(".vts") || (decodeWSScenarios && text.EndsWith(".vtsb")))
                            {
                                ConfigNode configNode;
                                configNode = text.EndsWith(".vtsb")
                                    ? VTSteamWorkshopUtils.ReadWorkshopConfig(text)
                                    : ConfigNode.LoadFromFile(text, true);
                                if (parentDirectory == VTResources.customScenariosDir)
                                {
                                    var fileName2 = Path.GetFileName(path);
                                    if (enforceDirectoryName && !fileName2.Equals(configNode.GetValue("scenarioID")))
                                    {
                                        enforceDirInvalidFlag = true;
                                        break;
                                    }
                                }

                                var vtscenarioInfo = new VTScenarioInfo(configNode, text);
                                var vehicle = vtscenarioInfo.vehicle;
                                if (vehicle != null && (VTResources.isEditorOrDevTools || vehicle.readyToFly))
                                {
                                    list.Add(vtscenarioInfo);
                                }
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            //Debug.LogError("KeyNotFoundException thrown when attempting to load VTScenario from " + text + ". It may be an outdated scenario file.");
                        }

                        goto __VTRPatch_JUMP_B;

                    __VTRPatch_JUMP_A:
                        j++;
                        continue;

                    __VTRPatch_JUMP_B:
                        if (!enforceDirInvalidFlag)
                            goto __VTRPatch_JUMP_A;

                        break;
                    }
                    if (enforceDirInvalidFlag)
                        break;
                }

                if (enforceDirInvalidFlag)
                {
                    //VTResources.RepairAllScenarioFilePaths(); // private invoke
                    var info = typeof(VTResources).GetMethod("RepairAllScenarioFilePaths",
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    info?.Invoke(null, null); // static so null obj, and no parameters so null

                    checkModified = false;
                    enforceDirectoryName = true;
                    decodeWSScenarios = false;
                    continue;
                }

                if (checkModified)
                {
                    Debug.Log("GAV25B - Skipped " + skippedCount + " scenarios (not modified).");
                }

                return list;
            }
        }

        public static bool Prefix(string parentDirectory, bool checkModified, bool enforceDirectoryName, bool decodeWSScenarios, ref List<VTScenarioInfo> __result)
        {
            __result = Helper(parentDirectory, checkModified, enforceDirectoryName, decodeWSScenarios);
            return false;
        }
    }


    [HarmonyPatch(typeof(VTResources), "LoadPlayerVehicles")]
    class GAV25B_PlayerVehiclePatch
    {
        static void Postfix()
        {
            //Debug.Log($"[PVPatch] Adding player vehicle {Main.customAircraftPV.vehicleName} to playerVehicles");
            var t = Traverse.Create(typeof(VTResources));
            var pvlist = (PlayerVehicleList)t.Field("playerVehicles").GetValue();

            pvlist.playerVehicles.Add(Main.customAircraftPV);
            t.Field("playerVehicles").SetValue(pvlist);
            //Debug.Log($"[PVPatch] Add complete!");
        }
    }

    [HarmonyPatch(typeof(VTResources), "GetPlayerVehicle")]
    class GAV25B_GetPlayerVehiclesCheck
    {
        public static bool Prefix(VTResources __instance, string vehicleName, ref PlayerVehicle __result)
        {
            if (Main.playerVehicleList == null)
            {
                VTResources.LoadPlayerVehicles();
            }
            foreach (PlayerVehicle playerVehicle in Main.playerVehicleList)
            {
                Debug.Log("GAV25B GPV - pvname = " + playerVehicle.vehicleName + ", " + vehicleName);
                if (playerVehicle.vehicleName == vehicleName)
                {
                    __result = playerVehicle;
                    return false;
                }
            }
            __result = null;
            return false;
        }


    }






    [HarmonyPatch(typeof(MeasurementManager), "Awake")]
    public static class GAV25B_MMPatch
    {
        public static bool Prefix(MeasurementManager __instance)
        {

            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            //Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("GAV25B MM Patch 1.0");
            Traverse traverse = Traverse.Create(__instance);
            Debug.Log("GAV25B MM Patch 1.1");
            if (PilotSaveManager.current != null && PilotSaveManager.currentVehicle != null)
            {
                Debug.Log("GAV25B MM Patch 1.2");
                VehicleSave vehicleSave = PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName);
                if (vehicleSave == null) { return false; }
                Debug.Log("GAV25B MM Patch 1.3");
                __instance.altitudeMode = vehicleSave.altitudeMode;
                Debug.Log("GAV25B MM Patch 1.4");
                __instance.distanceMode = vehicleSave.distanceMode;
                Debug.Log("GAV25B MM Patch 1.5");
                __instance.airspeedMode = vehicleSave.airspeedMode;

            }
            Debug.Log("GAV25B MM Patch 1.6");
            traverse.Field("flightInfo").SetValue(__instance.GetComponentInParent<FlightInfo>());
            Debug.Log("GAV25B MM Patch 1.7");
            //MeasurementManager.instance = __instance;

            return false;
        }
    }


    public static class CSIPatchUtils
    {
        /** Executes more iterations of the campaign select loop to add other planes' campaigns to our aircraft.
         *	>> It isn't exactly "better" yet, just gives us more flexibility to control it. Possible todo: make better
         */
        public static IEnumerator BetterSetupCampaignScreenRoutine(CampaignSelectorUI instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("GAV25B BSCSCR Patch 1.0");
            var traverse = Traverse.Create(instance);

            instance.loadingCampaignScreenObj.SetActive(true);
            Debug.Log("GAV25B BSCSCR Patch 1.1");
            bool wasInputEnabled = !ControllerEventHandler.eventsPaused;
            ControllerEventHandler.PauseEvents();
            VTScenarioEditor.returnToEditor = false;
            VTMapManager.nextLaunchMode = VTMapManager.MapLaunchModes.Scenario;
            PlayerVehicleSetup.godMode = false;
            Debug.Log("GAV25B BSCSCR Patch 1.2");
            instance.campaignDisplayObject.SetActive(true);
            instance.scenarioDisplayObject.SetActive(false);
            var _campaignsParent = (Transform)traverse.Field("campaignsParent").GetValue();
            Debug.Log("GAV25B BSCSCR Patch 1.3");
            if (_campaignsParent)
            {
                Debug.Log("GAV25B BSCSCR Patch 1.4");
                Object.Destroy(_campaignsParent.gameObject);
            }
            _campaignsParent = new GameObject("campaigns").transform;
            _campaignsParent.parent = instance.campaignTemplate.transform.parent;
            _campaignsParent.localPosition = instance.campaignTemplate.transform.localPosition;
            _campaignsParent.localRotation = Quaternion.identity;
            _campaignsParent.localScale = Vector3.one;
            Debug.Log("GAV25B BSCSCR Patch 1.5");
            traverse.Field("campaignsParent").SetValue(_campaignsParent);
            var _campaignWidth = ((RectTransform)instance.campaignTemplate.transform).rect.width;
            traverse.Field("campaignWidth").SetValue(_campaignWidth);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var _campaigns = new List<Campaign>();
            Debug.Log("GAV25B BSCSCR Patch 1.6");
            //this.campaigns = new List<Campaign>();

            /* --- BEGIN EDIT --- */
            /* THIS IS WHERE THE MAGIC HAPPENS, STEP BACK AND BEHOLD */
            var _aircraft = new List<string> { PilotSaveManager.currentVehicle.vehicleName };
            if (PilotSaveManager.currentVehicle.vehicleName == Main.customAircraftPV.vehicleName)
            {
                Debug.Log("GAV25B BSCSCR Patch 1.6.1");
                _aircraft.Add("F/A-26B");


            }
            Debug.Log("GAV25B BSCSCR Patch 1.7");
            foreach (var vName in _aircraft)
            {
                Debug.Log("GAV25B BSCSCR Patch 1.8: " + vName);
                foreach (var vtcampaignInfo in VTResources.GetBuiltInCampaigns()
                             .Where(vtcampaignInfo => vtcampaignInfo.vehicle == vName && !vtcampaignInfo.hideFromMenu))
                {
                    Debug.Log("GAV25B BSCSCR Patch 1.9: " + vtcampaignInfo.campaignName);

                    Campaign c = vtcampaignInfo.ToIngameCampaignAsync(instance, out var bdcoroutine);
                    yield return bdcoroutine;
                    Debug.Log("GAV25B BSCSCR Patch 1.10: ");

                    _campaigns.Add(c);
                }
            }
            /* ---- END EDIT ---- */
            sw.Stop();
            Debug.Log("GAV25B [CSIPatch] Time loading BuiltInCampaigns: " + sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            Debug.Log("GAV25B BSCSCR Patch 1.11");

            foreach (var vName in _aircraft)
            {
                foreach (var campaign in PilotSaveManager.GetVehicle(vName).campaigns)
                {
                    Debug.Log("GAV25B BSCSCR Patch 1.12: " + campaign + ", " + vName);

                    if (campaign.isSteamworksStandalone)
                    {
                        Debug.Log("GAV25B BSCSCR Patch 1.13");

                        if (!SteamClient.IsValid) continue;
                        traverse.Field("swStandaloneCampaign").SetValue(campaign);
                        Debug.Log("GAV25B BSCSCR Patch 1.14");

                        campaign.campaignName = (string)traverse.Field("campaign_ws").GetValue();
                        campaign.description = (string)traverse.Field("campaign_ws_description").GetValue();
                        Debug.Log("GAV25B BSCSCR Patch 1.15");

                    }
                    else if (campaign.readyToPlay)
                    {
                        Debug.Log("GAV25B BSCSCR Patch 1.16");

                        _campaigns.Add(campaign);
                        if (!campaign.isCustomScenarios || !campaign.isStandaloneScenarios ||
                            campaign.isSteamworksStandalone) continue;
                        Debug.Log("GAV25B BSCSCR Patch 1.17");

                        campaign.campaignName = (string)traverse.Field("campaign_customScenarios").GetValue();
                        campaign.description = (string)traverse.Field("campaign_customScenarios_description").GetValue();
                        Debug.Log("GAV25B BSCSCR Patch 1.18");

                    }
                }
            }
            sw.Stop();
            Debug.Log("GAV25B [CSIPatch] Time loading vehicle campaigns: " + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
            sw.Start();
            VTResources.GetCustomCampaigns();
            sw.Stop();
            Debug.Log("GAV25B [CSIPatch] Time loading custom campaigns list: " + sw.ElapsedMilliseconds.ToString());
            sw.Reset();
            sw.Start();

            /* --- BEGIN EDIT --- */
            /* MORE MAGIC IS HAPPENING, STEP BACK AND BEHOLD */
            foreach (var vName in _aircraft)
            {
                Debug.Log("GAV25B BSCSCR Patch 1.19: " + vName);

                foreach (var vtcampaignInfo2 in VTResources.GetCustomCampaigns()
                             .Where(vtcampaignInfo2 => vtcampaignInfo2.vehicle == vName && !vtcampaignInfo2.multiplayer))
                {
                    Debug.Log("GAV25B BSCSCR Patch 1.20: " + vtcampaignInfo2.campaignName);

                    Campaign c = vtcampaignInfo2.ToIngameCampaignAsync(instance, out var bdcoroutine2);
                    yield return bdcoroutine2;
                    Debug.Log("GAV25B BSCSCR Patch 1.21");
                    _campaigns.Add(c);
                    c = null;
                }
            }


            /* ---- END EDIT ---- */

            sw.Stop();
            Debug.Log("GAV25B [CSIPatch] Time converting custom campaigns ToIngameCampaigns: " + sw.ElapsedMilliseconds);
            sw.Reset();
            int num;
            Debug.Log("GAV25B BSCSCR Patch 1.22");
            for (var i = 0; i < _campaigns.Count; i = num + 1)
            {
                Debug.Log("GAV25B BSCSCR Patch 1.23");
                var gameObject = Object.Instantiate<GameObject>(instance.campaignTemplate, _campaignsParent);
                gameObject.transform.localPosition += _campaignWidth * (float)i * Vector3.right;
                Debug.Log("GAV25B BSCSCR Patch 1.24");
                var component = gameObject.GetComponent<CampaignInfoUI>();
                component.campaignImage.texture = instance.noImage;
                Debug.Log("GAV25B BSCSCR Patch 1.25");
                component.UpdateDisplay(_campaigns[i], PilotSaveManager.currentVehicle.vehicleName);
                gameObject.SetActive(true);
                Debug.Log("GAV25B BSCSCR Patch 1.26");
                yield return null;
                num = i;
            }
            var _campaignIdx = (int)traverse.Field("campaignIdx").GetValue();
            Debug.Log("GAV25B BSCSCR Patch 1.27");
            _campaignIdx = Mathf.Clamp(_campaignIdx, 0, _campaigns.Count - 1);
            traverse.Field("campaignIdx").SetValue(_campaignIdx);
            instance.campaignTemplate.SetActive(false);
            Debug.Log("GAV25B BSCSCR Patch 1.28");

            traverse.Field("campaigns").SetValue(_campaigns);

            var setupList = typeof(CampaignSelectorUI).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (setupList == null)
            {
             //   Debug.Log("GAV25B BSCSCR Patch 1.28.1");
            }
            var setupCampaignList = typeof(CampaignSelectorUI).GetMethod("SetupCampaignList", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Debug.Log("GAV25B BSCSCR Patch 1.29");
            //instance.SetupCampaignList(); // private method invoke required
            setupCampaignList.Invoke(instance, null);
            Debug.Log("GAV25B BSCSCR Patch 1.30");
            instance.loadingCampaignScreenObj.SetActive(false);
            Debug.Log("GAV25B BSCSCR Patch 1.31");
            if (wasInputEnabled)
            {
                ControllerEventHandler.UnpauseEvents();
            }
            Debug.Log("GAV25B BSCSCR Patch 1.32");
        }
    }

    [HarmonyPatch(typeof(CampaignSelectorUI), "FinallyOpenCampaignSelector")]
    class GAV25B_CSIPatch_FinallyOpenCampaignSelector
    {

        static CampaignSelectorUI instance;
        private static string String1;

        /** Replica of StartWorkshopCampaignRoutine. */
        static IEnumerator CSI_FinallyOpenCampaignSelectorRoutine(Action onOpenedSelector)
        {
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                Debug.unityLogger.logEnabled = Main.logging;
                Debug.Log("GAV25B CSI Patch 1.1");
            var traverse = Traverse.Create(instance);
            instance.campaignDisplayObject.SetActive(true);
            instance.missionBriefingDisplayObject.SetActive(false);
            Debug.Log("GAV25B CSI Patch 1.2");
            yield return instance.StartCoroutine(CSIPatchUtils.BetterSetupCampaignScreenRoutine(instance));
            Debug.Log("GAV25B CSI Patch 1.2.1.1");
            if (onOpenedSelector != null)
            {
                Debug.Log("GAV25B CSI Patch 1.3");
                onOpenedSelector();
                yield break;
            }
            Debug.Log("GAV25B CSI Patch 1.2.1.2");
            try
            {
                String1 = PilotSaveManager.currentCampaign.campaignName;
            }
            catch (NullReferenceException)
            { yield break; }

            Debug.Log("GAV25B CSI Patch 1.2.1.3");
            if (String1 == null) { yield break; }
            Debug.Log("GAV25B CSI Patch 1.2.1" + PilotSaveManager.currentCampaign.campaignName);
            if (!PilotSaveManager.currentCampaign) yield break;
            Debug.Log("GAV25B CSI Patch 1.4");
            var lastCSave = PilotSaveManager.current.lastVehicleSave.GetCampaignSave(PilotSaveManager.currentCampaign.campaignID);
            var _campaigns = (List<Campaign>)traverse.Field("campaigns").GetValue();
            var num = _campaigns.FindIndex(x => x.campaignID == PilotSaveManager.currentCampaign.campaignID);
            Debug.Log("GAV25B CSI Patch 1.5");
            if (num >= 0)
            {
                Debug.Log("GAV25B CSI Patch 1.6");

                PilotSaveManager.currentCampaign = _campaigns[num];
                var _campaignIdx = traverse.Field("campaignIdx");
                //this.campaignIdx = num;
                _campaignIdx.SetValue(num);
                //instance.ViewCampaign(num); // private method invoke
                var viewCampaign = instance.GetType().GetMethod("ViewCampaign", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                Debug.Log("GAV25B CSI Patch 1.7");
                viewCampaign.Invoke(instance, new System.Object[] { num });

                instance.SelectCampaign();
                if (PilotSaveManager.currentScenario == null) yield break;
                Debug.Log("GAV25B CSI Patch 1.8");
                var _viewingCampaign = (Campaign)traverse.Field("viewingCampaign").GetValue();
                while (_viewingCampaign == null)
                {
                    Debug.Log("GAV25B CSI Patch 1.9");
                    _viewingCampaign = (Campaign)traverse.Field("viewingCampaign").GetValue();
                    yield return null;
                }
                var num2 = _campaigns[(int)_campaignIdx.GetValue()].missions
                    .FindIndex(x => PilotSaveManager.currentScenario.scenarioID == x.scenarioID);
                if (num2 >= 0)
                {
                    Debug.Log("GAV25B CSI Patch 1.10");
                    //this.missionIdx = num2;
                    traverse.Field("missionIdx").SetValue(num2);
                    instance.MissionsButton();
                }
                else
                {
                    Debug.Log("GAV25B CSI Patch 1.11");
                    num2 = _campaigns[(int)_campaignIdx.GetValue()].trainingMissions
                        .FindIndex(x => PilotSaveManager.currentScenario.scenarioID == x.scenarioID);
                    if (num2 < 0) yield break;
                    //this.trainingIdx = num2;
                    Debug.Log("GAV25B CSI Patch 1.12");
                    traverse.Field("trainingIdx").SetValue(num2);
                    instance.TrainingButton();
                }
            }

            else if ((bool)traverse.Field("openedWorkshopCampaign").GetValue()) //CampaignSelectorUI.openedWorkshopCampaign
            {
                Debug.Log("GAV25B CSI Patch 1.13");
                var _loadedWorkshopCampaigns = (List<Campaign>)traverse.Field("loadedWorkshopCampaigns").GetValue();
                using (var enumerator = _loadedWorkshopCampaigns.GetEnumerator())
                {
                    Debug.Log("GAV25B CSI Patch 1.14");
                    while (enumerator.MoveNext())
                    {
                        Debug.Log("GAV25B CSI Patch 1.15");
                        var _workshopCampaignID = (string)traverse.Field("workshopCampaignID").GetValue(); //static
                        if (enumerator.Current.campaignID != _workshopCampaignID) continue;
                        instance.StartWorkshopCampaign(_workshopCampaignID);

                        Debug.Log("GAV25B CSI Patch 1.16");
                        var _viewingCampaign = (Campaign)traverse.Field("viewingCampaign").GetValue();
                        while (_viewingCampaign == null)
                        {
                            Debug.Log("GAV25B CSI Patch 1.17");
                            _viewingCampaign = (Campaign)traverse.Field("viewingCampaign").GetValue();
                            yield return null;
                        }
                        if (lastCSave == null)
                        {
                            break;
                        }
                        if (lastCSave.lastScenarioWasTraining)
                        {
                            Debug.Log("GAV25B CSI Patch 1.18");
                            //this.trainingIdx = lastCSave.lastScenarioIdx;
                            traverse.Field("trainingIdx").SetValue(lastCSave.lastScenarioIdx);
                            instance.TrainingButton();
                            break;
                        }
                        Debug.Log("GAV25B CSI Patch 1.19");
                        //this.missionIdx = lastCSave.lastScenarioIdx;
                        traverse.Field("missionIdx").SetValue(lastCSave.lastScenarioIdx);
                        instance.MissionsButton();
                        break;
                    }
                }
            }
        }

        static void Postfix(CampaignSelectorUI __instance, Action onOpenedSelector)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            Debug.Log("GAV25B CSI Patch 1.20");
            instance = __instance;
            Debug.Log("GAV25B CSI Patch 1.21");
            instance.StartCoroutine(CSI_FinallyOpenCampaignSelectorRoutine(onOpenedSelector));
            Debug.Log("GAV25B CSI Patch 1.22");
        }
    }

    [HarmonyPatch(typeof(CampaignSelectorUI), "StartWorkshopCampaign")]
    class GAV25B_CSIPatch_StartWorkshopCampaign
    {
        static CampaignSelectorUI instance;

        /** Replica of StartWorkshopCampaignRoutine. */
        static IEnumerator CSI_StartWorkshopCampaignRoutine(string campaignID)
        {
            //   Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("GAV25B CSI Patch 2.1");
            var traverse = Traverse.Create(instance);

            //CampaignSelectorUI.openedFromWorkshop = true;
            traverse.Field("openedFromWorkshop").SetValue(true);
            //CampaignSelectorUI.openedWorkshopCampaign = true;
            traverse.Field("openedWorkshopCampaign").SetValue(true);
            //CampaignSelectorUI.workshopCampaignID = campaignID;
            traverse.Field("workshopCampaignID").SetValue(campaignID);
            Debug.Log("GAV25B CSI Patch 2.2");
            var campaign = VTResources.GetSteamWorkshopCampaign(campaignID);
            if (campaign == null)
            {
                Debug.Log("GAV25B CSI Patch 2.3");
                Debug.Log("GAV25B Missing campaign in CSI_StartWorkshopCampaignRoutine");
            }
            var vehicle = campaign.vehicle;
            PilotSaveManager.current.lastVehicleUsed = vehicle;
            PilotSaveManager.currentVehicle = VTResources.GetPlayerVehicle(vehicle);

            Debug.Log("GAV25B CSI Patch 2.4");
            yield return instance.StartCoroutine(CSIPatchUtils.BetterSetupCampaignScreenRoutine(instance));

            //instance.campaignIdx = 0;
            //instance.campaignIdx = 0;
            traverse.Field("campaignIdx").SetValue(0);
            Debug.Log("GAV25B CSI Patch 2.5");
            var _loadedWorkshopCampaigns = (List<Campaign>)traverse.Field("loadedWorkshopCampaigns").GetValue();

            var campaign2 = _loadedWorkshopCampaigns.FirstOrDefault(campaign3 => campaign3.campaignID == campaignID);
            Debug.Log("GAV25B CSI Patch 2.6");
            if (campaign2 == null)
            {
                Debug.Log("GAV25B CSI Patch 2.7");
                campaign2 = campaign.ToIngameCampaign();
                _loadedWorkshopCampaigns.Add(campaign2);
            }
            traverse.Field("loadedWorkshopCampaigns").SetValue(_loadedWorkshopCampaigns); //just to be safe

            if (campaign2 == null) yield break;
            Debug.Log("GAV25B CSI Patch 2.8");
            instance.campaignDisplayObject.SetActive(false);
            PilotSaveManager.currentCampaign = campaign2;
            Debug.Log("GAV25B CSI Patch 2.9");
            instance.SetupCampaignScenarios(campaign2, true);
        }

        static bool Prefix(CampaignSelectorUI __instance, string campaignID)
        {
            Debug.Log("GAV25B CSI Patch 2.0.1");
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
            {
                Debug.Log("GAV25B CSI Patch 2.0.2");
                instance = __instance;
                instance.StartCoroutine(CSI_StartWorkshopCampaignRoutine(campaignID));
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CampaignSelectorUI), "StartWorkshopMission")]
    class GAV25B_CSIPatch_StartWorkshopMission
    {
        static CampaignSelectorUI instance;

        /** Replica of StartWorkshopCampaignRoutine. */
        static IEnumerator CSI_StartWorkshopMissionRoutine(string scenarioID)
        {
            // Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("GAV25B CSI Patch 3.0.1");
            var traverse = Traverse.Create(instance);

            //CampaignSelectorUI.openedFromWorkshop = true;
            traverse.Field("openedFromWorkshop").SetValue(true);
            //CampaignSelectorUI.openedWorkshopCampaign = false;
            traverse.Field("openedWorkshopCampaign").SetValue(false);
            Debug.Log("GAV25B CSI Patch 3.0.2");

            Debug.Log("GAV25B [CSIPatch] ========== A steam workshop standalone mission should be loaded at this time ===========");
            var scenario = VTResources.GetSteamWorkshopStandaloneScenario(scenarioID);
            var vehicle = scenario.vehicle;
            Debug.Log("GAV25B CSI Patch 3.0.3");
            PilotSaveManager.currentVehicle = vehicle;
            PilotSaveManager.current.lastVehicleUsed = vehicle.vehicleName;
            yield return instance.StartCoroutine(CSIPatchUtils.BetterSetupCampaignScreenRoutine(instance));
            Debug.Log("GAV25B CSI Patch 3.1");
            var campaign = (Campaign)traverse.Field("swStandaloneCampaign").GetValue();
            if (campaign.missions != null)
            {
                Debug.Log("GAV25B CSI Patch 3.2");
                campaign.missions.Clear();
            }
            else
            {
                Debug.Log("GAV25B CSI Patch 3.3");
                campaign.missions = new List<CampaignScenario>(1);
            }
            Debug.Log("GAV25B CSI Patch 3.4");
            var _loadedWorkshopSingleScenarios = (List<CampaignScenario>)traverse.Field("loadedWorkshopSingleScenarios").GetValue();

            var campaignScenario = _loadedWorkshopSingleScenarios.FirstOrDefault(campaignScenario2 => campaignScenario2.scenarioID == scenarioID) ??
                                   scenario.ToIngameScenario(null);
            Debug.Log("GAV25B CSI Patch 3.5");
            campaign.missions.Add(campaignScenario);
            PilotSaveManager.currentCampaign = campaign;
            instance.SetupCampaignScenarios(campaign, false);
            Debug.Log("GAV25B CSI Patch 3.6");
            foreach (var mission in campaign.missions)
            {
                Debug.Log("GAV25B CSI Patch 3.1");
                if (mission.scenarioID != scenarioID) continue;
                //instance.StartMission(campaign.missions[i]);
                var _startMission = instance.GetType().GetMethod("StartMission", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                Debug.Log("GAV25B CSI Patch 3.1");
                _startMission.Invoke(instance, new object[] { mission });
                yield break;
            }
        }

        static void Postfix(CampaignSelectorUI __instance, ref CampaignSelectorUI.WorkshopLaunchStatus __result, string scenarioID)
        {
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            Debug.Log("GAV25B CSI Patch 4.1");
            instance = __instance;

            var steamWorkshopStandaloneScenario = VTResources.GetSteamWorkshopStandaloneScenario(scenarioID);
            Debug.Log("GAV25B CSI Patch 4.2");
            if (steamWorkshopStandaloneScenario == null)
            {
                //Debug.LogError("[CSIPatch] Tried to run workshop scenario but scenario was null!");
                __result = new CampaignSelectorUI.WorkshopLaunchStatus
                {

                    success = false,
                    message = VTLStaticStrings.err_scenarioNotFound
                };
                return;
            }
            if (GameStartup.version < steamWorkshopStandaloneScenario.gameVersion)
            {
                Debug.Log("GAV25B CSI Patch 4.3");
                var str = "[CSIPatch] Tried to run workshop scenario but version incompatible.  Game version: ";
                var str2 = GameStartup.version.ToString();
                var str3 = ", scenario game version: ";
                var gameVersion = steamWorkshopStandaloneScenario.gameVersion;
                //Debug.LogError(str + str2 + str3 + gameVersion);
                __result = new CampaignSelectorUI.WorkshopLaunchStatus
                {
                    success = false,
                    message = VTLStaticStrings.err_version
                };
                return;
            }
            instance.StartCoroutine(CSI_StartWorkshopMissionRoutine(scenarioID));
            __result = new CampaignSelectorUI.WorkshopLaunchStatus
            {
                success = true
            };
        }
    }


    [HarmonyPatch(typeof(PilotSaveManager), "EnsureVehicleCollections")]
    class GAV25B_PlayerVehicleListPatch
    {
        public static void Postfix()
        {
            //Debug.unityLogger.logEnabled = Main.logging;
            // this all executes after PilotSaveManager loads all the vehicles from base game

            // private static field = Traverse babyyyyy
            Traverse trav = Traverse.Create<PilotSaveManager>();
            // yoink the existing vars that PilotSaveManager uses
            var vehicles = trav.Field("vehicles").GetValue<Dictionary<string, PlayerVehicle>>();
            var vehicleList = trav.Field("vehicleList").GetValue<List<PlayerVehicle>>();

            if (!vehicles.ContainsKey(Main.customAircraftPV.vehicleName))
            {

                // then add our vehicle to the list
                Debug.Log("GAV25B PV3.0");

                Debug.Log("GAV25B PPV3.0");


                Debug.Log("GAV25B PV3.1");
                vehicles.Add(Main.customAircraftPV.vehicleName, Main.customAircraftPV);
                vehicleList.Add(Main.customAircraftPV);
                // and set them back with our fancy updated data structures
                trav.Field("vehicles").SetValue(vehicles);
                trav.Field("vehicleList").SetValue(vehicleList);
            }



        }
    }

    [HarmonyPatch(typeof(MultiplayerSpawn), "VehicleName")]
    class GAV25B_Patch_MultiplayerSpawn_VehicleName
    {
        static void Postfix(MultiplayerSpawn __instance, ref string __result)
        {
            Debug.Log("GAV25B MPSVN1.0");
            if (__instance.vehicle == (MultiplayerSpawn.Vehicles)Main.aircraftMSVId /* some unique value */)
            {
                Debug.Log("GAV25B MPSVN1.1");
                __result = Main.customAircraftPV.vehicleName;
                Debug.Log("GAV25B MPSVN1.2");
            }
        }
    }

    [HarmonyPatch(typeof(MultiplayerSpawn), "GetVehicleName")]
    class GAV25B_Patch_MultiplayerSpawn_GetVehicleName
    {
        static void Postfix(MultiplayerSpawn.Vehicles v, ref string __result)
        {
            Debug.Log("GAV25B MPSGVN1.0");
            if (v == (MultiplayerSpawn.Vehicles)Main.aircraftMSVId /* some unique value */)
            {
                Debug.Log("GAV25B MPSGVN1.1");
                __result = Main.customAircraftPV.vehicleName;
                Debug.Log("GAV25B MPSGVN1.2");
            }
        }
    }

    [HarmonyPatch(typeof(MultiplayerSpawn), "GetVehicleEnum")]
    class GAV25B_Patch_MultplayerSpawn_GetVehicleEnum
    {
        static void Postfix(string vehicleName, ref MultiplayerSpawn.Vehicles __result)
        {
            Debug.Log("GAV25B MPSGVE1.0");
            if (vehicleName == Main.customAircraftPV.vehicleName)
            {
                Debug.Log("GAV25B MPSGVE1.1");

                __result = Main.aircraftMSVId;
                Debug.Log("GAV25B MPSGVE1.2");
            }
        }
    }
    
    [HarmonyPatch(typeof(CommRadioSource), "SetAsRadioSource")]
    class GAV25B_CRSSRSPatch
    {
        public static bool Prefix(CommRadioSource __instance)
        {
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return true;
            Debug.Log("GAV25B CRSSRSPatch 1.0 :" + __instance.minDistance + "," + __instance.name + "," + __instance.maxDistance + ", " + __instance.volume);
            __instance.EnsureCommSource();

            Debug.Log("GAV25B CRSSRSPatch 1.1: " + __instance.minDistance + "," + __instance.name + "," + __instance.maxDistance + ", " + __instance.volume);

            AudioSource CommRadioSourceVar = CommRadioSource.commSource;
            Debug.Log("GAV25B CRSSRSPatch 1.1.1: " + CommRadioSourceVar.name);
            GameObject CRSObj = GameObject.Find("CommRadioSource");

            GameObject CRA = AircraftAPI.GetChildWithName(Main.aircraftCustom, "CommRadioAudio", true);
            Debug.Log("GAV25B CRSSRSPatch 1.1.2");
            CRSObj.GetComponent<AudioSource>().transform.position = CRA.transform.position;
            Debug.Log("GAV25B CRSSRSPatch 1.1.3");
            CRSObj.GetComponent<AudioSource>().transform.SetParent(CRA.transform);
            Debug.Log("GAV25B CRSSRSPatch 1.1.4");
            //CommRadioManager.instance.SetAudioSource(__instance);
            Debug.Log("GAV25B CRSSRSPatch 1.2");
            __instance.ApplySettings(CommRadioSource.commSource);
            Debug.Log("GAV25B CRSSRSPatch 1.3");
            __instance.ApplySettings(CommRadioSource.copilotCommSource);
            Debug.Log("GAV25B CRSSRSPatch 1.4");
            return false;
        }
    }



   

    [HarmonyPatch(typeof(VTEdOptionSelector), "Display", new Type[] {typeof(string), typeof(string[]),typeof(object[]),typeof(int), typeof(VTEdOptionSelector.OptionSelectionObjectDelegate), typeof(VTEdOptionSelector.OptionHoverInfoDelegate) })]
    class GAV25B_Patch_VTEdOptionSelector_Display
    {
        public static void Postfix(VTEdOptionSelector __instance, string[] options, int selected)
        {
            Debug.Log("GAV25B VTEDOSD 1.0");
            var t = Traverse.Create(__instance);
            Debug.Log("GAV25B VTEDOSD 1.1");
            var returnValues = (object[])t.Field("returnValues").GetValue();
            Debug.Log("GAV25B VTEDOSD 1.2");
            if (returnValues.Contains(Main.customAircraftPV.vehicleName))
                return;
            var newReturnValues = new object[returnValues.Length + 1];
            Debug.Log("GAV25B VTEDOSD 1.3:" + returnValues.Length);
            for (var i = 0; i < returnValues.Length; i++)
                newReturnValues[i] = returnValues[i];
            Debug.Log("GAV25B VTEDOSD 1.4");
            newReturnValues[returnValues.Length] = Main.customAircraftPV.vehicleName;
            Debug.Log("GAV25B VTEDOSD 1.5: " + newReturnValues[returnValues.Length]);
            t.Field("returnValues").SetValue(newReturnValues);
            Debug.Log("GAV25B VTEDOSD 1.6");
            var SetupUI = __instance.GetType()
                .GetMethod("SetupUI", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            Debug.Log("GAV25B VTEDOSD 1.7");
            SetupUI.Invoke(__instance, new object[] { options, selected });
            Debug.Log("GAV25B VTEDOSD 1.8");
        }
    }

    
    [HarmonyPatch(typeof(VTEnumProperty), "SelectButton")]
    class GAV25B_Patch_VTEnumProperty_SelectButton
    {
        public static bool Prefix(VTEnumProperty __instance)
        {
            Debug.Log("GAV25B VTESB 1.0");
            var t = Traverse.Create(__instance);
            // do a quick dirty check that this is a selector for aircraft...
            var options = t.Field("options").GetValue<string[]>();
            Debug.Log("GAV25B VTESB 1.1");
            if (!options.Contains("AV42C"))
                return true;

            // do the rapid check for our aircraft in that list, too
            Debug.Log("GAV25B VTESB 1.2");
            if (!options.Contains(Main.customAircraftPV.vehicleName))
            {
                Debug.Log("GAV25B VTESB 1.3");
                // if we don't have, just bolt it in there
                var newOptions = new string[options.Length + 1];
                Debug.Log("GAV25B VTESB 1.4");
                for (var i = 0; i < options.Length; i++)
                    newOptions[i] = options[i];
                Debug.Log("GAV25B VTESB 1.5");
                newOptions[options.Length] = Main.customAircraftPV.vehicleName;
                Debug.Log("GAV25B VTESB 1.6");
                t.Field("options").SetValue(newOptions);
            }

            // bolt it on if it ain't there
            Debug.Log("GAV25B VTESB 1.7");
            object[] values = (object[])t.Field("values").GetValue();
            Debug.Log("GAV25B VTESB 1.8");
            if (values.Contains(Main.aircraftMSVId))
            {

                Debug.Log("GAV25B [VTEnumProperty] Custom aircraft with this MultiplayerSpawn.Vehicles ID already exists in values");
                return true;
            }
            Debug.Log("GAV25B VTESB 1.9");
            var newValues = new object[values.Length + 1];
            Debug.Log("GAV25B VTESB 1.10");
            for (var i = 0; i < values.Length; i++)
                newValues[i] = values[i];
            Debug.Log("GAV25B VTESB 1.11");
            newValues[values.Length] = Main.aircraftMSVId;
            Debug.Log("GAV25B VTESB 1.12");
            t.Field("values").SetValue(newValues);
            
            return true;
        }
    }
    
    [HarmonyPatch(typeof(VTEdUnitOptionsWindow), "SetupOptionsList")]
    class GAV25B_Patch_VTEdUnitOptionsWindow_SetupOptionsList
    {
        public static void Postfix(VTEdUnitOptionsWindow __instance)
        {
            Debug.Log("GAV25B VTESOLPatch 1.0");

            var t = Traverse.Create(__instance);
            Debug.Log("GAV25B VTESOLPatch 1.1");
            var currentUnitFields = (List<VTPropertyField>)t.Field("currentUnitFields").GetValue();
            // approach: anywhere we have a vehicle selector option, we bolt our vehicle to it
            Debug.Log("GAV25B VTESOLPatch 1.2");
            foreach (var field in currentUnitFields)
            {
                Debug.Log("GAV25B VTESOLPatch 1.3: " + field.fieldName);
                if (field.fieldName == "vehicle" || field.type == typeof(VTEnumProperty))
                { 
                                
                    Debug.Log("GAV25B VTESOLPatch 1.4");
                    // if this is what we're looking for, let's bolt onto it
                    var t2 = Traverse.Create((VTEnumProperty)field);
                    Debug.Log("GAV25B VTESOLPatch 1.5");
                    var options = (string[])t2.Field("options").GetValue();

                    // make a deep copy of the original...
                    Debug.Log("GAV25B VTESOLPatch 1.6");
                    var optionsCopy = options.Select(o => o.Copy()).ToList();
                    // ...add our bad boy name in...
                    Debug.Log("GAV25B VTESOLPatch 1.7");
                    optionsCopy.Add(Main.customAircraftPV.vehicleName);
                    Debug.Log("GAV25B VTESOLPatch 1.8");
                    var optionsPlus = optionsCopy.ToArray();
                    // ...and set it back! hopefully
                    Debug.Log("GAV25B VTESOLPatch 1.9");
                    t2.Field("options").SetValue((string[]) optionsPlus);
                }
                
            }
        }
    }
}

