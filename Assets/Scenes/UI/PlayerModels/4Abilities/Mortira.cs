using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mortira : MonoBehaviour
{
    public GameObject[] grenades;
    public ParticleSystem effectShoot;
    public GameObject particleOnDestroy;
    public Transform point;
    public Image fillImage;

    public Vector2 force;

    private void Start()
    {
        cd = 10f;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot() // 2f
    {
        while (true)
        {
            GetComponent<Animation>().Play();
            yield return new WaitForSeconds(0.5f);

            effectShoot.Play();

            var prefab = Instantiate(grenades[Random.Range(0, grenades.Length)], point.position, Quaternion.identity);
            prefab.name = "MORTIRA GRENADE";
            prefab.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("dialog");
            prefab.GetComponent<SpriteRenderer>().sortingOrder = -2000;

            for (int i = 0; i < 4; ++i)
            {
                prefab.transform.GetChild(0).GetChild(i).gameObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("dialog");
                prefab.transform.GetChild(0).GetChild(i).gameObject.GetComponent<Renderer>().sortingOrder += -3000;
            }
           

            prefab.GetComponent<Rigidbody2D>().AddForce(force);

            yield return new WaitForSeconds(1.6f);
        } 
    }

    private float cd;
    private void Update()
    {
        fillImage.fillAmount = cd / 10.0f;

        cd -= Time.deltaTime;

        if (cd <= 0.0f)
        {
            Instantiate(particleOnDestroy, transform.position, Quaternion.identity);
            StopCoroutine(Shoot());

            Destroy(gameObject);
        }
    }
}
