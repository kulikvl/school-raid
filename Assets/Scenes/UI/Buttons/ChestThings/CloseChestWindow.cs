using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseChestWindow : MonoBehaviour
{
    public GameObject ChestOpeningWindow;

    private void OnMouseUpAsButton()
    {

        AbilitiesButton.IsAllowedToScroll = true;

        StartCoroutine(CloseTabShop());

    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(58.68001f, 58.68001f, 58.68001f);
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
        ChestOpeningWindow.GetComponent<Animation>().Play("win2");
        yield return new WaitForSeconds(0.3f);
        SetButtonCollidersShop(true);
        ChestOpeningWindow.SetActive(false);
    }
}
