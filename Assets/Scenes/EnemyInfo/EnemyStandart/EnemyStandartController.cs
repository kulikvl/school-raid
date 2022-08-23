using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemyStandartController : Enemy
{
    public RuntimeAnimatorController animator1, animator2;
    public GameObject weapon1, weapon2;
    public BoxCollider2D colliderToChange;

    private GameObject currentWeapon = null;

    private void Start()
    {
        AudioManager.instance.Play("LaughEnemyStandart" + Random.Range(1, 3).ToString());
    }

    public override void Awake()
    {
        speed = Random.Range(0.8f, 1.2f);
        Animator currentAnimator = mainGameObject.GetComponent<Animator>();

        if (Random.Range(0, 2) == 0)
        {
            currentAnimator.runtimeAnimatorController = animator1;
            mainGameObject.GetComponent<PartsOfEnemy>().rbs[10] = weapon1.GetComponent<Rigidbody2D>();

            if (SceneManager.GetActiveScene().name != "Intro")
                Destroy(weapon2);
            currentWeapon = weapon1;

            colliderToChange.size = new Vector2(Random.Range(2f, 3f), GetComponent<BoxCollider2D>().size.y);
        }
        else
        {
            currentAnimator.runtimeAnimatorController = animator2;
            mainGameObject.GetComponent<PartsOfEnemy>().rbs[10] = weapon2.GetComponent<Rigidbody2D>();

            if (SceneManager.GetActiveScene().name != "Intro")
                Destroy(weapon1);

            currentWeapon = weapon2;

            colliderToChange.size = new Vector2(Random.Range(4.8f, 5f), GetComponent<BoxCollider2D>().size.y);
        }

        base.Awake();


        if (isMenuScene)
        {
            speed = 0f;
            IsDamaging = false;
            mainGameObject.GetComponent<Animator>().SetTrigger("win");
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

            AudioManager.instance.Play("Destroy" + Random.Range(3, 5).ToString());

            shakeEffect();
        }
        else if (TargetToDamage == null && IsDamaging)
        {
            stopDamaging();
        }
    }
}
