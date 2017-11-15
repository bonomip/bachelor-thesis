using UnityEngine;
using System.Collections;

namespace Application.Model
{
    public class Model : MonoBehaviour
    {
        public const string NAME = "model";

        public static Vector3 PLAYER_SPAWN_POSITION = new Vector3( 0f, 0.6f, 0f );

        public static void attach()
        {
            GameObject.Find(NAME).AddComponent<Model>();
        }

        public void createPlayer()
        {
            new View.Component.Player( PLAYER_SPAWN_POSITION );
        }
    }
}
