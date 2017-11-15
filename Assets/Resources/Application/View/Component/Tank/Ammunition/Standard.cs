using UnityEngine;
using System.Collections;

namespace Application.View.Component.Ammunition
{
    public class Standard : MonoBehaviour
    {

        
        private const string GUN_FIRE_PREFAB_PATH = "Prefab/gun_fire";
        private const string AMMO_PREFAB_PATH = "Prefab/ammunition";
        private const string EXPLOSION_PREFAB_PATH = "Prefab/explosion";
        private const float AMMO_VELOCITY = 40f;
        public const float AMMO_DAMAGE = 1000f;
        private const float EXPLOSION_POWER = 100f;
        private const float EXPLOSION_RADIUS = 15f;
        private const float EXPLOSION_UPWARDS = 0.5f;
    
        public static void Shoot(Vector3 exit_hole_position, Vector3 direction, Transform parent, int layer)
        {
            CreateGunFire(exit_hole_position, parent);
            CreateAmmo(exit_hole_position, direction, layer);
        }

        private static void CreateGunFire(Vector3 position, Transform parent)
        {
            GameObject p = (GameObject) Instantiate(

                Resources.Load(GUN_FIRE_PREFAB_PATH),
                position,
                new Quaternion(),
                parent

            );

            Destroy(p, 5f);
        }

        private static void CreateAmmo(Vector3 position, Vector3 normal, int layer)
        {
            GameObject a = (GameObject)Instantiate(

                Resources.Load(AMMO_PREFAB_PATH),
                position,
                Quaternion.identity,
                GameObject.Find(Application.VIEW).transform

                );
                
            a.layer = layer;
            a.tag = Application.AMMUNITION_TAG;
            a.GetComponent<Rigidbody>().AddForce(normal * AMMO_VELOCITY, ForceMode.Impulse);
            a.AddComponent<Standard>();

            Destroy(a, 10f);
        }

        private static void CreateExplosion(Vector3 position)
        {
            GameObject e = (GameObject) Instantiate(

                Resources.Load(EXPLOSION_PREFAB_PATH),
                position,
                Quaternion.identity,
                GameObject.Find(Application.VIEW).transform
            );
                
            Destroy(e, 6.5f);

            foreach ( Collider hit in Physics.OverlapSphere( position, EXPLOSION_RADIUS ) ) if ( hit.attachedRigidbody != null )
                hit.attachedRigidbody
                    .AddExplosionForce( EXPLOSION_POWER, position, EXPLOSION_RADIUS, EXPLOSION_UPWARDS, ForceMode.Impulse );
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            CreateExplosion(collision.contacts[0].point);
            Destroy(this.gameObject);
        }
    }
}
