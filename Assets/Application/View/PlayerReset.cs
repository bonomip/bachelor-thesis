using UnityEngine;
using System.Collections;

namespace Application.View
{
    public class PlayerReset : MonoBehaviour
    {
    
        private Component.Tank.Main player;

        public static void attach()
        {
            GameObject.Find(Application.VIEW).AddComponent<PlayerReset>();
        }

        void Start()
        {   
            this.player = GameObject.Find(Application.PLAYER).GetComponent<Component.Tank.Main>();
            Component.Reset.Hand.CreateHand(this.player.transform.position + new Vector3(0f,7f,0f)).linkView(this);
        }

        public void resetComplete(){
            PlayerInput.attach();
            Destroy(this);
        }   
    }
}