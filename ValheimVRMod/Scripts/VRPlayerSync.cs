using System;
using UnityEngine;

namespace ValheimVRMod.Scripts {
    public class VRPlayerSync : MonoBehaviour {

        private bool vrikInitialized;
        
        public GameObject camera = new GameObject();
        public GameObject rightHand = new GameObject();
        public GameObject leftHand = new GameObject();

        private void Awake() {
            GetComponent<ZNetView>().Register("VrSyncObjects", new Action<long, Transform, Transform, Transform>(RPC_VrSyncObjects));
        }

        private void RPC_VrSyncObjects(long sender, Transform cam, Transform lHand, Transform rHand) {
            
            Debug.Log("Invoked RPC_VrSyncObjects !");
            camera.transform.position = cam.position;
            camera.transform.rotation = cam.rotation;
            leftHand.transform.position = lHand.position;
            leftHand.transform.rotation = lHand.rotation;
            rightHand.transform.position = rHand.position;
            rightHand.transform.rotation = rHand.rotation;
            
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
                GetComponent<ZNetView>().InvokeRPC(ZNetView.Everybody, "VrSyncObjects", 
                    camera.transform, leftHand.transform, rightHand.transform);
            }
        }
    }
}