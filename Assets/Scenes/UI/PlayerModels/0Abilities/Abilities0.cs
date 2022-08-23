using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities0 : MonoBehaviour, IAbility
{
    [Header("Prefabs")]
    public GameObject abilityConstructionPrefab;
    public GameObject ImmortalEffectPrefab;
    public GameObject SpeedEffectPrefab;

    private BusController busController;
    private GameObject currentAbility;

    private void Start()
    {
        busController = GetComponent<BusController>();
    }

    public void ActivateAbility(Abilities _abilities, out bool succeed)
    {
        succeed = false;

        switch (_abilities)
        {
            case Abilities.Defense:

                //////
                GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");
                if (gos != null)
                {
                    foreach (GameObject go in gos)
                    {
                        Enemy enemy = go.GetComponent<Enemy>();

                        if (enemy.IsDamaging && enemy.TargetToDamage.name.Contains("box"))
                        {
                            enemy.stopDamaging();

                        }
                    }
                }

                if (currentAbility != null) Destroy(currentAbility);

               

                RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, Vector2.down);

                for (int i = 0; i < allHits.Length; i++)
                {
                    GameObject target = allHits[i].transform.gameObject;

                    if (target.name.Contains("line"))
                    {
                        float distance = transform.position.y - allHits[i].point.y;
                        if (distance < 3f)
                        {
                            if (PlayerPrefs.GetInt("currentLevel") == 2 && !PlayerPrefs.HasKey("showedIntroSputnik2") && PlayerPrefs.GetString("currentMode") == "Campaign")
                            {
                                transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 3f - distance + 0.1f);
                                BusController.AbleToShoot = false;
                                currentAbility = Instantiate(abilityConstructionPrefab, transform);
                                currentAbility.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                                busController.Speed -= 1f;
                                Debug.Log("Defense Setted!");
                                succeed = true;
                                break;
                               
                            }
                            else
                            {
                                succeed = false;
                                Debug.Log("Distance is: " + distance);
                            }
                        }
                        else
                        {
                            succeed = true;

                            BusController.AbleToShoot = false;
                            currentAbility = Instantiate(abilityConstructionPrefab, transform);
                            currentAbility.transform.GetChild(0).GetComponent<HingeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                            busController.Speed -= 1f;
                            Debug.Log("Defense Setted!");
                        }

                        break;
                    }
                }

                break;
            case Abilities.Protect:

                StartCoroutine(StayImmortal());
                succeed = true;

                Debug.Log("Protection Activated");

                break;
            case Abilities.Attack:

                StartCoroutine(StayFaster());
                succeed = true;

                Debug.Log("Protection Activated");

                break;
            default:
                succeed = false;
                break;
        }
    }

    IEnumerator StayImmortal()
    {
        busController.CanBeDamaged = false;
        Instantiate(ImmortalEffectPrefab, transform);
        GetComponent<Animation>().Play("ImmortalAnim");
        yield return new WaitForSeconds(5f);
        busController.CanBeDamaged = true;
    }

    private GameObject speedEffectGO;
    private bool speedEffectIsActivated = false;

    IEnumerator StayFaster()
    {
        speedEffectIsActivated = true;

        speedEffectGO = Instantiate(SpeedEffectPrefab, transform);
        GetComponent<Animation>().Play("fasterAnim");
        busController.ShootDelay -= 0.5f;
        busController.Speed += 1.5f;

        yield return new WaitForSeconds(10f);

        speedEffectIsActivated = false;
        busController.Speed -= 1.5f;
        busController.ShootDelay += 0.5f;
    }

    private void Update()
    {
        if (speedEffectIsActivated)
        {
            float valueX = GetComponent<Rigidbody2D>().velocity.x;

            if (valueX >= 0) // moving right
            {
                for (int i = 0; i < 2; ++i)
                    speedEffectGO.transform.GetChild(1).gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startColor = new Color(1f, 1f, 1f, 1f);

                for (int i = 0; i < 2; ++i)
                    speedEffectGO.transform.GetChild(2).gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startColor = new Color(1f, 1f, 1f, 0f);
            }   
            else
            {
                for (int i = 0; i < 2; ++i)
                    speedEffectGO.transform.GetChild(1).gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startColor = new Color(1f, 1f, 1f, 0f);

                for (int i = 0; i < 2; ++i)
                    speedEffectGO.transform.GetChild(2).gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startColor = new Color(1f, 1f, 1f, 1f);
            }

        }
    }
}
