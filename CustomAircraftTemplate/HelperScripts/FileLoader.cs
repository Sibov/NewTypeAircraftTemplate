﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomAircraftTemplate
{
    static class FileLoader
    {
        //PUBLIC LOADING METHODS
        //Thanks Baan/GentleLeviathan!!!!
        public static AssetBundle GetAssetBundleAsGameObject(string path, string name)
        {
            Debug.Log("AssetBundleLoader: Attempting to load AssetBundle...");
            AssetBundle bundle = null;
            try
            {
                bundle = AssetBundle.LoadFromFile(path);
                Debug.Log("AssetBundleLoader: Success.");
                return (AssetBundle)bundle;
            }
            catch (Exception e)
            {
                Debug.Log("AssetBundleLoader: Couldn't load AssetBundle from path: '" + path + "'. Exception details: e: " + e.Message);
                return null;
            }

        }
        public static GameObject GetPrefabAsGameObject(AssetBundle bundleLoad, string name)
        {
            Debug.Log("AssetBundleLoader: Attempting to retrieve: '" + name + "' as type: 'GameObject'" + " from " + bundleLoad);
            try
            {
                var temp = bundleLoad.LoadAsset(name, typeof(GameObject));
                Debug.Log("AssetBundleLoader: Success.");
                return (GameObject)temp;
            }
            catch (Exception e)
            {
                Debug.Log("AssetBundleLoader: Couldn't retrieve GameObject from AssetBundle.");
                return null;
            }

        }
    }


}
