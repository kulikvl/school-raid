using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInformatikController : Enemy
{
    public GameObject Weapon;
    private BusController bus;

    public GameObject particleOnBus;
    public GameObject particleOnComputer;

    private ParticleSystem particleContainer;
    private float timeToRunBeforeDamaging;

    public override void Awake()
    {
        base.Awake();

        particleContainer = Weapon.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        timeToRunBeforeDamaging = Random.Range(8f, 13f);
    }

    public override void OnTriggerStay2D(Collider2D collision) // Boxes Case
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            speed = 0f;
        }
    }


    private void Start()
    {
        if (bus == null)
        {
            GameObject b = GameObject.FindGameObjectWithTag("BUS");

            if (b != null)
            {
                bus = b.GetComponent<BusController>();
                TargetToDamage = b;
            }
               
            else
            {
                if (!PlayerController.Lost)
                    Debug.Log("BUS WAS NOT FOUND!");
            }
        }

        AudioManager.instance.Play("LaughEnemyInformatik");
    }

    public override void Update()
    {
        if (ableToRun)
        {
            Run(speed);
            timeToRunBeforeDamaging -= Time.deltaTime;
        }

        if (timeToRunBeforeDamaging <= 0.0f && !IsDead && !IsFreezed && TargetToDamage != null && finishedDamaging && !PlayerController.Lost && !PlayerController.Won)
        {
            StartCoroutine(Attack());
        }
        else if (TargetToDamage == null && IsDamaging)
        {
            stopDamaging();
        }
    }

    private Vector3 GetPosOfBus()
    {
        Vector3 pos = new Vector3(Random.Range(bus.gameObject.transform.position.x - 1f, bus.gameObject.transform.position.x + 1f), bus.gameObject.transform.position.y);
        return pos;
    }

    private void DamageBus()
    {
        if (bus.gameObject != null && !PlayerController.Lost)
        {
            bus.takeDamage(damage);
            shakeEffect();
            Instantiate(particleOnBus, GetPosOfBus(), Quaternion.identity);
        }
    }

    public bool finishedDamaging = true;
    IEnumerator Attack() // 6f
    {

        Debug.Log("atttacj");

        speed = 0f;
        mainGameObject.GetComponent<Animator>().SetBool("damage", true);

        AudioManager.instance.Play("Informatik1");

        finishedDamaging = false;

        if (!particleContainer.isPlaying)
            particleContainer.Play();

        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.Play("Informatik2");

        for (int i = 0; i < 5; ++i)
        {
            yield return new WaitForSeconds(1f);
            if (finishedDamaging) yield break;
            DamageBus();
        }
       
        yield return new WaitForSeconds(0.5f);

        finishedDamaging = true;
    }

    public override void stopDamaging()
    {
        Debug.Log("Stopped Damaging!");

        StopCoroutine(Attack());

        finishedDamaging = true;

        particleContainer.Stop();

        IsDamaging = false;
        speed = initialSpeed;
        mainGameObject.GetComponent<Animator>().SetBool("damage", false);
    }

    public override void winAnimation()
    {
        ableToDamage = false;

        if (isDamaging) stopDamaging();

        speed = 0f;
        IsDamaging = false;
        mainGameObject.GetComponent<Animator>().SetTrigger("win");
    }

    public override void Die(bool Impact, bool Blood, bool Stamp)
    {
        if (!IsDead)
        {
            if (IsDamaging) stopDamaging();

            mainGameObject.GetComponent<Rigidbody2D>().isKinematic = true;

            Instantiate(particleOnComputer, Weapon.transform.position, Quaternion.identity);
            Destroy(Weapon);

            mainGameObject.GetComponent<PartsOfEnemy>().Die();

            if (Impact) mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
            if (Blood) mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
            if (Stamp) mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp();


        }
    }
}
