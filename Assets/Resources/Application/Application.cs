using UnityEngine;
using System.Collections;

namespace Application
{

    public class Application : MonoBehaviour
    {
        public const string NAME = "application";

        public const string MODEL = "model";
        public const string CONTROLLER = "controller";
        public const string VIEW = "view";

        public const string PLAYER = "player";
        public const string MAIN_CAMERA = "main_camera";
        public const string TANK_CAMERA = "tank_camera";
        
        public const string AMMUNITION_TAG = "ammunition";
        

        // Use this for initialization
        void Start()
        {
            Model.Model.attach();
            Controller.Controller.attach();
            View.PressAnyKey.attach();
        }

        // Update is called once per frame
        void Update()
        {
            if ( Input.GetKey(KeyCode.Escape) ) UnityEngine.Application.Quit();
        }
    }
}
