using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Wheel : MonoBehaviour
    {

        private const string PREFAB_PATH = "Prefab/wheel";
    
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                GameObject w = (GameObject)Instantiate(
                        Resources.Load(PREFAB_PATH),
                        this.transform.position,
                        new Quaternion(),
                        GameObject.Find(Application.VIEW).transform
                );         
                Destroy(this.gameObject);
            }
        }
    }
}
