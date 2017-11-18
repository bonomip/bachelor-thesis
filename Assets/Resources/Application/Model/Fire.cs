using UnityEngine;
using System.Collections;

namespace Application.Model
{
    public class Fire
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
            f.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
