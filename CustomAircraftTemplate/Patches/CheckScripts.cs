using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomAircraftTemplateGAV25B
{
    

    [HarmonyPatch(typeof(LoadoutConfigurator), "AttachRoutine")]
    public class GAV25B_AttachRoutinePatch2
    {
        public static void Prefix(LoadoutConfigurator __instance, int hpIdx)
        {
            //Debug.unityLogger.logEnabled = Main.logging;
            Debug.Log("GAV25B Running AttachRoutine");
        }
    }


 
}