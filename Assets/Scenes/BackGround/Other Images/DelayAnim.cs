using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAnim : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(startAnim());
    }

    IEnumerator startAnim()
    {
        GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.1f);
        GetComponent<Animation>().Stop();
        yield return new WaitForSeconds(Random.Range(0f, 6f));
        GetComponent<Animation>().Play();
        
    }
}
