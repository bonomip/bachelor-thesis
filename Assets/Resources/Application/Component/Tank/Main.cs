using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{
    public class Main : MonoBehaviour
    {

        private const float MASS = 150;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.05f;
        private const float MAX_ANGULAR_VELOCITY = 1f;

        private const float MAX_HEALT = 1000f;
        private float healt = MAX_HEALT;
        private bool healtIsLow = false;
        private const string SMOKE_HOLE = "smoke_hole";

        public Body body;
        public Turret turret;
        private Gun gun;
        private Engine engine;

        private float lastVelocity;
        private float cumulativeMass;

        void Start()
        {
            propagateLayer(this.transform, this.gameObject.layer);
            attachComponents();

            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;
            GetComponent<Rigidbody>().maxAngularVelocity = MAX_ANGULAR_VELOCITY;

            this.lastVelocity = 0f;
            this.cumulativeMass = 0f;
        }
        
        //ricorsiva
        private static void propagateLayer(Transform parent, int layer)
        {
            parent.gameObject.layer = layer;

            if (parent.transform.childCount == 0) return;

            for (int i = 0; i < parent.transform.childCount; i++) propagateLayer(parent.transform.GetChild(i), layer);
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
        }

        public void input(bool[] keyInputs)
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

            if (keyInputs[1] && !keyInputs[3] && !keyInputs[0] && !keyInputs[2] ) this.engine.rotateLeft();
            if (!keyInputs[1] && keyInputs[3] && !keyInputs[0] && !keyInputs[2] ) this.engine.rotateRight();

            if (!keyInputs[1] && !keyInputs[3] && !keyInputs[0] && !keyInputs[2]) this.engine.stop();

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
            StartCoroutine(destroyAnimation());
            Destroy(this.body);
            Destroy(this.turret);
            Destroy(this.gun);
            Destroy(this.engine);  
        }

        private IEnumerator destroyAnimation()
        {
            yield return new WaitForEndOfFrame();
            //TODO aggiungere fiammata temporanea e fumo random permanente
            Destroy(this); 
        }
    }
}
