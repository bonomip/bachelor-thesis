using UnityEngine;
using System.Collections;

namespace Application.Model
{
    public class Player
    {     
        private const string PREFAB_PATH = "Prefab/player";

        public Player()
        {
            GameObject.Find(Application.PLAYER).AddComponent<Component.Tank.Main>();
        }   
    }
}
