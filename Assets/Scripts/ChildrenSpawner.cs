using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenSpawner : MonoBehaviour
{
    public GameObject cir;
    public bool isCenter = false;

    private bool firstTime1, firstTime2;

    private void Start()
    {
        StartCoroutine(SpawnUpper());
        StartCoroutine(SpawnDown());
    }

    IEnumerator SpawnUpper()
    {
        while (true)
        {
            if (firstTime1 == false)
            {
                yield return new WaitForSeconds(Random.Range(0f, 2f));
                firstTime1 = true;
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(6.5f, 8f));
            }

            var prefab = Instantiate(cir, transform.position, Quaternion.identity, gameObject.transform);

            if (isCenter) prefab.GetComponent<cirScript>().isCenter = true;

            prefab.GetComponent<cirScript>().SpawnInRandomPlace(true);
        }
    }

    IEnumerator SpawnDown()
    {
        while (true)
        {
            if (firstTime2 == false)
            {
                yield return new WaitForSeconds(Random.Range(0f, 2f));
                firstTime2 = true;
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(6.5f, 8f));
            }

            yield return new WaitForSeconds(Random.Range(6.5f, 8f));

            var prefab = Instantiate(cir, transform.position, Quaternion.identity, gameObject.transform);

            if (isCenter) prefab.GetComponent<cirScript>().isCenter = true;

            prefab.GetComponent<cirScript>().SpawnInRandomPlace(false);
        }
    }
}
