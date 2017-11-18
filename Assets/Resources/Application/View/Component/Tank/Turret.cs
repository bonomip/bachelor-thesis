using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Turret : MonoBehaviour
    {
        private const string NAME = "turret";

        private Main main;

        private const float ROTATE_VELOCITY = 15f;
        private float state, angle;

        private const float ARMOUR = 0.45f;

        public static Turret attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<Turret>().GetComponent<Turret>();
        }

        public Turret linkMain(Main m)
        {
            this.main = m;
            return this;
        }

        private void Start()
        {
            this.state = 0;
            this.angle = 0;
        }

        public void rotateLeft()
        {
            this.angle = -ROTATE_VELOCITY * Time.deltaTime;

            if (this.state + this.angle <= -45) return;

            this.state += this.angle;

            this.transform.rotation = Quaternion.AngleAxis(this.angle, this.transform.forward) * transform.rotation;

            this.angle = 0;
        }

        public void rotateRight()
        {
            this.angle = ROTATE_VELOCITY * Time.deltaTime;

            if (state + angle >= 45) return;

            this.state += this.angle;

            this.transform.rotation = Quaternion.AngleAxis(this.angle, this.transform.forward) * transform.rotation;

            this.angle = 0;
         }
         
        public void OnCollisionEnter(Collision other)
        { 
           if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                Debug.Log(this.gameObject.name+" hitten");
                this.main.calculateAmmoDamage( this.transform, other, ARMOUR);
                return;
            }
            this.main.calculateCollisionDamage(other);    
        }
    }
}
