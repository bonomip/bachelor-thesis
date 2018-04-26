using UnityEngine;
using System.Collections;


namespace Application.Component.MainCamera
{
    public class GoToPlayer : MonoBehaviour
    {
        private static Controller.Controller ctrl;

        private float time, timer;
        
        private Transform camera;

        public static void attach(Controller.Controller controller)
        {
            GameObject.Find(Application.MAIN_CAMERA).AddComponent<GoToPlayer>();
            ctrl = controller;
        }
    
        void Start()
        {
            this.time = 0;
            this.timer = 3f;

            this.camera = GameObject.Find(Application.TANK_CAMERA).transform;
        }

        void FixedUpdate()
        {
            this.time += Time.deltaTime;
            
            if (this.time >= this.timer)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, this.camera.position, 0.05f);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.camera.rotation, 0.05f);

                if (Vector3.Distance(this.transform.position, this.camera.position) < 0.05f)
                {
                    ctrl.startGame(this);
                }
                return;
            }
            
            this.transform.position = Vector3.Lerp(this.transform.position, this.camera.position, 0.015f);
        }
    }
}
