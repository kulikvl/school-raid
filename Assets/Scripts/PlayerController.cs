using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject TutorialWindow, tutor1, tutor2;
    [SerializeField] private GameObject tabItems;

    [SerializeField] private GameObject itemSputnik, countOfSputnik, missionsButton;
    [SerializeField] private GameObject DMController, RUSHController;

    public static bool isMissionAnimationPlaying;
    public static bool Lost = false;
    public static bool Won = false;

    public static float leftHP = 1.0f;
    public static float centerHP = 1.0f;
    public static float rightHP = 1.0f;

    public static int Wave = 1;

    public static int TimesWithoutWatchingAd;
    public static bool HealedByWatchingAd;

    private const string playedTutorialHowToShoot = "PlayedTutorialHowToShoot";
    private const string playedTutorialNewChallange = "PlayedTutorialNewChallange";

    [System.Serializable]
    public struct BackGround
    {
        public GameObject backGround;
        public GameObject terrain;
    }
    public BackGround[] backgrounds;
    
    public static void SetFullHpSchool()
    {
        leftHP = 1.0f;
        centerHP = 1.0f;
        rightHP = 1.0f;
        Wave = 1;
    }

    public static void SetStars()
    {
        int currentLevel = PlayerPrefs.GetInt("currentLevel");

        if (rightHP > 0.0f && leftHP > 0.0f && centerHP > 0.0f)
        {
            if (HealedByWatchingAd == true)
            {
                Debug.Log("2 stars!!!!!!!" + rightHP + " " + leftHP + " " + centerHP);
                PlayerPrefs.SetInt("currentStarsOnLevel" + currentLevel.ToString(), 2);
            }
            else
            {
                Debug.Log("3 stars!!!!!!!" + rightHP + " " + leftHP + " " + centerHP);
                PlayerPrefs.SetInt("currentStarsOnLevel" + currentLevel.ToString(), 3);
            }
           
        }
        else if (((rightHP <= 0.0f && leftHP > 0.0f) || (rightHP > 0.0f && leftHP <= 0.0f)) && centerHP > 0.0f)
        {
            Debug.Log("2 stars!!!!!!!" + rightHP + " " + leftHP + " " + centerHP);
            PlayerPrefs.SetInt("currentStarsOnLevel" + currentLevel.ToString(), 2);
        }
        else if (rightHP <= 0.0f && leftHP <= 0.0f && centerHP > 0.0f)
        {
            Debug.Log("1 star!!!!!!!" + rightHP + " " + leftHP + " " + centerHP);
            PlayerPrefs.SetInt("currentStarsOnLevel" + currentLevel.ToString(), 1);
        }

        /// check for best stars

        if (PlayerPrefs.GetInt("currentStarsOnLevel" + currentLevel.ToString())
                > PlayerPrefs.GetInt("bestStarsOnLevel" + currentLevel.ToString()))
        {
            PlayerPrefs.SetInt("bestStarsOnLevel" + currentLevel.ToString(), PlayerPrefs.GetInt("currentStarsOnLevel" + currentLevel.ToString()));
        }
    }

    public static void IncreaseIfNeededHpSchool(float amount)
    {
        if (leftHP < 1.0f)
        {
            leftHP += amount;
            if (leftHP > 1.0f) leftHP = 1.0f;
        }
        if (centerHP < 1.0f)
        {
            centerHP += amount;
            if (centerHP > 1.0f) centerHP = 1.0f;
        }
        if (rightHP < 1.0f)
        {
            rightHP += amount;
            if (rightHP > 1.0f) rightHP = 1.0f;
        }
    }

    private void Awake()
    {
        HealedByWatchingAd = false;

        //PlayerPrefs.SetInt("currentLevel", 2); // todo

        //Time.timeScale = 1f;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;

        //PlayerPrefs.SetInt("currentLevel", 2);
        //PlayerPrefs.SetString("currentMode", "Campaign");

        //// after tutorSputnik
        //if (SettingsInGame.GameIsFreezed && PlayerPrefs.GetInt("currentLevel") == 2 && PlayerPrefs.HasKey("showedIntroSputnik") && PlayerPrefs.GetString("currentMode") == "Campaign")
        //{
        //    Debug.Log("UNFREEEEEZED");
        //    SettingsInGame.GameIsFreezed = false;
        //    Time.timeScale = 1f;
        //    Time.fixedDeltaTime = Time.timeScale * 0.02f;
        //}

        //Debug.Log(Time.timeScale + " " + SettingsInGame.GameIsFreezed);

        if (Application.systemLanguage == SystemLanguage.Russian)
        {
            Debug.Log("system lang is russian");
            LocalisationSystem.language = LocalisationSystem.Language.Russian;
        }
        else
        {
            Debug.Log("system lang is en or another");
            LocalisationSystem.language = LocalisationSystem.Language.English;
        }

        SetBackGround();
        SetFullHpSchool();
        PlayTutorial();

        GameObject busManager = GameObject.FindGameObjectWithTag("BusManager");

        if (PlayerPrefs.GetInt("currentLevel") == 2 && PlayerPrefs.GetString("currentMode") == "Campaign" && !PlayerPrefs.HasKey("showedIntroSputnik2"))
            Instantiate(busManager.GetComponent<BusManager>().getBusByIndex(0), player);
        else
            Instantiate(busManager.GetComponent<BusManager>().getCurrentBus(), player);

        isMissionAnimationPlaying = false;
        Lost = false;
        Won = false;

        if (PlayerPrefs.GetInt("currentLevel") == 1 && PlayerPrefs.GetString("currentMode") == "Campaign") // todo
            tabItems.transform.position = new Vector3(tabItems.transform.position.x, tabItems.transform.position.y + 100f);

        if (PlayerPrefs.GetString("currentMode") == "Deathmatch")
        {
            itemSputnik.SetActive(false);
            countOfSputnik.SetActive(false);
            missionsButton.SetActive(true);
            DMController.SetActive(true);
        }
        else
        {
            itemSputnik.SetActive(true);
            countOfSputnik.SetActive(true);
            missionsButton.SetActive(false);
            RUSHController.SetActive(true);
        }
    }

    private void SetBackGround()
    {
        AudioManager.instance.Stop("BackGroundCountrySide");
        AudioManager.instance.Stop("BackGroundHalloween");
        AudioManager.instance.Stop("MainTheme");

        if (PlayerPrefs.GetString("currentMode") == "Deathmatch")
        {
            AudioManager.instance.Play("BackGroundHalloween");
            AudioManager.instance.Play("BackGroundHalloween+");
            AudioManager.instance.SetVolume("BackGroundHalloween", 0.1f);

            backgrounds[0].backGround.SetActive(false);
            backgrounds[0].terrain.SetActive(false);
            backgrounds[1].backGround.SetActive(false);
            backgrounds[1].terrain.SetActive(false);
            backgrounds[2].backGround.SetActive(true);
            backgrounds[2].terrain.SetActive(true);
        }
        else
        {
            if (PlayerPrefs.GetInt("currentLevel") <= 20)
            {
                AudioManager.instance.Play("BackGroundCity");
                AudioManager.instance.Play("BackGroundCity+");

                backgrounds[0].backGround.SetActive(true);
                backgrounds[0].terrain.SetActive(true);
                backgrounds[1].backGround.SetActive(false);
                backgrounds[1].terrain.SetActive(false);
                backgrounds[2].backGround.SetActive(false);
                backgrounds[2].terrain.SetActive(false);
            }
            else
            {
                AudioManager.instance.Play("BackGroundCountrySide");
                AudioManager.instance.Play("BackGroundCity+");

                backgrounds[0].backGround.SetActive(false);
                backgrounds[0].terrain.SetActive(false);
                backgrounds[1].backGround.SetActive(true);
                backgrounds[1].terrain.SetActive(true);
                backgrounds[2].backGround.SetActive(false);
                backgrounds[2].terrain.SetActive(false);
            }
        }
       
    }

    private void PlayTutorial()
    {
        if (!PlayerPrefs.HasKey(playedTutorialHowToShoot) && PlayerPrefs.GetInt("currentLevel") == 1 && PlayerPrefs.GetString("currentMode") == "Campaign")
        {
            Debug.Log("PLAYTUTORHOWTOSHOOT");
            TutorialWindow.SetActive(true);
            tutor1.SetActive(true);
            tutor2.SetActive(false);
        }
        if (!PlayerPrefs.HasKey(playedTutorialNewChallange) && PlayerPrefs.GetInt("currentLevel") == 15 && PlayerPrefs.GetString("currentMode") == "Campaign")
        {
            Debug.Log("PLAYTUTORNEWCHALLANGE");
            TutorialWindow.SetActive(true);
            tutor1.SetActive(false);
            tutor2.SetActive(true);
        }
    }
}

