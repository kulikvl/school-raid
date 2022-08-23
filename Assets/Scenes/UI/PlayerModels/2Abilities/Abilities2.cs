using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities2 : MonoBehaviour, IAbility
{
    [Header("Prefabs")]
    public GameObject abilityFreezeEveryOnePrefab;
    public GameObject healthRedParticles;
    public GameObject rewardParticles;

    public static bool HealthForKilling = false;
    public static bool AdditionalRewardOn = false;

    private BusController busController;
    private GameObject currentAbility;

    private void Start()
    {
        AdditionalRewardOn = false;
        HealthForKilling = false;
        busController = GetComponent<BusController>();
    }

    public void ActivateAbility(Abilities _abilities, out bool succeed)
    {
        succeed = false;
        if (currentAbility != null) Destroy(currentAbility);

        switch (_abilities)
        {
            case Abilities.Defense:

                currentAbility = Instantiate(abilityFreezeEveryOnePrefab);
                AudioManager.instance.Play("FreezeAll");

                GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");

                if (gos.Length > 0)
                foreach (GameObject go in gos)
                {
                    Enemy enemy = go.GetComponent<Enemy>();
                    enemy.DecreaseSpeed((enemy.Speed * 0.75f));
                }

                Debug.Log("FreezedEveryOne!");

                succeed = true;

                break;
            case Abilities.Protect:

                StartCoroutine(StaySelfHealing());
                succeed = true;

                Debug.Log("Self-Healing Activateed");

                break;
            case Abilities.Attack:

                cd = 20f;
                timerIsOver = false;
                StayWithAdditionalReward();
                succeed = true;

                Debug.Log("ENERGY COINS FOR ENEMIES!");

                break;
            default:
                succeed = false;
                break;
        }
    }

    private float cd;
    private bool timerIsOver;

    private void FixedUpdate()
    {
        if (!timerIsOver)
        cd -= Time.fixedDeltaTime;

        if (cd <= 0.0f && !timerIsOver)
        {
            AdditionalRewardOn = false;
            timerIsOver = true;
        }
    }
    private void StayWithAdditionalReward()
    {
        currentAbility = Instantiate(rewardParticles, transform);
        AdditionalRewardOn = true;
    }

    //IEnumerator IStayWithAdditionalReward()
    //{
    //    currentAbility = Instantiate(rewardParticles, transform);

    //    AdditionalRewardOn = true;
    //    yield return new WaitForSeconds(20f);
    //    AdditionalRewardOn = false;
    //}

    IEnumerator StaySelfHealing()
    {
        currentAbility = Instantiate(healthRedParticles, transform);

        HealthForKilling = true;
        yield return new WaitForSeconds(10f);
        HealthForKilling = false;
    }


}