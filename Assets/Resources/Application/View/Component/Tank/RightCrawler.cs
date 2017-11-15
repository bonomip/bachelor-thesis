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
    }
}
