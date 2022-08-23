using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    public float speed;
    public float y;

    public ParticleSystem particleEffect;
    private GameObject bus;

    public float distance;

    private void Start()
    {
        y = Random.Range(0f, -3f);

        if (TutorialSputnik.instance.activateCameraOnSputnik)
        {
            speed = 0.9f;
        }
        else
        speed = Random.Range(1f, 1.5f);

        bus = GameObject.FindGameObjectWithTag("BUS");
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (CheckingY())
            transform.Translate(Vector2.down * Time.deltaTime * speed);
        else
        {
            if (!particleEffect.isPlaying)
            {
                StartCoroutine(Die());
                GetComponent<Animation>().Play("CoinAnimation");
                particleEffect.Play();
            }     
        }

        float speedF = 8f;
        if (bus != null)
        distance = Vector2.Distance(bus.transform.position, gameObject.transform.position);

        if (distance <= 2.3f && bus != null)
            transform.position = Vector3.MoveTowards(transform.position, bus.transform.position, speedF * Time.deltaTime);
    }

    private bool CheckingY()
    {
        return transform.position.y > y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BUS")
        {
            ++sputnikCount.count;
            FindObjectOfType<sputnikCount>().gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
            FindObjectOfType<sputnikCount>().gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();

            AudioManager.instance.Play("Coin" + Random.Range(1, 3).ToString());

            if (TutorialSputnik.instance.readyToCountCoins)
            TutorialSputnik.instance.CoinsTaken += 1;

            Destroy(gameObject);
        }
    }
}
