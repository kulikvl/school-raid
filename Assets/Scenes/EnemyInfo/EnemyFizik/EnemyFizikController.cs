using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyFizikController : Enemy
{
    public int lives;

    public GameObject particleEffectOnDisappear;

    public Transform pointForCollisions;

    private bool appearedFistTime;

    public GameObject portalPrefab;
    private GameObject currentPortal;

    public GameObject magicStick;

    public override void Awake()
    {
        base.Awake();

        if (isMenuScene)
        {
            speed = 0f;
            IsDamaging = false;
            mainGameObject.GetComponent<Animator>().SetTrigger("win");
        }
    }

    public void Start()
    {
        if (!isMenuScene)
        {
            pointForCollisions.gameObject.GetComponent<CheckCollision>().enabled = false;
            appearedFistTime = false;
            AudioManager.instance.Play("LaughEnemyFizik");
            SetPortal();
        }
    }

    public override void Update()
    {
        if (ableToRun) Run(speed);


        cooldown -= Time.deltaTime;

        if (cooldown <= 0.0f && IsDamaging && !IsDead && !IsFreezed && TargetToDamage != null && finishedDamaging && !PlayerController.Lost && !PlayerController.Won && ableToDamage)
        {
            cooldown = cooldownTime;          
            ///////
            StartCoroutine(Attack());

        }
        else if (TargetToDamage == null && IsDamaging)
        {
            stopDamaging();
        }

        if (!isMenuScene && !PlayerController.Lost && !PlayerController.Won)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(pointForCollisions.position, new Vector2(2f, 3f), 0f);

            foreach (Collider2D col in colliders)
            {
                if ((col.gameObject.CompareTag("Bomb") || col.gameObject.CompareTag("BUS")) && !portalAnimationIsPlaying && lives > 0)
                {
                    Debug.Log("portalSet");
                    SetPortal();
                }
            }
        }
        
    }

    public bool finishedDamaging = true;
    IEnumerator Attack()
    {
        finishedDamaging = false;

        AudioManager.instance.Play("Fizik");

        IDamageable damageable = TargetToDamage.GetComponent<IDamageable>();

        GameObject particleContainer = magicStick.transform.GetChild(0).gameObject;

        if (!particleContainer.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().isPlaying)
        particleContainer.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(0.5f);
        if (finishedDamaging) yield break;

        particleContainer.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        particleContainer.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
        particleContainer.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play();
        particleContainer.transform.GetChild(4).gameObject.GetComponent<ParticleSystem>().Play();
        particleContainer.transform.GetChild(5).gameObject.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(0.5f);
        if (finishedDamaging) yield break;

        shakeEffect();
        damageable.takeDamage(damage);

        yield return new WaitForSeconds(1f);
        if (finishedDamaging) yield break;

        shakeEffect();
        damageable.takeDamage(damage);

        yield return new WaitForSeconds(3f);

        finishedDamaging = true;
    }

    public override void stopDamaging()
    {
        Debug.Log("Stopped Damaging!");

        StopCoroutine(Attack());

        finishedDamaging = true;

        GameObject particleContainer = magicStick.transform.GetChild(0).gameObject;
        particleContainer.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        particleContainer.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
        particleContainer.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Stop();
        particleContainer.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Stop();
        particleContainer.transform.GetChild(4).gameObject.GetComponent<ParticleSystem>().Stop();
        particleContainer.transform.GetChild(5).gameObject.GetComponent<ParticleSystem>().Stop();

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
        if (!IsDead && !portalAnimationIsPlaying)
        {
            if (IsDamaging) stopDamaging();

            mainGameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            StopCoroutine(AppearInPortal());

            mainGameObject.GetComponent<PartsOfEnemy>().Die();

            if (Impact) mainGameObject.GetComponent<PartsOfEnemy>().ImpactAdd();
            if (Blood) mainGameObject.GetComponent<PartsOfEnemy>().RandomBloodCome();
            if (Stamp) mainGameObject.GetComponent<PartsOfEnemy>().MakeStamp();

        }
    }

    public bool portalAnimationIsPlaying = false;
    IEnumerator AppearInPortal() // 3 seconds
    {
        portalAnimationIsPlaying = true;

        Transform[] gos = mainGameObject.GetComponentsInChildren<Transform>();
        List<Transform> gosWithIgoreLayers = new List<Transform>();

        foreach (var go in gos)
        {
            if (go.gameObject.layer == LayerMask.NameToLayer("IgnorePlayerAndEnemyAndBomb"))
            {
                gosWithIgoreLayers.Add(go);
            }
            else if (go.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
            {
                go.gameObject.layer = LayerMask.NameToLayer("IgnorePlayerAndEnemyAndBomb");
            }
        }

        Speed = 0f;

        mainGameObject.GetComponent<Rigidbody2D>().isKinematic = true;

        mainGameObject.GetComponent<Animator>().SetTrigger("start");

        yield return new WaitForSeconds(2.5f);

        mainGameObject.GetComponent<Rigidbody2D>().isKinematic = false;

        yield return new WaitForSeconds(0.01f);

        if (mainGameObject.gameObject.name.Contains("Right"))
            mainGameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-150f, 50f));
        else
            mainGameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(150f, 50f));

        yield return new WaitForSeconds(0.5f);

        Speed = initialSpeed;

       // yield return new WaitForSeconds(0.5f);

        foreach (var go in gos)
        {
            if (go != null)
            go.gameObject.layer = LayerMask.NameToLayer("ENEMY");
        }
        foreach (var go in gosWithIgoreLayers)
        {
            if (go != null)
                go.gameObject.layer = LayerMask.NameToLayer("IgnorePlayerAndEnemyAndBomb");
        }

        portalAnimationIsPlaying = false;
    }

    private void SetPortal()
    {
        lives--;
        if (lives <= 0) pointForCollisions.gameObject.GetComponent<CheckCollision>().enabled = true;

        if (appearedFistTime)
        {
            if (IsDamaging) stopDamaging();

            mainGameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            StopCoroutine(AppearInPortal());

            Instantiate(particleEffectOnDisappear, transform.position, Quaternion.identity);
        }
        else appearedFistTime = true;

        if (currentPortal != null)
        Destroy(currentPortal);

        currentPortal = Instantiate(portalPrefab);

        AudioManager.instance.Play("Portal" + Random.Range(1, 3).ToString());
       
        Vector3 pos;

        if (!mainGameObject.name.Contains("Right")) // !... 
        {
            // spawns right

            if (Random.Range(0, 2) == 0)
            {
                pos = new Vector3(11.55f, -4.3f, 0f);
            }
                
            else
            {
                pos = new Vector3(8f, -4.6f, 0f);
            }

            mainGameObject.name = "EnemyFizikRight";
            mainGameObject.transform.rotation = new Quaternion(0, 180, 0, 0);

        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                pos = new Vector3(-9.865f, -4.3f, 0f);
            }

            else
            {
                pos = new Vector3(-6.25f, -4.6f, 0f);
            }

            mainGameObject.name = "EnemyFizik";
            mainGameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        currentPortal.transform.position = pos;

        if (mainGameObject.gameObject.name.Contains("Right"))
            mainGameObject.transform.position = new Vector2(pos.x - 3.4f, pos.y + 0.3f);
        else
            mainGameObject.transform.position = new Vector2(pos.x - 3.1f, pos.y + 0.3f);

        StartCoroutine(AppearInPortal());
    }

}
