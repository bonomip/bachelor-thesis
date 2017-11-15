using UnityEngine;
using System.Collections;


namespace Application.View.Component.Tank
{
    public class RightCrawler : Crawler
    {
        private const string NAME = "rCrawler";

        public static RightCrawler attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<RightCrawler>().GetComponent<RightCrawler>();
        }
        
        public new void move(float force)
        {
            if (this.damaged) return;
            base.move(force);
        }

        public new void brake()
        {
            if (this.damaged) return;
            base.brake();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == Application.AMMUNITION_TAG)
            {
                //apply damage
                Debug.Log("right crawler hitten");
                this.damaged = true;
            }
        }

    }
}
