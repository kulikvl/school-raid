using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public GameObject dynamitePrefab;
    public GameObject Bus;

    public Vector2 regulatePos;

    private GameObject curDynamite;
    private int number;

    [Header("Others")]
    public GameObject particleEffectAtSplitting;
    public GameObject littleDynamitePrefab;

    public void Shoot()
    {
        Vector3 pos = new Vector3(Bus.transform.position.x + regulatePos.x, Bus.transform.position.y + regulatePos.y, 0);
        curDynamite = Instantiate(dynamitePrefab, pos, transform.rotation);

        curDynamite.name = "DYNAMITE" + number++;
        curDynamite.GetComponent<Rigidbody2D>().velocity = Bus.GetComponent<Rigidbody2D>().velocity;
        curDynamite.GetComponent<Rigidbody2D>().mass = 0.3f;
        curDynamite.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;
    }

    public void ShootWithLittleTntsAtTheBottom()
    {
        Shoot();
        curDynamite.GetComponent<Destructible2D.D2dImpactSpawner>().ToThrowTnts = true;
    }

    public void Throw3LittleTnts(Vector3 point) // for impactSpawner
    {
        Debug.Log("throw 3 tnts!");

        Vector3 pos = new Vector3(point.x, point.y + 0.5f);

        for (int i = 0; i < 3; ++i)
        {
            if (i == 0)
                pos = new Vector3(point.x - 0.1f, point.y + 0.5f);
            else if (i == 2)
                pos = new Vector3(point.x + 0.1f, point.y + 0.5f);


            var prefab = Instantiate(littleDynamitePrefab, pos, transform.rotation);
            prefab.name = "DYNLITTLE FROM IMPACT";
            prefab.GetComponent<Rigidbody2D>().mass = 0.1f;
            prefab.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;

            if (i == 0)
            prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(-5f, 30f)); // left
            else if (i == 1)
            prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 30f)); // center
            else if (i == 2)
            prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(5f, 30f)); // right
        }

        //Vector3 pos = new Vector3(point.x, point.y + 0.5f);

        //var prefab = Instantiate(littleDynamitePrefab, pos, transform.rotation);
        //prefab.name = "DYNAMITE LITTLE FROM IMPACT";

        //prefab.GetComponent<Rigidbody2D>().velocity = curDynamite.GetComponent<Rigidbody2D>().velocity;
        //prefab.GetComponent<Rigidbody2D>().mass = 0.1f;
        //prefab.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;

        //prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 30f)); // centre
        //prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(5f, 30f)); // right
        //prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(-5f, 30f)); // left
    }

    public void ShootSplit()
    {
        Shoot();

        if (curDynamite != null)
            StartCoroutine(Splitting());
    }

    IEnumerator Splitting()
    {
        yield return new WaitForSeconds(0.5f);

        if (curDynamite != null)
        {
            Vector3 pos = new Vector3(curDynamite.transform.position.x, curDynamite.transform.position.y - 0.5f, curDynamite.transform.position.z);
            Instantiate(particleEffectAtSplitting, pos, Quaternion.identity);

            ShootSplit5();

            Destroy(curDynamite);
        }
    }

    private void ShootSplit5()
    {
        float plusX = 0f;

        for (int i = 0; i < 5; ++i) // left -> right
        {
            if (i == 0)
                plusX = -0.2f;
            else if (i == 1)
                plusX = -0.1f;
            else if (i == 2)
                plusX = 0.0f;
            else if (i == 3)
                plusX = 0.1f;
            else if (i == 4)
                plusX = 0.2f;

            Vector3 pos = new Vector3(curDynamite.transform.position.x + plusX + 0.1f, curDynamite.transform.position.y, 0);

            var prefab = Instantiate(littleDynamitePrefab, pos, transform.rotation);
            prefab.name = "DYNLITTLE SPLITTING (5)";

            prefab.GetComponent<Rigidbody2D>().velocity = curDynamite.GetComponent<Rigidbody2D>().velocity;
            prefab.GetComponent<Rigidbody2D>().mass = 0.1f;
            prefab.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = Bus.GetComponent<BusController>().BlastRadius;

            if (i == 0)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(-7f, 0f)); 
            else if (i == 1)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3.5f, 0f)); 
            else if (i == 2)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 0f));
            else if (i == 3)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(3.5f, 0f));
            else if (i == 4)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(7f, 0f));
        }
    }
}
