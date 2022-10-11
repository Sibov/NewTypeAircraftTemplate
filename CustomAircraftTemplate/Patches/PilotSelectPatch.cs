using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Harmony;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Steamworks;

namespace CAT
{
    // TODO: false-prefix, need to replace
    //[HarmonyPatch(typeof(VTScenario), "LoadFromNode")]
    //internal class CAT_GetLFNCheck
    //{
    //    public static bool Prefix(VTScenario __instance, ConfigNode saveNode)
    //    {
    //        UnitCatalogue.UpdateCatalogue();

    //        ConfigNodeUtils.TryParseValue(saveNode, "gameVersion", ref __instance.gameVersion);
    //        __instance.scenarioName = saveNode.GetValue("scenarioName");
    //        __instance.scenarioID = saveNode.GetValue("scenarioID");
    //        __instance.scenarioDescription = saveNode.GetValue("scenarioDescription");

    //        ConfigNodeUtils.TryParseValue(saveNode, "multiplayer", ref __instance.multiplayer);
    //        ConfigNodeUtils.TryParseValue(saveNode, "campaignID", ref __instance.campaignID);
    //        ConfigNodeUtils.TryParseValue(saveNode, "campaignOrderIdx", ref __instance.campaignOrderIdx);
            
    //        __instance.mapID = saveNode.HasValue("mapID") ? saveNode.GetValue("mapID") : VTMapManager.fetch.map.mapID;

    //        __instance.globalValues.LoadFromScenarioNode(saveNode);
    //        __instance.units.LoadFromScenarioNode(saveNode);
    //        __instance.staticObjects.LoadFromScenarioNode(saveNode);
    //        __instance.paths.LoadFromScenarioNode(saveNode);
    //        __instance.waypoints.LoadFromScenarioNode(saveNode);

    //        __instance.groups.LoadFromScenarioNode(saveNode);
    //        __instance.conditionals.LoadFromScenarioNode(saveNode);
    //        __instance.timedEventGroups.LoadFromScenarioNode(saveNode);
    //        __instance.conditionalActions.LoadFromScenarioNode(saveNode);

    //        __instance.triggerEvents.LoadFromScenarioNode(saveNode);
    //        __instance.objectives.LoadFromScenarioNode(saveNode);
    //        __instance.bases.LoadFromScenarioNode(saveNode);

    //        __instance.sequencedEvents.LoadFromScenarioNode(saveNode);
    //        __instance.conditionals.GatherReferences();
    //        __instance.vehicle = VTResources.GetPlayerVehicle(saveNode.GetValue("vehicle"));

    //        if (saveNode.HasValue("allowedEquips"))
    //        {
    //            __instance.allowedEquips = ConfigNodeUtils.ParseList(saveNode.GetValue("allowedEquips"));
    //        }
    //        if (saveNode.HasValue("forcedEquips"))
    //        {
    //            __instance.forcedEquips = ConfigNodeUtils.ParseList(saveNode.GetValue("forcedEquips"));
    //        }
    //        if (saveNode.HasValue("equipsOnComplete"))
    //        {
    //            __instance.equipsOnComplete = ConfigNodeUtils.ParseList(saveNode.GetValue("equipsOnComplete"));
    //        }
    //        __instance.forceEquips = ConfigNodeUtils.ParseBool(saveNode.GetValue("forceEquips"));
    //        ConfigNodeUtils.TryParseValue(saveNode, "normForcedFuel", ref __instance.normForcedFuel);

    //        __instance.equipsConfigurable = ConfigNodeUtils.ParseBool(saveNode.GetValue("equipsConfigurable"));
    //        __instance.isTraining = ConfigNodeUtils.ParseBool(saveNode.GetValue("isTraining"));
    //        __instance.baseBudget = ConfigNodeUtils.ParseFloat(saveNode.GetValue("baseBudget"));

    //        if (saveNode.HasValue("rtbWptID"))
    //        {
    //            __instance.rtbWptID = saveNode.GetValue("rtbWptID");
    //        }
    //        if (saveNode.HasValue("rtbWptID_B"))
    //        {
    //            __instance.rtbWptID_B = saveNode.GetValue("rtbWptID_B");
    //        }
    //        if (saveNode.HasValue("refuelWptID"))
    //        {
    //            __instance.refuelWptID = saveNode.GetValue("refuelWptID");
    //        }
    //        if (saveNode.HasValue("refuelWptID_B"))
    //        {
    //            __instance.refuelWptID_B = saveNode.GetValue("refuelWptID_B");
    //        }
    //        __instance.briefingNotes = ProtoBriefingNote.GetProtoBriefingsFromConfig(saveNode, false);
    //        if (__instance.multiplayer)
    //        {
    //            __instance.briefingNotesB = ProtoBriefingNote.GetProtoBriefingsFromConfig(saveNode, true);
    //            ConfigNodeUtils.TryParseValue(saveNode, "separateBriefings", ref __instance.separateBriefings);
    //        }

    //        var t1 = Traverse.Create(__instance);

    //        var resourceManifest2 = new List<string>();
    //        if (saveNode.HasNode("ResourceManifest"))
    //        {
    //            resourceManifest2.AddRange(saveNode.GetNode("ResourceManifest").GetValues().Select(configValue => configValue.value));
    //        }
    //        t1.Field("resourceManifest").SetValue(resourceManifest2);

    //        __instance.envName = saveNode.HasValue("envName") ? saveNode.GetValue("envName") : "day";

    //        ConfigNodeUtils.TryParseValue(saveNode, "qsMode", ref __instance.qsMode);
    //        ConfigNodeUtils.TryParseValue(saveNode, "qsLimit", ref __instance.qsLimit);

    //        ConfigNodeUtils.TryParseValue(saveNode, "selectableEnv", ref __instance.selectableEnv);
    //        __instance.ApplyBaseInfo();
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(VTResources), "LoadScenariosFromDir")]
    //class SU35_VTR_LoadScenariosPatch
    //{
    //    public static List<VTScenarioInfo> Helper(string parentDirectory, bool checkModified, bool enforceDirectoryName, bool decodeWSScenarios)
    //    {
    //        // replaced tail recursion with iteration here
    //        while (true)
    //        {
    //            var t1 = Traverse.Create(typeof(VTResources));

    //            var enforceDirInvalidFlag = false;
    //            var list = new List<VTScenarioInfo>();
    //            var skippedCount = 0;
    //            foreach (var path in Directory.GetDirectories(parentDirectory, "*", SearchOption.TopDirectoryOnly))
    //            {
    //                var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
    //                var j = 0;
    //                while (j < files.Length)
    //                {
    //                    var text = files[j];
    //                    try
    //                    {
    //                        if (text.EndsWith(".vts") || (decodeWSScenarios && text.EndsWith(".vtsb")))
    //                        {
    //                            ConfigNode configNode;
    //                            configNode = text.EndsWith(".vtsb")
    //                                ? VTSteamWorkshopUtils.ReadWorkshopConfig(text)
    //                                : ConfigNode.LoadFromFile(text);
    //                            if (parentDirectory == VTResources.customScenariosDir)
    //                            {
    //                                var fileName2 = Path.GetFileName(path);
    //                                if (enforceDirectoryName && !fileName2.Equals(configNode.GetValue("scenarioID")))
    //                                {
    //                                    enforceDirInvalidFlag = true;
    //                                    break;
    //                                }
    //                            }

    //                            var vtscenarioInfo = new VTScenarioInfo(configNode, text);
    //                            var vehicle = vtscenarioInfo.vehicle;
    //                            if (vehicle != null && (VTResources.isEditorOrDevTools || vehicle.readyToFly))
    //                            {
    //                                list.Add(vtscenarioInfo);
    //                            }
    //                        }
    //                    }
    //                    catch (KeyNotFoundException)
    //                    {
    //                        //Debug.LogError("KeyNotFoundException thrown when attempting to load VTScenario from " + text + ". It may be an outdated scenario file.");
    //                    }

    //                    goto __VTRPatch_JUMP_B;

    //                __VTRPatch_JUMP_A:
    //                    j++;
    //                    continue;

    //                __VTRPatch_JUMP_B:
    //                    if (!enforceDirInvalidFlag)
    //                        goto __VTRPatch_JUMP_A;

    //                    break;
    //                }
    //                if (enforceDirInvalidFlag)
    //                    break;
    //            }

    //            if (enforceDirInvalidFlag)
    //            {
    //                //VTResources.RepairAllScenarioFilePaths(); // private invoke
    //                var info = typeof(VTResources).GetMethod("RepairAllScenarioFilePaths",
    //                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
    //                info?.Invoke(null, null); // static so null obj, and no parameters so null

    //                checkModified = false;
    //                enforceDirectoryName = true;
    //                decodeWSScenarios = false;
    //                continue;
    //            }

    //            if (checkModified)
    //            {
    //                Debug.Log("SU35 - Skipped " + skippedCount + " scenarios (not modified).");
    //            }

    //            return list;
    //        }
    //    }

    //    public static bool Prefix(string parentDirectory, bool checkModified, bool enforceDirectoryName, bool decodeWSScenarios, ref List<VTScenarioInfo> __result)
    //    {
    //        __result = Helper(parentDirectory, checkModified, enforceDirectoryName, decodeWSScenarios);
    //        return false;
    //    }
    //}


    //[HarmonyPatch(typeof(VTResources), "LoadPlayerVehicles")]
    //internal class CAT_PlayerVehiclePatch
    //{
    //    static void Postfix()
    //    {
    //        var t = Traverse.Create(typeof(VTResources));
    //        var pvlist = (PlayerVehicleList)t.Field("playerVehicles").GetValue();

    //        pvlist.playerVehicles.Add(Main.CustomAircraftPv);
    //        t.Field("playerVehicles").SetValue(pvlist);
    //    }
    //}

    // TODO: false-prefix, need to replace
    //[HarmonyPatch(typeof(VTResources), "GetPlayerVehicle")]
    //internal class CAT_GetPlayerVehiclesCheck
    //{
    //    public static bool Prefix(VTResources __instance, string vehicleName, ref PlayerVehicle __result)
    //    {
    //        if (Main.PlayerVehicleList == null)
    //        {
    //            VTResources.LoadPlayerVehicles();
    //        }
    //        foreach (var playerVehicle in (Main.PlayerVehicleList ?? new List<PlayerVehicle>())
    //                 .Where(playerVehicle => playerVehicle.vehicleName == vehicleName))
    //        {
    //            __result = playerVehicle;
    //            return false;
    //        }
    //        __result = null;
    //        return false;
    //    }
    //}

    //// TODO: false-prefix, need to replace
    //[HarmonyPatch(typeof(MeasurementManager), "Awake")]
    //internal static class CAT_MMPatch
    //{
    //    public static bool Prefix(MeasurementManager __instance)
    //    {
    //        if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
    //            return true;

    //        var traverse = Traverse.Create(__instance);
    //        if (PilotSaveManager.current != null && PilotSaveManager.currentVehicle != null)
    //        {
    //            var vehicleSave = PilotSaveManager.current.GetVehicleSave(PilotSaveManager.currentVehicle.vehicleName);
    //            if (vehicleSave == null) return false;
    //            __instance.altitudeMode = vehicleSave.altitudeMode;
    //            __instance.distanceMode = vehicleSave.distanceMode;
    //            __instance.airspeedMode = vehicleSave.airspeedMode;
    //        }
    //        traverse.Field("flightInfo").SetValue(__instance.GetComponentInParent<FlightInfo>());
    //        //MeasurementManager.instance = __instance;

    //        return false;
    //    }
    //}
    
    //public static class CSIPatchUtils
    //{
    //    /** Executes more iterations of the campaign select loop to add other planes' campaigns to our aircraft.
    //     *	>> It isn't exactly "better" yet, just gives us more flexibility to control it. Possible todo: make better
    //     */
    //    public static IEnumerator BetterSetupCampaignScreenRoutine(CampaignSelectorUI instance)
    //    {
    //        var traverse = Traverse.Create(instance);

    //        instance.loadingCampaignScreenObj.SetActive(true);

    //        bool wasInputEnabled = !ControllerEventHandler.eventsPaused;
    //        ControllerEventHandler.PauseEvents();
    //        VTScenarioEditor.returnToEditor = false;
    //        VTMapManager.nextLaunchMode = VTMapManager.MapLaunchModes.Scenario;
    //        PlayerVehicleSetup.godMode = false;

    //        instance.campaignDisplayObject.SetActive(true);
    //        instance.scenarioDisplayObject.SetActive(false);
    //        var _campaignsParent = (Transform)traverse.Field("campaignsParent").GetValue();

    //        if (_campaignsParent)
    //        {
    //            Object.Destroy(_campaignsParent.gameObject);
    //        }
    //        _campaignsParent = new GameObject("campaigns").transform;
    //        _campaignsParent.parent = instance.campaignTemplate.transform.parent;
    //        _campaignsParent.localPosition = instance.campaignTemplate.transform.localPosition;
    //        _campaignsParent.localRotation = Quaternion.identity;
    //        _campaignsParent.localScale = Vector3.one;

    //        traverse.Field("campaignsParent").SetValue(_campaignsParent);
    //        var campaignWidth = ((RectTransform)instance.campaignTemplate.transform).rect.width;
    //        traverse.Field("campaignWidth").SetValue(campaignWidth);

    //        var campaigns = new List<Campaign>();
    //        //this.campaigns = new List<Campaign>();

    //        /* --- BEGIN EDIT --- */
    //        /* THIS IS WHERE THE MAGIC HAPPENS, STEP BACK AND BEHOLD */
    //        var aircraft = new List<string> { PilotSaveManager.currentVehicle.vehicleName };
    //        if (PilotSaveManager.currentVehicle.vehicleName == Main.CustomAircraftPv.vehicleName)
    //        {
    //            aircraft.Add("F/A-26B");
    //        }

    //        foreach (var vtCampaignInfo in aircraft.SelectMany(vName => VTResources.GetBuiltInCampaigns()
    //                        .Where(vtci => vtci.vehicle == vName && !vtci.hideFromMenu)))
    //        {
    //            var c = vtCampaignInfo.ToIngameCampaignAsync(instance, out var bdCoroutine);
    //            yield return bdCoroutine;

    //            campaigns.Add(c);
    //        }
    //        /* ---- END EDIT ---- */

    //        foreach (var campaign in aircraft.SelectMany(vName => PilotSaveManager.GetVehicle(vName).campaigns))
    //        {
    //            if (campaign.isSteamworksStandalone)
    //            {
    //                if (!SteamClient.IsValid) continue;
    //                traverse.Field("swStandaloneCampaign").SetValue(campaign);

    //                campaign.campaignName = (string)traverse.Field("campaign_ws").GetValue();
    //                campaign.description = (string)traverse.Field("campaign_ws_description").GetValue();

    //            }
    //            else if (campaign.readyToPlay)
    //            {
    //                campaigns.Add(campaign);
    //                if (!campaign.isCustomScenarios || !campaign.isStandaloneScenarios ||
    //                    campaign.isSteamworksStandalone) continue;

    //                campaign.campaignName = (string)traverse.Field("campaign_customScenarios").GetValue();
    //                campaign.description = (string)traverse.Field("campaign_customScenarios_description").GetValue();
    //            }
    //        }
    //        VTResources.GetCustomCampaigns();

    //        /* --- BEGIN EDIT --- */
    //        /* MORE MAGIC IS HAPPENING, STEP BACK AND BEHOLD */
    //        foreach (var vtCampaignInfo in aircraft.SelectMany(vName => VTResources.GetCustomCampaigns()
    //                     .Where(vtci => vtci.vehicle == vName && !vtci.multiplayer)))
    //        {
    //            var c = vtCampaignInfo.ToIngameCampaignAsync(instance, out var bdCoroutine);
    //            yield return bdCoroutine;
    //            campaigns.Add(c);
    //        }
            
    //        /* ---- END EDIT ---- */
            
    //        for (var i = 0; i < campaigns.Count; i++)
    //        {
    //            var gameObject = Object.Instantiate(instance.campaignTemplate, _campaignsParent);
    //            gameObject.transform.localPosition += campaignWidth * i * Vector3.right;

    //            var component = gameObject.GetComponent<CampaignInfoUI>();
    //            component.campaignImage.texture = instance.noImage;

    //            component.UpdateDisplay(campaigns[i], PilotSaveManager.currentVehicle.vehicleName);
    //            gameObject.SetActive(true);

    //            yield return null;
    //        }
    //        var campaignIdx = (int)traverse.Field("campaignIdx").GetValue();

    //        campaignIdx = Mathf.Clamp(campaignIdx, 0, campaigns.Count - 1);
    //        traverse.Field("campaignIdx").SetValue(campaignIdx);
    //        instance.campaignTemplate.SetActive(false);

    //        traverse.Field("campaigns").SetValue(campaigns);

    //        //var setupList = typeof(CampaignSelectorUI).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
    //        var setupCampaignList = typeof(CampaignSelectorUI).GetMethod("SetupCampaignList", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    //        if (setupCampaignList != null) setupCampaignList.Invoke(instance, null);

    //        instance.loadingCampaignScreenObj.SetActive(false);
    //        if (wasInputEnabled)
    //        {
    //            ControllerEventHandler.UnpauseEvents();
    //        }
    //    }
    //}

    //[HarmonyPatch(typeof(CampaignSelectorUI), "FinallyOpenCampaignSelector")]
    //internal class CAT_CSIPatch_FinallyOpenCampaignSelector
    //{
    //    private static CampaignSelectorUI _instance;
    //    private static string _selectedCampaignName;

    //    /** Replica of StartWorkshopCampaignRoutine. */
    //    private static IEnumerator CSI_FinallyOpenCampaignSelectorRoutine(Action onOpenedSelector)
    //    {
    //        if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
    //            Debug.unityLogger.logEnabled = Main.Logging;

    //        var traverse = Traverse.Create(_instance);
    //        _instance.campaignDisplayObject.SetActive(true);
    //        _instance.missionBriefingDisplayObject.SetActive(false);

    //        yield return _instance.StartCoroutine(CSIPatchUtils.BetterSetupCampaignScreenRoutine(_instance));

    //        if (onOpenedSelector != null)
    //        {
    //            onOpenedSelector();
    //            yield break;
    //        }
    //        try
    //        {
    //            _selectedCampaignName = PilotSaveManager.currentCampaign.campaignName;
    //        }
    //        catch (NullReferenceException)
    //        { yield break; }
            
    //        if (_selectedCampaignName == null) { yield break; }

    //        if (!PilotSaveManager.currentCampaign) yield break;

    //        var lastCSave = PilotSaveManager.current.lastVehicleSave.GetCampaignSave(PilotSaveManager.currentCampaign.campaignID);
    //        var campaigns = (List<Campaign>)traverse.Field("campaigns").GetValue();
    //        var campaignAtIdx = campaigns.FindIndex(x => x.campaignID == PilotSaveManager.currentCampaign.campaignID);

    //        if (campaignAtIdx >= 0)
    //        {
    //            PilotSaveManager.currentCampaign = campaigns[campaignAtIdx];
    //            var campaignIdx = traverse.Field("campaignIdx");
    //            //this.campaignIdx = num;
    //            campaignIdx.SetValue(campaignAtIdx);
    //            //instance.ViewCampaign(num); // private method invoke
    //            var viewCampaign = _instance.GetType().GetMethod("ViewCampaign", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

    //            if (viewCampaign != null) viewCampaign.Invoke(_instance, new object[] {campaignAtIdx});

    //            _instance.SelectCampaign();
    //            if (PilotSaveManager.currentScenario == null) yield break;

    //            var viewingCampaign = (Campaign)traverse.Field("viewingCampaign").GetValue();
    //            while (viewingCampaign == null)
    //            {
    //                viewingCampaign = (Campaign)traverse.Field("viewingCampaign").GetValue();
    //                yield return null;
    //            }
    //            var missionIdx = campaigns[(int)campaignIdx.GetValue()].missions
    //                .FindIndex(x => PilotSaveManager.currentScenario.scenarioID == x.scenarioID);
    //            if (missionIdx >= 0)
    //            {
    //                //this.missionIdx = num2;
    //                traverse.Field("missionIdx").SetValue(missionIdx);
    //                _instance.MissionsButton();
    //            }
    //            else
    //            {
    //                missionIdx = campaigns[(int)campaignIdx.GetValue()].trainingMissions
    //                    .FindIndex(x => PilotSaveManager.currentScenario.scenarioID == x.scenarioID);
    //                if (missionIdx < 0) yield break;
    //                //this.trainingIdx = num2;
    //                traverse.Field("trainingIdx").SetValue(missionIdx);
    //                _instance.TrainingButton();
    //            }
    //        }

    //        else if ((bool)traverse.Field("openedWorkshopCampaign").GetValue()) //CampaignSelectorUI.openedWorkshopCampaign
    //        {
    //            var loadedWorkshopCampaigns = (List<Campaign>)traverse.Field("loadedWorkshopCampaigns").GetValue();
    //            using (var enumerator = loadedWorkshopCampaigns.GetEnumerator())
    //            {
    //                while (enumerator.MoveNext())
    //                {
    //                    var workshopCampaignId = (string)traverse.Field("workshopCampaignID").GetValue(); //static
    //                    if (enumerator.Current != null && enumerator.Current.campaignID != workshopCampaignId) continue;
    //                    _instance.StartWorkshopCampaign(workshopCampaignId);
                        
    //                    var viewingCampaign = (Campaign)traverse.Field("viewingCampaign").GetValue();
    //                    while (viewingCampaign == null)
    //                    {
    //                        viewingCampaign = (Campaign)traverse.Field("viewingCampaign").GetValue();
    //                        yield return null;
    //                    }
    //                    if (lastCSave == null)
    //                    {
    //                        break;
    //                    }
    //                    if (lastCSave.lastScenarioWasTraining)
    //                    {
    //                        //this.trainingIdx = lastCSave.lastScenarioIdx;
    //                        traverse.Field("trainingIdx").SetValue(lastCSave.lastScenarioIdx);
    //                        _instance.TrainingButton();
    //                        break;
    //                    }
    //                    //this.missionIdx = lastCSave.lastScenarioIdx;
    //                    traverse.Field("missionIdx").SetValue(lastCSave.lastScenarioIdx);
    //                    _instance.MissionsButton();
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    private static void Postfix(CampaignSelectorUI __instance, Action onOpenedSelector)
    //    {
    //        if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
    //            return;
    //        _instance = __instance;
    //        _instance.StartCoroutine(CSI_FinallyOpenCampaignSelectorRoutine(onOpenedSelector));
    //    }
    //}

    //[HarmonyPatch(typeof(CampaignSelectorUI), "StartWorkshopCampaign")]
    //internal class CAT_CSIPatch_StartWorkshopCampaign
    //{
    //    private static CampaignSelectorUI _instance;

    //    /** Replica of StartWorkshopCampaignRoutine. */
    //    private static IEnumerator CSI_StartWorkshopCampaignRoutine(string campaignID)
    //    {
    //        var traverse = Traverse.Create(_instance);
            
    //        traverse.Field("openedFromWorkshop").SetValue(true);
    //        traverse.Field("openedWorkshopCampaign").SetValue(true);
    //        traverse.Field("workshopCampaignID").SetValue(campaignID);

    //        var campaign = VTResources.GetSteamWorkshopCampaign(campaignID);
    //        if (campaign == null)
    //        {
    //            Debug.Log("[CAT] Missing campaign in CSI_StartWorkshopCampaignRoutine");
    //            yield break;
    //        }

    //        var vehicle = campaign.vehicle;
    //        PilotSaveManager.current.lastVehicleUsed = vehicle;
    //        PilotSaveManager.currentVehicle = VTResources.GetPlayerVehicle(vehicle);
            
    //        yield return _instance.StartCoroutine(CSIPatchUtils.BetterSetupCampaignScreenRoutine(_instance));
            
    //        traverse.Field("campaignIdx").SetValue(0);
    //        var loadedWorkshopCampaigns = (List<Campaign>)traverse.Field("loadedWorkshopCampaigns").GetValue();

    //        var campaign2 = loadedWorkshopCampaigns.FirstOrDefault(campaign3 => campaign3.campaignID == campaignID);
    //        if (campaign2 == null)
    //        {
    //            campaign2 = campaign.ToIngameCampaign();
    //            loadedWorkshopCampaigns.Add(campaign2);
    //        }
    //        traverse.Field("loadedWorkshopCampaigns").SetValue(loadedWorkshopCampaigns); //just to be safe

    //        if (campaign2 == null) yield break;
    //        _instance.campaignDisplayObject.SetActive(false);
    //        PilotSaveManager.currentCampaign = campaign2;
    //        _instance.SetupCampaignScenarios(campaign2);
    //    }

    //    private static bool Prefix(CampaignSelectorUI __instance, string campaignID)
    //    {
    //        if (PilotSaveManager.currentVehicle.vehicleName == Main.CustomAircraftPv.vehicleName) return true;

    //        _instance = __instance;
    //        _instance.StartCoroutine(CSI_StartWorkshopCampaignRoutine(campaignID));
    //        return true;
    //    }
    //}

    //[HarmonyPatch(typeof(CampaignSelectorUI), "StartWorkshopMission")]
    //internal class CAT_CSIPatch_StartWorkshopMission
    //{
    //    private static CampaignSelectorUI _instance;

    //    /** Replica of StartWorkshopCampaignRoutine. */
    //    private static IEnumerator CSI_StartWorkshopMissionRoutine(string scenarioID)
    //    {
    //        var traverse = Traverse.Create(_instance);
            
    //        traverse.Field("openedFromWorkshop").SetValue(true);
    //        traverse.Field("openedWorkshopCampaign").SetValue(false);

    //        Debug.Log("[CAT] ========== A steam workshop standalone mission should be loaded at this time ===========");
    //        var scenario = VTResources.GetSteamWorkshopStandaloneScenario(scenarioID);
    //        var vehicle = scenario.vehicle;

    //        PilotSaveManager.currentVehicle = vehicle;
    //        PilotSaveManager.current.lastVehicleUsed = vehicle.vehicleName;
    //        yield return _instance.StartCoroutine(CSIPatchUtils.BetterSetupCampaignScreenRoutine(_instance));

    //        var campaign = (Campaign)traverse.Field("swStandaloneCampaign").GetValue();
    //        if (campaign.missions != null)
    //        {
    //            campaign.missions.Clear();
    //        }
    //        else
    //        {
    //            campaign.missions = new List<CampaignScenario>(1);
    //        }
    //        var loadedWorkshopSingleScenarios = (List<CampaignScenario>)traverse.Field("loadedWorkshopSingleScenarios").GetValue();

    //        var campaignScenario = loadedWorkshopSingleScenarios.FirstOrDefault(campaignScenario2 => campaignScenario2.scenarioID == scenarioID) ??
    //                               scenario.ToIngameScenario(null);

    //        campaign.missions.Add(campaignScenario);
    //        PilotSaveManager.currentCampaign = campaign;
    //        _instance.SetupCampaignScenarios(campaign, false);

    //        //foreach (var mission in campaign.missions)
    //        //{
    //        //    if (mission.scenarioID != scenarioID) continue;
    //        //    //instance.StartMission(campaign.missions[i]);
    //        //    var startMission = _instance.GetType().GetMethod("StartMission", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    //        //    startMission.Invoke(_instance, new object[] { mission });
    //        //    yield break;
    //        //}

    //        var missionToStart = campaign.missions.DefaultIfEmpty(null).FirstOrDefault(m => m.scenarioID == scenarioID);
    //        if (missionToStart == null) yield break;

    //        var startMission = _instance.GetType().GetMethod("StartMission", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    //        if (startMission != null) startMission.Invoke(_instance, new object[] {missionToStart});
    //    }

    //    private static void Postfix(CampaignSelectorUI __instance, ref CampaignSelectorUI.WorkshopLaunchStatus __result, string scenarioID)
    //    {
    //        if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
    //            return;

    //        _instance = __instance;

    //        var steamWorkshopStandaloneScenario = VTResources.GetSteamWorkshopStandaloneScenario(scenarioID);

    //        if (steamWorkshopStandaloneScenario == null)
    //        {
    //            Debug.LogError("[CAT] Tried to start workshop scenario but scenario was null!");
    //            __result = new CampaignSelectorUI.WorkshopLaunchStatus
    //            {
    //                success = false,
    //                message = VTLStaticStrings.err_scenarioNotFound
    //            };
    //            return;
    //        }
    //        if (GameStartup.version < steamWorkshopStandaloneScenario.gameVersion)
    //        {
    //            const string errFormat = "[CAT] Tried to run workshop scenario but version incompatible. Game version: {0}, scenario game version: {1}";
    //            Debug.LogErrorFormat(errFormat, GameStartup.version, steamWorkshopStandaloneScenario.gameVersion);
    //            __result = new CampaignSelectorUI.WorkshopLaunchStatus
    //            {
    //                success = false,
    //                message = VTLStaticStrings.err_version
    //            };
    //            return;
    //        }
    //        _instance.StartCoroutine(CSI_StartWorkshopMissionRoutine(scenarioID));
    //        __result = new CampaignSelectorUI.WorkshopLaunchStatus
    //        {
    //            success = true
    //        };
    //    }
    //}


    //[HarmonyPatch(typeof(PilotSaveManager), "EnsureVehicleCollections")]
    //internal class CAT_PlayerVehicleListPatch
    //{
    //    public static void Postfix()
    //    {
    //        AircraftApi.VehicleListUpdate();
    //    }
    //}

    // TODO: deprecated?
    //[HarmonyPatch(typeof(MultiplayerSpawn), "VehicleName")]
    //class SU35_Patch_MultiplayerSpawn_VehicleName
    //{
    //    static void Postfix(MultiplayerSpawn __instance, ref string __result)
    //    {
    //        Debug.Log("SU35 MPSVN1.0");
    //        if (__instance.vehicle == (MultiplayerSpawn.Vehicles)Main.aircraftMSVId /* some unique value */)
    //        {
    //            Debug.Log("SU35 MPSVN1.1");
    //            __result = Main.CustomAircraftPv.vehicleName;
    //            Debug.Log("SU35 MPSVN1.2");
    //        }
    //    }
    //}

    // TODO: deprecated?
    //[HarmonyPatch(typeof(MultiplayerSpawn), "GetVehicleName")]
    //class SU35_Patch_MultiplayerSpawn_GetVehicleName
    //{
    //    static void Postfix(MultiplayerSpawn.Vehicles v, ref string __result)
    //    {
    //        Debug.Log("SU35 MPSGVN1.0");
    //        if (v == (MultiplayerSpawn.Vehicles)Main.aircraftMSVId /* some unique value */)
    //        {
    //            Debug.Log("SU35 MPSGVN1.1");
    //            __result = Main.CustomAircraftPv.vehicleName;
    //            Debug.Log("SU35 MPSGVN1.2");
    //        }
    //    }
    //}

    //// TODO: false-prefix, need to replace
    //[HarmonyPatch(typeof(CommRadioSource), "SetAsRadioSource")]
    //internal class CAT_CRSSRSPatch
    //{
    //    public static bool Prefix(CommRadioSource __instance)
    //    {
    //        if (PilotSaveManager.currentVehicle.vehicleName != Main.CustomAircraftPv.vehicleName)
    //            return true;
            
    //        __instance.EnsureCommSource();
            
    //        var _ = CommRadioSource.commSource;
    //        var crsObj = GameObject.Find("CommRadioSource");
    //        var cra = AircraftApi.GetChildWithName(Main.AircraftCustom, "CommRadioAudio", true);

    //        crsObj.GetComponent<AudioSource>().transform.position = cra.transform.position;
    //        crsObj.GetComponent<AudioSource>().transform.SetParent(cra.transform);
    //        //CommRadioManager.instance.SetAudioSource(__instance);
    //        __instance.ApplySettings(CommRadioSource.commSource);
    //        __instance.ApplySettings(CommRadioSource.copilotCommSource);
    //        return false;
    //    }
    //}
    
    //// TODO: false-prefix, need to replace
    //[HarmonyPatch(typeof(UnitSpawnAttribute), "SetupUIOptions")]
    //internal class CAT_USASUOPatch
    //{
    //    public static bool Prefix(UnitSpawnAttribute __instance, params string[] uiOptionsParams)
    //    {
    //        if (uiOptionsParams == null) return false;
    //        __instance.uiOptions = new List<string>();
    //        foreach (var param in uiOptionsParams)
    //        {
    //            __instance.uiOptions.Add(param);
    //        }
    //        return false;
    //    }
    //}

    //[HarmonyPatch(typeof(VTEdOptionSelector), "Display", typeof(string), typeof(string[]), typeof(object[]), typeof(int), typeof(VTEdOptionSelector.OptionSelectionObjectDelegate), typeof(VTEdOptionSelector.OptionHoverInfoDelegate))]
    //internal class CAT_Patch_VTEdOptionSelector_Display
    //{
    //    public static void Postfix(VTEdOptionSelector __instance, string[] options, int selected)
    //    {
    //        var t = Traverse.Create(__instance);
    //        var returnValues = (object[])t.Field("returnValues").GetValue();
    //        if (returnValues.Contains(Main.CustomAircraftPv.vehicleName))
    //            return;

    //        var newReturnValues = new object[returnValues.Length + 1];

    //        for (var i = 0; i < returnValues.Length; i++)
    //            newReturnValues[i] = returnValues[i];

    //        newReturnValues[returnValues.Length] = Main.CustomAircraftPv.vehicleName;

    //        t.Field("returnValues").SetValue(newReturnValues);

    //        var setupUi = __instance.GetType()
    //            .GetMethod("SetupUI", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
    //        if (setupUi != null) setupUi.Invoke(__instance, new object[] {options, selected});
    //    }
    //}
    
    //[HarmonyPatch(typeof(VTEnumProperty), "SelectButton")]
    //internal class CAT_Patch_VTEnumProperty_SelectButton
    //{
    //    public static bool Prefix(VTEnumProperty __instance)
    //    {
    //        var t = Traverse.Create(__instance);
    //        // do a quick dirty check that this is a selector for aircraft...
    //        var options = t.Field("options").GetValue<string[]>();
    //        if (!options.Contains("AV42C"))
    //            return true;

    //        var values = t.Field("values").GetValue<object[]>();
    //        // do the rapid check for our aircraft in that list, too
    //        if (options.Contains(Main.CustomAircraftPv.vehicleName)) return true;

    //        // if we don't have, just bolt it in there
    //        var newOptions = new string[options.Length + 1];
    //        var newValues = new object[values.Length + 1];

    //        for (var i = 0; i < options.Length; i++)
    //            newOptions[i] = options[i];
    //        for (var i = 0; i < values.Length; i++)
    //            newValues[i] = values[i];
    //        newOptions[options.Length] = Main.CustomAircraftPv.vehicleName;
    //        newValues[values.Length] = Main.aircraftMSVId;
    //        t.Field("options").SetValue(newOptions);
    //        t.Field("values").SetValue(newValues);
    //        return true;
    //    }
    //}
    
    //[HarmonyPatch(typeof(VTEdUnitOptionsWindow), "SetupOptionsList")]
    //internal class CAT_Patch_VTEdUnitOptionsWindow_SetupOptionsList
    //{
    //    public static void Postfix(VTEdUnitOptionsWindow __instance)
    //    {
    //        Debug.Log("SU35 VTESOLPatch 1.0");

    //        var t = Traverse.Create(__instance);
    //        Debug.Log("SU35 VTESOLPatch 1.1");
    //        var currentUnitFields = (List<VTPropertyField>)t.Field("currentUnitFields").GetValue();
    //        // approach: anywhere we have a vehicle selector option, we bolt our vehicle to it
    //        Debug.Log("SU35 VTESOLPatch 1.2");
    //        foreach (var field in currentUnitFields)
    //        {
    //            Debug.Log("SU35 VTESOLPatch 1.3: " + field.fieldName);
    //            if (field.fieldName == "vehicle" || field.type == typeof(VTEnumProperty))
    //            { 
                                
    //                Debug.Log("SU35 VTESOLPatch 1.4");
    //                // if this is what we're looking for, let's bolt onto it
    //                var t2 = Traverse.Create((VTEnumProperty)field);
    //                Debug.Log("SU35 VTESOLPatch 1.5");
    //                var options = (string[])t2.Field("options").GetValue();

    //                // make a deep copy of the original...
    //                Debug.Log("SU35 VTESOLPatch 1.6");
    //                var optionsCopy = options.Select(o => o.Copy()).ToList();
    //                // ...add our bad boy name in...
    //                Debug.Log("SU35 VTESOLPatch 1.7");
    //                optionsCopy.Add(Main.CustomAircraftPv.vehicleName);
    //                Debug.Log("SU35 VTESOLPatch 1.8");
    //                var optionsPlus = optionsCopy.ToArray();
    //                // ...and set it back! hopefully
    //                Debug.Log("SU35 VTESOLPatch 1.9");
    //                t2.Field("options").SetValue(optionsPlus);
    //            }
                
    //        }
    //    }
    //}
}

