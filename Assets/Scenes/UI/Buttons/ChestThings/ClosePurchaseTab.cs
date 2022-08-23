using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePurchaseTab : MonoBehaviour
{
    public static bool purchaseTabIsOpened;

    private void OnEnable()
    {
        transform.localScale = new Vector3(58.68001f, 58.68001f, 58.68001f);

        AbilitiesButton.IsAllowedToScroll = false;
        purchaseTabIsOpened = true;
        SetButtonColliders(false);
    }

    private void OnMouseUpAsButton()
    {
        if (purchaseTabIsOpened)
        {
            purchaseTabIsOpened = false;
            StartCoroutine(CloseTab());
        }
    }

    private void SetButtonColliders(bool param)
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

    IEnumerator CloseTab()
    {
        transform.parent.gameObject.GetComponent<Animation>().Play("win2");
        yield return new WaitForSeconds(0.3f);

        SetButtonColliders(true);

        AbilitiesButton.IsAllowedToScroll = true;

        transform.parent.gameObject.SetActive(false);
    }
}
