using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{

    public class Body : MonoBehaviour
    {
        private const float MASS = 150;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.05f;
        private const float MAX_ANGULAR_VELOCITY = 1f;
     
        private const string NAME = "body";
    
        void Start()
        {
            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;
            GetComponent<Rigidbody>().maxAngularVelocity = MAX_ANGULAR_VELOCITY;
        }

        public static Body attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<Body>().GetComponent<Body>();         
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                Fire.Create(other.contacts[0].point, this.transform);
                Debug.Log("body hitten");
            }
        }
    }
}
