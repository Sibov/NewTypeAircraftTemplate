using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CustomAircraftTemplate
{
    public class MpPlugin
    {
        public static bool MPActive = false;
     

        public void MPlock()
        {
            Debug.unityLogger.logEnabled = Main.logging;
            FlightLogger.Log($"Found Multiplayer set {AircraftInfo.AircraftName} mp");
            if (MPActive)
                return;
            MPActive = true;
            //PlayerManager.PlayerIsCustomPlane = true;
            //PlayerManager.LoadedCustomPlaneString = AircraftInfo.AircraftName;

            PlayerManager.onSpawnLocalPlayer = (UnityAction<PlayerManager.CustomPlaneDef>)Delegate.Combine(PlayerManager.onSpawnLocalPlayer, new UnityAction<PlayerManager.CustomPlaneDef>(MPRespawnHook));
            PlayerManager.onSpawnClient = (UnityAction<PlayerManager.CustomPlaneDef>)Delegate.Combine(PlayerManager.onSpawnClient, new UnityAction<PlayerManager.CustomPlaneDef>(ClientAircraftSpawned));

            RegisterCustomPlane();
        }

        private void MPRespawnHook(PlayerManager.CustomPlaneDef def)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            FlightLogger.Log("MP Respawn Hook");
            if (PlayerManager.LoadedCustomPlaneString == AircraftInfo.AircraftName && PlayerManager.PlayerIsCustomPlane)
            {
                this.MPRadio(def.planeObj);
            }

        }

        public void MPRadio(GameObject f26)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("MP Radio Start");

            GameObject mpradiobutton1 = AircraftAPI.GetChildWithName(f26, "1", false);
            GameObject mpradiobutton2 = AircraftAPI.GetChildWithName(f26, "2", false);
            GameObject mpradiobutton3 = AircraftAPI.GetChildWithName(f26, "3", false);
            GameObject mpradiobutton4 = AircraftAPI.GetChildWithName(f26, "4", false);
            GameObject mpradiobutton5 = AircraftAPI.GetChildWithName(f26, "5", false);
            GameObject mpradiobutton6 = AircraftAPI.GetChildWithName(f26, "6", false);
            GameObject mpradiobutton7 = AircraftAPI.GetChildWithName(f26, "7", false);
            GameObject mpradiobutton8 = AircraftAPI.GetChildWithName(f26, "8", false);
            GameObject mpradiobutton9 = AircraftAPI.GetChildWithName(f26, "9", false);
            GameObject mpradiobutton0 = AircraftAPI.GetChildWithName(f26, "0", false);
            GameObject mpradiobuttonClr = AircraftAPI.GetChildWithName(f26, "Clr", false);
            GameObject mpradionewDisplay = AircraftAPI.GetChildWithName(f26, "Display(Clone)", false);

            mpradiobutton0.SetActive(false);
            mpradiobutton1.SetActive(false);
            mpradiobutton2.SetActive(false);
            mpradiobutton3.SetActive(false);
            mpradiobutton4.SetActive(false);
            mpradiobutton5.SetActive(false);
            mpradiobutton6.SetActive(false);
            mpradiobutton7.SetActive(false);
            mpradiobutton8.SetActive(false);
            mpradiobutton9.SetActive(false);
            mpradiobuttonClr.SetActive(false);
            mpradionewDisplay.SetActive(false);
            
        }

        private void ClientAircraftSpawned(PlayerManager.CustomPlaneDef def)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            bool flag = def.CustomPlaneString == AircraftInfo.AircraftName;
            if (flag)
            {
                // Debug.Log("spawned f16 in mp");
                AiSetup.CreateAi(def.planeObj);
                //clientAircraftSwapF.aSwaper = this;
                //clientAircraftSwapF.doSetup();
            }
        }

        private void RegisterCustomPlane()
        {
            Debug.unityLogger.logEnabled = Main.logging;
            PlayerManager.RegisterCustomPlane(AircraftInfo.AircraftName, "F/A-26B");
        }

        public void SetCustomPlaneMP()
        {

            AircraftInfo.AircraftSelected = true;
            PlayerManager.SetCustomPlane(AircraftInfo.AircraftName);
            
        }

        public void UnSetCustomPlaneMP()
        {
            if (PlayerManager.LoadedCustomPlaneString != AircraftInfo.AircraftName) return;
            
            PlayerManager.PlayerIsCustomPlane = false;
            PlayerManager.SetCustomPlane("none");

        }


        public bool CheckPlaneSelected()
        {
            if (PlayerManager.LoadedCustomPlaneString == AircraftInfo.AircraftName && PlayerManager.PlayerIsCustomPlane)
            {
                AircraftInfo.AircraftSelected = true;
                return true;
            }

            AircraftInfo.AircraftSelected = false;
            return false;
       
        }

    }
}
