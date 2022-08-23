using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sputnik : MonoBehaviour
{
    public GameObject Coin;

    private float cooldownTimeFirstWay, cooldownTimeSecondWay;
    public float cooldownFirstWay, cooldownSecondWay;

    public bool readyToDrop1 = true;
    public bool readyToDrop2 = true;

    private void Start()
    {
        cooldownTimeFirstWay = Random.Range(1.5f, 3.5f);
        cooldownFirstWay = cooldownTimeFirstWay;

        cooldownTimeSecondWay = Random.Range(7.5f, 9.5f);
        cooldownSecondWay = cooldownTimeSecondWay;

        if (SelectItem.putniksSetted == 1)
        StartCoroutine(FirstWay());
        else
        {
            ToDropOrdinary = true;
        }
    }

    private bool ToDropOrdinary = true;
    public IEnumerator FirstWay() // 5f
    {
        ToDropOrdinary = false;

        yield return new WaitForSeconds(2f);
        DropCoin();
        yield return new WaitForSeconds(0.5f);
        DropCoin();
        yield return new WaitForSeconds(0.5f);
        DropCoin();

        yield return new WaitForSeconds(2f);
        ToDropOrdinary = true;
    }

    private void Update()
    {
        GetComponent<Animator>().speed = 1f;

        cooldownFirstWay -= Time.deltaTime;
        cooldownSecondWay -= Time.deltaTime;
           
        if (cooldownFirstWay <= 0.0f)
        {
            if (readyToDrop1)
            {
                cooldownTimeFirstWay = (12f - cooldownTimeFirstWay);
                cooldownFirstWay = cooldownTimeFirstWay;

                if (ToDropOrdinary)
                {
                    DropCoin();
                }

                readyToDrop1 = false;
            }
            else
            {
                cooldownTimeFirstWay = Random.Range(1.5f, 3.5f);
                cooldownFirstWay = cooldownTimeFirstWay;

                readyToDrop1 = true;
            }
        }

        if (cooldownSecondWay <= 0.0f)
        {
            if (readyToDrop2)
            {
                cooldownTimeSecondWay = (12f - cooldownTimeSecondWay);
                cooldownSecondWay = cooldownTimeSecondWay;

                if (SelectItem.putniksSetted == 3)
                {
                    if (Random.Range(0, 3) == 0)
                    DropCoin();
                }
                else if (SelectItem.putniksSetted == 2)
                {
                    if (Random.Range(0, 2) == 0)
                    DropCoin();
                }
                else
                {
                    DropCoin();
                }
               
                readyToDrop2 = false;
            }
            else
            {
                cooldownTimeSecondWay = Random.Range(7.5f, 9.5f);
                cooldownSecondWay = cooldownTimeSecondWay;

                readyToDrop2 = true;
            }
        }
    }

    private void DropCoin()
    {
        if (sputnikCount.count <= 25)
        Instantiate(Coin, transform.position, Quaternion.identity, transform.parent);
    }
}
