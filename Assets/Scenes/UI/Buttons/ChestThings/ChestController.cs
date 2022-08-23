using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestController : MonoBehaviour
{
    public TextMeshProUGUI text;

    public GameObject Coins;
    public GameObject[] upgradesParameters;
    public GameObject[] buses;
    public GameObject alterations;

    public ParticleSystem fountain;

    private GameObject PrizePrefab;

    private void OnEnable()
    {
        if (PrizePrefab != null) Destroy(PrizePrefab);

        if (BuyChest.VipOpened) // alt, buses (if nothing => coins)
        {
            /// BUS ///
            List<int> busesID = new List<int>();

            for (int i = 0; i < 5; ++i)
            {
                if (PlayerPrefs.GetString(i.ToString()) != "Bought")
                {
                    busesID.Add(i);
                }

            }
            /// BUS ///

            /// ALTS ///
            List<string> alts = new List<string>();

            for (int i = 0; i < 5; ++i)
            {
                if (PlayerPrefs.GetString(i.ToString()) == "Bought")
                {
                    if (PlayerPrefs.GetString(i.ToString() + "FIRST") != "Bought") alts.Add(i.ToString() + "FIRST");
                    if (PlayerPrefs.GetString(i.ToString() + "SECOND") != "Bought") alts.Add(i.ToString() + "SECOND");
                }
            }
            /// ALTS ///

            // 2/3 bus , 1/3 alt
            if (busesID.Count > 0 && Random.Range(0, 3) != 0) // bus
            {
                int randomID = busesID[Random.Range(0, busesID.Count)];
                DropBus(randomID);
            }
            else if (alts.Count > 0) // alt
            {
                string randomAlt = alts[Random.Range(0, alts.Count)];
                DropAlteration(randomAlt);
            }
            else
            {
                Debug.Log("THERE IS NO BUSES AND ALTS AVAILABLE!");
                DropCoins(5000);
            }
        }
        else // coins, upgrades (if nothing => coins)
        {
            // 25% coins , 75% upgrades
            if (Random.Range(0, 4) == 0) // coins
            {
                int num = Random.Range(0, 4);
                if (num == 0) DropCoins(200);
                if (num == 1) DropCoins(400);
                if (num == 2) DropCoins(600);
                if (num == 3) DropCoins(800);
            }
            else // upgrades
            {
                List<string> parameters = new List<string>();

                // if LVL of parameter > 15 => 3 cards, otherwise 5 cards

                for (int i = 0; i < 5; ++i)
                {
                    if (PlayerPrefs.GetString(i.ToString()) == "Bought")
                    {
                        if (PlayerPrefs.GetInt(i.ToString() + "HEALTH") < 25) parameters.Add(i.ToString() + "HEALTH");
                        if (PlayerPrefs.GetInt(i.ToString() + "SPEED") < 25) parameters.Add(i.ToString() + "SPEED");
                        if (PlayerPrefs.GetInt(i.ToString() + "RELOADTIME") < 25) parameters.Add(i.ToString() + "RELOADTIME");
                        if (PlayerPrefs.GetInt(i.ToString() + "BLASTRADIUS") < 25) parameters.Add(i.ToString() + "BLASTRADIUS");
                    }  
                }

                if (parameters.Count > 0)
                {
                    string randomParameter = parameters[Random.Range(0, parameters.Count)];

                    if (PlayerPrefs.GetInt(randomParameter) >= 0 && PlayerPrefs.GetInt(randomParameter) < 15) // todo balance
                    {
                        DropUpgrade(randomParameter, 3);
                    }
                    else if (PlayerPrefs.GetInt(randomParameter) >= 15 && PlayerPrefs.GetInt(randomParameter) < 20) // todo balance
                    {
                        DropUpgrade(randomParameter, 2);
                    }
                    else
                    {
                        DropUpgrade(randomParameter, 1);
                    }
                }
                else
                {
                    int num = Random.Range(0, 4);
                    if (num == 0) DropCoins(200);
                    if (num == 1) DropCoins(400);
                    if (num == 2) DropCoins(600);
                    if (num == 3) DropCoins(800);
                }
            }
        }

        fountain.Play();

        //Debug.Log(PrizePrefab.name);


        // effects
    }

    private void DropCoins(int value)
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + value);

        PrizePrefab = Instantiate(Coins, Vector3.zero, Quaternion.identity, transform);

        PrizePrefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(-4f, 67f, 0f);
        PrizePrefab.transform.localPosition = new Vector3(PrizePrefab.transform.localPosition.x, PrizePrefab.transform.localPosition.y, 0f);

        text.text = "COINS!";
        PrizePrefab.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "+ " + value.ToString();
    }

    bool ToAnimateDropUpgrade = false;
    float initialAmount;
    float finalAmount;

    private void OnDisable()
    {
        if (PrizePrefab != null) Destroy(PrizePrefab);
        t = 0f;
        ToAnimateDropUpgrade = false;
    }

    private void DropUpgrade(string parameterName, int value)
    {
        initialAmount = 1.0f / 25.0f * PlayerPrefs.GetInt(parameterName);
        PlayerPrefs.SetInt(parameterName, PlayerPrefs.GetInt(parameterName) + value);
        finalAmount = 1.0f / 25.0f * PlayerPrefs.GetInt(parameterName);

        //Debug.Log(initialAmount + " to " + finalAmount);

        PrizePrefab = Instantiate(upgradesParameters[GetSpriteParameterByName(parameterName)], Vector3.zero, Quaternion.identity, transform);

        PrizePrefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(240f, -17f, 0f);
        PrizePrefab.transform.localPosition = new Vector3(PrizePrefab.transform.localPosition.x, PrizePrefab.transform.localPosition.y, 0f);

        /// BUSES
        int n = (int)parameterName[0] - 48;
        Debug.Log(n);
        GameObject bus = Instantiate(buses[n], Vector3.zero, Quaternion.identity, PrizePrefab.transform.GetChild(1));

        if (bus.name.Contains("0")) bus.transform.localPosition = new Vector3(0f, -8f, 0f);
        if (bus.name.Contains("1")) bus.transform.localPosition = new Vector3(0f, -9f, 0f);
        if (bus.name.Contains("2")) bus.transform.localPosition = new Vector3(0f, -27f, 0f);
        if (bus.name.Contains("3")) bus.transform.localPosition = new Vector3(0f, 49f, 0f);
        if (bus.name.Contains("4")) bus.transform.localPosition = new Vector3(0f, 26f, 0f);

        bus.transform.localScale = new Vector3(105f, 105f, 105f);
        Renderer[] sprites = bus.GetComponentsInChildren<Renderer>();
        foreach (Renderer sp in sprites)
        {
            sp.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("ONLYMENU+");

            if (sp.gameObject.name != "ropeRight" && sp.gameObject.name != "ropeLeft" && sp.gameObject.name != "BUS" && sp.gameObject.name != "parahute")
                sp.GetComponent<Renderer>().sortingOrder += 3000;
        }

        //if (bus.name.Contains("4"))
        //{
        //    ParticleSystem[] parts = bus.GetComponentsInChildren<ParticleSystem>();
        //    foreach (ParticleSystem pr in parts)
        //    {
        //        pr.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("ONLYMENU+");
        //        pr.GetComponent<Renderer>().sortingOrder += 3000;
        //    }
        //}

        ///

        ToAnimateDropUpgrade = true;

        PrizePrefab.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text = "+ " + value.ToString();
        text.text = "UPGRADES!";
    }

    private void DropBus(int ID)
    {
        PlayerPrefs.SetString(ID.ToString(), "Bought");
        PrizePrefab = Instantiate(buses[ID], buses[ID].transform.position, buses[ID].transform.rotation, transform);

        if (PrizePrefab.name.Contains("1")) PrizePrefab.transform.localPosition = new Vector3(0f, -60f, 0f);
        if (PrizePrefab.name.Contains("2")) PrizePrefab.transform.localPosition = new Vector3(0f, -80f, 0f);
        if (PrizePrefab.name.Contains("3")) PrizePrefab.transform.localPosition = new Vector3(0f, 20f, 0f);
        if (PrizePrefab.name.Contains("4")) PrizePrefab.transform.localPosition = new Vector3(0f, 20f, 0f);

        PrizePrefab.transform.localScale = new Vector3(120f, 120f, 120f);

        Renderer[] sprites = PrizePrefab.GetComponentsInChildren<Renderer>();
        foreach (Renderer sp in sprites)
        {
            sp.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("ONLYMENU+");

            if (sp.gameObject.name != "ropeRight" && sp.gameObject.name != "ropeLeft" && sp.gameObject.name != "BUS" && sp.gameObject.name != "parahute")
                sp.GetComponent<Renderer>().sortingOrder += 3000;
        }

        // check upgradeButtons

        PlayerPrefs.SetInt("busesBought", PlayerPrefs.GetInt("busesBought") + 1);
        if (PlayerPrefs.GetInt("busesBought") == 5)
        {
            GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.WealtyPerson);
            PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.WealtyPerson);
        }

        GameObject[] UpgradeButtons = GameObject.FindGameObjectsWithTag("UpgradeButton");
        foreach (GameObject go in UpgradeButtons)
        {
            if (go.GetComponent<UpgradeButton>().thisBusIndex == ID) go.GetComponent<UpgradeButton>().ActivateButton();
        }

        text.text = "WOW! NEW BUS!";
    }

    private void DropAlteration(string altName)
    {
        // "0FIRST, 3SECOND"

        PlayerPrefs.SetString(altName, "Bought");
        PrizePrefab = Instantiate(alterations, Vector3.zero, Quaternion.identity, transform);

        PrizePrefab.GetComponent<RectTransform>().anchoredPosition = new Vector3(180f, 6f, 0f);
        PrizePrefab.transform.localPosition = new Vector3(PrizePrefab.transform.localPosition.x, PrizePrefab.transform.localPosition.y, 0f);

        text.text = "ALTERATION!";

        int num = (altName.Contains("FIRST")) ? 1 : 2;
   
        int index = (int)altName[0] - 48;

        GameObject SpriteContainer = PrizePrefab.transform.GetChild(num).gameObject;
        GameObject LocalSprite = SpriteContainer.transform.GetChild(index).gameObject;

        LocalSprite.SetActive(true);

        Debug.Log(altName + " -> " + num + " -> " + index + " -> " + SpriteContainer.name + " -> " + LocalSprite.name + " FINISH! ");

        //////////
        GameObject bus = Instantiate(buses[index], Vector3.zero, Quaternion.identity, PrizePrefab.transform.GetChild(3));

        if (bus.name.Contains("0")) bus.transform.localPosition = new Vector3(0f, -8f, 0f);
        if (bus.name.Contains("1")) bus.transform.localPosition = new Vector3(0f, -9f, 0f);
        if (bus.name.Contains("2")) bus.transform.localPosition = new Vector3(0f, -27f, 0f);
        if (bus.name.Contains("3")) bus.transform.localPosition = new Vector3(0f, 49f, 0f);
        if (bus.name.Contains("4")) bus.transform.localPosition = new Vector3(0f, 26f, 0f);

        bus.transform.localScale = new Vector3(105f, 105f, 105f);
        Renderer[] sprites = bus.GetComponentsInChildren<Renderer>();
        foreach (Renderer sp in sprites)
        {
            sp.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("ONLYMENU+");

            if (sp.gameObject.name != "ropeRight" && sp.gameObject.name != "ropeLeft" && sp.gameObject.name != "BUS" && sp.gameObject.name != "parahute")
                sp.GetComponent<Renderer>().sortingOrder += 3000;
        }
    }

    private int GetSpriteParameterByName(string name)
    {
        if (name.Contains("HEALTH")) return 0;
        if (name.Contains("SPEED")) return 1;
        if (name.Contains("RELOADTIME")) return 2;
        if (name.Contains("BLASTRADIUS")) return 3;
        else return -1;
    }

    static float t = 0.0f;
    private void Update()
    {
        if (ToAnimateDropUpgrade)
        {
            PrizePrefab.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount = Mathf.Lerp(initialAmount, finalAmount, t);
            t += 0.3f * Time.deltaTime;
        }
        
    }
}
