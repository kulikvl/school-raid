using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyKachokController : Enemy
{
    public GameObject particleOnDamage;
    public Transform pointForDamage;
    public Image fillImage;

    public int lives;

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
        base.Awake();

        if (isMenuScene)
        {
            speed = 0f;
            IsDamaging = false;
            mainGameObject.GetComponent<Animator>().SetTrigger("menu");

        }
    }

    private void Start()
    {
        if (!isMenuScene)
        {
            AudioManager.instance.Play("LaughEnemyKachok");
        }
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            fillImage.gameObject.transform.parent.gameObject.SetActive(false);
        }
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
        if (!isMenuScene)
        fillImage.fillAmount = lives / 3.0f;

        if (ableToRun) Run(speed);

        if (IsDamaging && !IsDead && !IsFreezed && TargetToDamage != null && finishedDamaging && !PlayerController.Lost && !PlayerController.Won)
        {
            routine = Attack();
            StartCoroutine(routine);
        }
        else if (TargetToDamage == null && IsDamaging)
        {
            stopDamaging();
        }
    }

    public bool finishedDamaging = true;
    IEnumerator Attack() // 1.5f
    {
        Debug.Log("damage!");

        speed = 0f;
        mainGameObject.GetComponent<Animator>().SetTrigger("test");
        //mainGameObject.GetComponent<Animator>().SetBool("damage", true);

        finishedDamaging = false;

        yield return new WaitForSeconds(0.5f);
        if (finishedDamaging) yield break;

        Instantiate(particleOnDamage, pointForDamage.position, Quaternion.identity);
        IDamageable damageable = TargetToDamage.GetComponent<IDamageable>();
        shakeEffect();
        damageable.takeDamage(damage);

        AudioManager.instance.Play("Punch" + Random.Range(3, 5).ToString());

        yield return new WaitForSeconds(1f);
        if (finishedDamaging) yield break;

        finishedDamaging = true;
    }

    public IEnumerator routine;

    public override void stopDamaging()
    {
        Debug.Log("Stopped Damaging!");

        StopCoroutine(routine);

        finishedDamaging = true;

        IsDamaging = false;
        speed = initialSpeed;

        mainGameObject.GetComponent<Animator>().Play("Run", -1, 0f);

        targetToDamage = null;
        //mainGameObject.GetComponent<Animator>().SetBool("damage", false);
    }

    public override void winAnimation()
    {
        ableToDamage = false;

        if (isDamaging) stopDamaging();

        speed = 0f;
        IsDamaging = false;
        mainGameObject.GetComponent<Animator>().SetTrigger("win");
    }

    private bool allowedToHit = true;
    private IEnumerator routineHitting;
    IEnumerator CoolDownBetweenHitting()
    {
        yield return new WaitForSeconds(0.5f);
        allowedToHit = true;
    }

    public override void Die(bool Impact, bool Blood, bool Stamp)
    {
        if (!isMenuScene)
        {
            if (allowedToHit)
            {
                --lives;

                allowedToHit = false;

                routineHitting = CoolDownBetweenHitting();
                StartCoroutine(routineHitting);

                if (lives <= 0 && !IsDead)
                {
                    if (IsDamaging) stopDamaging();

                    mainGameObject.GetComponent<Rigidbody2D>().isKinematic = true;

                    Destroy(fillImage.gameObject.transform.parent.gameObject);

                    mainGameObject.GetComponent<PartsOfEnemy>().Die();

                    if (Impact) mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
                    if (Blood) mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
                    if (Stamp) mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp();
                    PlayerPrefs.SetInt("MissionFirstValue", PlayerPrefs.GetInt("MissionFirstValue") + 1);
                }
            }
        }
        else
        {
            mainGameObject.GetComponent<Rigidbody2D>().isKinematic = true;

            mainGameObject.GetComponent<PartsOfEnemy>().Die();

            if (Impact) mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
            if (Blood) mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
            if (Stamp) mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp();


        }
        
    }

    public void DieAnyway()
    {
        if (IsDamaging) stopDamaging();
        mainGameObject.GetComponent<Rigidbody2D>().isKinematic = true;

        Destroy(fillImage.gameObject.transform.parent.gameObject);

        mainGameObject.GetComponent<PartsOfEnemy>().Die();

        mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
        mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
        mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp();


    }
}
