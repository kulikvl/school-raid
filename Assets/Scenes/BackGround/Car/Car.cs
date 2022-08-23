using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float speed;
    private float scale;
    public GameObject carlight;

    void Start()
    {

        if (gameObject.name.Contains("0"))
        {
            scale = Random.Range(5.5f, 7f);
        }
        else
            scale = Random.Range(4.5f, 5.5f);


        speed = Random.Range(3f, 7f);
        transform.localScale = new Vector3(scale, scale, scale);

        //StartCoroutine(Idle());

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

    //IEnumerator Idle()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.05f);
    //        transform.localScale += new Vector3(0.08f, 0.08f, 0.1f);
    //        yield return new WaitForSeconds(0.05f);
    //        transform.localScale -= new Vector3(0.08f, 0.08f, 0.1f);
    //    }
    //}
}
