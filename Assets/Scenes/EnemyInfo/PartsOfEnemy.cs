using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Destructible2D;

public class PartsOfEnemy : MonoBehaviour
{
    [SerializeField] public Rigidbody2D[] rbs;
    [SerializeField] private GameObject[] limbs;
    [SerializeField] private BloodPart[] elementsWithBloodPart;

    [Space]

    [SerializeField] private GameObject currentEyes;
    [SerializeField] private GameObject deadEyes;

    [Space]

    [SerializeField] public GameObject logicGameObject;
    [SerializeField] protected float secondsToDisappear;

    [Space]

    [SerializeField] private bool enableBlood;
    [SerializeField] private bool enableDetachingBone;

    //public float Speed { get { return logicGameObject.GetComponent<Enemy>().Speed; } set { logicGameObject.GetComponent<Enemy>().Speed = value; } }
    //public void DecreaseSpeed(float value) { logicGameObject.GetComponent<Enemy>().DecreaseSpeed(value); }

    //public bool IsDead
    //{
    //    get
    //    {
    //        return logicGameObject.GetComponent<Enemy>().IsDead;
    //    }
    //    set
    //    {
    //        logicGameObject.GetComponent<Enemy>().IsDead = value;
    //    }
    //}

    public virtual void Start()
    {
        if (deadEyes != null)
        deadEyes.SetActive(false);

        foreach (BloodPart bp in elementsWithBloodPart)
        {
            bp.EnableBlood = enableBlood;
            bp.EnableDetachingBone = enableDetachingBone;
        }
        foreach (Rigidbody2D go in rbs)
        {
            go.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    virtual public IEnumerator ToDisappear()
    {
        yield return new WaitForSeconds(secondsToDisappear);
        Destroy(gameObject);
    }

    public virtual void Die()
    {
        //Debug.Log("DIE!" + " bodyRB: " + rbs[0].mass);
        logicGameObject.GetComponent<Enemy>().IsDead = true;

        if (currentEyes != null)
        {
            currentEyes.SetActive(false);
            deadEyes.SetActive(true);

        }

        foreach (GameObject go in limbs)
        {
            Destroy(go);
        }

        logicGameObject.SetActive(false);

        gameObject.GetComponent<Animator>().enabled = false;

        transform.localEulerAngles = new Vector3(0f, 0f, 0f);

        StartCoroutine(ToDisappear());

        if (gameObject.GetComponent<Rigidbody2D>() != null)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }

        //gameObject.GetComponent<BoxCollider2D>().enabled = false; //

        //if (gameObject.GetComponent<CircleCollider2D>() != null) //
        //    gameObject.GetComponent<CircleCollider2D>().enabled = false;

        transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false; //

        if (transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>() != null) //
            transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>().enabled = false;


        foreach (Rigidbody2D go in rbs)
        {
            go.bodyType = RigidbodyType2D.Dynamic;
            go.gravityScale = 1f;
            go.mass = 2f;
        }

        ChangeLayers();

        if (!logicGameObject.GetComponent<Enemy>().isMenuScene)
        gameObject.GetComponent<tabManager>().deleteTab();
    }

    public virtual void ChangeLayers()
    {
        Transform[] trs = gameObject.GetComponentsInChildren<Transform>();

        foreach (var ob in trs)
        {
            if (ob.name.Contains("WEAPON"))
            {
                Rigidbody2D rb = ob.GetComponent<Rigidbody2D>();

                rb.AddTorque(20f);

                if (Random.Range(0, 2) == 0)
                    rb.AddForce(new Vector2(200f, 500f));
                else
                    rb.AddForce(new Vector2(-200f, 500f));

                if (!logicGameObject.GetComponent<Enemy>().isMenuScene)
                    ob.GetComponent<SpriteRenderer>().sortingOrder -= 70;

                if (ob.name == "WEAPON" && ob.GetComponent<BoxCollider2D>() != null) ob.GetComponent<BoxCollider2D>().isTrigger = false;
                if (ob.name == "WEAPON2") ob.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            }
                
            else
            {
                if (ob.gameObject.layer != LayerMask.NameToLayer("IgnorePlayerAndEnemyAndBomb"))
                {
                    ob.gameObject.layer = LayerMask.NameToLayer("IgnorePlayerAndEnemy");
                }
            }
        }
    }

    public void RandomBloodCome()
    {
        Debug.Log("RANDOM BLOOD COME FROM PARTSOF ENEMY!");
        List<BloodPart> list = elementsWithBloodPart.ToList();
        int j = list.Count;

        for (int i = 0; i < j; ++i)
        {
            int num = Random.Range(0, list.Count);
            list[num].Release();
            list.RemoveAt(num);
        }
    }

    public void ImpactAdd()
    {
        Debug.Log("IMPACT FROM PARTSOFENEMY");

        if (rbs[0].name == "Body")
        {
            if (Random.Range(0, 2) == 0)
            {
                rbs[0].AddForce(new Vector2(Random.Range(2500f, 3500f), 8000f));
            }
            else
            {
                rbs[0].AddForce(new Vector2(Random.Range(-3500f, -2500f), 8000f));
            }
        }
        else
            Debug.LogError("error");
    }

    public void MakeStamp()
    {
        MakeStamp makeStamp = GetComponentInChildren<MakeStamp>();
        makeStamp.makeStamp();       
    }
}
