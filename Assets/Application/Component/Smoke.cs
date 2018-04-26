using UnityEngine;
using System.Collections;

namespace Application.Component
{
    public class Smoke : MonoBehaviour
    {
        private const string SMOKE_PREFAB_PATH = "Prefab/smoke";

        public static void Create(Vector3 position, Transform parent)
        {
            GameObject s = (GameObject)Instantiate(

                    Resources.Load(SMOKE_PREFAB_PATH),
                    position,
                    new Quaternion()
            );

            s.transform.parent = parent;
            s.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

    }
}
