using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Main : MonoBehaviour
    {

        private const float MASS = 150;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.05f;
        private const float MAX_ANGULAR_VELOCITY = 1f;

        private const float MAX_HEALT = 1000f;
        private float healt = MAX_HEALT;

        public const float HURTS = 500f;

        private bool destroy = false;

        private Body body;
        private Turret turret;
        private Gun gun;

        void Start()
        {
            propagateLayer(this.transform, this.gameObject.layer);

            attachComponents();

            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;
            GetComponent<Rigidbody>().maxAngularVelocity = MAX_ANGULAR_VELOCITY;
        }

        private static void propagateLayer(Transform parent, int layer)
        {
            parent.gameObject.layer = layer;

            if (parent.transform.childCount == 0) return;

            for (int i = 0; i < parent.transform.childCount; i++) propagateLayer(parent.transform.GetChild(i), layer);
        }

        private void attachComponents()
        {
            this.body = Body.attach(this.transform).linkMain(this);
            this.turret = Turret.attach(this.body.transform).linkMain(this);
            this.gun = Gun.attach(this.turret.transform).linkMain(this);
        }
        
        public void input(bool[] keyInputs)
        {
            if (this.destroy) return;
        
            if (keyInputs[0] && !keyInputs[2] ) this.body.moveForward(keyInputs[1], keyInputs[3]);
            if (!keyInputs[0] && keyInputs[2] ) this.body.moveBack(keyInputs[1], keyInputs[3]);

            if (keyInputs[1] && !keyInputs[3] && !keyInputs[0] && !keyInputs[2] ) this.body.rotateLeft();
            if (!keyInputs[1] && keyInputs[3] && !keyInputs[0] && !keyInputs[2] ) this.body.rotateRight();

            if (!keyInputs[1] && !keyInputs[3] && !keyInputs[0] && !keyInputs[2]) this.body.stop();

            if (keyInputs[4] && !keyInputs[5]) this.turret.rotateLeft();
            if (!keyInputs[4] && keyInputs[5]) this.turret.rotateRight();

            if (keyInputs[6] && !keyInputs[7]) this.gun.rotateUp();
            if (!keyInputs[6] && keyInputs[7]) this.gun.rotateDown();

            if (keyInputs[8]) this.gun.shoot();
        }

        public void calculateCollisionDamage(Collision other)
        {
            if (other.impulse.magnitude > 500f)
            {
                this.applyRawDamage( (other.impulse.magnitude - 500f) * 0.00012f * MAX_HEALT );
            }
        }

        public void calculateAmmoDamage(Transform parent, Collision ammo, float armour)
        {
            Model.Fire.Create(ammo.contacts[0].point, parent);    
            float damage = (1 - armour) * ammo.gameObject.GetComponent<Ammunition.Ammunition>().DAMAGE;
            this.applyRawDamage(damage);               
        }

        public void applyRawDamage(float damage)
        {
            this.healt -= damage;
            
            Debug.LogError(this.gameObject.name+" healt: "+ this.healt / 1000f * 100f + "%");

            if (this.healt <= 0)
            {
                this.destroy = true;
                
                for (int i = 0; i < this.transform.childCount; i++) this.propagateDestroyScript(this.transform.GetChild(i));
            }
        }

        private void propagateDestroyScript(Transform parent)
        {
            foreach (MonoBehaviour script in parent.gameObject.GetComponents<MonoBehaviour>())
            {
                Destroy(script);
            }

            for (int i = 0; i < parent.childCount; i++) this.propagateDestroyScript(parent.GetChild(i));
        }
    }
}
