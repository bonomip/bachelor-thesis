using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Gun : MonoBehaviour
    {    
        private const string NAME = "gun";

        private const float MASS = 10f;       
               
        private const float RINCULO = 400f;
        private const float SHOOT_RATE = 3.5f;
        private float lastShoot;
        private Transform hole;
        
        private const float ROTATE_VELOCITY = 4f;
        private float state, angle;

        private bool damaged = false;
        private const float ARMOUR = 0.975f;

        private Main main;

        public static Gun attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<Gun>().GetComponent<Gun>();
        }

        public Gun linkMain(Main m)
        {
            this.main = m;
            return this;
        }

        private void Start()
        {
            this.state = 0;
            this.angle = 0;

            this.hole = this.transform.Find("hole");

            this.lastShoot = 0;

            GetComponent<Rigidbody>().mass = MASS;
        }

        public void rotateUp()
        {
            if (damaged) return;
        
            this.angle = -ROTATE_VELOCITY * Time.deltaTime;

            if (this.state + this.angle <= -5) return;
            
            this.state += this.angle;
             
            this.transform.rotation = Quaternion.AngleAxis(this.angle, this.transform.right) * transform.rotation;

            this.angle = 0;
        }

        public void rotateDown()
        {
            if (damaged) return;
        
            this.angle = ROTATE_VELOCITY * Time.deltaTime; 
            
            if (state+angle >= 5) return;

            this.state += this.angle;
             
            this.transform.rotation = Quaternion.AngleAxis(this.angle, this.transform.right) * transform.rotation;

            this.angle = 0;
        }

        public void shoot()
        {
            if (lastShoot + SHOOT_RATE > Time.time || this.damaged) return;
            
            lastShoot = Time.time;

            Ammunition.Standard.Shoot(this.hole.position, this.hole.position - this.transform.position, this.transform, this.gameObject.layer);
    
            this.transform.parent.parent.parent.GetComponent<Rigidbody>()
                    .AddForce( ( this.transform.position - this.hole.position ).normalized * RINCULO, ForceMode.Impulse);
        }
        
        public void OnCollisionEnter(Collision other)
        {
           if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                Debug.Log(this.gameObject.name+" hitten");
                this.damaged = true;
                this.main.calculateAmmoDamage(this.transform, other, ARMOUR);
                return;
            }  
           this.main.calculateCollisionDamage(other);  
        }
    }
}
