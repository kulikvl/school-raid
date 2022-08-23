using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chicken : MonoBehaviour
{
    private bool left = false;
    private bool right = false;

    public float speed = 1f;
    public GameObject ChickenBlood;

    public bool menu = false;

    IEnumerator Idle() // 0 - 1.30  4 - 5.30
    {
        while(true)
        {
            left = true;
            right = false;

            yield return new WaitForSeconds(1.5f);
            left = false;
            right = false;

            yield return new WaitForSeconds(2.5f);
            left = false;
            right = true;

            yield return new WaitForSeconds(1.5f);
            left = false;
            right = false;

            yield return new WaitForSeconds(2.5f);
        }
    }

    public void Die()
    {
        Instantiate(ChickenBlood, transform.position, Quaternion.identity);
        //BirdBlood.SetActive(true);
        Destroy(gameObject);
    }

    private void Awake()
    {
        //if (menu == true)
        //{
        //    int result = Random.Range(0, 3); // 0 1 2

        //    if (result == 0 || result == 1)
        //    {
        //        Destroy(gameObject);
        //    }

        //}

        //StartCoroutine(Idle());

        StartCoroutine(showInIntro());
    }

    IEnumerator showInIntro()
    {
        yield return new WaitForSeconds(3f);
        Die();
    }

    private void Update()
    {
        //if (right == true)
        //{
        //    transform.Translate(Vector2.right * Time.deltaTime * speed);
        //}
        //if (left == true)
        //{
        //    transform.Translate(Vector2.left * Time.deltaTime * speed);
        //}

        transform.Translate(Vector2.right * Time.deltaTime * speed);
    }
}
