using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyDirectorController : Enemy
{
    
    public Transform pointForRising;
    public float distanceForRising;

    public GameObject director;
    public GameObject[] tankParts;
    public GameObject enemy1Prefab, enemy2Prefab;
    public GameObject particleSmall;
    public GameObject particleBig;
    public GameObject particleOnDestroyDirector;

    public float distance;
    public int lives;
    public Image fillImage;

    public GameObject particleOnSchool;

    public Transform pointForSmoke;
    public GameObject particleSmoke;

    private Vector3 pointOfDamage;
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

    public static EnemyDirectorController instance;

    public override void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //mainGameObject.GetComponent<tabManager>().deleteTab();
            Destroy(mainGameObject);
            return;
        }

        base.Awake();
    }
    private int initalLives;
    private void Start()
    {
        initalLives = lives;

        AudioManager.instance.Play("LaughEnemyDirector");

        if (SceneManager.GetActiveScene().name != "Intro")
        AudioManager.instance.Play("Director2");

        mainGameObject.transform.position = new Vector3(mainGameObject.transform.position.x - 0.8f, mainGameObject.transform.position.y + 0.3f);
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null && damageable.CanBeDamaged)
        {
            damageable.takeDamage(1f);
        }
    }

    protected override void Run(float _speed)
    {
        mainGameObject.transform.Translate(Vector2.right * Time.deltaTime * _speed);
    }
     
    public override void Update()
    {
        if (!IsDead)
        {
            fillImage.fillAmount = (float)lives / (float)initalLives;

            if (TargetToDamage == null)
            {
                RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, Vector2.right);

                if (allHits.Length > 0)
                {
                    for (int i = 0; i < allHits.Length; i++)
                    {
                        GameObject target = allHits[i].transform.gameObject;
                        float _distance = Mathf.Abs(allHits[i].point.x - transform.position.x);

                        if (target.CompareTag("school") && _distance <= distance)
                        {
                            ableToRun = false;
                            TargetToDamage = target;
                            IsDamaging = true;

                            pointOfDamage = allHits[i].point;

                            break;
                        }
                    }
                }
            }

            RaycastHit2D[] allHits2 = Physics2D.RaycastAll(pointForRising.position, Vector2.down);

            if (allHits2.Length > 0)
            {
                for (int i = 0; i < allHits2.Length; i++)
                {
                    GameObject target = allHits2[i].transform.gameObject;

                    if (target.name.Contains("line"))
                    {
                        distanceForRising = Mathf.Abs(pointForRising.position.y - allHits2[i].point.y);

                        if (distanceForRising > 0.3f)
                        {
                            speed = 2f;
                        }
                        else
                        {
                            speed = 0.3f;
                        }
                    }
                }
            }

            if (ableToRun) Run(speed);

            cooldown -= Time.deltaTime;

            if (IsDamaging && !IsDead && !IsFreezed && TargetToDamage != null && finishedDamaging && !PlayerController.Lost && !PlayerController.Won)
            {
                Debug.Log("attack");
                routine = Attack();
                StartCoroutine(routine);
            }
            else if (TargetToDamage == null && IsDamaging)
            {
                stopDamaging();
            }
        }
    }

    private bool finishedDamaging = true;

    IEnumerator Attack() // 1.5f
    {
        finishedDamaging = false;

        mainGameObject.GetComponent<Animator>().SetTrigger("damage");

        yield return new WaitForSeconds(0.25f);
        if (finishedDamaging) yield break;

        Instantiate(particleOnSchool, pointOfDamage, Quaternion.identity);
        Instantiate(particleSmoke, pointForSmoke.position, Quaternion.identity);

        IDamageable damageable = TargetToDamage.GetComponent<IDamageable>();
        shakeEffect();
        damageable.takeDamage(damage);

        AudioManager.instance.Play("Director3");

        yield return new WaitForSeconds(1.25f);
        if (finishedDamaging) yield break;

        finishedDamaging = true;
    }

    public IEnumerator routine;

    public override void stopDamaging()
    {
        Debug.Log("STOP!");

        StopCoroutine(routine);

        finishedDamaging = true;

        IsDamaging = false;

        ableToRun = true;

        mainGameObject.GetComponent<Animator>().Play("Run", -1, 0f);

        if (targetToDamage != null && targetToDamage.name == "Left" || targetToDamage.name == "Right")
        {
            distance = 5.5f;
        }

        targetToDamage = null;
    }

    public override void winAnimation()
    {
        AudioManager.instance.Stop("Director2");

        ableToDamage = false;

        AudioManager.instance.Play("Director1");

        if (IsDamaging) stopDamaging();

        ableToRun = false;
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
        if (allowedToHit)
        {
            --lives;

            Debug.Log("FFFFFF");
            allowedToHit = false;

            routineHitting = CoolDownBetweenHitting();
            StartCoroutine(routineHitting);

            if (lives <= 0 && !IsDead)
            {
                if (IsDamaging) stopDamaging();

                IsDead = true;

                // on destroy todo
                Destroy(fillImage.gameObject.transform.parent.gameObject);
                StartCoroutine(DieAnimation());

            }
        }
    }

    IEnumerator DieAnimation()
    {
        AudioManager.instance.Stop("Director2");
        mainGameObject.GetComponent<Animator>().SetTrigger("die");
        AudioManager.instance.Play("Director4");

        AudioManager.instance.Play("Explosion10");
        Instantiate(particleSmall, new Vector3(transform.position.x - 0.3f, transform.position.y), Quaternion.identity);

        yield return new WaitForSeconds(0.3f);

        AudioManager.instance.Play("Explosion11");
        Instantiate(particleSmall, new Vector3(transform.position.x + 0.4f, transform.position.y), Quaternion.identity);

        yield return new WaitForSeconds(0.3f);

        AudioManager.instance.Play("Explosion10");
        Instantiate(particleSmall, new Vector3(transform.position.x + 1.1f, transform.position.y), Quaternion.identity);

        yield return new WaitForSeconds(0.4f);
        // tank destroy
        AudioManager.instance.Play("Explosion1");
        Instantiate(particleBig, transform.position, Quaternion.identity);

        Instantiate(enemy1Prefab, transform.position, Quaternion.identity);
        Instantiate(enemy2Prefab, transform.position, Quaternion.identity);

        foreach (GameObject go in tankParts) Destroy(go);

        yield return new WaitForSeconds(1f);
        //director destroy

        director.GetComponent<Rigidbody2D>().isKinematic = true;
       // director.GetComponent<PartsOfEnemy>().Die();

        Instantiate(particleOnDestroyDirector, director.transform.position, Quaternion.identity);

        AudioManager.instance.Play("BloodSplash1");
        Destroy(director);
        Destroy(mainGameObject);
        
    }
}
