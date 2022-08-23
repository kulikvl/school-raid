using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectAbility : MonoBehaviour, IPointerDownHandler
{
    public Abilities ability;
    public int price;

    public Sprite ordinarySprite;
    public Sprite spriteWhenReady;
    private Image curImage;

    private BusController busController;
    private float cooldown;
    public bool allowedToChoose;
    public float cooldownTime;
    public Image fillImage;
    public TextMeshProUGUI _price;
    public Animation countOfSputnik;

    public GameObject[] sprites;

    public static int TimesAbilityActivated;

    private void Start()
    {
        curImage = GetComponent<Image>();
        TimesAbilityActivated = 0;

        foreach (GameObject go in sprites) go.SetActive(false);
        sprites[PlayerPrefs.GetInt("currentModel")].SetActive(true);

        if (gameObject.name.Contains("0"))
        {
            switch(PlayerPrefs.GetInt("currentModel"))
            {
                case 0:
                    price = 5;
                    break;
                case 1:
                    price = 5;
                    break;
                case 2:
                    price = 5;
                    break;
                case 3:
                    price = 3;
                    break;
                case 4:
                    price = 3;
                    break;

            }
        }
        else if (gameObject.name.Contains("1"))
        {
            switch (PlayerPrefs.GetInt("currentModel"))
            {
                case 0:
                    price = 3;
                    break;
                case 1:
                    price = 3;
                    break;
                case 2:
                    price = 2;
                    break;
                case 3:
                    price = 3;
                    break;
                case 4:
                    price = 5;
                    break;

            }
        }
        else if (gameObject.name.Contains("2"))
        {
            switch (PlayerPrefs.GetInt("currentModel"))
            {
                case 0:
                    price = 2;
                    break;
                case 1:
                    price = 4;
                    break;
                case 2:
                    price = 3;
                    break;
                case 3:
                    price = 3;
                    break;
                case 4:
                    price = 6;
                    break;

            }
        }

        allowedToChoose = true;
        GameObject bus = GameObject.FindGameObjectWithTag("BUS");
        if (bus != null) busController = bus.GetComponent<BusController>();
        else Debug.LogError("BUS WAS NOT FOUND!");

        if (PlayerPrefs.GetString("currentMode") == "Deathmatch") _price.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!allowedToChoose)
            cooldown -= Time.deltaTime;

        if (cooldown <= 0.0f && !allowedToChoose)
        {
            allowedToChoose = true;
        }

        fillImage.fillAmount = cooldown / cooldownTime;

        if (PlayerPrefs.GetString("currentMode") == "Campaign")
        _price.text = (price != 0) ? price.ToString() : " ";
        //_price.color = (sputnikCount.count >= price) ? Color.green : Color.red;

        if ((sputnikCount.count >= price  || PlayerPrefs.GetString("currentMode") == "Deathmatch") && allowedToChoose) curImage.sprite = spriteWhenReady;
        else curImage.sprite = ordinarySprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Collider2D>().enabled)
        if (PlayerPrefs.GetInt("currentLevel") == 2 && !PlayerPrefs.HasKey("showedIntroSputnik2"))
        {
            if (TutorialSputnik.instance.isReadyToPressAbility && gameObject.name.Contains("0"))
            {
                busController.gameObject.GetComponent<IAbility>().ActivateAbility(ability, out bool result);
                if (result) SetCooldown();

                TutorialSputnik.instance.IfPressedAbility();
            }
            
        }
        else if (allowedToChoose && busController.gameObject.active && !PlayerController.Lost && !PlayerController.Won && !SettingsInGame.GameIsFreezed)
        {
            if ((sputnikCount.count >= price && PlayerPrefs.GetString("currentMode") == "Campaign") || PlayerPrefs.GetString("currentMode") == "Deathmatch")
            {
                busController.gameObject.GetComponent<IAbility>().ActivateAbility(ability, out bool result);

                if (result) SetCooldown();
            }
            else
            {
                countOfSputnik.Play();
            }
        }
    }

    private void SetCooldown()
    {
        TimesAbilityActivated++;

        if (gameObject.name.Contains("0"))
        {
            AudioManager.instance.Play("Ability5");
        }
        else if (gameObject.name.Contains("1"))
        {
            AudioManager.instance.Play("Ability1");
        }
        else if (gameObject.name.Contains("2"))
        {
            AudioManager.instance.Play("Ability6");
        }


        sputnikCount.count -= price;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("ItemAbility");

        foreach (GameObject go in gos)
        {
            go.GetComponent<SelectAbility>().cooldown = cooldownTime;
            go.GetComponent<SelectAbility>().allowedToChoose = false;
        }
    }
}
