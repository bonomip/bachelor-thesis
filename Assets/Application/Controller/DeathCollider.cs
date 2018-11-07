using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Controller
{
	public class DeathCollider : MonoBehaviour {

		private void OnTriggerEnter(Collider other)
        {
			if(other.gameObject.tag == "Player"){
            GameObject.Find(Application.CONTROLLER).GetComponent<Controller>().lose();
			}
        }
}

}


