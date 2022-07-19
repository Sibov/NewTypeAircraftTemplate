using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomAircraftTemplateSU35
{
    /*
    [HarmonyPatch(typeof(LoadoutConfigurator), "AttachImmediate")]
    public class AttachImmediateStartPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, string weaponName, int hpIdx)
        {
            Debug.Log("SU35 Running AttachImmediate");
            __instance.DetachImmediate(hpIdx);
            Traverse traverse2 = Traverse.Create(__instance);

            
            Dictionary<string, EqInfo> allWeaponPrefabsOutput = new Dictionary<string, EqInfo>();
            Debug.Log("SU35 AI: 1.0 : " + weaponName + "," + hpIdx);
            allWeaponPrefabsOutput = (Dictionary<string, EqInfo>)traverse2.Field("allWeaponPrefabs").GetValue();
            Debug.Log("SU35 AI: 1.1");

            Transform[] hpTransformsList = (Transform[])traverse2.Field("hpTransforms").GetValue();
            Debug.Log("SU35 AI: 1.2");

            if (allWeaponPrefabsOutput.ContainsKey(weaponName))
            {
                Debug.Log("SU35 AI: 1.3");


                GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(allWeaponPrefabsOutput[weaponName].prefabPath));
                gameObject.name = allWeaponPrefabsOutput[weaponName].eqObject.name;
                gameObject.SetActive(true);
                Transform transform = gameObject.transform;
                Debug.Log("SU35 AI: 1.4");

                __instance.equips[hpIdx] = transform.GetComponent<HPEquippable>();
                Debug.Log("SU35 AI: 1.5");

                transform.parent = hpTransformsList[hpIdx];
                Debug.Log("SU35 AI: 1.6");
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                Debug.Log("SU35 AI: 1.7");
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
    

    [HarmonyPatch(typeof(LoadoutConfigurator), "DetachImmediate")]
    public class DetachImmediateStartPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, int hpIdx)
        {
            //Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("SU35 Running DetachImmediate");
            return true;
        }
    }
    */
    [HarmonyPatch(typeof(LoadoutConfigurator), "DetachRoutine")]
    public class SU35_DetachRoutinePatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, int hpIdx)
        {
            //Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("SU35 Running DetachRoutine");
            return true;
        }
    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "AttachRoutine")]
    public class SU35_AttachRoutinePatch2
    {
        public static void Prefix(LoadoutConfigurator __instance, int hpIdx)
        {
            //Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("SU35 Running AttachRoutine");
        }
    }

    [HarmonyPatch(typeof(LoadoutConfigurator), "Attach")]
    public class SU35_AttachPatch
    {
        public static bool Prefix(LoadoutConfigurator __instance, string weaponName, int hpIdx)
        {
            //Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("SU35 Running AttachRoutine");
            return true;
            
        }
    }

 
}