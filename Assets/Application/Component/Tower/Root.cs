using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Component.Tower
{
    public class Root : MonoBehaviour {

        private const string NAME = "root";
        private Main main;

	    // Use this for initialization
	    void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        public static Root attach(Transform parent, Main m)
        {
            return parent.Find(NAME).gameObject.AddComponent<Root>().linkMain(m);  
        }

        public Root linkMain(Main m)
        {
            this.main = m;
            return this;
        }

	    void OnCollisionEnter(Collision other) {
            this.main.onHit(other, this.transform);
	    }

    }
}