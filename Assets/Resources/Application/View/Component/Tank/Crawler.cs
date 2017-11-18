using UnityEngine;
using System.Collections;


namespace Application.View.Component.Tank
{

    public abstract class Crawler : MonoBehaviour
    {
        public Rigidbody[] wheels;

        private const float MASS = 50;
        private const float DRAG = 0.05f;
        private const float ANGULAR_DRAG = 10f;
        
        private const float WHEEL_MASS = 15;
        private const float WHEEL_DRAG = 0.5f;
        private const float WHEEL_ANGULAR_DRAG = 0.5f;
        private const float WHEEL_MAX_ANGULAR_VELOCITY = 45f;

        private bool damaged = false;
        private const float ARMOUR = 0.95f;

        private Main main;
        
        private readonly string[] wheelNames = { "0", "1", "2", "3", "4", "4.5", "0.5" };

        void Start()
        {
            GetComponent<Rigidbody>().mass = MASS;
            GetComponent<Rigidbody>().drag = DRAG;
            GetComponent<Rigidbody>().angularDrag = ANGULAR_DRAG;

            this.wheels = new Rigidbody[wheelNames.Length];

            for (int i = 0; i < wheelNames.Length; i++)
            {
                this.wheels[i] = this.transform.Find(wheelNames[i]).GetComponent<Rigidbody>();
                this.wheels[i].gameObject.AddComponent<Wheel>().GetComponent<Wheel>().linkMain(this.main);
                this.wheels[i].mass = WHEEL_MASS;
                this.wheels[i].maxAngularVelocity = WHEEL_MAX_ANGULAR_VELOCITY;
                this.wheels[i].drag = WHEEL_DRAG;
                this.wheels[i].angularDrag = WHEEL_ANGULAR_DRAG;
            }
        }

        public void move(float force)
        {
            if( !this.damaged ) foreach (Rigidbody rb in this.wheels) if( rb != null ) rb.AddTorque(rb.transform.forward * force);
        }

        public void brake()
        {
            foreach (Rigidbody rb in this.wheels) if( rb != null ) rb.angularVelocity = Vector3.zero;
        }
        
        public void OnCollisionEnter(Collision other)
        {
            if ( other.gameObject.tag == Application.AMMUNITION_TAG )
            { 
                Debug.Log( this.gameObject.name+" hitten" );
                this.damaged = true;
                this.main.calculateAmmoDamage( this.transform ,other, ARMOUR);
                return;
            }
            this.main.calculateCollisionDamage(other);
        }    
        
        
        // INNER CLASS RIGHT & LEFT CRAWLER
        
        public class Right : Crawler
        {
            private const string NAME = "rCrawler";

            public static Right attach(Transform parent)
            {
                return parent.Find(NAME).gameObject.AddComponent<Right>().GetComponent<Right>();
            }
            
            public Right linkMain(Main m)
            {
                this.main = m;
                return this;
            }
        }
        
        public class Left : Crawler
        {
            private const string NAME = "lCrawler";

            public static Left attach(Transform parent)
            {
                return parent.Find(NAME).gameObject.AddComponent<Left>().GetComponent<Left>();
            }
            
            public Left linkMain(Main m)
            {
                this.main = m;
                return this;
            }
        }
        
        
        //INNER CLASS WHEEL
        
        public class Wheel : MonoBehaviour
        {
            private const string PREFAB_PATH = "Prefab/wheel";

            private Main main;

            public void linkMain(Main m) { this.main = m; }
        
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
                    main.applyRawDamage(25f);
                    Destroy(this.gameObject);
                    return;
                }
                this.main.calculateCollisionDamage(other);
            }
        }
    }
}
