using UnityEngine;
using System.Collections;

namespace Application.Model{
    public class Opponent{

        private const string PREFAB_PATH = "Prefab/opponent";
        private GameObject gameobject;

        public Opponent(Vector3 position){
            this.gameobject = (GameObject) GameObject.Instantiate(Resources.Load(PREFAB_PATH), position, Quaternion.identity, GameObject.Find(Application.VIEW).transform);
            this.gameobject.AddComponent<Component.Tank.Main>();
        }

        public GameObject getGameObject(){
            return gameobject;
        }
    }
}