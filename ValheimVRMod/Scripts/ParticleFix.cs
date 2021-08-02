using UnityEngine;
using ValheimVRMod.Utilities;

namespace ValheimVRMod.Scripts {
    [DefaultExecutionOrder(WeaponUtils.k_VrikExecutionOrder)]
    public class ParticleFix : MonoBehaviour {

        private Transform origin;
        
        private void Awake() {
            origin = new GameObject().transform;
            origin.parent = transform.parent;
            origin.localPosition = transform.localPosition;
            transform.SetParent(null);
        }
        
        private void LateUpdate() {

            if (origin == null) {
                Destroy(gameObject);
                return;
            }
            
            transform.position = origin.position;
        }

        public static void maybeFix(GameObject target) {

            var particleSystem = target.GetComponentInChildren<ParticleSystem>();

            if (particleSystem != null) {
                particleSystem.gameObject.AddComponent<ParticleFix>();
            }
        }
    }
}