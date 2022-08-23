using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class missionCompleted : MonoBehaviour
{
    public GameObject misCompleted;
    public GameObject textCoins;

    public bool allowedToPlay;

    public int coins;

    public int inst;

    public void Update()
    {
        inst = PlayerPrefs.GetInt("Coins");
    }

    public void Start()
    {
        if (allowedToPlay == true)
        {
            if (PlayerController.isMissionAnimationPlaying == false)
            {
                misCompleted.SetActive(true);
                textCoins.GetComponent<TextMeshProUGUI>().text = "+" + coins;
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + coins);

                StartCoroutine(ToSetOff());
            }
            else
            {
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + coins);
                Destroy(gameObject);
            }
        } 
    }

    //public void CompleteMission(int coins)
    //{
    //    misCompleted.SetActive(true);
    //    textCoins.GetComponent<TextMeshProUGUI>().text = "+" + coins;
    //    PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + coins);

    //    //StartCoroutine(ToSetOff());
    //}

    IEnumerator ToSetOff()
    {
        PlayerController.isMissionAnimationPlaying = true;
        yield return new WaitForSeconds(4f);
        PlayerController.isMissionAnimationPlaying = false;

        Destroy(gameObject);
    }
}
