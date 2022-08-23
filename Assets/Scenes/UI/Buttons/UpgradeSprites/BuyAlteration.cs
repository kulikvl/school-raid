using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyAlteration : MonoBehaviour
{

    private void Awake() //todo
    {
        UpgradeButton.currentBusIndex = 0;
    }

    public int price;

    public Sprite spriteChoose, spriteBuy;
    public GameObject textChoose, textBuy;
    public TextMeshProUGUI nameOfAbility;
    public GameObject SELECTED;

    private enum Alterations
    {
        DEFAULT = 0,
        FIRST = 1,
        SECOND = 2,
        All = 3,
    }
    [SerializeField] private Alterations alteration;

    private string getNameOfAlteration()
    {
        switch (alteration)
        {
            case Alterations.DEFAULT:
                return UpgradeButton.currentBusIndex.ToString() + "DEFAULT";
            case Alterations.FIRST:
                return UpgradeButton.currentBusIndex.ToString() + "FIRST";
            case Alterations.SECOND:
                return UpgradeButton.currentBusIndex.ToString() + "SECOND";
        }

        Debug.LogError("ERROR!");
        return "";
    }

    private int getIndexOfAlteration()
    {
        switch (alteration)
        {
            case Alterations.DEFAULT:
                return 0;
            case Alterations.FIRST:
                return 1;
            case Alterations.SECOND:
                return 2;
        }

        Debug.LogError("ERROR!");
        return -1;
    }

    public enum ButtonType
    {
        CHOOSE = 0,
        PRICECOINS = 1,
        CHOSEN = 2,
        All = 3,
    }

    public ButtonType _buttonType;
    private ButtonType buttonType
    {
        get
        {
            return _buttonType;
        }
        set
        {
            hideTexts();

            switch (value)
            {
                case ButtonType.CHOOSE:
                    
                    gameObject.GetComponent<Image>().enabled = true;
                    gameObject.GetComponent<Image>().sprite = spriteChoose;
                    textChoose.SetActive(true);
                    break;
                case ButtonType.CHOSEN:

                    GameObject[] gos = GameObject.FindGameObjectsWithTag("BuyAlt");
                    foreach (GameObject go in gos)
                        if (go != gameObject && go.GetComponent<BuyAlteration>().buttonType == ButtonType.CHOSEN)
                            go.GetComponent<BuyAlteration>().buttonType = ButtonType.CHOOSE;

                    gameObject.GetComponent<Image>().enabled = false;
                    SELECTED.SetActive(true);

                    break;
                case ButtonType.PRICECOINS:
                    gameObject.GetComponent<Image>().enabled = true;
                    gameObject.GetComponent<Image>().sprite = spriteBuy;
                    textBuy.SetActive(true);
                    break;
            }

            _buttonType = value;
        }
    }

    [Header("Icons")]
    public GameObject[] sprites;

    private void hideTexts()
    {
        textChoose.SetActive(false);
        textBuy.SetActive(false);
        SELECTED.SetActive(false);
    }

    private void Start()
    {
        OnStart();
    }

    private void OnMouseUpAsButton()
    {
        if (PlayerPrefs.GetInt("Coins") >= price && PlayerPrefs.GetString(getNameOfAlteration()) != "Bought")
        {
            Debug.Log("PURCHASED: " + getNameOfAlteration());

            RankManager.instance.AddXP(100);

            buttonType = ButtonType.CHOOSE;
            PlayerPrefs.SetString(getNameOfAlteration(), "Bought");
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price);
        }
        else if (PlayerPrefs.GetString(getNameOfAlteration()) == "Bought")
        {
            buttonType = ButtonType.CHOSEN;
            PlayerPrefs.SetInt((UpgradeButton.currentBusIndex.ToString() + "currentAlteration"), getIndexOfAlteration());
            gameObject.GetComponent<Image>().enabled = false;
            hideTexts();
            SELECTED.SetActive(true);
        }
    }

    public void OnStart()
    {
        textBuy.GetComponent<TextMeshProUGUI>().text = price.ToString();

        foreach (GameObject go in sprites) go.SetActive(false);

        switch (UpgradeButton.currentBusIndex)
        {
            case 0:
                sprites[0].SetActive(true);

                if (alteration == Alterations.DEFAULT) nameOfAbility.text = "DEFAULT";
                if (alteration == Alterations.FIRST)   nameOfAbility.text = "FLAMING";
                if (alteration == Alterations.SECOND)  nameOfAbility.text = "SPLITTING";
                break;

            case 1:
                sprites[1].SetActive(true);

                if (alteration == Alterations.DEFAULT) nameOfAbility.text = "DEFAULT";
                if (alteration == Alterations.FIRST)   nameOfAbility.text = "AIMING";
                if (alteration == Alterations.SECOND)  nameOfAbility.text = "GIANT";
                break;

            case 2:
                sprites[2].SetActive(true);

                if (alteration == Alterations.DEFAULT) nameOfAbility.text = "DEFAULT";
                if (alteration == Alterations.FIRST)   nameOfAbility.text = "LONGER";
                if (alteration == Alterations.SECOND)  nameOfAbility.text = "FREEZING";
                break;

            case 3:
                sprites[3].SetActive(true);

                if (alteration == Alterations.DEFAULT) nameOfAbility.text = "DEFAULT";
                if (alteration == Alterations.FIRST) nameOfAbility.text = "UNBOXING";
                if (alteration == Alterations.SECOND) nameOfAbility.text = "SPLITTING";
                break;

            case 4:
                sprites[4].SetActive(true);

                if (alteration == Alterations.DEFAULT) nameOfAbility.text = "DEFAULT";
                if (alteration == Alterations.FIRST) nameOfAbility.text = "MORE";
                if (alteration == Alterations.SECOND) nameOfAbility.text = "WILDFIRE";
                break;
        }

        //Debug.Log("INFO: " + getNameOfAlteration() + " BOUGHT: " + PlayerPrefs.GetString(getNameOfAlteration()) == "Bought");
        //Debug.Log("INFO: " + getNameOfAlteration() + " IsCURRENT: " + PlayerPrefs.GetInt((UpgradeButton.currentBusIndex.ToString() + "currentAlteration"), getIndexOfAlteration()));
        //PlayerPrefs.SetString("0FLAMING", "Bought");

        if (PlayerPrefs.GetString(getNameOfAlteration()) == "Bought")
        {
            if (PlayerPrefs.GetInt(UpgradeButton.currentBusIndex.ToString() + "currentAlteration") == getIndexOfAlteration())
            {
                buttonType = ButtonType.CHOSEN;
            }
            else
            {

                buttonType = ButtonType.CHOOSE;
            }
        }
        else
        {
            buttonType = ButtonType.PRICECOINS;
        }
    }
}
