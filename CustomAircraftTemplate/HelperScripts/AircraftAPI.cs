using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomAircraftTemplate
{
    class AircraftAPI
    {
        public static GameObject SEAT_ADJUST_POSE_BOUNDS;
        private static Texture2D MenuTexture;

        public static void FindSwitchBounds()
        {
            SEAT_ADJUST_POSE_BOUNDS = GetChildWithName(Main.playerGameObject, ("MasterArmPoseBounds"),false);
            Debug.Log("pose bound found: " + SEAT_ADJUST_POSE_BOUNDS);


        }

        public static VRInteractable FindInteractable(string interactableName)
        {
            foreach(VRInteractable interactble in Main.playerGameObject.GetComponentsInChildren<VRInteractable>(true))
            {
                if(interactble.interactableName == interactableName)
                {
                    return interactble;
                }
            }

            Debug.LogError($"Could not find VRinteractable: {interactableName}");
            return null;
        }

        public static HardpointVehiclePart FindHardpoint(int idx)
        {
            foreach (HardpointVehiclePart vp in Main.playerGameObject.GetComponentsInChildren<HardpointVehiclePart>(true))
            {
                if (vp.hpIdx == idx)
                {
                    Debug.Log("Found hardpoint index: " + idx);
                    return vp;
                }
            }

            Debug.LogError($"Could not find hardpoint index: {idx}");
            return null;
        }


        public static Component CopyComponent(Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }

        public static void DisableMesh(GameObject parent, WeaponManager wm, bool nochildren)
        {
            if (!nochildren)
                return;
            MeshRenderer meshes = parent.GetComponent<MeshRenderer>();

            if (meshes)
                meshes.enabled = false;


        }

        public static void DisableMesh(GameObject parent, WeaponManager wm)
        {
            MeshRenderer[] meshes = parent.GetComponentsInChildren<MeshRenderer>(true);

            foreach (MeshRenderer mesh in meshes)
            {
                if (wm != null && !isHardPoint(mesh, wm))
                {
                    mesh.enabled = false;
                }

            }

        }

        public static void DisableMesh(GameObject parent)
        {
            MeshRenderer[] meshes = parent.GetComponentsInChildren<MeshRenderer>(true);

            foreach (MeshRenderer mesh in meshes)
            {

                mesh.enabled = false;


            }

        }

        public static bool isHardPoint(MeshRenderer mesh, WeaponManager wm)
        {
            foreach (Transform transform in wm.hardpointTransforms)
            {
                if (mesh.gameObject.transform.IsChildOf(transform) && transform != wm.hardpointTransforms[15])
                {
                    return true;
                }
            }

            return false;
        }


       /* public static void Disable26Mesh()
        {
            GameObject go = Main.playerGameObject;
            WeaponManager wm = go.GetComponentInChildren<WeaponManager>(true);
            DisableMesh(GetChildWithName(go, "body"), wm);
            DisableMesh(GetChildWithName(go, "wingRightPart"), wm);
            DisableMesh(GetChildWithName(go, "wingLeftPart"), wm);
            DisableMesh(GetChildWithName(go, "elevonLeftPart"), wm);
            DisableMesh(GetChildWithName(go, "elevonRightPart"), wm);
            DisableMesh(GetChildWithName(go, "vertStabLeft_part"), wm);
            DisableMesh(GetChildWithName(go, "vertStabRight_part"), wm);
            DisableMesh(GetChildWithName(go, "fa26-leftEngine"), wm);
            DisableMesh(GetChildWithName(go, "fa26-rightEngine"), wm);
            DisableMesh(GetChildWithName(go, "LandingGear"), wm);
            DisableMesh(GetChildWithName(go, "Canopy"), wm);
            DisableMesh(GetChildWithName(go, "windshield"), wm);
            DisableMesh(GetChildWithName(go, "dash"), wm);
            DisableMesh(GetChildWithName(go, "HookTurret"), wm);
            DisableMesh(GetChildWithName(go, "airbrakeParent"), wm);
            DisableMesh(GetChildWithName(go, "HP14 TGP"));
            

        }
        */
        public static GameObject GetChildWithName(GameObject obj, string name, bool check)
        {

            Debug.unityLogger.logEnabled = Main.logging;
            Transform[] children = obj.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (check) { Debug.Log("Looking for:" + name + ", Found:" + child.name); }
                if (child.name == name || child.name == (name + "(Clone)"))
                {
                    return child.gameObject;
                }
            }


            return null;

        }

        public static void TestRadarDetection()
        {
            Debug.unityLogger.logEnabled = Main.logging;
            Radar radar = Main.aircraftMirage.GetComponent<Radar>();
            Debug.Log("Radar Count = " + radar.detectedUnits.Count);
        }
        
        //Loads a png into the game
        public static IEnumerator CreatePlaneMenuItem()
        {
            Debug.unityLogger.logEnabled = Main.logging;
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(Path.Combine(Main.instance.ModFolder, AircraftInfo.PreviewPngFileName));
            yield return www.SendWebRequest();

            if (www.responseCode != 200)
            {
                Debug.Log("WWW Response code isn't 200, it's " + www.responseCode + "\n" + www.error);
            }
            else
            {
                MenuTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Debug.Log("Loaded plane image.");
            }

            Debug.Log("Debug 1");
            Traverse traverse = Traverse.Create(typeof(VTResources));
            PlayerVehicleList vehicles = (PlayerVehicleList)traverse.Field("playerVehicles").GetValue();

            

            Debug.Log("Debug 2");
            PlayerVehicle newVehicle = ScriptableObject.CreateInstance<PlayerVehicle>();
            newVehicle.vehicleName = AircraftInfo.AircraftName;
            newVehicle.nickname = AircraftInfo.AircraftNickName;
            newVehicle.description = AircraftInfo.AircraftDescription;
            newVehicle.campaigns = PilotSaveManager.GetVehicle("F/A-26B").campaigns;
            newVehicle.vehicleImage = MenuTexture;
            newVehicle.vehiclePrefab = Main.aircraftPrefab;
            vehicles.playerVehicles.Add(newVehicle);

            Debug.Log("Debug 3");
            traverse.Field("playerVehicles").SetValue(vehicles);


            Debug.Log("Debug 4");
            Traverse traverse2 = Traverse.Create(typeof(PilotSaveManager));
            List<PlayerVehicle> vehicleList = (List<PlayerVehicle>)traverse2.Field("vehicleList").GetValue();
            vehicleList.Add(newVehicle);
            traverse.Field("vehicleList").SetValue(vehicleList);


        }




       
    }




}
