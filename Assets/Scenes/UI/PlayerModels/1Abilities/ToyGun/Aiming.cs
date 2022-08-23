using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    public float speed = 3f;
    private bool isAiming = false;

    public GameObject target;
    private GameObject targetCenter;

    public GameObject AimTargetPrefab;
    private GameObject currentAimTarget;

    private float startPointX;

    public void Aim()
    {
        if (!isAiming)
        {
            startPointX = transform.position.x;
            speed = Random.Range(1f, 3f);
            isAiming = true;

            target = getClosestEnemy();

            if (target != null)
            {
                currentAimTarget = Instantiate(AimTargetPrefab, targetCenter.transform.position, Quaternion.identity, targetCenter.transform);
            }
                
        }
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            if (!target.transform.GetChild(0).gameObject.GetComponent<Enemy>().IsDead)
                target.name = nameBefore;

            Destroy(currentAimTarget);
        }
        
    }

    private string nameBefore;

    private GameObject getClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closest = null;

        float minDistance = Mathf.Infinity;
        float currentDistnace;

        if (enemies.Length != 0)
        {
            for (int i = 0; i < enemies.Length; ++i)
            {
                currentDistnace = Vector2.Distance(enemies[i].transform.position, transform.position);

                if (currentDistnace < minDistance && !enemies[i].name.Contains("TARGETED") && !enemies[i].transform.GetChild(0).gameObject.GetComponent<Enemy>().IsDead)
                {
                    minDistance = currentDistnace;
                    closest = enemies[i];
                }
            }
        }

        if (closest != null)
        {
            nameBefore = closest.name;
            closest.name += "TARGETED";

            CheckCollision checkCol = closest.GetComponentInChildren<CheckCollision>();
            targetCenter = checkCol.gameObject;
        }
            

        return closest;
    }

    public float vel;

    private void Update()
    {
        if (isAiming)
        {
            //if (currentAimTarget != null && target != null)
            //    currentAimTarget.transform.position = targetCenter.transform.position;


            if (target != null)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0f;
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            }
            else
            {
                GetComponent<Rigidbody2D>().gravityScale = 0.2f;
            } 
        }


        if (isAiming)
        {
            if (name.Contains("BlueBird"))
            {
                if (startPointX > transform.position.x)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 180f);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f);
                }
            }

            if (name.Contains("Chicken"))
            {
                if (startPointX < transform.position.x)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 180f);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f);
                }
            }
        }
        
    }
}
