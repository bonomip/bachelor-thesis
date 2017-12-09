using UnityEngine;
using System.Collections;

public class Destroyed : MonoBehaviour
{

    public static void attach(GameObject toAttach)
    {
        toAttach.AddComponent<Destroyed>();
    }

	// Use this for initialization
	void Start()
	{
    
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.drag = 1;
            rb.angularDrag = 1;
        }
    
        //start destroy animation
	}

	// Update is called once per frame
	void Update()
	{
			
	}

}
