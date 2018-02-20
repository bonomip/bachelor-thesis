using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{

    public class Body : MonoBehaviour
    {
    
        private static float MASS = 12.5f * Main.SCALE;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.05f;
        private const float MAX_ANGULAR_VELOCITY = 1.5f;

        private const float TORQUE_FORCE = 120f;

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

        public static Body attach(Transform parent, Main m)
        {
            return parent.Find(NAME).gameObject.AddComponent<Body>().linkMain(m);  
        }

        public Body linkMain(Main m)
        {
            this.main = m;
            return this;
        }

        public float kmh(){
            return this.body.velocity.magnitude * 3.6f;
        }

        //TODO migliorare rotazione mentre in movimento
        public void rotateLeft()
        {
            if (this.body.velocity.magnitude * 3.6f <= 5f) return;
            this.body.AddTorque(this.transform.forward * -TORQUE_FORCE * ( 1 - this.body.velocity.magnitude * 3.6f / 150f ) );
        }
        
        public void rotateRight()
        {
            if (this.body.velocity.magnitude * 3.6f <= 5f) return;
            this.body.AddTorque(this.transform.forward * TORQUE_FORCE * ( 1 - this.body.velocity.magnitude * 3.6f / 150f ) );
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Body hitten");
            this.main.applyDamage(other, this.transform, ARMOUR);
        }
    }
}
