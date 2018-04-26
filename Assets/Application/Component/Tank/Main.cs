using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{
    public class Main : MonoBehaviour
    {
        public static float SCALE;

        private static float MASS;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.05f;
        private const float MAX_ANGULAR_VELOCITY = 1f;

        private const float MAX_HEALT = 1000f;
        private float healt = MAX_HEALT;
        private bool healtIsLow = false;
        private const string SMOKE_HOLE = "smoke_hole";

        public Body body;
        public Turret turret;
        public Gun gun;
        private Engine engine;

        private bool brake;

        private float lastVelocity;
        private float cumulativeMass;

        void Start()
        {
            SCALE = this.transform.localScale.x;
            MASS = 12.5f * SCALE;

            Debug.LogError(SCALE);
            
            DisableCollision.avoidSelfCollision(this.GetComponentsInChildren<Collider>());
            
            attachComponents();

            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;
            GetComponent<Rigidbody>().maxAngularVelocity = MAX_ANGULAR_VELOCITY;

            this.lastVelocity = 0f;
            this.cumulativeMass = 0f;

            this.calculateCumulativeMass(this.transform);

            this.brake = true;

            Debug.LogError(this.cumulativeMass);
        }

        private void attachComponents()
        {
            this.body = Body.attach(this.transform, this);
            this.turret = Turret.attach(this.transform, this);
            this.gun = Gun.attach(this.transform, this);
            this.engine = Engine.attach(this.transform, this);
        }

        //ricorsiva
        private void calculateCumulativeMass(Transform parent)
        {        
            if( parent.gameObject.GetComponent<Rigidbody>() != null ) this.cumulativeMass += parent.gameObject.GetComponent<Rigidbody>().mass;

            if ( parent.transform.childCount == 0 ) return;

            for (int i = 0; i < parent.transform.childCount; i++) calculateCumulativeMass(parent.GetChild(i));
        }

        private void FixedUpdate()
        {
            this.lastVelocity = GetComponent<Rigidbody>().velocity.magnitude;
            if (this.brake) this.engine.stop();
        }

        public void input(bool[] keyInputs)
        {

            this.brake = false;

            if (!keyInputs[1] && !keyInputs[3] && !keyInputs[0] && !keyInputs[2]) this.brake = true;
            else
            {
                if (keyInputs[0] && !keyInputs[2])
                {
                    this.engine.moveForward(keyInputs[1], keyInputs[3]);

                    if (keyInputs[1] && !keyInputs[3]) this.body.rotateLeft();
                    if (!keyInputs[1] && keyInputs[3]) this.body.rotateRight();
                }

                if (!keyInputs[0] && keyInputs[2])
                {
                    this.engine.moveBack(keyInputs[1], keyInputs[3]);

                    if (keyInputs[1] && !keyInputs[3]) this.body.rotateLeft();
                    if (!keyInputs[1] && keyInputs[3]) this.body.rotateRight();
                }

                if (!keyInputs[0] && !keyInputs[2])
                {
                    if (keyInputs[1] && !keyInputs[3]) this.engine.rotateLeft(this.body.kmh());
                    if (!keyInputs[1] && keyInputs[3]) this.engine.rotateRight(this.body.kmh());
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

            // GENERIC COLLISION
            this.cumulativeMass = 0;
            this.calculateCumulativeMass(this.transform);

            Debug.LogError("Km/h:" + this.lastVelocity * 3.6f);
            Debug.LogError("Impact force:"+this.lastVelocity * ( other.rigidbody != null ? other.rigidbody.mass : 1000 ) / this.cumulativeMass);
            
            // AMMUNITION COLLISION DAMAGE CALCULATIONS
            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                other.gameObject.GetComponent<Ammunition.Ammunition>().CreateFire(other.contacts[0].point, hitten);
                this.healt -= other.gameObject.GetComponent<Ammunition.Ammunition>().damage * (1 - armour);
                Debug.LogError(healt);
            }

            if (this.healt <= 350 && !this.healtIsLow) { Smoke.Create(this.turret.transform.Find(SMOKE_HOLE).position, this.turret.transform.Find(SMOKE_HOLE)); this.healtIsLow = true; }
            
            if (this.healt <= 0) this.DestroyTank();
        }

        private void DestroyTank()
        {
            Destroy(this.body);
            Destroy(this.turret);
            Destroy(this.gun);
            Destroy(this.engine);
            Destroyed.attach(this.gameObject);
            Destroy(this);
        }
    }
}
