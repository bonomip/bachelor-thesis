using UnityEngine;
using System.Collections;

namespace Application.Component.Tank.Ammunition
{
    public abstract class Ammunition : MonoBehaviour
    {
        public float damage;
        public float explosionPower;
        public float explosionRadius;
        public float explosionUpwards;

        public abstract void CreateFire(Vector3 position, Transform parent);
    }
}
