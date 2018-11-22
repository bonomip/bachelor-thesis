using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Application.Controller
{
    public class Controller : MonoBehaviour
    {
        private Model.Model model;
        private GameObject[] opponents;
        private GameObject camera;
        private GameObject view;

        public static void attach()
        {
            GameObject.Find(Application.CONTROLLER).AddComponent<Controller>();
        }

        private void Start()
        {
            //ONLY DEBUG
            //PlayerPrefs.SetFloat("score_map00", 1000000f);


            this.model = GameObject.Find(Application.MODEL).GetComponent<Model.Model>();
            this.camera = GameObject.Find(Application.MAIN_CAMERA);
            this.view = GameObject.Find(Application.VIEW);
        }


        //START VIEW

        public void populateScene()
        {
            this.model.createPlayer();
            //this.opponents = new GameObject[2];
            //this.opponents[0] = this.model.spawnOpponent(new  Vector3(-1.25f, 60f, 145f));
            //this.opponents[1] = this.model.spawnOpponent(new  Vector3(2.25f, 60f, 105f));

        }

        public void anyKeyPressed()
        {
            Destroy(GameObject.Find(Application.VIEW).GetComponent<View.PressAnyKey>());
            Component.MainCamera.GoToPlayer.attach(this);
            this.model.changeMainText("");
        }

        private float startTime = 0;

        public void startGame(Component.MainCamera.GoToPlayer cam)
        {
            Destroy(cam);
            this.model.changeMainText("");
            Component.MainCamera.FollowPlayer.attach();
            View.PlayerInput.attach();
            this.startTime = Time.time;

        }

        public void lose()
        {
            Destroy(this.view.GetComponent<View.PlayerInput>());
            Destroy(this.camera.GetComponent<Component.MainCamera.FollowPlayer>());
            this.model.changeMainText("Defeat");
            StartCoroutine(reloadSceneIn(3.5f));
        }

        IEnumerator reloadSceneIn(float time)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene("backup");
        }


        private int active_turrets = 6;
        public void turretDestroyed(){
            this.active_turrets -= 1;
            if(this.active_turrets <= 0){
                win();
            }
        }

        public void win(){
            if( this.model.updateScore(Time.time - this.startTime) )
                this.model.changeMainText("New record!\nTime: "+(this.model.getScore()).ToString("0.00"));
            else
                this.model.changeMainText("Victory!\nTime: "+(Time.time - this.startTime).ToString("0.00"));

            Destroy(this.view.GetComponent<View.PlayerInput>());
            Destroy(this.camera.GetComponent<Component.MainCamera.FollowPlayer>());

            StartCoroutine(reloadSceneIn(4.5f));
        } 
    }
}
