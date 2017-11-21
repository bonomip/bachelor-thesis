using UnityEngine;
using System.Collections;

namespace Application.Component.Tank
{
    public class Engine : MonoBehaviour
    {
        private const string NAME = "engine";

        public Main main;
        
        private const float WHEEL_TORQUE_FORCE = 185;

        Crawler left;
        Crawler right;

        public static Engine attach(Transform parent, Main m)
        {
            return parent.Find(NAME).gameObject.AddComponent<Engine>().linkMain(m);
        }

        public Engine linkMain(Main m)
        {
            this.main = m;
            return this;
        }

        private void Start()
        {
            this.left = Crawler.attach(true, this.transform, this.main);
            this.right = Crawler.attach(false, this.transform, this.main);
        }
        
        public void moveForward(bool leftPressed, bool rightPressed)
        {
            if(this.left != null) this.left.move (  WHEEL_TORQUE_FORCE  *  ( leftPressed ? 0.5f : 1f ) * ( rightPressed ? 0.5f : 1f ) );
            if(this.right != null) this.right.move( WHEEL_TORQUE_FORCE  *  ( leftPressed ? 0.5f : 1f ) * ( rightPressed ? 0.5f : 1f ) );
        }

        public void moveBack(bool leftPressed, bool rightPressed)
        {
            if(this.left != null) this.left.move (  - WHEEL_TORQUE_FORCE * 0.70f * ( leftPressed ? 0.5f : 1f ) * ( rightPressed ? 0.5f : 1f ) );
            if(this.right != null) this.right.move( - WHEEL_TORQUE_FORCE * 0.70f * ( leftPressed ? 0.5f : 1f ) * ( rightPressed ? 0.5f : 1f ) );
        }

        public void rotateLeft()
        {   
            if(this.left != null) this.left.move ( - WHEEL_TORQUE_FORCE );
            if(this.right != null) this.right.move( WHEEL_TORQUE_FORCE   );
        }
        
        public void rotateRight()
        {
            if(this.left != null) this.left.move ( WHEEL_TORQUE_FORCE   );
            if(this.right != null) this.right.move( - WHEEL_TORQUE_FORCE );
        }
        
        public void stop()
        {
            if(this.left != null) this.left.brake();
            if(this.right != null) this.right.brake();
        }

        private void OnDestroy()
        {
            Destroy(this.left);
            Destroy(this.right);
        }
    }
}
