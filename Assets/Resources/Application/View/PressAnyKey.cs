using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Application.View
{

    public class PressAnyKey : MonoBehaviour
    {

        private Controller.Controller ctrl;
        private Text commands;

        public static void attach()
        {
            GameObject.Find(Application.VIEW).AddComponent<PressAnyKey>();
        }

        void Start()
        {
            this.ctrl = GameObject.Find(Application.CONTROLLER).GetComponent<Controller.Controller>();
            this.ctrl.createScene();
            this.commands = GameObject.Find("text_commands").GetComponent<Text>();
        }
        
        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {
                Destroy(this.commands.gameObject);
                this.ctrl.anyKeyPressed();
            } 
        }
    }
}
