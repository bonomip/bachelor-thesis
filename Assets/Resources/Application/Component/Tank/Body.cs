using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{

    public class Body : MonoBehaviour
    {
        private const float MASS = 150;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.75f;
        private const float MAX_ANGULAR_VELOCITY = 1.5f;

        private const float TORQUE_FORCE = 6000f;

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

        public void Update()
        {
            if( GetComponent<Rigidbody>().velocity.magnitude > 15f ) Debug.Log(GetComponent<Rigidbody>().velocity.magnitude * 3.6f + " km/h");
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

        //TODO migliorare rotazione mentre in movimento a velocità alta ( 66km/h )
        public void rotateLeft()
        {
            if (this.body.velocity.magnitude * 3.6f <= 2.5f) return;
            this.body.AddTorque(this.transform.forward * -TORQUE_FORCE * ( 1 - this.body.velocity.magnitude * 3.6f / 100f ) );
        }
        
        public void rotateRight()
        {
            if (this.body.velocity.magnitude * 3.6f <= 2.5f) return;
            this.body.AddTorque(this.transform.forward * TORQUE_FORCE * ( 1 - this.body.velocity.magnitude * 3.6f / 100f ) );
        }
        
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Body hitten");
            this.main.applyDamage(other, this.transform, ARMOUR);
        }
    }
}
