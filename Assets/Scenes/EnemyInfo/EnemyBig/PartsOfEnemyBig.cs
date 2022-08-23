using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsOfEnemyBig : PartsOfEnemy
{
    public GameObject Hips;
    public Rigidbody2D Head;
    public Rigidbody2D Body;
    public GameObject Minigun;

    public override void Start()
    {
        if (gameObject.name.Contains("Right"))
        {
            Head.constraints = RigidbodyConstraints2D.FreezeAll;
            Body.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        base.Start();
    }

    public override void Die()
    {
        if (gameObject.name.Contains("Right"))
        {
            Head.constraints = RigidbodyConstraints2D.None;
            Body.constraints = RigidbodyConstraints2D.None;
        }

        base.Die();

        Hips.GetComponent<ShootEnemy1>().enabled = false;
        gameObject.GetComponent<Animation>().enabled = false;
        //Hips.transform.rotation = new Quaternion(0, 0, 0, 0);

        Minigun.GetComponent<Rigidbody2D>().AddTorque(20f);

        if (Random.Range(0, 2) == 0)
            Minigun.GetComponent<Rigidbody2D>().AddForce(new Vector2(200f, 500f));
        else
            Minigun.GetComponent<Rigidbody2D>().AddForce(new Vector2(-200f, 500f));
    }
}
