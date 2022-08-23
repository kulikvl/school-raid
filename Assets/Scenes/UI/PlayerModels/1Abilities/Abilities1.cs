using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities1 : MonoBehaviour, IAbility
{
    [Header("Prefabs")]
    public GameObject abilityTimberPrefab;
    public GameObject healthParticles;
    public GameObject thorns;

    private BusController busController;
    private GameObject currentAbility;

    private void Start()
    {
        busController = GetComponent<BusController>();
        timerIsOver = true;
    }

    public void ActivateAbility(Abilities _abilities, out bool succeed)
    {
        succeed = false;

        if (currentAbility != null && currentAbility.name.Contains("Timber"))
        {
            currentAbility.transform.GetChild(2).gameObject.GetComponent<TimberDetect>().DeleteObjects();
        }

        if (currentAbility != null) Destroy(currentAbility);

        switch (_abilities)
        {
            case Abilities.Defense:

                RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, Vector2.down);

                for (int i = 0; i < allHits.Length; i++)
                {
                    GameObject target = allHits[i].transform.gameObject;

                    if (target.name.Contains("line"))
                    {
                        float distance = transform.position.y - allHits[i].point.y;
                        if (distance < 2f) 
                        {
                            succeed = false;
                            Debug.Log("Distance is: " + distance);
                        }
                        else
                        {
                            succeed = true;

                            BusController.AbleToShoot = false;
                            currentAbility = Instantiate(abilityTimberPrefab, transform);
                            currentAbility.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                            currentAbility.transform.GetChild(1).GetComponent<HingeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                            busController.Speed -= 1f;
                            Debug.Log("Timber Setted!");
                        }

                        break;
                    }
                }

                break;
            case Abilities.Protect:

                StayWithAdditionalHealth();

                succeed = true;

                Debug.Log("Health+ Activated");

                break;
            case Abilities.Attack:

                StartCoroutine(SchoolImmortal());
                succeed = true;

                Debug.Log("School Protection Activated");

                break;
            default:
                succeed = false;
                break;
        }
    }

    private float cd;
    private bool timerIsOver = true;

    private void FixedUpdate()
    {
        if (!timerIsOver)
        cd -= Time.fixedDeltaTime;

        if (cd <= 0.0f && !timerIsOver)
        {
            if (busController.Health > busController.MaxHealth) busController.Health = busController.MaxHealth;
            timerIsOver = true;
        }
    }
    private void StayWithAdditionalHealth()
    {
        cd = 20f;
        timerIsOver = false;

        busController.Health += 300f;

        Instantiate(healthParticles, transform); 
    }

    IEnumerator SchoolImmortal()
    {
        GameObject[] schools = GameObject.FindGameObjectsWithTag("school");
        currentAbility = Instantiate(thorns);

        checkForCollisionsWithSchool = true;
        foreach (GameObject school in schools)
        {
            if (school.name == "Left")
                currentAbility.transform.GetChild(0).gameObject.SetActive(true);
            if (school.name == "Right")
                currentAbility.transform.GetChild(1).gameObject.SetActive(true);
            if (school.name == "Center")
                currentAbility.transform.GetChild(2).gameObject.SetActive(true);

            //school.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3f, 0.3f);
        }

        yield return new WaitForSeconds(5f);

        checkForCollisionsWithSchool = false;

        //foreach (GameObject school in schools)
        //{
        //    //school.GetComponent<SpriteRenderer>().color = Color.white;
        //}

        Destroy(currentAbility);
    }

    private bool checkForCollisionsWithSchool = false;
    private void Update()
    {
        if (checkForCollisionsWithSchool)
        {
            GameObject[] enemieslogic = GameObject.FindGameObjectsWithTag("logic");
            foreach (GameObject logic in enemieslogic)
            {
                Enemy enemy = logic.GetComponent<Enemy>();

                if (enemy.IsDamaging && enemy.TargetToDamage.CompareTag("school") && !enemy.IsDead) // if damaging school
                    enemy.Die(true, true, false);
            }
        }
    }
}
