using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Component.Tower
{

public class Main : MonoBehaviour {

	private Head head;
	private Root root;


	// Use this for initialization
	void Start () {
		this.head = Head.attach(this.transform, this);
		this.root = Root.attach(this.transform, this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onHit(Collision other, Transform hitten) {
		Debug.Log("Tower Hitten");
		if(other.gameObject.tag == Application.AMMUNITION_TAG){
			other.gameObject.GetComponent<Ammunition.Ammunition>().CreateFire(other.contacts[0].point, hitten);
			GameObject.Find(Application.CONTROLLER).GetComponent<Controller.Controller>().turretDestroyed();
			this.destroy();
		}
	}

	private void destroy(){
		Destroy(head);
		Destroy(root);
		Destroy(this);
	}

}

}
