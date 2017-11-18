using UnityEngine;
using System.Collections;

namespace Application.Model
{
    public class Player
    {     
        private const string PREFAB_PATH = "Prefab/player";

        public Player(Vector3 position)
        {
            GameObject p = (GameObject)Object.Instantiate(

                    Resources.Load(PREFAB_PATH),
                    position,
                    new Quaternion(),
                    GameObject.Find(Application.VIEW).transform
                
                );
            
            p.gameObject.name = Application.PLAYER;
            p.layer = 8;

            p.AddComponent<View.Component.Tank.Main>();
        }   
    }
}
