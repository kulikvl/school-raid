using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Abilities3 : MonoBehaviour, IAbility
{
    [Header("Prefabs")]
    public GameObject bigTNTPrefab;
    public GameObject MagnetEffectParticles;
    public GameObject ReloadTimeEffectParticles;

    private BusController busController;
    private GameObject currentAbility;

    private void Start()
    {
        busController = GetComponent<BusController>();
    }

    public void ActivateAbility(Abilities _abilities, out bool succeed)
    {
        switch (_abilities)
        {
            case Abilities.Defense:

                succeed = true;

                Vector3 pos = new Vector3(busController.gameObject.transform.position.x + 0.0f, busController.gameObject.transform.position.y - 1.5f, 0f);
                var prefab = Instantiate(bigTNTPrefab, pos, transform.rotation);
                prefab.GetComponent<Rigidbody2D>().velocity = busController.gameObject.GetComponent<Rigidbody2D>().velocity / 2f;

                Debug.Log("Big TNT has been thown");

                break;
            case Abilities.Protect:

                if (magnetIsActivated)
                {
                    succeed = false;
                }
                else
                {
                    StayWithMagnet();
                    succeed = true;
                    Debug.Log("Magnet Activated");
                }

                break;
            case Abilities.Attack:

                if (speedEffectIsActivated)
                {
                    succeed = false;
                }
                else
                {
                    StayWithLowerReloadTime();
                    succeed = true;

                    Debug.Log("Lower Reload Time Activated");

                }
                break;
            default:
                succeed = false;
                break;
        }
    }

    private bool magnetIsActivated = false;
    private float cd;
    private GameObject currentMagnetEffect;

    public Collider2D[] colliders;

    private void Update()
    {
        if ((magnetIsActivated ) || (speedEffectIsActivated))
            cd -= Time.deltaTime;

        if (cd <= 0.0f && magnetIsActivated)
        {
            currentMagnetEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            currentMagnetEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
            magnetIsActivated = false;
        }

        if (cd <= 0.0f && speedEffectIsActivated)
        {
            currentReloadTimeEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            currentReloadTimeEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
            busController.ShootDelay += 0.5f;
            speedEffectIsActivated = false;
        }


        if (magnetIsActivated)
        {
            //magnet actions

            colliders = Physics2D.OverlapCircleAll(busController.gameObject.transform.position, 7f);
            GameObject[] coins = GameObject.FindGameObjectsWithTag("EnergyCoin");
            float speed = 3.5f;

            if (coins.Length > 0)
            foreach (GameObject coin in coins)
            {
                if (colliders.Contains(coin.GetComponent<Collider2D>()))
                {
                    coin.transform.position = Vector3.MoveTowards(coin.transform.position, busController.gameObject.transform.position, speed * Time.deltaTime);
                }
            }
        }
    }

    private void StayWithMagnet()
    {
        if (speedEffectIsActivated)
        {
            speedEffectIsActivated = false;
            cd = 0.0f;
            busController.ShootDelay += 0.5f;
            currentReloadTimeEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            currentReloadTimeEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
        }

        cd = 40f;
        magnetIsActivated = true;
        currentMagnetEffect = Instantiate(MagnetEffectParticles, transform);
    }

    private bool speedEffectIsActivated = false;
    private GameObject currentReloadTimeEffect;

    public void StayWithLowerReloadTime() // 20f
    {
        if (magnetIsActivated)
        {
            magnetIsActivated = false;
            cd = 0.0f;
            currentMagnetEffect.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            currentMagnetEffect.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
        }

        cd = 20f;
        speedEffectIsActivated = true;
        currentReloadTimeEffect = Instantiate(ReloadTimeEffectParticles, transform);

        busController.ShootDelay -= 0.5f;
    }
}
