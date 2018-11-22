using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Application.Model
{
    public class Model : MonoBehaviour
    {
        public const string NAME = "model";
        private Text main_text;

        public static void attach()
        {
            GameObject.Find(NAME).AddComponent<Model>();
        }

        private void Start()
        {
            this.main_text = GameObject.Find("main_text").GetComponent<Text>();
        }


        public void createPlayer()
        {
            new Player();
        }

        public void changeMainText(string text){
            this.main_text.text = text;
        }

        public GameObject spawnOpponent(Vector3 position){
            return new Opponent(position).getGameObject();
        }

        public bool updateScore(float score){
            if (PlayerPrefs.GetFloat("score_map00", 10000000f) > score){
                PlayerPrefs.SetFloat("score_map00", score);
                return true;
            } else return false;
        }

        public float getScore(){
            return  PlayerPrefs.GetFloat("score_map00", 0f);
        }
    }
}
