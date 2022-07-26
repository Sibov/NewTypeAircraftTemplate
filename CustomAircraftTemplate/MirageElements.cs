using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Harmony;
using UnityEngine.UI;
using TMPro;



namespace CustomAircraftTemplateF35
{
    public class MirageElements : MonoBehaviour
    {
        private static int nameLength;
        private static string shortenedEquipName;
        private static string equippedItemName;
        private static HPEquippable equippedItem;
        private static int i;
        private static VRLever masterArmingSwitchInteractableiVRLever;
        private Text textItem;
        private static DateTime currentTime;
        private static string timeNow;
        private static int currentHour;
        private static int currentMinute;
        private static Radar radar;
        private static GameObject radarcontactlistobj;
        public static String oldcumText;
        private static string cumText;
        private static string text;

        public static void SetupArmingText()
              {
            //if (!AircraftInfo.AircraftSelected) { return; }
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            //Debug.unityLogger.logEnabled = Main.logging;
                  Debug.Log("F35 Setup Arming Text");

                  WeaponManager wm = Main.aircraftCustom.GetComponent<WeaponManager>();
                  int aircraftNodeLength = wm.hardpointTransforms.Length;
                  Debug.Log("F35 Setup Arming Text 1.0: Length = " + aircraftNodeLength);
                  GameObject aircraft = AircraftAPI.GetChildWithName(Main.aircraftCustom, "WeaponArmingButtons", false);
                  Debug.Log("F35 Setup Arming Text 1.1");
                  for (int i = 0; i < aircraftNodeLength; i++)
                  {
                      string textArmingItem = "Text" + i;
                       equippedItem = wm.GetEquip(i);
                      Debug.Log("F35 Setup Arming Text 1.1.1");


                      GameObject textItem = AircraftAPI.GetChildWithName(aircraft, textArmingItem, false);
                      Debug.Log("F35 Setup Arming Text 1.1.2");
                      if (equippedItem != null && equippedItem.armable)
                      {
                          Debug.Log("F35 Setup Arming Text 1.2: " + i + " , Item: " + equippedItem.shortName);
                          nameLength = equippedItem.shortName.Length;
                          shortenedEquipName = equippedItem.shortName.Substring(0, 1) + equippedItem.shortName.Substring(nameLength-2, 2);
                      }
                      else
                      {
                          Debug.Log("F35 Setup Arming Text 1.2.2 ");
                          shortenedEquipName = "NON";
                      }
                      Debug.Log("F35 Setup Arming Text 1.3");
                      Text textComponentItem = textItem.GetComponent<Text>();
                      textComponentItem.text = shortenedEquipName;

                  }
                    GameObject masterArmingSwitchInteractable = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MasterArmingSwitchInteractable", false);
                    masterArmingSwitchInteractableiVRLever = masterArmingSwitchInteractable.GetComponent<VRLever>();
                    masterArmingSwitchInteractableiVRLever.OnSetState.AddListener(SwitchWeaponTextOn);


                    return ;
              }

        public static void SwitchWeaponTextOn(int MasterArmOn)
        {
           // if (!AircraftInfo.AircraftSelected) { return; }
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            GameObject weaponArmingButtons = AircraftAPI.GetChildWithName(Main.aircraftCustom, "WeaponArmingButtons", false);
            
                for (int i = 0; i < 10; i++)
                {

                    GameObject weaponArmingText = AircraftAPI.GetChildWithName(weaponArmingButtons, "Text"+i, false);
                    Text textItem = weaponArmingText.GetComponent<Text>();

                if (MasterArmOn == 1)
                {
                    textItem.enabled = true;
                }
                else
                {
                    textItem.enabled = false;
                }
                }

            }

        public static void SetUpGauges()
        {
            Battery componentInChildren = Main.aircraftCustom.GetComponentInChildren<Battery>(true);
            FlightInfo componentInChildren2 = Main.aircraftCustom.GetComponentInChildren<FlightInfo>(true);
            GameObject childWithName3 = AircraftAPI.GetChildWithName(Main.aircraftCustom, "ClimbGauge", false);
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
            dashVertGauge.measures = Main.aircraftCustom.GetComponent<MeasurementManager>();
        }

        public static void IdentifiedRadarTargetsSetup()
        {
            Debug.Log("F35 IRTS 1.1");
            Main.miniicp = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MiniMFDICP", false);
            Debug.Log("F35 IRTS 1.2");
            radarcontactlistobj = AircraftAPI.GetChildWithName(Main.miniicp, "RadarContactList", false);
            Main.radarcontactlist = radarcontactlistobj.GetComponent<TextMeshPro>();
            Debug.Log("F35 IRTS 1.3");
            GameObject radarObject = AircraftAPI.GetChildWithName(Main.aircraftCustom, "Radar2", false);
             Main.radar = radarObject.GetComponentInChildren<Radar>();
            Debug.Log("F35 IRTS 1.4");
            
        }
        public static void IdentifiedRadarTargets()
        {
            
            i = 0;
            oldcumText = "";
            Debug.Log("F35 IRT 1.1");
            foreach (Actor unit in Main.radar.detectedUnits)
            {
                Debug.Log("F35 unit: " + unit);
                text = i + ": " + unit.actorName + " \n";
                Debug.Log("F35 text: " + text);
                cumText = oldcumText + text;
                oldcumText = cumText;
                i++;
            }
            Main.radarcontactlist.text = cumText;

        }


            public static void ClockUpdate()
        {
            currentTime = DateTime.Now;

            timeNow = currentTime.ToString("HH:mm:ss");
            currentHour = currentTime.Hour;
            currentMinute = currentTime.Minute;
            GameObject clockItem = AircraftAPI.GetChildWithName(Main.aircraftCustom, "Clock", false);
            GameObject clockDisplayLocal = AircraftAPI.GetChildWithName(clockItem, "testWatchLocal", false);
            if (!clockDisplayLocal) { Main.aircraftLoaded = false; }
            GameObject clockDisplayLocalText = AircraftAPI.GetChildWithName(clockDisplayLocal, "Text", false);
            var clockDisplayLocalTextComp = clockDisplayLocalText.GetComponent<Text>();



            if (clockDisplayLocalTextComp != null)
            {
                Debug.Log("F35 found text mesh");
                clockDisplayLocalTextComp.text = timeNow;
            }
        }

        [HarmonyPatch(typeof(Radar), "ProcessUnit")]
        public static class F35_PatchRadarProcessingForGroundAttack
        {
            public static RaycastHit raycastHit;

            public static bool Prefix(Radar __instance, Actor a, float dotThresh, bool hasMapGen)
            {
                // if (!AircraftInfo.AircraftSelected) { return true; }
                if (!Main.aircraftLoaded)
                {
                    return true;
                }
                if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                    return true;
                //Debug.unityLogger.logEnabled = Main.logging;

                if (!a || !a.gameObject.activeSelf || a.name == "Enemy Infantry MANPADS" || a.name == "Enemy Infantry" || a.name == "Allied Infantry MANPADS" || a.name == "Allied Infantry")
                {
                    return false;
                }
                if (a.finalCombatRole == Actor.Roles.Air || a.role == Actor.Roles.GroundArmor || a.role == Actor.Roles.Ground)
                {
                    Debug.Log("F35 Radar found: " + a.actorName);
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
                        float radarSignalStrength = Radar.GetRadarSignalStrength(__instance.radarTransform.position, a,false);
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

            public static void Postfix()
            {
                if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                    return;
                IdentifiedRadarTargets();
            }
        }

       


    }
}
