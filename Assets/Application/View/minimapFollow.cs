using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.View
{

public class minimapFollow : MonoBehaviour {

	private Transform player;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find(Application.PLAYER).transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		this.transform.position = new Vector3(this.player.position.x, this.transform.position.y, this.player.position.z);
		this.transform.rotation = Quaternion.Euler(90f, this.player.eulerAngles.y, 0f);
	}
}
}
