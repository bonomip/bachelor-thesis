using UnityEngine;
using System.Collections;

namespace Application.Controller
{
    public class Controller : MonoBehaviour
    {
        private Model.Model model;
        private GameObject[] opponents;

        public static void attach()
        {
            GameObject.Find(Application.CONTROLLER).AddComponent<Controller>();
        }

        private void Start()
        {
            this.model = GameObject.Find(Application.MODEL).GetComponent<Model.Model>();
        }


        //START VIEW

        public void populateScene()
        {
            this.model.createPlayer();

            this.opponents = new GameObject[1];
            this.opponents[0] = this.model.spawnOpponent(new  Vector3(-1.25f, 60f, 145f));
        }

        public void anyKeyPressed()
        {
            Destroy(GameObject.Find(Application.VIEW).GetComponent<View.PressAnyKey>());
            Component.MainCamera.GoToPlayer.attach(this);
        }

        public void startGame(Component.MainCamera.GoToPlayer cam)
        {
            Destroy(cam);
            Component.MainCamera.FollowPlayer.attach();
            View.PlayerInput.attach();

            //use this to give AI to tank
            foreach(GameObject o in this.opponents){
                o.AddComponent<Component.TankAi.ProvaMira>();
            }
        }
        
        
        // PLAY VIEW
        
        
      
        
    }
}
