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
        private const float HEALTH = 1000f;

        public Body body;
        public Turret turret;
        private Gun gun;
        private Engine engine;
        
        
        void Start()
        {
            propagateLayer(this.transform, this.gameObject.layer);
            attachComponents();
            
            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;
            GetComponent<Rigidbody>().maxAngularVelocity = MAX_ANGULAR_VELOCITY;
            
        }
        
        private void Update()
        {
            Debug.Log(GetComponent<Rigidbody>().velocity.magnitude * 3.6f + " km/h");
        }
        
        public void input(bool[] keyInputs)
        {
            if (keyInputs[0] && !keyInputs[2] ) this.engine.moveForward(keyInputs[1], keyInputs[3]);
            if (!keyInputs[0] && keyInputs[2] ) this.engine.moveBack(keyInputs[1], keyInputs[3]);

            if (keyInputs[1] && !keyInputs[3] && !keyInputs[0] && !keyInputs[2] ) this.engine.rotateLeft();
            if (!keyInputs[1] && keyInputs[3] && !keyInputs[0] && !keyInputs[2] ) this.engine.rotateRight();

            if (!keyInputs[1] && !keyInputs[3] && !keyInputs[0] && !keyInputs[2]) this.engine.stop();

            if (keyInputs[4] && !keyInputs[5]) this.turret.rotateLeft();
            if (!keyInputs[4] && keyInputs[5]) this.turret.rotateRight();

            if (keyInputs[6] && !keyInputs[7]) this.gun.rotateUp();
            if (!keyInputs[6] && keyInputs[7]) this.gun.rotateDown();

            if (keyInputs[8]) this.gun.shoot();
        }

        private static void propagateLayer(Transform parent, int layer)
        {
            parent.gameObject.layer = layer;

            if (parent.transform.childCount == 0) return;

            for (int i = 0; i < parent.transform.childCount; i++) propagateLayer(parent.transform.GetChild(i), layer);
        }

        private void attachComponents()
        {
            this.body = Body.attach(this.transform);
            this.turret = Turret.attach(this.body.transform);
            this.gun = Gun.attach(this.turret.transform);
            
            this.engine = Engine.attach(this.transform);
        }
    }
}
