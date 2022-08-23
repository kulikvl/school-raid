using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFizrukController : Enemy
{
    public RuntimeAnimatorController animatorRight;

    public override void OnTriggerStay2D(Collider2D collision)
    {
       
    }

    public override void DecreaseSpeed(float value)
    {
        StartCoroutine(IDecreaseSpeed());
    }

    IEnumerator IDecreaseSpeed()
    {
        IsFreezed = true;
        Debug.Log("decreased: " + gameObject.transform.parent.name);
        bool WasDamaging = IsDamaging;

        SpriteRenderer[] sprites = transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
        List<Color> InitialColors = new List<Color>();
        for (int i = 0; i < sprites.Length; ++i)
        {
            InitialColors.Add(sprites[i].color);
            sprites[i].color = new Color(0f, 1f, 1f);
        }

        if (IsDamaging)
        {
            ableToDamage = false;
            IsDamaging = false;
        }

        mainGameObject.GetComponent<Animator>().speed = 0f;

        yield return new WaitForSeconds(5f);

        IsFreezed = false;

        mainGameObject.GetComponent<Animator>().speed = 1f;

        if (WasDamaging)
        {
            ableToDamage = true;
            IsDamaging = true;
        }

        for (int i = 0; i < sprites.Length; ++i)
        {
            sprites[i].color = InitialColors[i];
        }
    }

    public void Start()
    {
        GameObject container = new GameObject("Fizruk Container");
        mainGameObject.transform.parent = container.transform;

        if (mainGameObject.name.Contains("Right"))
        {
            mainGameObject.GetComponent<Animator>().runtimeAnimatorController = animatorRight;
            container.transform.position = new Vector2(Random.Range(0f, -1f), 0f);
        }
        else
        {
            container.transform.position = new Vector2(Random.Range(0f, 1f), 0f);
        }

        AudioManager.instance.Play("LaughEnemyFizruk");
    }

    public override void winAnimation()
    {
        ableToDamage = false;

        speed = 0f;

        if (IsDamaging)
        {
            mainGameObject.GetComponent<Animator>().SetTrigger("win");
            IsDamaging = false;
        }
        else
        {
            mainGameObject.GetComponent<Animator>().enabled = false;
            mainGameObject.GetComponent<Animation>().Play();
        }
        
    }

    private float cd = 11f;
    private bool startedToDamage = false;

    public override void Update()
    {
        if (!IsFreezed)
            cd -= Time.deltaTime;

        if (cd <= 0.0f && !startedToDamage && !IsFreezed && !PlayerController.Lost && !PlayerController.Won)
        {
            IsDamaging = true;
            mainGameObject.GetComponent<Animator>().SetBool("damage", true);

            GameObject[] gos = GameObject.FindGameObjectsWithTag("school");
            foreach (GameObject go in gos)
                if (go.name == "Center")
                    TargetToDamage = go;

            startedToDamage = true;
        }

        ////////
        if (ableToRun) Run(speed);

        cooldown -= Time.deltaTime;

        if (cooldown <= 0.0f && IsDamaging && !IsDead && !IsFreezed && TargetToDamage != null)
        {
            cooldown = cooldownTime;

            IDamageable damageable = TargetToDamage.GetComponent<IDamageable>();
            damageable.takeDamage(damage);

            AudioManager.instance.Play("Punch" + Random.Range(1, 4).ToString() + 'f');

            shakeEffect();
        }
        else if (TargetToDamage == null && IsDamaging)
        {
            stopDamaging();
        }

    }

}
