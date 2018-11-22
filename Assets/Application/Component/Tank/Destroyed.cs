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

private Vector3 smokeScale;
private Vector3[] explosionPoints;

	void Start()
	{
    
        this.smokeScale = new Vector3(0.5f, 0.5f, 0.5f);
        this.explosionPoints = new Vector3[] {
            new Vector3(0.6f, -0.2f, 0.1f),
            new Vector3(-0.1f, -0.2f, -0.6f),
            new Vector3(-0.6f, -0.2f, -0.3f)
        };


        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.drag = 1;
            rb.angularDrag = 1;
        }

        foreach( Vector3 pt in this.explosionPoints)
        {
            GameObject e = (GameObject) Instantiate(

                Resources.Load(Ammunition.Standard.EXPLOSION_PREFAB_PATH),
                this.transform.position + pt,
                Quaternion.identity,
                this.transform
            );
                
            Destroy(e, 5f);

            GameObject s = (GameObject) Instantiate
            (
                Resources.Load("Prefab/smoke"),
                this.transform.position + pt,
                Quaternion.identity,
                this.transform
            );
            s.transform.localScale = this.smokeScale;



        } 
    }
}
}