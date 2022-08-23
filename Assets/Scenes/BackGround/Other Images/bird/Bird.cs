using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private float speed;
    private float scale;

    public GameObject BirdBlood;

    private void Start()
    {
        speed = Random.Range(3f, 7f);
        scale = Random.Range(0.12f, 0.18f);

        transform.localScale = new Vector3(scale, scale, scale);

        StartCoroutine(Dief());
    }

    public void Die()
    {
        Instantiate(BirdBlood, transform.position, Quaternion.identity);
       //BirdBlood.SetActive(true);
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    }

    IEnumerator Dief()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }

}
