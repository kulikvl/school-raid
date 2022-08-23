using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilitiesButton : MonoBehaviour
{
    public GameObject AbilitiesTab;
    public BoxCollider2D[] colliders;
    public static bool IsAllowedToScroll;

    public bool IsShopVersion;

    //public int test;
    //private void Update()
    //{
    //    test = UpgradeButton.currentBusIndex;
    //}

    private void OnMouseUpAsButton()
    {
        if (Buttons.AbleToClick)
        {
            if (!gameObject.name.Contains("Close") && IsAllowedToScroll)
            {
                IsAllowedToScroll = false;


                if (IsShopVersion)
                {
                    UpgradeButton.currentBusIndex = Convert.ToInt32(transform.parent.parent.gameObject.name);
                    SetButtonCollidersShop(false);
                    AbilitiesTab.SetActive(true);
                    AbilitiesTab.transform.GetChild(1).gameObject.GetComponent<AbilitiesButton>().IsShopVersion = true;
                }
                else
                {
                    SetButtonColliders(false);
                    AbilitiesTab.SetActive(true);

                    AbilitiesTab.transform.GetChild(1).gameObject.GetComponent<AbilitiesButton>().IsShopVersion = false;
                }

            }
            else
            {
                if (!IsShopVersion)
                    StartCoroutine(CloseTab());
                else
                    StartCoroutine(CloseTabShop());
            }
        }
        
    }

    private void Start()
    {
        if (AbilitiesTab == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("AbilitiesButton");
            AbilitiesTab = go.GetComponent<AbilitiesButton>().AbilitiesTab;
        }

        if (gameObject.name.Contains("Shop"))
        {
            IsShopVersion = true;
        }
        if (!gameObject.name.Contains("Close"))
        {
            IsAllowedToScroll = true;
        }
    }

    private void OnEnable()
    {
        if (gameObject.name.Contains("Close"))
            transform.localScale = new Vector3(58.68001f, 58.68001f, 58.68001f);
    }

    IEnumerator CloseTab()
    {
        AbilitiesTab.GetComponent<Animation>().Play("win2");
        yield return new WaitForSeconds(0.3f);

        SetButtonColliders(true);

        IsAllowedToScroll = true;
        AbilitiesTab.SetActive(false);
    }

    private void SetButtonColliders(bool param)
    {
        foreach (Collider2D col in colliders)
            col.enabled = param;
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
            if (go.GetComponent<BuyChest>().ChestName == "OrdinaryChestCount" && PlayerPrefs.GetInt("OrdinaryChestCount") <= 0 && !BuyChest.isReadyToWatchAd)
            {
                go.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                go.GetComponent<BoxCollider2D>().enabled = param;
            }
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

    IEnumerator CloseTabShop()
    {
        AbilitiesTab.GetComponent<Animation>().Play("win2");
        yield return new WaitForSeconds(0.3f);

        SetButtonCollidersShop(true);

        IsAllowedToScroll = true;

        AbilitiesTab.SetActive(false);
    }
}
