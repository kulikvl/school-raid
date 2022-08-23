using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sputnikCount : MonoBehaviour
{
    public TextMeshProUGUI TEXT;
    public static int count;

    public float cooldown;

    private void Awake()
    {
        cooldown = 10f;

        if ((PlayerPrefs.GetInt("currentLevel") == 1 && PlayerPrefs.GetString("currentMode") == "Campaign")
            || PlayerPrefs.GetString("currentMode") == "Deathmatch")
            count = 10000;
        else
            count = 5;
    }

    private void Update()
    {
        TEXT.text = count.ToString();

        cooldown -= Time.deltaTime;

        if (cooldown <= 0.0f)
        {
            cooldown = 10f;

            if (count < 5)
            {
                transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
                transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                count++;
            }
        }
    }
}
