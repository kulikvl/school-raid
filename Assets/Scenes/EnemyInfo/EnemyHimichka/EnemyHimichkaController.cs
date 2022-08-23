using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHimichkaController : Enemy
{
    //public float distanceOfDamaging;

    public GameObject Weapon;
    public GameObject Weapon1;
    public GameObject poisonPrefab;

    public Vector2 force;
    public BoxCollider2D triggerCollider;

    public override GameObject TargetToDamage
    {
        get
        {
            return targetToDamage;
        }
        set
        {
            if (value == null)
            {
                stopDamaging();
            }

            Debug.Log("CHANGED TARGET!");
            targetToDamage = value;
        }
    }

    public override void Awake()
    {
        //speed =

        float offsetX = Random.Range(0f, 3.3f);

        triggerCollider.offset = new Vector2(10f + offsetX, triggerCollider.offset.y);
        triggerCollider.size = new Vector2(19.7f + (offsetX * 2f), triggerCollider.size.y);

        base.Awake(); 
    }

    private void Start()
    {
        AudioManager.instance.Play("LaughEnemyHimichka");
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null && damageable.CanBeDamaged && ableToDamage && targetToDamage == null)
        {
            speed = 0f;
            TargetToDamage = collision.gameObject;

            IsDamaging = true;
        }

    }

    public override void Update()
    {
        if (ableToRun)
        {
            Run(speed);

            if (!Weapon1.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().isPlaying)
            {
                Weapon1.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
            }
        }

        cooldown -= Time.deltaTime;

        if (IsDamaging && !IsDead && !IsFreezed && TargetToDamage != null && FinishedDamaging && !PlayerController.Lost && !PlayerController.Won)
        {
            Debug.Log("attack");
            routine = Attack();
            StartCoroutine(routine);
        }

        if (currentPoison != null && keepPoisonAttached && !FinishedDamaging)
        {
            currentPoison.transform.position = Weapon.transform.position;
            currentPoison.transform.rotation = Weapon.transform.rotation;
        }
    }

    private bool finishedDamaging = true;
    public bool FinishedDamaging { get { return finishedDamaging; }  set { finishedDamaging = value; } }
    private GameObject currentPoison;
    private bool keepPoisonAttached;
    public int attacktimes;
   
    IEnumerator Attack() // 4 seconds
    {
        attacktimes++;

        FinishedDamaging = false;

        if (!FinishedDamaging)
        {
            mainGameObject.GetComponent<Animator>().SetTrigger("test");
            
            currentPoison = Instantiate(poisonPrefab, Weapon.transform.position, Weapon.transform.rotation);
            currentPoison.name = "POISON FOR " + targetToDamage.name;

            keepPoisonAttached = true;

            Weapon1.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
        }

        yield return new WaitForSeconds(3.5f);

        // throw
        if (currentPoison != null && !FinishedDamaging)
        {
            currentPoison.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (mainGameObject.name.Contains("Right"))
            {
                if (targetToDamage.CompareTag("school"))
                    force = new Vector2(Random.Range(-36f, -24f), 10f);
                else
                    force = new Vector2(-35f, 2f);
            }
            else
            {
                if (targetToDamage.CompareTag("school"))
                    force = new Vector2(Random.Range(24f, 36f), 10f);
                else
                    force = new Vector2(35f, 2f);
            }
            

            keepPoisonAttached = false;
            currentPoison.GetComponent<Rigidbody2D>().AddForce(force);
            currentPoison.GetComponent<Poison>().startedFlying = true;
            currentPoison.GetComponent<Poison>().damage = damage;

            currentPoison.GetComponent<Poison>().isRight = mainGameObject.name.Contains("Right");

            if (targetToDamage.name.Contains("box")) currentPoison.GetComponent<Poison>().radius = 0.2f;
           
        }
        
        yield return new WaitForSeconds(0.5f);

        FinishedDamaging = true;
    }

    public IEnumerator routine;

    public override void stopDamaging()
    {
        //Debug.Log(routine.Current);
        Debug.Log("STOP! destroy poison");

        StopCoroutine(routine);

        FinishedDamaging = true;

        Weapon1.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();

        Destroy(currentPoison);
        keepPoisonAttached = false;

        IsDamaging = false;
        speed = initialSpeed;

        //mainGameObject.GetComponent<Animator>().SetBool("damage", false);
        mainGameObject.GetComponent<Animator>().Play("Run", -1, 0f);

        if (targetToDamage != null && targetToDamage.name == "Left" || targetToDamage.name == "Right")
        {
            triggerCollider.size = new Vector2(triggerCollider.size.x - 5f, triggerCollider.size.y);
            triggerCollider.offset = new Vector2(triggerCollider.offset.x - 2.5f, triggerCollider.offset.y);
        }

        
        targetToDamage = null;
    }

    public override void winAnimation()
    {
        ableToDamage = false;

        if (IsDamaging) stopDamaging();

        Weapon1.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
        speed = 0f;
        IsDamaging = false;
        mainGameObject.GetComponent<Animator>().SetTrigger("win");
    }
}
