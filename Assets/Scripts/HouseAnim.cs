using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseAnim : MonoBehaviour
{
    public int number;

    private void Start()
    {
        StartCoroutine(start());
    }

    IEnumerator start()
    {
        yield return new WaitForSeconds(number);
        gameObject.GetComponent<Animation>().Play();
    }
}
