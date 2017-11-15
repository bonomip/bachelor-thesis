using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class Engine : MonoBehaviour
    {
        private const string NAME = "engine";
        
        private const float WHEEL_TORQUE_FORCE = 185;

        LeftCrawler left;
        RightCrawler right;

        public static Engine attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<Engine>().GetComponent<Engine>();
        }

        private void Start()
        {
            this.left = LeftCrawler.attach(this.transform);
            this.right = RightCrawler.attach(this.transform);
        }
        
        public void moveForward(bool leftPressed, bool rightPressed)
        {
            this.left.move ( WHEEL_TORQUE_FORCE * (leftPressed ? -0.5f : 1f) );
            this.right.move( WHEEL_TORQUE_FORCE * (rightPressed ? -0.5f : 1f) );
        }

        public void moveBack(bool leftPressed, bool rightPressed)
        {
            this.left.move ( - WHEEL_TORQUE_FORCE * 0.60f * (rightPressed ? -0.5f : 1f) );
            this.right.move( - WHEEL_TORQUE_FORCE * 0.60f * (leftPressed ? -0.5f : 1f) );
        }

        public void rotateLeft()
        {   
            this.left.move ( - WHEEL_TORQUE_FORCE );
            this.right.move( WHEEL_TORQUE_FORCE   );
        }
        
        public void rotateRight()
        {
            this.left.move ( WHEEL_TORQUE_FORCE   );
            this.right.move( - WHEEL_TORQUE_FORCE );
        }
        
        public void stop()
        {
            this.left.brake();
            this.right.brake();
        }
    }
}
