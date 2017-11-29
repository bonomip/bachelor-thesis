using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{
    public class Gun : MonoBehaviour
    {
        private const string NAME = "gun";
        private static float MASS = 0.45f * Main.SCALE;

        private Main main;
        private new GCollider collider;

        private HingeJoint joint;

        private const float VELOCITY = 10f;
        private const float MAX_VELOCITY = 3f;
        
        //TODO cambiare rate
        private const float SHOOT_RATE = 3f;
        private const float RINCULO = 40f;
        private float lastShoot;

        private Transform hole;

        public static Gun attach(Transform parent, Main m)
        {
            return parent.Find(NAME).gameObject.AddComponent<Gun>().linkMain(m);
        }

        public Gun linkMain(Main m)
        {
            this.main = m;
            return this;
        }

        private void Start()
        {
            this.hole = this.transform.Find("hole");

            this.joint = GetComponent<HingeJoint>();

            GetComponent<Rigidbody>().mass = MASS;
            
            this.lastShoot = 0;

            this.collider = GCollider.attach(this.gameObject, this, this.main);
        }

        public void rotateUp()
        {
            if (this.joint.spring.targetPosition <= this.joint.limits.min) return;
            JointSpring js = this.joint.spring;
            js.targetPosition = js.targetPosition - VELOCITY * Time.fixedDeltaTime < this.joint.limits.min ? this.joint.limits.min : ( js.targetPosition - VELOCITY * Time.fixedDeltaTime ) ; 
            this.joint.spring = js; 
        }
        
        public void rotateDown()
        {
            if (this.joint.spring.targetPosition >= this.joint.limits.max) return;
            JointSpring js = this.joint.spring;
            js.targetPosition = js.targetPosition - VELOCITY * Time.fixedDeltaTime > this.joint.limits.max ? this.joint.limits.max : ( js.targetPosition + VELOCITY * Time.fixedDeltaTime ) ;
            this.joint.spring = js;
        }

        public void shoot()
        {
            if (lastShoot + SHOOT_RATE > Time.time) return;

            lastShoot = Time.time;

            GameObject a = Ammunition.Standard.Shoot(this.hole.position, this.hole.position - this.transform.position, this.transform);

            Physics.IgnoreCollision(GetComponent<Collider>(), a.GetComponent<Collider>());

            //TODO aggiungere beccheggio ( oscillazione avanti e indietro )
            this.transform.parent.GetComponent<Rigidbody>()
                    .AddForce((this.transform.position - this.hole.position).normalized * RINCULO, ForceMode.Impulse);
        }

        private void disable()
        {
            Smoke.Create(this.hole.position, this.transform);
            this.enabled = false;
        }

        private void OnDestroy()
        {
            Destroy(this.collider);
        }

        public class GCollider : MonoBehaviour {

            private Gun gun;
            private Main main;

            public static GCollider attach(GameObject toAttach, Gun g, Main m)
            {
                return toAttach.AddComponent<GCollider>().link(g, m);
            }

            private GCollider link(Gun g, Main m)
            {
                this.gun = g;
                this.main = m;
                return this;
            }
        
            private void OnCollisionEnter(Collision other)
            {
                Debug.Log("Gun hitten");
                
                this.main.applyDamage(other, this.transform, 0.95f);
                
                if (other.gameObject.tag == Application.AMMUNITION_TAG && this.gun.enabled)
                {
                    Debug.Log("Gun Destroy");
                    this.gun.disable();
                }
            }
        }
    }
}
