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
<<<<<<< HEAD
=======

	// Update is called once per frame
	void Update()
	{
			
	}

>>>>>>> 6878b80b3c691c34b360e039eb0b2a3680e75ed9
}
