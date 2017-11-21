using UnityEngine;
using System.Collections;

namespace Application.Controller
{
    public class Controller : MonoBehaviour
    {
        private Model.Model model;

        public static void attach()
        {
            GameObject.Find(Application.CONTROLLER).AddComponent<Controller>();
        }

        private void Start()
        {
            this.model = GameObject.Find(Application.MODEL).GetComponent<Model.Model>();
        }


        //START VIEW

        public void createScene()
        {
            this.model.createPlayer();
        }

        public void anyKeyPressed()
        {
            Destroy(GameObject.Find(Application.VIEW).GetComponent<View.PressAnyKey>());
            Component.MainCamera.GoToPlayer.attach(this);
        }

        public void mainCameraOnPlayer(Component.MainCamera.GoToPlayer cam)
        {
            Destroy(cam);
            Component.MainCamera.FollowPlayer.attach();
            View.PlayerInput.attach();
        }
        
        
        // PLAY VIEW
        
        
      
        
    }
}
