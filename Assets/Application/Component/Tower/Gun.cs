using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Component.Tower
{



    public class Gun : MonoBehaviour {

        private const string NAME = "gun";
        private Main main;
        private Transform hole;

	    // Use this for initialization
	    void Start () {
            this.hole = this.transform.parent.Find("hole").gameObject.transform;
	    }
	
	    // Update is called once per frame

        float lastShoot = 0f;
	    void Update () {
		
                if (lastShoot + 5 > Time.time) return;

                    lastShoot = Time.time; shoot();

	    }

        public void shoot(){
            GameObject a = Ammunition.Standard.Shoot(this.hole.position, this.hole.position - this.transform.position, this.transform);
            Physics.IgnoreCollision(this.transform.parent.parent.GetComponent<Collider>(), a.GetComponent<Collider>());
        }

        public static Gun attach(Transform parent, Main m)
        {
            return parent.Find("head").Find(NAME).gameObject.AddComponent<Gun>().linkMain(m);  
        }

        public Gun linkMain(Main m)
        {
            this.main = m;
            return this;
        }

	    void OnCollisionEnter(Collision other) {
            this.main.onHit(other, this.transform);
	    }

    }
}