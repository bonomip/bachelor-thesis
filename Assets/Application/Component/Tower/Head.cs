using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Component.Tower
{
    public class Head : MonoBehaviour {
        
        private Main main;
        private Transform target;
        private Transform v2;
        private Transform v1;

        public float x;
        public float range = 50f;

        public static Head attach(Transform parent, Main m)
        {
            return parent.Find("head").gameObject.AddComponent<Head>().linkMain(m);  
        }

        public Head linkMain(Main m)
        {
            this.main = m;
            return this;
        }

	    void Start () {
            this.target = GameObject.Find(Application.PLAYER).transform;
            this.v1 = this.transform.Find("v1").gameObject.transform;
            this.v2 = this.transform.Find("v2").gameObject.transform;
	    }

        float lastShoot = 0f;

	    void Update () {
            if ( aim() && ( lastShoot + 5 < Time.time )) {
                lastShoot = Time.time;
                shoot();
            }
	    }

        public bool aim(){
            transform.rotation = Quaternion.LookRotation(this.target.position - this.transform.position, Vector3.up);
            
            float dist = Vector3.Distance(this.target.position, this.transform.position);
            float high = this.target.position.y - this.transform.position.y;

            if(dist > range) return false;
            if(dist <= 12) x = 0;
            else x = (dist - 12) / (range - 12) * 10f;
            this.transform.Rotate(new Vector3(-x,0,0));
            
            RaycastHit info;
            if( Physics.Raycast(this.v2.transform.position, this.target.position - this.transform.position, out info) )
                return info.transform.gameObject.tag == "Player";

            return false;    
        }

        public void shoot(){
            GameObject a = Ammunition.Standard.Shoot(this.v2.position, this.v2.position - this.v1.position, this.transform);
            Physics.IgnoreCollision(this.transform.GetComponent<Collider>(), a.GetComponent<Collider>());
        }

	    void OnCollisionEnter(Collision other) {
            this.main.onHit(other, this.transform);
	    }
    }
}
