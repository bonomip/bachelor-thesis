using UnityEngine;
using System.Collections;

namespace Application.Component.Tank.Ammunition
{
    public class Standard : Ammunition
    {
        private const string GUN_FIRE_PREFAB_PATH = "Prefab/gun_fire";
        private const string AMMO_PREFAB_PATH = "Prefab/ammunition";
        private const string EXPLOSION_PREFAB_PATH = "Prefab/explosion";
        private const string FIRE_PREFAB_PATH = "Prefab/fire";

        private const float VELOCITY = 50f;

        private void Start()
        {
            this.damage = 1000f;
            this.explosionPower = 850f;
            this.explosionRadius = 15f;
            this.explosionUpwards = 1f;
        }

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
            a.GetComponent<Rigidbody>().AddForce(normal * VELOCITY, ForceMode.Impulse);
            a.AddComponent<Standard>();

            Destroy(a, 10f);
        }

        private void CreateExplosion(Vector3 position)
        {
            GameObject e = (GameObject) Instantiate(

                Resources.Load(EXPLOSION_PREFAB_PATH),
                position,
                Quaternion.identity,
                GameObject.Find(Application.VIEW).transform
            );
                
            Destroy(e, 6.5f);

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

            f.transform.parent = parent;
            f.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        } 
        
        private void OnCollisionEnter(Collision collision)
        {
            //TODO delete this
            if (collision.gameObject.tag == "Opponent") Debug.Log("Hit target " + Vector3.Distance(collision.contacts[0].point, GameObject.Find("player").transform.Find("body").position) + " meters far! " );
        
            CreateExplosion(collision.contacts[0].point);
            Destroy(this.gameObject);
        }
    }
}
