using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySchoolBoyFatController : Enemy
{
    public GameObject schoolBoyBlood;
    public GameObject schoolExplosion;

    public override void Update()
    {
        if (ableToRun)
        {
            Run(speed);
        }
    }

    public override void Awake()
    {
        speed = Random.Range(1.4f, 1.6f);
        base.Awake();
    }

    private void Start()
    {
        AudioManager.instance.Play("LaughSchoolBoyFat");
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null && damageable.CanBeDamaged)
        {
            IsDamaging = true;

            speed = 0f;
            mainGameObject.GetComponent<Animator>().SetTrigger("explode");
            TargetToDamage = collision.gameObject;

            Explode();
        }
    }

    private void Explode()
    {
        StartCoroutine(IExplode());
    }

    public override void Die(bool Impact, bool Blood, bool Stamp)
    {
        //DOES NOT NEED PARAMETERS

        if (!IsDead)
        {
            Instantiate(schoolBoyBlood, transform.position, Quaternion.identity);

            mainGameObject.GetComponent<tabManager>().deleteTab();

            IsDead = true;


            Destroy(mainGameObject);
        }
    }

    IEnumerator IExplode()
    {
        mainGameObject.GetComponent<Animator>().SetTrigger("explode");

        yield return new WaitForSeconds(0.5f);

        IDamageable damageable = TargetToDamage.GetComponent<IDamageable>();
        damageable.takeDamage(damage);

        shakeEffect();

        Boom();

        AudioManager.instance.Play("Explosion1");

        Die(false, false, false);
    }

    public void Boom()
    {
        Instantiate(schoolExplosion, transform.position, Quaternion.identity);
    }

}
