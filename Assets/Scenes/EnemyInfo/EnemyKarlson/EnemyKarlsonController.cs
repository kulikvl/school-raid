using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyKarlsonController : Enemy
{
    public GameObject particleOnDamage;
    public Transform pointForDamage;

    public GameObject balloon;

    public float speedY = 0.1f;
    private float initialSpeedY;

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

    private void Start()
    {
        initialSpeedY = speedY;
        mainGameObject.transform.position = new Vector3(transform.position.x, -2f);

        AudioManager.instance.Play("LaughEnemyKarlson");

        if (SceneManager.GetActiveScene().name == "Intro")
        {
            Debug.Log("CHANGE POS");
            mainGameObject.transform.localPosition = new Vector3(0f, 0.6f);
            //mainGameObject.transform.position = new Vector3(transform.position.x, -4f);
        }
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null && damageable.CanBeDamaged && ableToDamage && targetToDamage == null)
        {
            ableToRun = false;
            TargetToDamage = collision.gameObject;

            IsDamaging = true;
        }

    }

    protected override void Run(float _speed)
    {
        mainGameObject.transform.Translate(Vector2.right * Time.deltaTime * _speed);

        if (mainGameObject.transform.position.y > -4f)
            mainGameObject.transform.Translate(Vector2.down * Time.deltaTime * speedY);
        else
            mainGameObject.transform.position = new Vector3(mainGameObject.transform.position.x, -4f);
    }

    public override void Update()
    {
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
    IEnumerator Attack() // 1f
    {
        Debug.Log("damage!");

        ableToRun = false;

        mainGameObject.GetComponent<Animator>().SetTrigger("test");

        finishedDamaging = false;

        yield return new WaitForSeconds(0.3f);
        if (finishedDamaging) yield break;

        Instantiate(particleOnDamage, pointForDamage.position, Quaternion.identity);
        IDamageable damageable = TargetToDamage.GetComponent<IDamageable>();
        shakeEffect();
        damageable.takeDamage(damage);

        AudioManager.instance.Play("Punch" + Random.Range(8, 11).ToString());

        yield return new WaitForSeconds(0.7f);
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
        //speed = initialSpeed;
        ableToRun = true;

        mainGameObject.GetComponent<Animator>().Play("Run 1", -1, 0f);

        targetToDamage = null;
    }

    public override void winAnimation()
    {
        ableToDamage = false;

        if (isDamaging) stopDamaging();

        ableToRun = false;
        IsDamaging = false;
        mainGameObject.GetComponent<Animator>().SetTrigger("win");
    }

    public override void Die(bool Impact, bool Blood, bool Stamp)
    {

        if (IsDamaging) stopDamaging();

        mainGameObject.GetComponent<Rigidbody2D>().isKinematic = true;

        Destroy(balloon);

        mainGameObject.GetComponent<PartsOfEnemy>().Die();

        if (Impact) mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
        if (Blood) mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
        if (Stamp) mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp();

    }
}
