using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBigController : Enemy
{
    public GameObject Hips;
    public GameObject leftArm;
    public GameObject rightArm;

    [SerializeField] private SpriteRenderer idle, angry;

    public void SetAlpha(bool active, bool isIdle)
    {
        float num = (active) ? 1f : 0f;

        if (isIdle) idle.color = new Color(idle.color.r, idle.color.g, idle.color.b, num);
        else angry.color = new Color(angry.color.r, angry.color.g, angry.color.b, num);
    }

    public void setPartOfSchool(GameObject go)
    {
        TargetToDamage = go;
    }

    public override void Awake()
    {
        speed = Random.Range(0.5f, 1f);
        base.Awake(); 
    }

    private void Start()
    {
        AudioManager.instance.Play("LaughEnemyBig" + Random.Range(1, 3).ToString());
    }

    public override void winAnimation()
    {
        ableToDamage = false;

        Hips.GetComponent<ShootEnemy1>().enabled = false;
        leftArm.SetActive(true);
        rightArm.SetActive(true);

        mainGameObject.GetComponent<Animator>().enabled = true;
        mainGameObject.GetComponent<Animator>().speed = 1f;

        ableToDamage = false;

        ///

        base.winAnimation();
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
