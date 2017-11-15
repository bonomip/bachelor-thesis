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
    }
}
