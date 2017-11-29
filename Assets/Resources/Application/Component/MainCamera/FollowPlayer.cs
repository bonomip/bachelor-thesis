using UnityEngine;
using System.Collections;


namespace Application.Component.MainCamera
{
    public class FollowPlayer : MonoBehaviour
    {
        private Transform camera;

        public float y_offset = 2.1f, z_offset = -10f;

        public static void attach()
        {
            GameObject.Find(Application.MAIN_CAMERA).AddComponent<FollowPlayer>();
        }

        void Start()
        {
            this.camera = GameObject.Find(Application.TANK_CAMERA).transform;
        }

        //TODO fixare camera con i movimenti del tank... tipo quando si capovolge ecc.

        private void FixedUpdate()
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.camera.position, 0.1f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.camera.rotation, 0.15f);
            Vector3 rot = this.transform.rotation.eulerAngles; rot.z = 0; this.transform.rotation = Quaternion.Euler(rot);
        }
    }
}
