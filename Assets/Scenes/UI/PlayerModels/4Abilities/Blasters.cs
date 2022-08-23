using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blasters : MonoBehaviour
{
    public GameObject grenadePrefab1;
    public GameObject grenadePrefab2;

    public GameObject grenadePrefabGreen;

    public GameObject Bus;

    private GameObject curGrenade;
    private int number;

    private void ShootGrenade()
    {
        AudioManager.instance.Play("Whistle1");

        if (Random.Range(0, 2) == 0)
            curGrenade = Instantiate(grenadePrefab1, transform.position, transform.rotation);
        else
            curGrenade = Instantiate(grenadePrefab2, transform.position, transform.rotation);

        curGrenade.name = "GRENADE" + number++;
        curGrenade.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
        curGrenade.GetComponent<Rigidbody2D>().mass = 0.3f;

        if (Bus.GetComponent<BusMenu>() != null)
        {
            curGrenade.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = 2f;
        }
        else
        curGrenade.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;
  
    }

    public void Shoot()
    {
        StartCoroutine(IShoot());
    }

    public float GetX()
    {
        float x = Random.Range(2.5f, 5f);

        if (Random.Range(0, 2) == 0)
            return x;
        else
            return -x;
    }

    IEnumerator IShoot()// 2.33f
    {
        yield return new WaitForSeconds(0.5f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.25f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.25f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.25f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(1.05f); // 1.5

    }

    public void ShootMore()
    {
        StartCoroutine(IShootMore());
    }

    IEnumerator IShootMore()// 2.33f
    {
        yield return new WaitForSeconds(0.5f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.2f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.15f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.1f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.1f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));
        yield return new WaitForSeconds(0.1f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));
        yield return new WaitForSeconds(0.1f);

        ShootGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(1.05f); // 1.5

    }

    public void ShootWildFire()
    {
        StartCoroutine(IShootWILDFIRE());
    }

    IEnumerator IShootWILDFIRE()// 2.33f
    {
        yield return new WaitForSeconds(0.5f);

        ShootWildFireGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.38f);

        ShootWildFireGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));

        yield return new WaitForSeconds(0.38f);

        ShootWildFireGrenade();
        curGrenade.GetComponent<Rigidbody2D>().AddForce(new Vector2(GetX(), 0));


        yield return new WaitForSeconds(1.05f); // 1.5

    }

    private void ShootWildFireGrenade()
    {
        AudioManager.instance.Play("Whistle2");
        curGrenade = Instantiate(grenadePrefabGreen, transform.position, transform.rotation);

        curGrenade.name = "GRENWILD" + number++;
        curGrenade.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
        curGrenade.GetComponent<Rigidbody2D>().mass = 0.3f;
        curGrenade.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;

    }

}
