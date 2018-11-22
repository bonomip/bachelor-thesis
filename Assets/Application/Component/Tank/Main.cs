using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{
    public class Main : MonoBehaviour
    {
        public static float SCALE;

        public static float MASS = 4f;
        public const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.05f;
        private const float MAX_ANGULAR_VELOCITY = 1f;

        private const float MAX_HEALT = 1000f;
        private float healt = MAX_HEALT;
        private bool healtIsLow = false;
        private const string SMOKE_HOLE = "smoke_hole";
        private bool reset;

        public Body body;
        public Turret turret;
        public Gun gun;
        private Engine engine;

        private bool brake;

        private float lastVelocity;

        void Start()
        {
            SCALE = this.transform.localScale.x;
        
            DisableCollision.avoidSelfCollision(this.GetComponentsInChildren<Collider>());
            
            attachComponents();

            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;
            GetComponent<Rigidbody>().maxAngularVelocity = MAX_ANGULAR_VELOCITY;

            this.lastVelocity = 0f;
            Debug.Log(""+this.calculateCumulativeMass(this.transform));

            this.brake = true;
        }

        private void attachComponents()
        {
            this.body = Body.attach(this.transform, this);
            this.turret = Turret.attach(this.transform, this);
            this.gun = Gun.attach(this.transform, this);
            this.engine = Engine.attach(this.transform, this);
        }

        private float calculateCumulativeMass(Transform parent)
        {       
            float res = 0f;

            if( parent.gameObject.GetComponent<Rigidbody>() != null )
                res += parent.gameObject.GetComponent<Rigidbody>().mass;

            if ( parent.transform.childCount == 0 ) return res;

            for (int i = 0; i < parent.transform.childCount; i++) res += calculateCumulativeMass(parent.GetChild(i));

            return res;
        }

        private void FixedUpdate()
        {
            this.lastVelocity = GetComponent<Rigidbody>().velocity.magnitude;
            if (this.brake) this.engine.stop();
        }

        public void toBrake(){
            this.brake = true;
        }

        public void input(bool[] keyInputs)
        {
            this.brake = false;

            if (!keyInputs[1] && !keyInputs[3] && !keyInputs[0] && !keyInputs[2]) this.brake = true;
            else
            {
                bool on = false;

                if (keyInputs[0] && !keyInputs[2])
                {
                    on = true;
                    this.engine.moveForward(keyInputs[1], keyInputs[3]);
                    //this.body.push(this.lastVelocity, 1);
                }

                if (!keyInputs[0] && keyInputs[2])
                {
                    on = true;
                    this.engine.moveBack(keyInputs[1], keyInputs[3]);
                    //this.body.push(this.lastVelocity, 1);
                }

                if(on){
                    if (keyInputs[1] && !keyInputs[3]) this.body.rotate(this.lastVelocity, -1); // soft rotation left
                    if (!keyInputs[1] && keyInputs[3]) this.body.rotate(this.lastVelocity, 1); // soft rotation right
                }

                if (!on) //if the player is not input W or S (forward, back) do an hard rotation
                {
                    if (keyInputs[1] && !keyInputs[3]) this.engine.rotate(this.lastVelocity, -1); //left
                    if (!keyInputs[1] && keyInputs[3]) this.engine.rotate(this.lastVelocity, +1); //right
                }
            }
            
            if (keyInputs[4] && !keyInputs[5]) this.turret.rotateLeft();
            if (!keyInputs[4] && keyInputs[5]) this.turret.rotateRight();

            if (keyInputs[6] && !keyInputs[7] ) this.gun.rotateUp();
            if (!keyInputs[6] && keyInputs[7] ) this.gun.rotateDown();

            if (keyInputs[8]) this.gun.shoot();
        }
        
        public void applyDamage(Collision other, Transform hitten, float armour)
        {      
            if (this.healt <= 0) return;

            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                other.gameObject.GetComponent<Ammunition.Ammunition>().CreateFire(other.contacts[0].point, hitten);
                this.healt -= other.gameObject.GetComponent<Ammunition.Ammunition>().damage * (1 - armour);
                Debug.LogError(healt);
            } else {
                // GENERIC COLLISION
                // this.cumulativeMass = 0;
                //this.calculateCumulativeMass(this.transform);
                //Debug.LogError("magnitude" + this.lastVelocity);
                Debug.LogError("Impact force:"+other.impactForceSum.magnitude);
            }

            if (this.healt <= 350 && !this.healtIsLow) { Smoke.Create(this.turret.transform.Find(SMOKE_HOLE).position, this.turret.transform.Find(SMOKE_HOLE)); this.healtIsLow = true; }
            
            if (this.healt <= 0) this.DestroyTank();
        }

        public void DestroyTank()
        {
            Destroy(this.body);
            Destroy(this.turret);
            Destroy(this.gun);
            Destroy(this.engine);
            Destroyed.attach(this.gameObject);
            GameObject.Find(Application.CONTROLLER).GetComponent<Controller.Controller>().lose();
            Destroy(this);
        }

        public void setColliderStatus(bool active, Transform parent){
            foreach(Collider c in parent.gameObject.GetComponents<Collider>())
                c.enabled = active;
            foreach(Rigidbody r in parent.gameObject.GetComponents<Rigidbody>()){
                r.useGravity = active;
                r.isKinematic = !active;
            }
            for (int i = 0; i < parent.transform.childCount; i++) setColliderStatus(active, parent.GetChild(i));

        }
    }
}