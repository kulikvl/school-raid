using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuyChest : MonoBehaviour
{
    private GameObject chestOpeningWindow;
    public static bool VipOpened;

    public string ChestName;
    public TextMeshProUGUI description;

    public Sprite OpenSprite, BuySprite;

    public GameObject OpenText, BuyText;

    public static bool isReadyToWatchAd;
    public static bool agreedToWatchAd;

    public void Start()
    {
        agreedToWatchAd = false;

        VipOpened = false;
        chestOpeningWindow = GameObject.FindGameObjectWithTag("ChestOpeningWindow");

        if (PlayerPrefs.GetInt(ChestName) > 0)
        {
            Debug.Log(ChestName + "true");
            ChangeButton(true);
        }
        else
        {
            Debug.Log(ChestName + "false");
            ChangeButton(false);
        }
    }

    public void ChangeButton(bool changeToOpen)
    {
        if (changeToOpen)
        {
            GetComponent<Image>().sprite = OpenSprite;

            int count = PlayerPrefs.GetInt(ChestName);

            if (count == 1)
            {
                description.text = "x" + PlayerPrefs.GetInt(ChestName) + " CHEST";
            }
            else
            {
                description.text = "x" + PlayerPrefs.GetInt(ChestName) + " CHESTS";
            }

            GetComponent<Collider2D>().enabled = true;

            OpenText.SetActive(true);
            BuyText.SetActive(false);

            bool isVip = ChestName.Contains("Vip");

            if (!isVip)
            {
                transform.GetChild(2).gameObject.SetActive(false);
            }

        }
        else
        {
            bool isVip = ChestName.Contains("Vip");
            GetComponent<Image>().sprite = BuySprite;

            if (!isVip)
            {
                BuyText.SetActive(true);
                description.text = "WIN COINS OR UPGRADES";
            }
            else
            {
                BuyText.SetActive(true);
                description.text = "WIN BUSES OR ALTERATIONS";
            }

            OpenText.SetActive(false);

            if (!isVip)
            {
                TimeSpan difference = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastWatchedAd"));
                int minutes = difference.Minutes;

                //int hours = difference.Seconds;
                if (minutes >= 30)
                {
                    isReadyToWatchAd = true;
                    transform.GetChild(2).gameObject.SetActive(true);
                    BuyText.SetActive(false);
                    GetComponent<Collider2D>().enabled = true;
                }
                else
                {
                    GetComponent<Collider2D>().enabled = false;
                    isReadyToWatchAd = false;
                    transform.GetChild(2).gameObject.SetActive(false);
                    BuyText.SetActive(true);
                }
            }    
        }
    }

    public void AfterWatchingAd()
    {
        PlayerPrefs.SetString("LastWatchedAd", DateTime.Now.ToString());
        PlayerPrefs.SetInt(ChestName, PlayerPrefs.GetInt(ChestName) + 1);

        ChangeButton(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        if (Buttons.AbleToClick && !UpgradeButton.currentPageIsUpgrade && AbilitiesButton.IsAllowedToScroll)
        {
            // buy
            bool isVip = (ChestName == "VipChestCount");

            if (PlayerPrefs.GetInt(ChestName) <= 0 && !isVip) 
            {
                if (isReadyToWatchAd)
                {
                    agreedToWatchAd = true;
                    AudioManager.instance.Play("BtnBuy");
                    AdManager.instance.PlayRewardedVideoAd();
                }
            }
            else if (isVip)
            {
                if (PlayerPrefs.GetInt(ChestName) <= 0)
                {
                    AudioManager.instance.Play("Btn1");

                    GameObject go = GameObject.FindGameObjectWithTag("CANVASBUYVIP");
                    go.transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    PlayerPrefs.SetInt(ChestName, PlayerPrefs.GetInt(ChestName) - 1);

                    if (PlayerPrefs.GetInt(ChestName) <= 0)
                    {
                        ChangeButton(false);
                    }
                    else
                    {
                        ChangeButton(true);
                    }

                    VipOpened = true;
                    AbilitiesButton.IsAllowedToScroll = false;
                    SetButtonCollidersShop(false);
                    chestOpeningWindow.transform.GetChild(0).gameObject.SetActive(true);

                    AudioManager.instance.Play("BtnChest");
                }
            }
            // open
            else
            {
                PlayerPrefs.SetInt(ChestName, PlayerPrefs.GetInt(ChestName) - 1);

                PlayerPrefs.SetInt("chestsOpened", PlayerPrefs.GetInt("chestsOpened") + 1);
                if (PlayerPrefs.GetInt("chestsOpened") == 1)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Trader1);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Trader1);
                }
                else if (PlayerPrefs.GetInt("chestsOpened") == 5)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Trader2);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Trader2);
                }
                else if (PlayerPrefs.GetInt("chestsOpened") == 25)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.UltimateTrader);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.UltimateTrader);
                }
                
                if (PlayerPrefs.GetInt(ChestName) <= 0)
                {
                    ChangeButton(false);
                }
                else
                {
                    ChangeButton(true);
                }

                VipOpened = (ChestName == "VipChestCount") ? true : false; // todo
                AbilitiesButton.IsAllowedToScroll = false;
                SetButtonCollidersShop(false);
                chestOpeningWindow.transform.GetChild(0).gameObject.SetActive(true);

                AudioManager.instance.Play("BtnChest");
            }
        }
    }

    private void SetButtonCollidersShop(bool param)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("BuyButton");
        foreach (GameObject go in gos)
        {
            go.GetComponent<BoxCollider2D>().enabled = param;
        }

        gos = GameObject.FindGameObjectsWithTag("BuyChest");
        foreach (GameObject go in gos)
        {
            go.GetComponent<BoxCollider2D>().enabled = param;
        }

        gos = GameObject.FindGameObjectsWithTag("UpgradeButton");
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<UpgradeButton>().isClickable)
                go.GetComponent<BoxCollider2D>().enabled = param;
        }

        gos = GameObject.FindGameObjectsWithTag("AbilitiesButtonShop");
        foreach (GameObject go in gos)
        {
            go.GetComponent<BoxCollider2D>().enabled = param;
        }

        GameObject exitButton = GameObject.FindGameObjectWithTag("ExitButton");
        exitButton.GetComponent<BoxCollider2D>().enabled = param;
    }

}
