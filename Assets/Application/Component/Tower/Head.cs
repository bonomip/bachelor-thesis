using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Component.Tower
{



    public class Head : MonoBehaviour {

        private const string NAME = "head";
        private Main main;
        private Gun gun;

	    // Use this for initialization
	    void Start () {
        this.gun = Gun.attach(this.transform, this.main);
	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        public static Head attach(Transform parent, Main m)
        {
            return parent.Find(NAME).gameObject.AddComponent<Head>().linkMain(m);  
        }

        public Head linkMain(Main m)
        {
            this.main = m;
            return this;
        }

	    void OnCollisionEnter(Collision other) {
            this.main.onHit(other, this.transform);
	    }

    }
}
