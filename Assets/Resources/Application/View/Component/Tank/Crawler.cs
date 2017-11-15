using UnityEngine;
using System.Collections;


namespace Application.View.Component.Tank
{

    public abstract class Crawler : MonoBehaviour
    {

        public Rigidbody[] wheels;

        private const float MASS = 50;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 10f;
        
        private const float WHEEL_MASS = 15;
        private const float WHEEL_DRAG = 0.5f;
        private const float WHEEL_ANGULAR_DRAG = 0.5f;

        private const float WHEEL_MAX_ANGULAR_VELOCITY = 45f;

        public bool damaged = false;

        private readonly string[] wheelNames = { "0", "1", "2", "3", "4", "4.5", "0.5" };

        void Start()
        {
            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;

            this.wheels = new Rigidbody[wheelNames.Length];

            for (int i = 0; i < wheelNames.Length; i++)
            {
                this.wheels[i] = this.transform.Find(wheelNames[i]).GetComponent<Rigidbody>();
                this.wheels[i].gameObject.AddComponent<Wheel>();
                this.wheels[i].mass = WHEEL_MASS;
                this.wheels[i].maxAngularVelocity = WHEEL_MAX_ANGULAR_VELOCITY;
                this.wheels[i].drag = WHEEL_DRAG;
                this.wheels[i].angularDrag = WHEEL_ANGULAR_DRAG;
            }
        }

        public void move(float force)
        {
            if( !this.damaged ) foreach (Rigidbody rb in this.wheels) if( rb != null ) rb.AddTorque(rb.transform.forward * force);
        }

        public void brake()
        {
            foreach (Rigidbody rb in this.wheels) if( rb != null ) rb.angularVelocity = Vector3.zero;
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                Fire.Create(other.contacts[0].point, this.transform);
                Debug.Log(this.gameObject.name+" hitten");
                this.damaged = true;
            }    
        }
    }
}
