using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{
    public class Turret : MonoBehaviour
    {
        public const string NAME = "turret";
        public static float MASS = Main.MASS;
        public static float DRAG = Main.DRAG;
        private Main main;
        private HingeJoint joint;
        private const float VELOCITY = 20f;
        private const float ARMOUR = 0.55f;

        public static Turret attach(Transform parent, Main m)
        {
            return parent.Find(NAME).gameObject.AddComponent<Turret>().linkMain(m);
        }

        public Turret linkMain(Main m)
        {
            this.main = m;
            return this;
        }

        private void Start()
        {
            this.joint = GetComponent<HingeJoint>();
            GetComponent<Rigidbody>().mass = MASS;
        }

        public void rotateLeft()
        {
            if (this.joint.spring.targetPosition <= this.joint.limits.min) return;
            JointSpring js = this.joint.spring;
            js.targetPosition = js.targetPosition - VELOCITY * Time.fixedDeltaTime < this.joint.limits.min ? this.joint.limits.min : ( js.targetPosition - VELOCITY * Time.fixedDeltaTime ) ; 
            this.joint.spring = js;
        }

        public void rotateRight()
        {
            if (this.joint.spring.targetPosition >= this.joint.limits.max) return;
            JointSpring js = this.joint.spring;
            js.targetPosition = js.targetPosition - VELOCITY * Time.fixedDeltaTime > this.joint.limits.max ? this.joint.limits.max : ( js.targetPosition + VELOCITY * Time.fixedDeltaTime ) ;
            this.joint.spring = js;
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Turret hitten");
            this.main.applyDamage(other, this.transform, ARMOUR);
        }
    }
}
