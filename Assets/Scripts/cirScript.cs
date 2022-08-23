using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cirScript : MonoBehaviour
{
    public bool isCenter = false;

    private void Awake()
	{
        float rand = Random.Range(0.4f, 0.53f);
        gameObject.transform.localScale = new Vector3(rand, rand);
	}

    public void SpawnInRandomPlace(bool up)
    {
        if (up == true)
        {
            int num = Random.Range(0, 2);

            if (isCenter)
            {
                GetComponent<Animation>().Play("UpperCenter");
                SelfDestroy(6.5f);
            }
            else
            {
                if (num == 0)
                {
                    GetComponent<Animation>().Play("Upper1");
                    SelfDestroy(6.5f);
                }
                else
                {
                    GetComponent<Animation>().Play("Upper2");
                    SelfDestroy(5f);
                }
            }   
        }
        else
        {
            int num = Random.Range(0, 2);

            if (isCenter)
            {
                GetComponent<Animation>().Play("DownCenter");
                SelfDestroy(7.5f);
            }
            else
            {
                if (num == 0)
                {
                    GetComponent<Animation>().Play("Down1");
                    SelfDestroy(5f);
                }
                else
                {
                    GetComponent<Animation>().Play("Down2");
                    SelfDestroy(6.5f);
                }
            }
            
        }
        
    }

    private void SelfDestroy(float time)
    {
        StartCoroutine(ISelfDestroy(time));
    }

    IEnumerator ISelfDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
