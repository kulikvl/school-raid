using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDynamite : MonoBehaviour
{
    public GameObject littleDynamitePrefab;

    public void Throw3LittleTnts(Vector3 point) // for impactSpawner
    {
        Debug.Log("throw 5 tnts!");

        Vector3 pos = new Vector3(point.x, point.y + 0.5f);

        for (int i = 0; i < 5; ++i)
        {
            if (i == 0)
                pos = new Vector3(point.x - 0.2f, point.y + 0.7f);
            else if (i == 2)
                pos = new Vector3(point.x + 0.2f, point.y + 0.7f);
            else if (i == 3)
                pos = new Vector3(point.x - 0.3f, point.y + 0.7f);
            else if (i == 4)
                pos = new Vector3(point.x + 0.3f, point.y + 0.7f);


            var prefab = Instantiate(littleDynamitePrefab, pos, transform.rotation);
            prefab.name = "DYNLITTLE FROM BIIIG IMPACT";
            prefab.GetComponent<Rigidbody2D>().mass = 0.1f;
            prefab.GetComponent<Destructible2D.D2dImpactSpawner>().ExplosionRadius = FindObjectOfType<BusController>().BlastRadius;

            if (i == 0)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(-5f, 50f)); // left
            else if (i == 1)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 50f)); // center
            else if (i == 2)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(5f, 50f)); // right
            else if (i == 3)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10f, 60f)); // +
            else if (i == 4)
                prefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(10f, 60f)); // +
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
}
