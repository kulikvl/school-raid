using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class tabSorting : MonoBehaviour
{
    public tabFollow[] trs;
    private tabFollow[] tabs;
    private bool isRight;
    private float value;

    private void Start()
    {
        isRight = (gameObject.name.Contains("Right")) ? true : false;
    }

    private void Update()
    {
        SortTabs(); 
    }

    public void SortTabs()
    {
        trs = GetComponentsInChildren<tabFollow>();
        
        if (trs != null)
        {
            if (isRight)
            {
                var sortedTrs = from i in trs
                                orderby i.posOfEnemyX ascending
                                select i;

                tabs = sortedTrs.ToArray();
            }
            else
            {
                var sortedTrs = from i in trs
                                orderby i.posOfEnemyX descending
                                select i;

                tabs = sortedTrs.ToArray();
            }
            
            float addY = 0.6f;

            for (int i = 0; i < tabs.Length; ++i)
            {
                tabs[i].addToY = addY;

                value = (0.5f - (Screen.height / 1080) * 0.5f) + 0.55f;

                if (Screen.height < 1080) value = 0.6f;

                addY += value;

                if (i == 0)
                {
                    tabs[i].first = true;
                    tabs[i].posY = tabs[i].posOfEnemyY;
                }
                else
                {
                    tabs[i].first = false;
                    tabs[i].posY = tabs[0].posY;
                }
                    
            }
        }
        
    }
}
