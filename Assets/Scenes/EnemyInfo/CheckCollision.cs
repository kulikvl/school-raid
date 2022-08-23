using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckCollision : MonoBehaviour
{
    public Enemy logicGameObject;
    public GameObject ExplosionOnCollision;

    private Transform bus;
    private bool bombCreated;

    public Vector2 overlapVector = new Vector2(1.3f, 2f);

    private void CheckCollisionWithBus()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, overlapVector, 0f);

        if (colliders.Contains(bus.gameObject.GetComponent<Collider2D>()) && bus.gameObject.CompareTag("BUS") && !logicGameObject.IsDead )
        {
            DestroyThisGameObject();
        }
    }

    private void DestroyThisGameObject()
    {
        if (!bombCreated && logicGameObject.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
        {
            if (logicGameObject.gameObject.transform.parent.gameObject.name.Contains("Director"))
            {
                bus.gameObject.GetComponent<BusController>().CanBeDamaged = true;
                bus.gameObject.GetComponent<BusController>().takeDamage(2000f); // todo
            }
            else if (logicGameObject.gameObject.transform.parent.gameObject.name.Contains("Kachok"))
            {
                Instantiate(ExplosionOnCollision, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

                bus.gameObject.GetComponent<BusController>().takeDamage(300f); // todo

                logicGameObject.GetComponent<EnemyKachokController>().DieAnyway();

                bombCreated = true;
            }
            else
            {
                Instantiate(ExplosionOnCollision, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);

                bus.gameObject.GetComponent<BusController>().takeDamage(100f); // todo

                logicGameObject.Die(true, true, true);

                bombCreated = true;

            }
            
        }
    }

    private void Start()
    {
        bombCreated = false;

        if (bus == null)
        {
            GameObject b = GameObject.FindGameObjectWithTag("BUS");

            if (b != null)
                bus = b.transform;
            else
            {
                if (!PlayerController.Lost)
                    Debug.LogError("BUS WAS NOT FOUND!");
            }
        }  
    }

    private void Update()
    {
        if (bus != null)
            CheckCollisionWithBus();
    }

    //private void BloodCome()
    //{
    //    BloodPart[] blds = mainGameObject.GetComponentsInChildren<BloodPart>();
    //    List<BloodPart> list = blds.ToList();
    //    for (int i = 0; i < 3; ++i)
    //    {
    //        int num = Random.Range(0, list.Count);
    //        list[num].Release();
    //        list.RemoveAt(num);
    //    }

    //    //Debug.Log("BLOOD COLLISION COME!");
    //}

    //private void ImpactAdd()
    //{
    //    Rigidbody2D[] rbs = mainGameObject.GetComponentsInChildren<Rigidbody2D>();

    //    foreach (Rigidbody2D rb in rbs)
    //    {
    //        if (rb.gameObject.name == "Body")
    //        {
    //            if (Random.Range(0, 2) == 0)
    //            {
    //                rb.AddForce(new Vector2(2000f, 7000f));
    //            }
    //            else
    //            {
    //                rb.AddForce(new Vector2(-2000f, 7000f));
    //            }
    //        }
    //    }
    //}
}
