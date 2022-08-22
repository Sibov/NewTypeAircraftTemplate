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



namespace CustomAircraftTemplateGAV25B
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
            Debug.Log("GAV25B Setup Arming Text");

            WeaponManager wm = Main.aircraftCustom.GetComponent<WeaponManager>();
            int aircraftNodeLength = wm.hardpointTransforms.Length;
            Debug.Log("GAV25B Setup Arming Text 1.0: Length = " + aircraftNodeLength);
            GameObject aircraft = AircraftAPI.GetChildWithName(Main.aircraftCustom, "WeaponArmingButtons", false);
            Debug.Log("GAV25B Setup Arming Text 1.1");
            for (int i = 0; i < aircraftNodeLength; i++)
            {
                string textArmingItem = "Text" + i;
                equippedItem = wm.GetEquip(i);
                Debug.Log("GAV25B Setup Arming Text 1.1.1");


                GameObject textItem = AircraftAPI.GetChildWithName(aircraft, textArmingItem, false);
                Debug.Log("GAV25B Setup Arming Text 1.1.2");
                if (equippedItem != null && equippedItem.armable)
                {
                    Debug.Log("GAV25B Setup Arming Text 1.2: " + i + " , Item: " + equippedItem.shortName);
                    nameLength = equippedItem.shortName.Length;
                    shortenedEquipName = equippedItem.shortName.Substring(0, 1) + equippedItem.shortName.Substring(nameLength - 2, 2);
                }
                else
                {
                    Debug.Log("GAV25B Setup Arming Text 1.2.2 ");
                    shortenedEquipName = "NON";
                }
                Debug.Log("GAV25B Setup Arming Text 1.3");
                Text textComponentItem = textItem.GetComponent<Text>();
                textComponentItem.text = shortenedEquipName;

            }
            GameObject masterArmingSwitchInteractable = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MasterArmingSwitchInteractable", false);
            masterArmingSwitchInteractableiVRLever = masterArmingSwitchInteractable.GetComponent<VRLever>();
            masterArmingSwitchInteractableiVRLever.OnSetState.AddListener(SwitchWeaponTextOn);


            return;
        }

        public static void SwitchWeaponTextOn(int MasterArmOn)
        {
            // if (!AircraftInfo.AircraftSelected) { return; }
            if (PilotSaveManager.currentVehicle.vehicleName != Main.customAircraftPV.vehicleName)
                return;
            GameObject weaponArmingButtons = AircraftAPI.GetChildWithName(Main.aircraftCustom, "WeaponArmingButtons", false);

            for (int i = 0; i < 10; i++)
            {

                GameObject weaponArmingText = AircraftAPI.GetChildWithName(weaponArmingButtons, "Text" + i, false);
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
            Debug.Log("GAV25B IRTS 1.1");
            Main.miniicp = AircraftAPI.GetChildWithName(Main.aircraftCustom, "MiniMFDICP", false);
            Debug.Log("GAV25B IRTS 1.2");
            radarcontactlistobj = AircraftAPI.GetChildWithName(Main.miniicp, "RadarContactList", false);
            Main.radarcontactlist = radarcontactlistobj.GetComponent<TextMeshPro>();
            Debug.Log("GAV25B IRTS 1.3");
            GameObject radarObject = AircraftAPI.GetChildWithName(Main.aircraftCustom, "Radar2", false);
            Main.radar = radarObject.GetComponentInChildren<Radar>();
            Debug.Log("GAV25B IRTS 1.4");

        }
        public static void IdentifiedRadarTargets()
        {

            i = 0;
            oldcumText = "";
            Debug.Log("GAV25B IRT 1.1");
            foreach (Actor unit in Main.radar.detectedUnits)
            {
                Debug.Log("GAV25B unit: " + unit);
                text = i + ": " + unit.actorName + " \n";
                Debug.Log("GAV25B text: " + text);
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
                Debug.Log("GAV25B found text mesh");
                clockDisplayLocalTextComp.text = timeNow;
            }
        }
    }
}


