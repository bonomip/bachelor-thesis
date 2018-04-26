using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
        avoidSelfCollision(this.GetComponentsInChildren<Collider>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void avoidSelfCollision(Collider[] childColliders)
    {
        foreach (Collider c0 in childColliders)
            foreach (Collider c1 in childColliders)
                if( c0 != c1 ) Physics.IgnoreCollision(c0, c1, true);
    }
}
