using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Turret : MonoBehaviour
    {

        public const string NAME = "turret";

        public const float ROTATE_VELOCITY = 15f;
        private float state, angle;

        public static Turret attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<Turret>().GetComponent<Turret>();
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
         
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                Fire.Create(other.contacts[0].point, this.transform);
                Debug.Log("turret hitten");
            }
        }
    }
}
