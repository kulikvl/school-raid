using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bombPrefab;
    public GameObject Bus;

    public Vector2 regulatePos;

    private GameObject curBomb;
    private int number;

    [Header("Others")]
    public GameObject particleEffectAtSplitting;

    public void Shoot()
    {
        Vector3 pos = new Vector3(Bus.transform.position.x + regulatePos.x, Bus.transform.position.y + regulatePos.y, 0);
        curBomb = Instantiate(bombPrefab, pos, transform.rotation);

        curBomb.name = "BombOrdinary" + number++;
        curBomb.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
        curBomb.GetComponent<Rigidbody2D>().mass = 0.3f;
        curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;
    }

    public void ShootFire()
    {
        Shoot();

        curBomb.name = "BombFire";
        curBomb.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        curBomb.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

    }

    public void ShootSplit()
    {
        Shoot();

        if (curBomb != null)
        StartCoroutine(Splitting());
    }

    IEnumerator Splitting()
    {
        yield return new WaitForSeconds(0.5f);

        if (curBomb != null)
        {
            Vector3 pos = new Vector3(curBomb.transform.position.x, curBomb.transform.position.y - 0.5f, curBomb.transform.position.z);
            Instantiate(particleEffectAtSplitting, pos, Quaternion.identity);
            ShootSplit(true);
            ShootSplit(false);
        }
    }

    private void ShootSplit(bool isRight)
    {
        float plusX = (isRight) ? 0.3f : -0.3f;
        float addForceX = (isRight) ? 20f : -20f;

        Vector3 pos = new Vector3(curBomb.transform.position.x + plusX, curBomb.transform.position.y, 0);
        var prefab = Instantiate(bombPrefab, pos, transform.rotation);
        prefab.name = "BombOrdinary SPLITTING";

        prefab.GetComponent<Rigidbody2D>().velocity = curBomb.GetComponent<Rigidbody2D>().velocity;
        prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(addForceX, 0f));
        prefab.GetComponent<Rigidbody2D>().mass = Random.Range(0.2f, 0.4f);
        prefab.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;
    }
}
