using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseChest : MonoBehaviour
{
    public Sprite supperOfferBoughtSprite;
    public bool isOrdinaryVIP;

    public void ChangeSuperVipPackButton()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Image>().sprite = supperOfferBoughtSprite;
        transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "PURCHASED!";
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(58.68001f, 58.68001f, 58.68001f);

        if (!isOrdinaryVIP && (PlayerPrefs.GetInt("1_OFFER") == 1))
        {
            ChangeSuperVipPackButton();
        }
    }

    private void OnMouseUpAsButton()
    {
       
            AudioManager.instance.Play("BtnBuy");
            TurnColliders(false);

            if (isOrdinaryVIP)
            {
           
                IAPManager.instance.BuyOrdinaryVipChest();
            }
            else
            {
                IAPManager.instance.BuySuperVipPack();
            }
        
    }

    private List<Collider2D> cols = new List<Collider2D>();
    public void TurnColliders(bool action)
    {
        if (action)
        {
            BuyChest[] gos = FindObjectsOfType<BuyChest>();
            if (gos.Length > 0)
            {
                foreach (BuyChest go in gos)
                {
                    bool value = go.gameObject.GetComponent<Collider2D>().enabled;
                    go.Start();
                    go.gameObject.GetComponent<Collider2D>().enabled = value;
                }
            }
        }

        //GameObject[] gos2 = GameObject.FindGameObjectsWithTag("PURCHASECHEST");
        //foreach (GameObject go in gos2)
        //{
        //    if (!go.GetComponent<PurchaseChest>().isOrdinaryVIP && (PlayerPrefs.GetInt("1_OFFER") == 1))
        //    {
        //        go.GetComponent<Collider2D>().enabled = false;
        //    }
        //    else
        //    go.GetComponent<Collider2D>().enabled = action;
        //}

        //ClosePurchaseTab cl = FindObjectOfType<ClosePurchaseTab>();
        //cl.gameObject.GetComponent<Collider2D>().enabled = action;


    }
}
