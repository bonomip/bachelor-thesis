using UnityEngine;
using System.Collections;

namespace Application.Component.Reset
{
    public class Hand : MonoBehaviour
    {
        private const string HAND_PREFAB_PATH = "Prefab/hand";
        private View.PlayerReset view;

        public static Hand CreateHand(Vector3 position)
        {
            GameObject a = (GameObject)Instantiate(

                Resources.Load(HAND_PREFAB_PATH),
                position,
                Quaternion.Euler(0f,-90f,0f),
                GameObject.Find(Application.VIEW).transform
                );
                
            a.tag = Application.HAND_TAG;
            return a.AddComponent<Hand>();
        }

        public Hand linkView(View.PlayerReset v){
            this.view = v;
            return this;
        }


        private Vector3 p1;
        private Vector3 p2;
        private Vector3 v1;
        private Transform player;
        private Component.Tank.Main p;
        private bool step1, step2, step3, step4;

        private void Start(){
            this.p = GameObject.Find(Application.PLAYER).GetComponent<Component.Tank.Main>();
            this.player = p.transform;
            this.p1 = this.transform.position;
            this.p2 = this.player.position;
            this.v1 = this.player.GetComponent<Rigidbody>().velocity;
            this.step1 = this.step2 = this.step3 = this.step4 = true;

        }

        private void FixedUpdate()
        {
            if(step1) {
                this.transform.position = Vector3.Lerp(this.transform.position, this.player.position, 0.05f);
                if (Vector3.Distance(this.transform.position, this.player.position) < 0.05f){
                    this.p.setColliderStatus(false, this.player);
                    this.step1 = false;
                }
                return;    
                }

            if(step2) {
                this.transform.position = Vector3.Lerp(this.transform.position, this.p1, 0.05f);
                this.player.position = this.transform.position;
                this.rotate();
                if (Vector3.Distance(this.transform.position, this.p1) < 0.05f)
                    this.step2 = false;
                return;
            }

            if(step3) {
                this.transform.position = Vector3.Lerp(this.transform.position, this.p2 + new Vector3(0f, 0.5f, 0f), 0.02f);
                this.player.position = this.transform.position;
                this.rotate();
                if (Vector3.Distance(this.transform.position, this.p2 + new Vector3(0f, 0.5f, 0f)) < 0.2f) {
                    this.view.resetComplete();
                    this.step3 = false;
                    this.p.setColliderStatus(true, this.player);
                }
                return;
            }

            if(step4) {
                this.transform.position = Vector3.Lerp(this.transform.position, this.p1, 0.05f);
                if (Vector3.Distance(this.transform.position, this.p1) < 0.05f)
                    Destroy(this.gameObject);
                return;
            }
        }

        private void rotate(){
            this.player.rotation = this.transform.rotation;
        }
    }
}