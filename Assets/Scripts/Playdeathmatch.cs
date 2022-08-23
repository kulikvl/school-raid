using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Playdeathmatch : MonoBehaviour
{
    private GameObject shop, modes, menuCoins;
    public GameObject textUnlocks;

    private void Start()
    {
        if (PlayerPrefs.GetInt("unlockedLevels") >= 7)
        {
            modes = GameObject.FindGameObjectWithTag("Mode");
            shop = GameObject.FindGameObjectWithTag("Shop");
            menuCoins = GameObject.FindGameObjectWithTag("MenuCoins");
            textUnlocks.SetActive(false);
        }
        else
        {
            textUnlocks.SetActive(true);
            textUnlocks.GetComponent<TextMeshProUGUI>().text = "CAMPAIGN LEVEL " + PlayerPrefs.GetInt("unlockedLevels") + "/7";
            gameObject.SetActive(false);
        }
       
    }
    private void OnMouseUpAsButton()
    {

        PlayerPrefs.SetString("currentMode", "Deathmatch");

        modes.GetComponent<Animation>().Play("ShopSwipe2");
        shop.GetComponent<Animation>().Play("ShopSwipe1");
        menuCoins.GetComponent<Animation>().Play();
    }
}
