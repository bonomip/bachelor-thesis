using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{

    public class Body : MonoBehaviour
    {
    
        private static float MASS = Main.MASS;
        private const float DRAG = Main.DRAG;
        private const float ANGULAR_DRAG = 0.05f;
        private const float MAX_ANGULAR_VELOCITY = 3f;

        private float TORQUE_FORCE = 120f;

        private const float ARMOUR = 0.65f;

        private Main main;

        private Rigidbody body;
        
        private const string NAME = "body";
    
        void Start()
        {
            this.body = GetComponent<Rigidbody>();
            this.body.mass = MASS;
            this.body.drag = DRAG;
            this.body.angularDrag = ANGULAR_DRAG;
            this.body.maxAngularVelocity = MAX_ANGULAR_VELOCITY;
        }


        public float debug;
        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        void FixedUpdate()
        {
            //TORQUE_FORCE = debug;
        }

        public static Body attach(Transform parent, Main m)
        {
            return parent.Find(NAME).gameObject.AddComponent<Body>().linkMain(m);  
        }

        public Body linkMain(Main m)
        {
            this.main = m;
            return this;
        }

        public void rotate(float mgn, int verse)
        {
            if (mgn <= 1.4f)
                return;
            if (mgn <= 6.4f)
                this.body.AddTorque(this.transform.forward * TORQUE_FORCE * verse * ( 1 - this.body.velocity.magnitude / 41.7f ) );
            else{
                this.body.AddTorque(this.transform.forward * TORQUE_FORCE * verse * 20f);
            }
        }

        public void push(float mgn, int verse){
           //this.body.AddForce(this.transform.forward * debug * verse);
        }


        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Body hitten");
            this.main.applyDamage(other, this.transform, ARMOUR);
        }
    }
}
