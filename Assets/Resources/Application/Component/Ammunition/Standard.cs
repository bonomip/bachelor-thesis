using UnityEngine;
using System.Collections;

namespace Application.Component.Ammunition
{
    public class Standard : Ammunition
    {
        private const string GUN_FIRE_PREFAB_PATH = "Prefab/gun_fire_standard";
        private const string AMMO_PREFAB_PATH = "Prefab/ammo_standard";
        private const string EXPLOSION_PREFAB_PATH = "Prefab/explosion_standard";
        private const string FIRE_PREFAB_PATH = "Prefab/fire_standard";

        private const float VELOCITY = 15f;

        private void Start()
        {
            this.damage = 1000f;
            this.explosionPower = 35f;
            this.explosionRadius = 7f;
            this.explosionUpwards = 0.0f;
        }

        public static GameObject Shoot(Vector3 exit_hole_position, Vector3 direction, Transform parent)
        {
            CreateGunFire(exit_hole_position, parent);
            return CreateAmmo(exit_hole_position, direction);
        }

        private static void CreateGunFire(Vector3 position, Transform parent)
        {
            GameObject p = (GameObject) Instantiate(

                Resources.Load(GUN_FIRE_PREFAB_PATH),
                position,
                new Quaternion(),
                parent

            );

            Destroy(p, 3f);
        }

        private static GameObject CreateAmmo(Vector3 position, Vector3 normal)
        {
            GameObject a = (GameObject)Instantiate(

                Resources.Load(AMMO_PREFAB_PATH),
                position,
                Quaternion.identity,
                GameObject.Find(Application.VIEW).transform
                );
                
            a.tag = Application.AMMUNITION_TAG;
            a.GetComponent<Rigidbody>().AddForce(normal * VELOCITY, ForceMode.Impulse);
            a.AddComponent<Standard>();

            Destroy(a, 10f);
            
            return a;
        }

        private void CreateExplosion(Vector3 position)
        {
            GameObject e = (GameObject) Instantiate(

                Resources.Load(EXPLOSION_PREFAB_PATH),
                position,
                Quaternion.identity,
                GameObject.Find(Application.VIEW).transform
            );
                
            Destroy(e, 5f);
            
            foreach ( Collider hit in Physics.OverlapSphere( position, this.explosionRadius ) ) if ( hit.attachedRigidbody != null )
                hit.attachedRigidbody
                    .AddExplosionForce( this.explosionPower, position, this.explosionRadius, this.explosionUpwards, ForceMode.Impulse );
        }

        public override void CreateFire(Vector3 position, Transform parent)
        {
            GameObject f = (GameObject) Instantiate(

                Resources.Load(FIRE_PREFAB_PATH),
                position,
                new Quaternion()
            );
            Vector3 s = f.transform.localScale;
            f.transform.parent = parent;
            f.transform.localScale = s;
        } 
        
        private void OnCollisionEnter(Collision collision)
        {
            this.CreateExplosion(collision.contacts[0].point);
            Destroy(this.gameObject);
        }
    }
}
