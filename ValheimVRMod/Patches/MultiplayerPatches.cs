using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using ValheimVRMod.Scripts;

namespace ValheimVRMod.Patches {

    [HarmonyPatch(typeof(Player), "Start")]
    class PatchPlayerStart {
        static void Postfix(Player __instance) {
            if (__instance == Player.m_localPlayer) {
                return;
            }
            Debug.Log("PATCHING VR RPLAYER !");
            __instance.gameObject.AddComponent<VRPlayerSync>();
        }
    }
    
    [HarmonyPatch(typeof(ZNetScene), "Awake")]
    class PatchZNetSceneAwake {
        static void Postfix(ref Dictionary<int, GameObject> ___m_namedPrefabs) {
            Debug.Log("PATCHING ZNETSCENE AWAKE");
            ___m_namedPrefabs.Add("VrObj".GetStableHashCode(), new GameObject());
        }
    }
}