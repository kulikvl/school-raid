using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeParameter : MonoBehaviour
{
    private enum Parameters
    {
        HEALTH = 0,
        SPEED = 1,
        RELOADTIME = 2,
        BLASTRADIUS = 3,
        All= 4,
    }
    [SerializeField] private Parameters parameter;

    private string getNameOfParameter()
    {
        switch (parameter)
        {
            case Parameters.HEALTH:
                return UpgradeButton.currentBusIndex.ToString() + "HEALTH";
            case Parameters.SPEED:
                return UpgradeButton.currentBusIndex.ToString() + "SPEED";
            case Parameters.RELOADTIME:
                return UpgradeButton.currentBusIndex.ToString() + "RELOADTIME";
            case Parameters.BLASTRADIUS:
                return UpgradeButton.currentBusIndex.ToString() + "BLASTRADIUS";
        }

        Debug.LogError("ERROR!");
        return "";
    }

    public int difference;
    public Image fillSprite;
    public TextMeshProUGUI indicator, value;
    public GameObject MAXEDOUT;

    private int price;

    public void OnStart()
    {
        SetPrice();
        SetInfos();

        //Debug.Log("INFO: " + getNameOfParameter() + " " + PlayerPrefs.GetInt(getNameOfParameter()).ToString() + "/25");
    }

    private void Start()
    {
        OnStart();
    }

    private void OnMouseUpAsButton()
    {
        if (PlayerPrefs.GetInt(getNameOfParameter()) < 25 && PlayerPrefs.GetInt("Coins") >= price)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price);
            PlayerPrefs.SetInt(getNameOfParameter(), PlayerPrefs.GetInt(getNameOfParameter()) + 1);

            AudioManager.instance.Play("BtnUpgrade");

            SetPrice();
            SetInfos();

            Debug.Log("INFO: " + getNameOfParameter() + " " + PlayerPrefs.GetInt(getNameOfParameter()).ToString() + "/25");
        }
    }

    private void SetPrice()
    {
        price = PlayerPrefs.GetInt(getNameOfParameter()) * difference + 100;
    }

    private void SetInfos()
    {
        fillSprite.fillAmount = 1.0f / 25.0f * PlayerPrefs.GetInt(getNameOfParameter());
        value.text = price.ToString();
        indicator.text = PlayerPrefs.GetInt(getNameOfParameter()).ToString() + "/25";

        if (PlayerPrefs.GetInt(getNameOfParameter()) == 25)
        {
            MAXEDOUT.SetActive(true);
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            MAXEDOUT.SetActive(false);
            gameObject.GetComponent<Image>().enabled = true;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    //public static void SetUpgradesToDefaultValue()
    //{
    //    PlayerPrefs.SetInt("HEALTH", 0);
    //    PlayerPrefs.SetInt("SPEED", 0);
    //    PlayerPrefs.SetInt("RELOADTIME", 0);
    //    PlayerPrefs.SetInt("BLASTRADIUS", 0);
    //}
}
