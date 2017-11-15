using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Gun : MonoBehaviour
    {    
        public const string NAME = "gun";        
        public const float ROTATE_VELOCITY = 4f;
        public const float RINCULO = 400f;
        private float state, angle;

        public const float SHOOT_RATE = 3.5f;
        private float lastShoot;

        private Transform hole;

        public static Gun attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<Gun>().GetComponent<Gun>();
        }

        private void Start()
        {
            this.state = 0;
            this.angle = 0;

            this.hole = this.transform.Find("hole");

            this.lastShoot = 0;
        }

        //TODO constrain rotation

        public void rotateUp()
        {
            this.angle = -ROTATE_VELOCITY * Time.deltaTime;

            if (this.state + this.angle <= -5) return;
            
            this.state += this.angle;
             
            this.transform.rotation = Quaternion.AngleAxis(this.angle, this.transform.right) * transform.rotation;

            this.angle = 0;
        }
        public void rotateDown()
        {
            this.angle = ROTATE_VELOCITY * Time.deltaTime; 
            
            if (state+angle >= 5) return;

            this.state += this.angle;
             
            this.transform.rotation = Quaternion.AngleAxis(this.angle, this.transform.right) * transform.rotation;

            this.angle = 0;
        }

        public void shoot()
        {
            if (lastShoot + SHOOT_RATE > Time.time) return;
            
            lastShoot = Time.time;

            Ammunition.Standard.Shoot(this.hole.position, this.hole.position - this.transform.position, this.transform, this.gameObject.layer);
    
            this.transform.parent.parent.parent.GetComponent<Rigidbody>()
                    .AddForce( ( this.transform.position - this.hole.position ).normalized * RINCULO, ForceMode.Impulse);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                //apply damage
                Debug.Log("gun hitten");
            }
        }
    }
}
