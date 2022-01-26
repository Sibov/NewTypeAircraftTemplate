using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomAircraftTemplate
{
    /*
    [HarmonyPatch(typeof(LoadoutConfigurator), "AttachImmediate")]
    public class AttachImmediateStartPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, string weaponName, int hpIdx)
        {
            Debug.Log("Running AttachImmediate");
            __instance.DetachImmediate(hpIdx);
            Traverse traverse2 = Traverse.Create(__instance);

            
            Dictionary<string, EqInfo> allWeaponPrefabsOutput = new Dictionary<string, EqInfo>();
            Debug.Log("AI: 1.0 : " + weaponName + "," + hpIdx);
            allWeaponPrefabsOutput = (Dictionary<string, EqInfo>)traverse2.Field("allWeaponPrefabs").GetValue();
            Debug.Log("AI: 1.1");

            Transform[] hpTransformsList = (Transform[])traverse2.Field("hpTransforms").GetValue();
            Debug.Log("AI: 1.2");

            if (allWeaponPrefabsOutput.ContainsKey(weaponName))
            {
                Debug.Log("AI: 1.3");


                GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(allWeaponPrefabsOutput[weaponName].prefabPath));
                gameObject.name = allWeaponPrefabsOutput[weaponName].eqObject.name;
                gameObject.SetActive(true);
                Transform transform = gameObject.transform;
                Debug.Log("AI: 1.4");

                __instance.equips[hpIdx] = transform.GetComponent<HPEquippable>();
                Debug.Log("AI: 1.5");

                transform.parent = hpTransformsList[hpIdx];
                Debug.Log("AI: 1.6");
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                Debug.Log("AI: 1.7");
                transform.localScale = Vector3.one;
                __instance.equips[hpIdx].OnConfigAttach(__instance);
                /*
                if (__instance.OnAttachHPIdx != null)
                {
                    __instance.OnAttachHPIdx(hpIdx);
                }
                
            }
            __instance.UpdateNodes();
            return false;

        }
    }
    */

    [HarmonyPatch(typeof(LoadoutConfigurator), "DetachImmediate")]
    public class DetachImmediateStartPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, int hpIdx)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("Running DetachImmediate");
            return true;
        }
    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "DetachRoutine")]
    public class DetachRoutinePatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, int hpIdx)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("Running DetachRoutine");
            return true;
        }
    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "AttachRoutine")]
    public class AttachRoutinePatch2
    {
        public static void Prefix(LoadoutConfigurator __instance, int hpIdx)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("Running AttachRoutine");
        }
    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "Attach")]
    public class AttachPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, string weaponName, int hpIdx)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("Running AttachRoutine");
            return true;
            
        }
    }

    [HarmonyPatch(typeof(FloatingOriginShifter), "FixedUpdate")]
    public class FOSPatch
    {
        public static bool Prefix(FloatingOriginShifter __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("FOS Rigidbody1 0");
            __instance.rb = Main.aircraftMirage.GetComponent<Rigidbody>();
            Debug.Log("FOS Rigidbody:" + __instance.rb);
            Debug.Log("FOS Rigidbody Sqr Mag:" + __instance.rb.position.sqrMagnitude);
            Traverse traverse = Traverse.Create(__instance);
            object sqrThreshold = traverse.Field("sqrThreshold").GetValue();
            Debug.Log("FOS Rigidbody Sqr Thresh:" + sqrThreshold);
            return true;
        }
    }

    [HarmonyPatch(typeof(DashAttitudeIndicator), "Update")]
    public class DashAttPatch
    {
        public static bool Prefix(DashAttitudeIndicator __instance)
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Quaternion b = Quaternion.Euler(0f, 0f, -__instance.flightInfo.roll);
            Quaternion b2 = Quaternion.Euler(__instance.flightInfo.pitch, 0f, 0f);
            Quaternion b3 = Quaternion.Euler(0f, __instance.flightInfo.heading, 0f);

            __instance.rollTf.localRotation = Quaternion.Slerp(__instance.rollTf.localRotation, b, __instance.lerpRate * Time.deltaTime);
            __instance.pitchTf.localRotation = Quaternion.Slerp(__instance.pitchTf.localRotation, b2, __instance.lerpRate * Time.deltaTime);
          //  __instance.rollTf.localRotation = Quaternion.Slerp(__instance.rollTf.localRotation, b3, __instance.lerpRate * Time.deltaTime);

            return false;
        }

        }
    

}