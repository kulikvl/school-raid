using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyGun : MonoBehaviour
{
    public GameObject Bomb1, Bomb2;
    public Transform point1, point2;

    public GameObject Bus;

    private GameObject curBomb;

    public void Shoot()
    {
        StartCoroutine(IShoot());
    }

    public void ShootAiming()
    {
        StartCoroutine(IShootAiming());
    }

    public void ShootGiant()
    {
        StartCoroutine(IShootGiant());
    }

    IEnumerator IShoot()
    {
        yield return new WaitForSeconds(0.5f);

        CreateBomb(true);
        CreateBomb(false);
    }

    IEnumerator IShootAiming()
    {
        yield return new WaitForSeconds(0.5f);

        CreateBombAiming(true);
        
        CreateBombAiming(false);
    }

    IEnumerator IShootGiant()
    {
        yield return new WaitForSeconds(0.5f);

        CreateBombGiant(true);

        CreateBombGiant(false);
    }

    private void CreateBomb(bool first)
    {
        if (first)
        {
            curBomb = Instantiate(Bomb1, point1.position, point1.rotation);

            curBomb.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
            curBomb.GetComponent<Rigidbody2D>().gravityScale = Random.Range(0.5f, 0.8f);
            curBomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-25f, -15f), 0f));
            curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;
        }
        else
        {
            curBomb = Instantiate(Bomb2, point2.position, point2.rotation);

            curBomb.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
            curBomb.GetComponent<Rigidbody2D>().gravityScale = Random.Range(0.5f, 0.8f);
            curBomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(15f, 25f), 0f));
            curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;
        }
        
    }

    private void CreateBombAiming(bool isRight)
    {
        if (isRight)
        {
            curBomb = Instantiate(Bomb1, point1.position, point1.rotation);
   
            curBomb.GetComponent<Aiming>().Aim();
            curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;
        }
        else
        {
            curBomb = Instantiate(Bomb2, point2.position, point2.rotation);

            curBomb.GetComponent<Aiming>().Aim();
            curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;
        }

    }

    private void CreateBombGiant(bool first)
    {
        if (first)
        {
            curBomb = Instantiate(Bomb1, point1.position, point1.rotation);

            curBomb.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
            curBomb.GetComponent<Rigidbody2D>().gravityScale = Random.Range(0.5f, 0.8f);
            curBomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-25f, -15f), 0f));
            curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;

            curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ToDelay = true;
            
        }
        else
        {
            curBomb = Instantiate(Bomb2, point2.position, point2.rotation);

            curBomb.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
            curBomb.GetComponent<Rigidbody2D>().gravityScale = Random.Range(0.5f, 0.8f);
            curBomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(15f, 25f), 0f));
            curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;

            curBomb.GetComponent<Destructible2D.D2dImpactSpawner>().ToDelay = true;
        }

    }

    //private float ForceToClosestEnemy()
    //{
    //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //    GameObject closest = null;

    //    float minDistance = Mathf.Infinity;
    //    float currentDistnace;

    //    if (enemies.Length != 0)
    //    {
    //        for (int i = 0; i < enemies.Length; ++i)
    //        {
    //            currentDistnace = Vector2.Distance(enemies[i].transform.position, transform.position);
    //            //currentDistnace = Mathf.Sqrt(Mathf.Pow(enemies[i].transform.position.x, 2f) - Mathf.Pow(transform.position.x, 2f));
    //            //currentDistnace = enemies[i].transform.position.x - transform.position.x;
    //            //currentDistnace = Mathf.Abs(currentDistnace);

    //            if (currentDistnace < minDistance)
    //            {
    //                minDistance = currentDistnace;
    //                closest = enemies[i];
    //            }
    //        }

    //        if (closest != null )//&& Mathf.Abs(minDistance) <= 6f && Mathf.Abs(minDistance) >= 2f)
    //        {
    //            if (closest.transform.position.x < transform.position.x) minDistance *= -1;
    //        }
    //        else minDistance = 0f;

    //        Debug.Log("minDist: " + minDistance);

    //        return minDistance * 10f;
    //    }
    //    else
    //    {
    //        return 0;
    //    }
        
    //}
}
