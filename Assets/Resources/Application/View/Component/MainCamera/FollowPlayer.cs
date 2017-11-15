using UnityEngine;
using System.Collections;


namespace Application.View.Component.MainCamera
{
    public class FollowPlayer : MonoBehaviour
    {
        private Transform tank_camera;

        public static void attach()
        {
            GameObject.Find(Application.MAIN_CAMERA).AddComponent<FollowPlayer>();
        }

        void Start()
        {
            this.tank_camera = GameObject.Find(Application.TANK_CAMERA).transform;    
        }

        private void FixedUpdate()
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.tank_camera.position, 0.15f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.tank_camera.rotation, 0.25f);
        }
    }
}
