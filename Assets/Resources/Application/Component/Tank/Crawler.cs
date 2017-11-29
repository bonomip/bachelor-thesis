using UnityEngine;
using System.Collections;


namespace Application.Component.Tank
{

    public class Crawler : MonoBehaviour
    {
        public Rigidbody[] wheels;

        private Main main;
        private new CCollider collider;

        private static float MASS = 4.2f * Main.SCALE;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 0.05f;

        private static float WHEEL_MASS = 1.25f * Main.SCALE;
        private const float WHEEL_DRAG = 0.05f;
        private const float WHEEL_ANGULAR_DRAG = 0.05f;

        private const float WHEEL_MAX_ANGULAR_VELOCITY = 45f;

        private readonly string[] wheelNames = { "0", "1", "2", "3", "4", "4.5", "0.5" };

        private const string R_NAME = "rCrawler";
        private const string L_NAME = "lCrawler";
            
        public static Crawler attach(bool left, Transform parent, Main m)
        {  
            return parent.Find( left ? L_NAME : R_NAME ).gameObject.AddComponent<Crawler>().linkMain(m);
        }

        public Crawler linkMain(Main m)
        {
            this.main = m;
            return this;
        }

        void Start()
        {
            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;

            this.wheels = new Rigidbody[wheelNames.Length];

            for (int i = 0; i < wheelNames.Length; i++)
            {
                this.wheels[i] = this.transform.Find(wheelNames[i]).GetComponent<Rigidbody>();
                this.wheels[i].gameObject.AddComponent<Wheel>();
                this.wheels[i].mass = WHEEL_MASS;
                this.wheels[i].maxAngularVelocity = WHEEL_MAX_ANGULAR_VELOCITY;
                this.wheels[i].drag = WHEEL_DRAG;
                this.wheels[i].angularDrag = WHEEL_ANGULAR_DRAG;
            }

            this.collider = CCollider.attach(this.gameObject, this, this.main);
        }

        public void move(float force)
        {
            foreach (Rigidbody rb in this.wheels) if( rb != null ) rb.AddTorque(rb.transform.forward * force);
        }

        public void brake()
        {
            foreach (Rigidbody rb in this.wheels) if( rb != null ) rb.angularVelocity = Vector3.zero;
        }

        private void OnDestroy()
        {
            Destroy(this.collider);   
        }

        public class Wheel : MonoBehaviour
        {
            private const string PREFAB_PATH = "Prefab/wheel";
    
            private void OnCollisionEnter(Collision other)
            {
                if (other.gameObject.tag == Application.AMMUNITION_TAG)
                {
                    GameObject w = (GameObject)Instantiate(
                        Resources.Load(PREFAB_PATH),
                        this.transform.position,
                        new Quaternion(),
                        GameObject.Find(Application.VIEW).transform
                    );         
                    Destroy(this.gameObject);
                }
            }
        }

        public class CCollider : MonoBehaviour
        {
            private Crawler crawler;
            
            private Main main;

            public static CCollider attach(GameObject toAttach, Crawler c, Main m)
            {
                return toAttach.AddComponent<CCollider>().link(c, m);
            }

            public CCollider link(Crawler c, Main m)
            {
                this.crawler = c;
                this.main = m;
                return this;
            }
            
            private void OnCollisionEnter(Collision other)
            {
            
                Debug.Log("Crawler hitten");
            
                this.main.applyDamage(other, this.transform, 0.95f);
                           
                if (other.gameObject.tag == Application.AMMUNITION_TAG)
                {
                    Debug.Log("Crawler Destroyed");
                    this.crawler.enabled = false;
                }
            }
        }
    }
}
