using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detachThrowTime : MonoBehaviour
{
    public float time;

    private void Start()
    {
        time = Random.Range(7f, 12f);

        StartCoroutine(detach());
    }

    IEnumerator detach()
    {
        yield return new WaitForSeconds(time);
        GetComponent<HingeJoint2D>().enabled = false;
    }
}
