using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities4 : MonoBehaviour, IAbility
{
    [Header("Prefabs")]
    public GameObject mortirasPrefab;
    public GameObject rainOfBombsPrefab;

    public GameObject particleLeft, particleRight, particleCenter;

    public void ActivateAbility(Abilities _abilities, out bool succeed)
    {
        succeed = false;

        switch (_abilities)
        {
            case Abilities.Defense:

                Instantiate(mortirasPrefab);
                succeed = true;

                Debug.Log("Mortiras were set");

                break;
            case Abilities.Protect:

                Instantiate(rainOfBombsPrefab);
                succeed = true;

                Debug.Log("Rain of bombs!");

                break;
            case Abilities.Attack:

                GameObject[] schools = GameObject.FindGameObjectsWithTag("school");
                bool healed = false;

                if (schools.Length > 0)
                {
                    foreach(GameObject school in schools)
                    {
                        if (!school.GetComponent<School>().destroyed)
                        {
                            if (school.name == "Center" && PlayerController.centerHP < 1.0f)
                            {
                                PlayerController.centerHP = 1f;
                                Instantiate(particleCenter);
                                school.GetComponent<DefectManager>().StopAllFires();
                                healed = true;
                            }
                                
                            if (school.name == "Left" && PlayerController.leftHP < 1.0f)
                            {
                                PlayerController.leftHP = 1f;
                                Instantiate(particleLeft);
                                school.GetComponent<DefectManager>().StopAllFires();
                                healed = true;
                            }
                                
                            if (school.name == "Right" && PlayerController.rightHP < 1.0f)
                            {
                                PlayerController.rightHP = 1f;
                                Instantiate(particleRight);
                                school.GetComponent<DefectManager>().StopAllFires();
                                healed = true;
                            }
                        }
                    }
                }

                if (healed)
                {
                    succeed = true;
                    Debug.Log("School was healed!");
                }
                else
                {
                    succeed = false;
                    Debug.Log("Every school building has MAX Hp");
                }
                
                break;
            default:
                succeed = false;
                break;
        }
    }
}
