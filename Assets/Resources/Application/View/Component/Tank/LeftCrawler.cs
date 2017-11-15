using UnityEngine;
using System.Collections;

namespace Application.View.Component.Tank
{
    public class LeftCrawler : Crawler
    {
        private const string NAME = "lCrawler";

        public static LeftCrawler attach(Transform parent)
        {
            return parent.Find(NAME).gameObject.AddComponent<LeftCrawler>().GetComponent<LeftCrawler>();
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
                Debug.Log("left crawler hitten");
                this.damaged = true;
            }
        }
    }
}
