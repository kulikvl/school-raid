using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarsCounting : MonoBehaviour
{
    public int starscount;

    public string NameStarsLevel;

    public static int SelectedLevel; //todo

    public GameObject RT, RCT;

    public GameObject AnimationCoins, PlusCoins;

    public GameObject Star1, Star2, Star3;

    private void Awake()
    {
        NameStarsLevel = "S" + PlayerPrefs.GetInt("currentLevel");
    }

    private void Start()
    {
        StartCoroutine(TurnOn());

        CheckStars();
    }

    public void CheckStars()
    {
        if (starscount > PlayerPrefs.GetInt(NameStarsLevel))
        {
            switch (starscount)
            {
                case 1: // 30
                    if (PlayerPrefs.GetInt(NameStarsLevel) == 0)
                    {
                        GiveReward(30, true);
                    }
                    break;


                case 2: // 50
                    if (PlayerPrefs.GetInt(NameStarsLevel) == 0)
                    {
                        GiveReward(50, true);
                    }
                    else if (PlayerPrefs.GetInt(NameStarsLevel) == 1)
                    {
                        GiveReward(20, true);
                    }
                    break;


                case 3: // 100
                    if (PlayerPrefs.GetInt(NameStarsLevel) == 1)
                    {
                        GiveReward(70, true);
                    }
                    else if (PlayerPrefs.GetInt(NameStarsLevel) == 2)
                    {
                        GiveReward(50, true);
                    }
                    else if (PlayerPrefs.GetInt(NameStarsLevel) == 0)
                    {
                        GiveReward(100, true);
                    }
                    break;
            }

            PlayerPrefs.SetInt(NameStarsLevel, starscount);
        }
        else
        {
            GiveReward(0, false);
        }
    }

    public void GiveReward(int coinsReward, bool Give)
    {
        if (Give)
        {
            RT.GetComponent<TextMeshProUGUI>().text = "REWARD + " + coinsReward;
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + coinsReward);
            AnimationCoins.GetComponent<Animation>().Play();
            PlusCoins.GetComponent<TextMeshProUGUI>().text = "+ " + coinsReward;
        }
        else
        {
            RT.SetActive(false);
            RCT.SetActive(true); 
        }
    }

    IEnumerator TurnOn()
    {
        yield return new WaitForSeconds(0.5f);

        ///////////// IF 1 STAR  /////////////

        Star1.SetActive(true);

        ///////////// IF 2 STAR  /////////////

        yield return new WaitForSeconds(0.5f);
        if (starscount >= 2)
        {
            Star2.SetActive(true);
        }

        ///////////// IF 3 STAR  /////////////

        yield return new WaitForSeconds(0.5f);
        if (starscount == 3)
        {
            Star3.SetActive(true);
        }
    }
}
