using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public BuySkin buySkin;
    private GameObject shop, upgradeBack, upgradePoint, abilitiesButton, upgradeBackGround, upgradeStatistics;
    private GameObject[] upgradeParameterButtons, buyAlteraionButtons;
    public static int currentBusIndex;
    public int thisBusIndex;
    public static bool currentPageIsUpgrade;

    private static GameObject createdBus;

    public bool isClickable;

    private void Start()
    {
        isClickable = true;
        currentPageIsUpgrade = false;
        shop = GameObject.FindGameObjectWithTag("Shop");
        upgradeBack = GameObject.FindGameObjectWithTag("UpgradeBack");
        upgradeBackGround = GameObject.FindGameObjectWithTag("UpgradeBackGround");
        upgradeStatistics = GameObject.FindGameObjectWithTag("UpgradeStatistics");
        upgradePoint = GameObject.FindGameObjectWithTag("UpgradePoint");
        abilitiesButton = GameObject.FindGameObjectWithTag("AbilitiesButton");

        upgradeParameterButtons = GameObject.FindGameObjectsWithTag("UpgradeParameter");
        buyAlteraionButtons = GameObject.FindGameObjectsWithTag("BuyAlt");

        thisBusIndex = Convert.ToInt32(buySkin.gameObject.transform.parent.parent.gameObject.name);
        if (PlayerPrefs.GetString(thisBusIndex.ToString()) != "Bought")
        {
            isClickable = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0.8f, 0.8f, 0.8f);
        }
    }

    public void ActivateButton()
    {
        isClickable = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Image>().color = Color.white;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    private void OnMouseUpAsButton()
    {
        if (!PlayCampaign.currentPageIsLevelSelection && Buttons.AbleToClick)
        {
            CreateBus();

            currentPageIsUpgrade = true;

            abilitiesButton.GetComponent<Animation>().Play("AbilitiesAppear");
            shop.GetComponent<Animation>().Play("ShopSwipe2");
            upgradeBack.GetComponent<Animation>().Play();
            upgradeBackGround.GetComponent<Animation>().Play();
            upgradeStatistics.GetComponent<Animation>().Play();
            upgradePoint.GetComponent<Animation>().Play();

            currentBusIndex = Convert.ToInt32(buySkin.gameObject.transform.parent.parent.gameObject.name);

            foreach (GameObject go in upgradeParameterButtons)
            {
                go.GetComponent<UpgradeParameter>().OnStart();
            }

            foreach (GameObject go in buyAlteraionButtons)
            {
                go.GetComponent<BuyAlteration>().OnStart();
            }
        }
        
    }
    private void CreateBus()
    {
        if (createdBus != null)
        {
            Destroy(createdBus);
        }
        createdBus = Instantiate(buySkin.busInfo.getModelOfTheBus(), upgradePoint.transform);

        if (createdBus.name.Contains("4"))
        {
            Debug.Log("bigger Fire");

            for (int i = 0; i < 6; ++i)
                createdBus.transform.GetChild(3).GetChild(0).GetChild(i).gameObject.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
        }
    }
}
