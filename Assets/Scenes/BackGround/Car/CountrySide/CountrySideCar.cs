using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountrySideCar : MonoBehaviour
{
    private float speed;
    private float scale;

    void Start()
    {
        scale = Random.Range(17f, 19f);
        speed = Random.Range(1f, 4f);

        transform.localScale = new Vector3(scale, scale, scale);
        StartCoroutine(Die());
    }

    private void Update()
    {
        if (gameObject.name.Contains("TORIGHT"))
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        else
            transform.Translate(Vector2.left * Time.deltaTime * speed);

    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }

}
