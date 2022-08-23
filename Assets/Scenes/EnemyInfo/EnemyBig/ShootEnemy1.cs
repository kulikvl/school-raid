using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShootEnemy1 : MonoBehaviour
{
    public GameObject mainGameObject;
    public GameObject leftArm, rightArm;
    public GameObject minigun;

    [Space]

    public Collider2D[] collisionsToShow;

    public Enemy logic;

    private Quaternion startRotation;
    private Transform bus;
    private float radius = 4f;
    private float difference = 1.5f;

    public bool isFrozen = false;

    private EnemyBigController enemyController;

    private void Start()
    {
        GameObject _bus = GameObject.FindGameObjectWithTag("BUS");

        if (_bus != null)
            bus = _bus.transform;
        else
        {
            if (!PlayerController.Lost)
                Debug.LogError("BUS WAS NOT FOUND! ");
        }
            
        //////////

        enemyController = logic.GetComponent<EnemyBigController>();

        if (enemyController == null)
            Debug.LogError("LOGIC (EnemyBigController) WAS NOT FOUND! ");

        ///////////

        startRotation = transform.rotation; 
        difference = Random.Range(1f, 2f);
        radius = Random.Range(3f, 5f);

        ///////////
    }

    private Transform RequiredTarget() //  просто ищет обьект для уничтожения и в приоритете - автобус
    {
        Transform target = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(mainGameObject.transform.position, radius);
        collisionsToShow = colliders;

        if (!PlayerController.Lost && colliders.Contains(bus.gameObject.GetComponent<Collider2D>()) &&
            bus.gameObject.name == "BUS" && isCorrectDistanceWithBus())
        {
            return bus;
        }
        else
        {
            var distance = Mathf.Infinity;
            var playerPos = transform.position;

            foreach (var tar in colliders)
            {
                if (tar.gameObject.name == "Left" || tar.gameObject.name == "Center" || tar.gameObject.name == "Right")
                {
                        var diff = tar.gameObject.transform.position - playerPos;
                        var currDistance = diff.sqrMagnitude;

                        if (currDistance < distance)
                        {
                            target = tar.gameObject.transform;
                            distance = currDistance;
                        }
                }
            }

            return target;
        } 
    }

    private bool isCorrectDistance()
    {
        if (mainGameObject.name.Contains("Right"))
        {
            return (mainGameObject.transform.position.x - RequiredTarget().transform.position.x) > difference;
        }
        else
        {
            return RequiredTarget().transform.position.x - gameObject.transform.position.x > difference;
        }
    }

    private bool isCorrectDistanceWithBus()
    {
        if (mainGameObject.name.Contains("Right"))
        {
            return (mainGameObject.transform.position.x - bus.transform.position.x) > difference;
        }
        else
        {
            return bus.transform.position.x - gameObject.transform.position.x > difference;
        }
    }

    private void Update()
    {
        if (logic.IsDead == false && RequiredTarget() != null && PlayerController.Lost == false && !isFrozen && !logic.IsFreezed)
        {
            if (isCorrectDistance())
            {
                // ЕСЛИ КОГОТО ДАМАЖИТ
                

                enemyController.speed = 0f;

                enemyController.SetAlpha(false, true);
                enemyController.SetAlpha(true, false);

                if (RequiredTarget().name == "BUS")
                {
                    enemyController.IsDamaging = true;
                }
                else
                {
                    enemyController.IsDamaging = true;
                    enemyController.setPartOfSchool(RequiredTarget().gameObject);
                }


                mainGameObject.GetComponent<Animator>().enabled = false;
                mainGameObject.GetComponent<Animation>().Play();

                ////////// СТАВИМ РУКИ В СТАТИЧЕСКОЕ ПОЛОЖЕНИЕ
                rightArm.transform.position = new Vector3(-1.482f, 1.4f, 0f);
                leftArm.transform.position = new Vector3(1.2f, 1.3f, 0f);

                if (!logic.IsDead && PlayerController.Lost == false)
                {
                    rightArm.SetActive(false);
                    leftArm.SetActive(false);
                }

                if (!logic.IsDead && PlayerController.Lost == false && !isFrozen && !logic.IsFreezed)
                {
                    if (mainGameObject.name.Contains("Right"))
                    {
                        Vector2 direction = transform.position - RequiredTarget().position;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 180, 90 - angle);
                        minigun.GetComponent<Minigun>().Shoot();
                    }
                    else
                    {
                        Vector2 direction = RequiredTarget().position - transform.position;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                        transform.rotation = rotation;
                        minigun.GetComponent<Minigun>().Shoot();
                    }
                }
            }

            else // ЕСЛИ НЕ ДАМАЖИТ
            {
                ifNotDamaging();
            }     
        }
        else // ЕСЛИ НЕТ ВРАГА
        {
            ifNotDamaging();
        }
    }

    private void ifNotDamaging()
    {

        if (!PlayerController.Lost && !logic.IsDead && !isFrozen && !logic.IsFreezed)
        {
            enemyController.SetAlpha(true, true);
            enemyController.SetAlpha(false, false);

            enemyController.IsDamaging = false;
            enemyController.speed = logic.initialSpeed;
            mainGameObject.GetComponent<Animator>().enabled = true;
            
        }
        else
        {
            enemyController.SetAlpha(true, true);
            enemyController.SetAlpha(false, false);

            enemyController.speed = 0f;
            mainGameObject.GetComponent<Animator>().enabled = false;
        }

        if (!logic.IsDead)
        {
            rightArm.SetActive(true);
            leftArm.SetActive(true);
            transform.rotation = startRotation;
        }
    }

    public void Freeze(bool freeze)
    {
        isFrozen = freeze;
    }
}
