using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Fire : MonoBehaviour
    {
        private const string PREFAB_PATH = "Prefab/fire";

        public static void Create(Vector3 position, Transform parent)
        {
            GameObject f = (GameObject)Object.Instantiate(

                Resources.Load(PREFAB_PATH),
                position,
                new Quaternion()
            );

            f.transform.parent = parent;

        }
    }
}
