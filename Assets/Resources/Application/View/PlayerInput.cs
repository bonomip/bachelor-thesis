using UnityEngine;
using System.Collections;

namespace Application.View
{
    public class PlayerInput : MonoBehaviour
    {
        private Component.Tank.Main player;

        private bool[] inputs;
        private KeyCode[] keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.I, KeyCode.P,  KeyCode.O, KeyCode.L, KeyCode.Space };
        
        public static void attach()
        {
            GameObject.Find(Application.VIEW).AddComponent<PlayerInput>();
        }

        void Start()
        {   
            this.inputs = new bool[this.keys.Length];
            this.player = GameObject.Find(Application.PLAYER).GetComponent<Component.Tank.Main>();
        }

        void Update()
        {
            for (int i = 0; i < this.keys.Length; i++) this.inputs[i] = Input.GetKey(this.keys[i]);
        }

        private void FixedUpdate()
        {
            this.player.input(this.inputs);                   
        }        
    }
}
