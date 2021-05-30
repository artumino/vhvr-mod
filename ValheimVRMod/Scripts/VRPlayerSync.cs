using UnityEngine;

namespace ValheimVRMod.Scripts {
    public class VRPlayerSync : MonoBehaviour {

        private bool vrikInitialized;
        
        public GameObject camera = new GameObject();
        public GameObject rightHand = new GameObject();
        public GameObject leftHand = new GameObject();
        
        public void initialize(GameObject cam, GameObject lHand, GameObject rHand) {
            camera = syncable(cam);
            leftHand = syncable(lHand);
            rightHand = syncable(rHand);

        }

        private GameObject syncable(GameObject obj) {
            
            Debug.Log("Making Objects Syncable");
            
            obj.AddComponent<ZNetView>();
            obj.AddComponent<ZSyncTransform>();
            return obj;
        }

        private void FixedUpdate() {
            
            if (GetComponent<Player>() == Player.m_localPlayer) {
                sendVrData();
            }
            else if (GetComponent<ZNetView>() != null) {
                receiveVrData();
            }
        }

        private float lastTimeChecked;
        private void sendVrData() {

            // send vr data after each second
            if (Time.fixedTime - lastTimeChecked < 1.0f) {
                return;
            } 
            
            Debug.Log("Sending VR Data");
            
            ZPackage pkg = new ZPackage();
            pkg.Write(camera.GetComponent<ZNetView>().GetZDO().m_uid);
            pkg.Write(leftHand.GetComponent<ZNetView>().GetZDO().m_uid);
            pkg.Write(rightHand.GetComponent<ZNetView>().GetZDO().m_uid);
            GetComponent<ZNetView>().GetZDO().Set("vr_data", pkg.GetArray());
            lastTimeChecked = Time.fixedTime;
        }
        
        private void receiveVrData() {
            
            if (vrikInitialized) {
                return;
            }

            var vr_data = GetComponent<ZNetView>().GetZDO().GetByteArray("vr_data");
            
            if (vr_data == null) {
                return;
            }
            
            Debug.Log("Trying init VRIK");

            ZPackage pkg = new ZPackage(vr_data);

            camera = ZNetScene.instance.FindInstance(pkg.ReadZDOID()).gameObject;
            leftHand = ZNetScene.instance.FindInstance(pkg.ReadZDOID()).gameObject;
            rightHand = ZNetScene.instance.FindInstance(pkg.ReadZDOID()).gameObject;
            
            VrikCreator.initialize(gameObject, leftHand.transform,
                rightHand.transform, camera.transform);
            
            vrikInitialized = true;
        }
        
    }
}