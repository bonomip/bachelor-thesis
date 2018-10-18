using UnityEngine;
using System.Collections;


namespace Application.Component.Tank
{

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
        GameObject e = (GameObject) Instantiate(

            Resources.Load("Prefab/smoke"),
            this.transform.position,
            Quaternion.identity,
            this.transform
        );
        e.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        e = (GameObject) Instantiate(

            Resources.Load("Prefab/smoke"),
            this.transform.position + new Vector3(0.6f, -0.2f, 0.1f),
            Quaternion.identity,
            this.transform
        );
                e.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        e = (GameObject) Instantiate(

            Resources.Load("Prefab/smoke"),
            this.transform.position + new Vector3(-0.1f, -0.2f, -0.6f),
            Quaternion.identity,
            this.transform
        );
                e.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        e = (GameObject) Instantiate(

            Resources.Load("Prefab/smoke"),
            this.transform.position + new Vector3(-0.6f, -0.2f, -0.3f),
            Quaternion.identity,
            this.transform
        );
                e.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);


    }
}
}