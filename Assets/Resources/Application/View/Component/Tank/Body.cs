using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Body : MonoBehaviour
    { 
        private const string NAME = "body";
    
        private Crawler.Left left;
        private Crawler.Right right;

        private Main main;
    
        private const float MASS = 150;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.05f;
        private const float MAX_ANGULAR_VELOCITY = 1f;
        private const float WHEEL_TORQUE_FORCE = 200;

        private const float ARMOUR_CENTER = 0.65f;
        private const float ARMOUR_REAR = 0.10f;
        private const float ARMOUR_FRONT = 0.85f;
        
    
        public static Body attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<Body>().GetComponent<Body>();         
        }
            
        void Start()
        {
            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;
            GetComponent<Rigidbody>().maxAngularVelocity = MAX_ANGULAR_VELOCITY;
            
            this.left = Crawler.Left.attach(this.main.transform).linkMain(this.main);
            this.right = Crawler.Right.attach(this.main.transform).linkMain(this.main);
        }
              
        public Body linkMain(Main m)
        {
            this.main = m;
            return this;
        }      
              
        private void Update()
        {
            if( GetComponent<Rigidbody>().velocity.magnitude * 3.6f > 10f )Debug.Log(GetComponent<Rigidbody>().velocity.magnitude * 3.6f + " km/h");
        }

        public void moveForward(bool leftPressed, bool rightPressed)
        {
            this.left.move ( WHEEL_TORQUE_FORCE * (leftPressed ? -0.5f : 1f) );
            this.right.move( WHEEL_TORQUE_FORCE * (rightPressed ? -0.5f : 1f) );
        }

        public void moveBack(bool leftPressed, bool rightPressed)
        {
            this.left.move ( - WHEEL_TORQUE_FORCE * 0.60f * (rightPressed ? -0.5f : 1f) );
            this.right.move( - WHEEL_TORQUE_FORCE * 0.60f * (leftPressed ? -0.5f : 1f) );
        }

        public void rotateLeft()
        {   
            this.left.move ( - WHEEL_TORQUE_FORCE );
            this.right.move( WHEEL_TORQUE_FORCE   );
        }
        
        public void rotateRight()
        {
            this.left.move ( WHEEL_TORQUE_FORCE   );
            this.right.move( - WHEEL_TORQUE_FORCE );
        }
        
        public void stop()
        {
            this.left.brake();
            this.right.brake();
        }
        
        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                Debug.Log(this.gameObject.name+" hitten");
                // TODO find where impact is on center, rear or front anche apply the correct damage
                this.main.calculateAmmoDamage( this.transform, other, ARMOUR_CENTER);
                return;
            } 
            this.main.calculateCollisionDamage(other);   
        }
    }
}
