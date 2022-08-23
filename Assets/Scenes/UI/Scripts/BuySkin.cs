using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class BuySkin : MonoBehaviour
{
    [System.Serializable]
    public struct Sprites
    {
        public Sprite CHOOSE;
        public Sprite PRICECOINS;
        public Sprite PRICEMONEY;
        public Sprite VIP;
    }
    public Sprites sprites;

    ////////////////////////

    [System.Serializable]
    public struct Texts
    {
        public GameObject CHOOSE;
        public GameObject PRICECOINS;
        public GameObject PRICEMONEY;
        public GameObject VIP;
    }
    public Texts texts;

    ////////////////////////

    public enum ButtonType
    {
        CHOOSE = 0,
        PRICECOINS = 1,
        PRICEMONEY = 2,
        VIP = 3,
        All = 4,
    }

    private ButtonType _buttonType;
    public ButtonType buttonType
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

                    isChosen = false;
                    SELECTED.SetActive(false);
                    gameObject.GetComponent<Image>().enabled = true;

                    gameObject.GetComponent<Image>().sprite = sprites.CHOOSE;
                    texts.CHOOSE.SetActive(true);
                    break;
                case ButtonType.PRICECOINS:
                    gameObject.GetComponent<Image>().sprite = sprites.PRICECOINS;
                    texts.PRICECOINS.SetActive(true);
                    break;
                case ButtonType.PRICEMONEY:
                    gameObject.GetComponent<Image>().sprite = sprites.PRICEMONEY;
                    texts.PRICEMONEY.SetActive(true);
                    break;
                case ButtonType.VIP:
                    gameObject.GetComponent<Image>().sprite = sprites.VIP;
                    //VIPprefab.SetActive(true);
                    //VIPMark.SetActive(true);
                    break;
            }

            _buttonType = value;
        }
    }
    private void hideTexts()
    {
        texts.CHOOSE.SetActive(false);
        texts.PRICECOINS.SetActive(false);
        texts.PRICEMONEY.SetActive(false);
        texts.VIP.SetActive(false);
    }

    ////////////////////////

    [System.Serializable]
    public class BusInfo
    {
        public Transform placeToSpawnModel;
        public TextMeshProUGUI nameOfTheBus;

        private int price;
        private int busIndex;
        private GameObject modelOfTheBus;
        
        public void spawnBus(GameObject Content, GameObject thisGameObject)
        {
            busIndex = Convert.ToInt32(thisGameObject.transform.parent.parent.gameObject.name);
            //Debug.Log(busIndex);
            modelOfTheBus = Content.GetComponent<SnapScrolling>().Models[busIndex];
            Instantiate(modelOfTheBus, placeToSpawnModel);
            setNameAndPrice();
        }

        public GameObject getModelOfTheBus()
        {
            return modelOfTheBus;
        }

        private void setNameAndPrice()
        {
            switch (busIndex)
            {
                case 0:
                    nameOfTheBus.text = "SCHOOL";
                    price = 0;
                    break;
                case 1:
                    nameOfTheBus.text = "AIRPORT";
                    price = 15000;
                    break;
                case 2:
                    nameOfTheBus.text = "MODERN";
                    price = 25000;
                    break;
                case 3:
                    nameOfTheBus.text = "ICE CREAM";
                    price = 35000;
                    break;
                case 4:
                    nameOfTheBus.text = "INCREDIBLE";
                    price = 50000;
                    break;
            }
        }

        public int Price
        {
            get
            {
                return price;
            }
        }

        public int BusIndex
        {
            get
            {
                return busIndex;
            }
        }

        public string getIndexString()
        {
            return busIndex.ToString();
        }
    }
    public BusInfo busInfo;

    public GameObject SELECTED;

    ////////////////////////

    private GameObject Content;
    private bool startedProcessing;
    public bool isChosen;

    ////////////////////////

    private void Start()
    {
        isChosen = false;
        startedProcessing = false;
        Content = GameObject.FindGameObjectWithTag("CONTENT");
        busInfo.spawnBus(Content, gameObject);
    }

    private void Update()
    {
        texts.PRICECOINS.GetComponent<TextMeshProUGUI>().text = busInfo.Price.ToString();

        if (!startedProcessing)
        {
            if (PlayerPrefs.GetString(busInfo.getIndexString()) == "Bought")
            {
                buttonType = ButtonType.CHOOSE;
            }
            else
            {
                buttonType = ButtonType.PRICECOINS;
            }
        } 
    }

    private void OnMouseUpAsButton()
    {
        if (Buttons.AbleToClick && !UpgradeButton.currentPageIsUpgrade)
        {
            if (PlayerPrefs.GetInt("Coins") >= busInfo.Price && PlayerPrefs.GetString(busInfo.getIndexString()) != "Bought")
            {
                buttonType = ButtonType.CHOOSE;
                PlayerPrefs.SetString(busInfo.getIndexString(), "Bought");
                PlayerPrefs.SetInt("busesBought", PlayerPrefs.GetInt("busesBought") + 1);
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - busInfo.Price);

                RankManager.instance.AddXP(200);

                transform.parent.GetChild(6).gameObject.GetComponent<UpgradeButton>().ActivateButton();

                playAnimationOnBuying();

                AudioManager.instance.Play("BtnBuy");

                if (PlayerPrefs.GetInt("busesBought") == 5)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.WealtyPerson);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.WealtyPerson);
                }
               
            }

            else if (PlayerPrefs.GetString(busInfo.getIndexString()) == "Bought")
            {
                startedProcessing = true;
                isChosen = true;

                PlayerPrefs.SetInt("currentModel", busInfo.BusIndex);

                GameObject go = Content.GetComponent<SnapScrolling>().FindBackgroundOfPan(busInfo.BusIndex);
                go.GetComponent<Animation>().Play();

                Content.GetComponent<SnapScrolling>().moveToSelectedButton = true; //todo
                Content.GetComponent<SnapScrolling>().selectedButtonID = busInfo.BusIndex;

                gameObject.GetComponent<Image>().enabled = false;

                hideTexts();
                SELECTED.SetActive(true);

                Content.GetComponent<SnapScrolling>().PlayAnimation();

                AudioManager.instance.Play("Btn1");

            }
        }
    }

    private void playAnimationOnBuying()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Conf");

        Transform[] trs = go.GetComponentsInChildren<Transform>();

        foreach(Transform t in trs)
        {
            if(t.gameObject.name.Contains("Confetti"))
            {
                t.gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
