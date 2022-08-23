using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public int randomnumberfromstart;
    public GameObject Car0, Car0TORIGHT, Car1, Car1TORIGHT;
    public Transform parent;

    public bool ready = true;

    private void Start()
    {



    }

    private void Update()
    {
        if (ready)
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        ready = false;

        randomnumberfromstart = Random.Range(3, 8); // 8 30

        yield return new WaitForSeconds(randomnumberfromstart);

        if (Random.Range(0, 2) == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                Instantiate(Car0TORIGHT, transform.position, Quaternion.identity, parent);
            }
            else
            {
                Vector3 pos = new Vector3(transform.position.x + 60f, transform.position.y + 0.5f, transform.position.z);
                Instantiate(Car0, pos, Quaternion.identity, parent);
            }
                
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                Vector3 pos = new Vector3(transform.position.x + 60f, transform.position.y + 0.4f, transform.position.z);
                Instantiate(Car1, pos, Quaternion.identity, parent);
            }
            else
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
                Instantiate(Car1TORIGHT, pos, Quaternion.identity, parent);
            }
        }

        ready = true;

    }
}
