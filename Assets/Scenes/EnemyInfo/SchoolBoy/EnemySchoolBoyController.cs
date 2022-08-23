using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySchoolBoyController : Enemy
{
    [SerializeField] private GameObject schoolBoyBlood;
    [SerializeField] private GameObject backPackToThow;

    public bool isMenu = false;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        AudioManager.instance.Play("LaughSchoolBoy" + Random.Range(1, 3).ToString());
    }

    public override void Die(bool Impact, bool Blood, bool Stamp)
    {
        //DOES NOT NEED PARAMETERS

        if (!IsDead)
        {
            Instantiate(schoolBoyBlood, transform.position, Quaternion.identity);

            var pref = Instantiate(backPackToThow, transform.position, Quaternion.identity);
            pref.GetComponent<Rigidbody2D>().AddTorque(20f);

            if (Random.Range(0, 2) == 0)
                pref.GetComponent<Rigidbody2D>().AddForce(new Vector2(-200f, 400f));
            else
                pref.GetComponent<Rigidbody2D>().AddForce(new Vector2(200f, 400f));


            if (!isMenu) mainGameObject.GetComponent<tabManager>().deleteTab();

            IsDead = true;

            if (TutorialSputnik.instance.isReadyToKill && mainGameObject.name.Contains("FINISHER"))
                TutorialSputnik.instance.AfterKilling();

            Destroy(mainGameObject);
        }
    }

    public override void Update()
    {
        if (ableToRun) Run(speed);

        cooldown -= Time.deltaTime;

        if (cooldown <= 0.0f && IsDamaging && !IsDead && !IsFreezed && TargetToDamage != null && !PlayerController.Lost && !PlayerController.Won)
        {
            cooldown = cooldownTime;

            IDamageable damageable = TargetToDamage.GetComponent<IDamageable>();
            damageable.takeDamage(damage);

            AudioManager.instance.Play("Impact" + Random.Range(1, 3).ToString());

            shakeEffect();
        }
        else if (TargetToDamage == null && IsDamaging)
        {
            stopDamaging();
        }
    }
}
