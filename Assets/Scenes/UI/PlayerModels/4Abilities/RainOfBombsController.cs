using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainOfBombsController : MonoBehaviour
{
    public float minSec, maxSec;
    public GameObject[] bombs;

    private void Start()
    {
        cd = 10f;
        StartCoroutine(ThrowingBombs());
    }

    private Vector3 GetRandomPos()
    {
        return new Vector3(Random.Range(-17f, 11f), 5f, 0f);
    }
    IEnumerator ThrowingBombs()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSec, maxSec));

            transform.position = GetRandomPos();
            var prefab = Instantiate(bombs[Random.Range(0, bombs.Length)], transform.position, Quaternion.identity);
            prefab.name = "MORTIRA GRENADE";
        }
    }

    private float cd;
    private void Update()
    {
        //if (!FreezeEnemies.FREEZED)
        cd -= Time.deltaTime;

        if (cd <= 0.0f)
        {
            StopCoroutine(ThrowingBombs());
            Destroy(gameObject);
        }
    }
}
