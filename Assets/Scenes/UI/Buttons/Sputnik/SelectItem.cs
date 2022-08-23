using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectItem : MonoBehaviour, IPointerDownHandler
{
    public Image fillImage;
    public Animation countOfSputnik;
    public bool isFull = false;
    public TextMeshProUGUI indicator;
    public TextMeshProUGUI price;

    public Sprite ordinarySprite;
    public Sprite spriteWhenReady;
    private Image curImage;

    public static int putniksSetted;

    private GameObject[] gos;
    private int priceForSputnik;

    private float cooldown;
    private bool allowedToSet;
    public float cooldownTime;

    private void Awake()
    {
        curImage = GetComponent<Image>();

        putniksSetted = 0;
        SetNewPrice();
        allowedToSet = true;
    }

    private void Update()
    {
        indicator.text = putniksSetted.ToString() + '/' + '3';
        price.text = (priceForSputnik != 0) ? priceForSputnik.ToString() : " ";

        if (!allowedToSet)
            cooldown -= Time.deltaTime;

        if (cooldown <= 0.0f && !allowedToSet)
        {
            allowedToSet = true;
        }

        fillImage.fillAmount = cooldown / cooldownTime;

        if (sputnikCount.count >= priceForSputnik && allowedToSet && putniksSetted < 3) curImage.sprite = spriteWhenReady;
        else curImage.sprite = ordinarySprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Collider2D>().enabled)
        if (PlayerPrefs.GetInt("currentLevel") == 2 && !PlayerPrefs.HasKey("showedIntroSputnik2"))
        {
            if (TutorialSputnik.instance.isReadyToPressSputnik)
            {
                chooseThePlace(2);
                TutorialSputnik.instance.IfPressedSputnik();
            }
            
        }
        else if ((PlayerPrefs.GetInt("currentLevel") == 3  || PlayerPrefs.GetInt("currentLevel") == 2 )&& TutorialSputnik.instance.isReadyToPressSputnik)
        {
            chooseThePlace();
            TutorialSputnik.instance.IfPressedSputnik3();
        }
        else if (!isFull && allowedToSet && !SettingsInGame.GameIsFreezed)
        {
            if (sputnikCount.count >= priceForSputnik)
            {
                chooseThePlace();
            }
            else
            {
                countOfSputnik.Play();
            }
        }   
    }

    private void chooseThePlace()
    {
        gos = GameObject.FindGameObjectsWithTag("placeForSputnik");

        if (gos.Length != 0)
        {
            AudioManager.instance.Play("Ability3");
            sputnikCount.count -= priceForSputnik;

            gos[Random.Range(0, gos.Length)].GetComponent<placeSputnik>().SetSputnik();
            SetNewPrice();

            cooldown = cooldownTime;
            allowedToSet = false;
        }
        else
        {
            isFull = true;
        }
    }

    private void chooseThePlace(int place)
    {
        gos = GameObject.FindGameObjectsWithTag("placeForSputnik");

        if (gos.Length != 0)
        {
            AudioManager.instance.Play("Ability3");
            sputnikCount.count -= priceForSputnik;

            gos[place].GetComponent<placeSputnik>().SetSputnik();
            SetNewPrice();

            cooldown = cooldownTime;
            allowedToSet = false;
        }
        else
        {
            isFull = true;
        }
    }

    private void SetNewPrice()
    {
        if (putniksSetted == 0)
        {
            priceForSputnik = 4;
        }
        if (putniksSetted == 1)
        {
            priceForSputnik = 6;
        }
        if (putniksSetted == 2)
        {
            priceForSputnik = 8;
        }
        if (putniksSetted == 3)
        {
            priceForSputnik = 0;
        }
    }
}
