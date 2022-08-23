using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raketa : MonoBehaviour
{
    public float speed;
    public GameObject particleOnDamage;
    [HideInInspector] public float damage;

    private void Start()
    {
        int num = Random.Range(0, 3);

        bool l = false;
        bool r = false;
        bool c = false;

        GameObject[] schools = GameObject.FindGameObjectsWithTag("school");
        foreach(GameObject school in schools)
        {
            if (school.name == "Left" && !school.GetComponent<School>().destroyed) l = true;
            if (school.name == "Right" && !school.GetComponent<School>().destroyed) r = true;
            if (school.name == "Center" && !school.GetComponent<School>().destroyed) c = true;
        }

        if (num == 0 && l) // left
        {
            transform.position = new Vector3(-4.6f, 16f);
        }
        else if (num == 1 && r) // right
        {
            transform.position = new Vector3(-0.35f, 16f);
        }
        else if (c)// centre
        {
            transform.position = new Vector3(-2.5f, 16f);
        }
        else
        {
            Debug.Log("NO SURVIVED SCHOOLS!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("school"))
        {
            AudioManager.instance.Play("Veteran2");
            Debug.Log("Boom!");
            collision.gameObject.GetComponent<School>().takeDamage(damage);
            Instantiate(particleOnDamage, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * speed);
    }

}
