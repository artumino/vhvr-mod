using System;
using UnityEngine;

namespace ValheimVRMod.Scripts {
    public class VRPlayerSync : MonoBehaviour {

        private bool vrikInitialized;
        
        public GameObject camera = new GameObject();
        public GameObject rightHand = new GameObject();
        public GameObject leftHand = new GameObject();

        private void Awake() {
            //if (GetComponent<Player>() == Player.m_localPlayer) {
            GetComponent<ZNetView>().Register("VrSyncObjects", new Action<long, ZPackage>(RPC_VrSyncObjects));
            //}
        }

        private void RPC_VrSyncObjects(long sender, ZPackage pkg) {
            
            foreach (Player player in Player.GetAllPlayers()) {
                if (player != Player.m_localPlayer && player.GetComponent<ZNetView>().GetZDO().m_owner == sender) {
                    player.GetComponent<VRPlayerSync>().UpdateOtherPlayer(pkg);
                }
            }
        }

        public void UpdateOtherPlayer(ZPackage pkg) {

            Debug.Log("Invoked RPC_VrSyncObjects !");
            camera.transform.position = pkg.ReadVector3();
            camera.transform.rotation = pkg.ReadQuaternion();
            leftHand.transform.position = pkg.ReadVector3();
            leftHand.transform.rotation = pkg.ReadQuaternion();
            rightHand.transform.position = pkg.ReadVector3();
            rightHand.transform.rotation = pkg.ReadQuaternion();

            if (vrikInitialized) {
                return;
            }

            VrikCreator.initialize(GetComponent<Player>().gameObject, leftHand.transform,
                rightHand.transform, camera.transform);

            vrikInitialized = true;
            
        }
        
        private void Update() {

            Player player = GetComponent<Player>();

            if (player == Player.m_localPlayer) {
                ZPackage pkg = new ZPackage();
                pkg.Write(camera.transform.position);
                pkg.Write(camera.transform.rotation);
                pkg.Write(leftHand.transform.position);
                pkg.Write(leftHand.transform.rotation);
                pkg.Write(rightHand.transform.position);
                pkg.Write(rightHand.transform.rotation);
                GetComponent<ZNetView>().InvokeRPC(ZNetView.Everybody, "VrSyncObjects", pkg);
            }
        }
    }
}