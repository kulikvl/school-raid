using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVeteranController : Enemy
{
    public GameObject Weapon;

    public GameObject Raketa;

    private float timeToRunBeforeDamaging;

    public override void Awake()
    {
        base.Awake();

        timeToRunBeforeDamaging = Random.Range(7f, 12f);
    }

    private void Start()
    {
        AudioManager.instance.Play("LaughEnemyVeteran");
    }

    public override void OnTriggerStay2D(Collider2D collision) // Boxes Case
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            speed = 0f;

            timeToRunBeforeDamaging = 0.0f;
        }
    }

    public override void Update()
    {
        if (ableToRun)
        {
            Run(speed);
            timeToRunBeforeDamaging -= Time.deltaTime;
        }

        if (timeToRunBeforeDamaging <= 0.0f && !IsDead && !IsFreezed && finishedDamaging && !PlayerController.Lost && !PlayerController.Won)
        {
            StartCoroutine(Attack());
        }
    }

    public bool finishedDamaging = true;
    IEnumerator Attack() // 3f
    {
        Debug.Log("atttack");

        speed = 0f;
        mainGameObject.GetComponent<Animator>().SetBool("damage", true);

        finishedDamaging = false;

        yield return new WaitForSeconds(2f);

        AudioManager.instance.Play("Veteran1");

        var raketa = Instantiate(Raketa);
        raketa.GetComponent<Raketa>().damage = damage;

        yield return new WaitForSeconds(1f);

        timeToRunBeforeDamaging = Random.Range(3f, 5f);

        stopDamaging();
    }

    public override void stopDamaging()
    {
        Debug.Log("Stopped Damaging!");

        StopCoroutine(Attack());

        finishedDamaging = true;

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

            Destroy(Weapon);

            mainGameObject.GetComponent<PartsOfEnemy>().Die();

            if (Impact) mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
            if (Blood) mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
            if (Stamp) mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp();

        }
    }
}
