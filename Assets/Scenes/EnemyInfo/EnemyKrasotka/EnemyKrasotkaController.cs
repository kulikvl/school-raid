using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKrasotkaController : Enemy
{
    public GameObject Weapon;
    public GameObject particleHeartsOnEnemies;
    public ParticleSystem particleHearts;

    private ParticleSystem particleContainer;
    private float timeToRunBeforeDamaging;

    public override void Awake()
    {
        base.Awake();

        particleContainer = Weapon.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        timeToRunBeforeDamaging = Random.Range(5f, 8f);
    }

    private void Start()
    {
        AudioManager.instance.Play("Krasotka" + Random.Range(1, 4).ToString());
    }

    public override void OnTriggerStay2D(Collider2D collision) // Boxes Case
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null && finishedDamaging)
        {
            speed = 0f;

            timeToRunBeforeDamaging = 0.0f;
        }
    }

    public override void Update()
    {
        if (ableToRun && finishedDamaging)
        {
            Run(speed);
            timeToRunBeforeDamaging -= Time.deltaTime;
        }

        if (timeToRunBeforeDamaging <= 0.0f && !IsDead && !IsFreezed && finishedDamaging && !PlayerController.Lost && !PlayerController.Won)
        {
            StartCoroutine(Attack());
        }

    }

    private void PowerEveryone()
    {
        GameObject[] logics = GameObject.FindGameObjectsWithTag("logic");

        bool isRight = mainGameObject.name.Contains("Right");

        if (logics.Length > 0)
        foreach (GameObject logic in logics)
        {
            Enemy enemy = logic.GetComponent<Enemy>();
            if (!enemy.IsDead && !enemy.IsDamageIncreased && enemy.gameObject.GetComponent<EnemyKrasotkaController>() == null)
            {
                if (isRight == enemy.gameObject.transform.parent.gameObject.name.Contains("Right"))
                {
                    enemy.IsDamageIncreased = true;

                    //Debug.Log("FFFFFFF");
                    var prefab = Instantiate(particleHeartsOnEnemies, enemy.CenterOfEnemy);
                    //prefab.GetComponent<FollowEnemy>().enemyToFollow = enemy.gameObject.transform.parent.gameObject;
                }
            }
        }
    }

    public bool finishedDamaging = true;
    IEnumerator Attack() // 6f
    {
        Debug.Log("stand");

        speed = 0f;
        mainGameObject.GetComponent<Animator>().SetBool("damage", true);

        finishedDamaging = false;

        PowerEveryone();

        AudioManager.instance.Play("Krasotka" + Random.Range(1, 4).ToString());

        particleContainer.Play();
        particleHearts.Play();

        yield return new WaitForSeconds(6f);

        timeToRunBeforeDamaging = Random.Range(3f, 5f);

        stopDamaging();
    }

    public override void stopDamaging()
    {
        Debug.Log("Stopped Damaging!");

        StopCoroutine(Attack());

        finishedDamaging = true;

        particleContainer.Stop();
        particleHearts.Stop();

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

            Destroy(particleHearts.gameObject);
            Destroy(Weapon);

            mainGameObject.GetComponent<PartsOfEnemy>().Die();

            if (Impact) mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
            if (Blood) mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
            if (Stamp) mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp();


        }
    }
}
