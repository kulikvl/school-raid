using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountrySideSpawnerCar : MonoBehaviour
{
    private float randomnumberfromstart;
    public GameObject Car, CarTORIGHT;

    public bool ready = true;

    private void Update()
    {
        if (ready)
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        ready = false;

        randomnumberfromstart = 1f;

        yield return new WaitForSeconds(Random.Range(3f, 7f));

        if (Random.Range(0, 2) == 0)
        {
            var prefab = Instantiate(CarTORIGHT, transform.position, Quaternion.identity, gameObject.transform);
            prefab.GetComponent<SpriteRenderer>().sortingOrder += 5;
        }
        else
        {
            Vector3 pos = new Vector3(transform.position.x + 40f, transform.position.y + 0.5f, transform.position.z);
            Instantiate(Car, pos, Quaternion.identity, gameObject.transform);
        }

        ready = true;
    }
}
