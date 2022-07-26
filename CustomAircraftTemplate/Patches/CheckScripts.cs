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
    

    [HarmonyPatch(typeof(LoadoutConfigurator), "AttachRoutine")]
    public class SU35_AttachRoutinePatch2
    {
        public static void Prefix(LoadoutConfigurator __instance, int hpIdx)
        {
            //Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("SU35 Running AttachRoutine");
        }
    }


 
}