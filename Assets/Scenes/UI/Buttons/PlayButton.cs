using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public Transform play;
    public Animation lightbackground, Modes;
    public GameObject ContainerOfColliders;
    public GameObject bus;

    public Animation ExitButton;
    public Animation SettingsButton;

    public BusMenu busMenu;

    private void Awake()
    {
        CheckIfFirstTime();
        Application.targetFrameRate = 60;

        if (!PlayerPrefs.HasKey("LastWatchedAd")) PlayerPrefs.SetString("LastWatchedAd", DateTime.Now.ToString());

        // todo
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
    }
    private void Start()
    {
        if (!AudioManager.instance.isPlaying("MainTheme"))
            AudioManager.instance.Play("MainTheme");

            AudioManager.instance.Stop("BackGroundHalloween");
            AudioManager.instance.Stop("BackGroundHalloween+");
            AudioManager.instance.Stop("BackGroundCity");
            AudioManager.instance.Stop("BackGroundCity+");
            AudioManager.instance.Stop("BackGroundCountrySide");
            AudioManager.instance.Stop("BackGroundCountrySide+");
            AudioManager.instance.Stop("Director2");

    }

    private void CheckIfFirstTime()
    {
        if (!PlayerPrefs.HasKey("FirstTime11"))
        {
            for (int currentLevel = 1; currentLevel <= 40; ++currentLevel)
            {
                PlayerPrefs.SetInt("bestStarsOnLevel" + currentLevel.ToString(), 0);
                PlayerPrefs.SetInt("currentStarsOnLevel" + currentLevel.ToString(), 0);
            }
            PlayerPrefs.SetInt("FirstTime11", 1);
        }
        if (!PlayerPrefs.HasKey("FirstTime10"))
        {

            // TODO
            PlayerPrefs.DeleteAll();

            // todo
            PlayerPrefs.SetInt("OrdinaryChestCount", 3);
          
            // settings
            PlayerPrefs.SetString("Sound", "ON");

            if (iOSHapticFeedback.Instance.IsSupported())
            PlayerPrefs.SetString("Vibration", "ON");
            else
            PlayerPrefs.SetString("Vibration", "OFF");

            PlayerPrefs.SetString("Music", "ON");
            // settings

            PlayerPrefs.SetInt("Coins", 100);
   
            PlayerPrefs.SetInt("currentModel", 0);
            PlayerPrefs.SetInt("chestsOpened", 0);

            PlayerPrefs.SetInt("maxWavesPassed", 0);

            //todo
            PlayerPrefs.SetInt("unlockedLevels", 1);
            PlayerPrefs.SetInt("currentLevel", 1);

            for (int currentLevel = 1; currentLevel <= 40; ++currentLevel)
            {
                PlayerPrefs.SetInt("bestStarsOnLevel" + currentLevel.ToString(), 0);
                PlayerPrefs.SetInt("currentStarsOnLevel" + currentLevel.ToString(), 0);
            }
               

            PlayerPrefs.SetInt("busesBought", 1);
            PlayerPrefs.SetString("0", "Bought");

            PlayerPrefs.SetInt("currentRank", 1);
            PlayerPrefs.SetInt("currentXP", 0);

            for (int i = 0; i < 5; ++i)
            {
                PlayerPrefs.SetString((i.ToString() + "DEFAULT"), "Bought");
                PlayerPrefs.SetInt((i.ToString() + "currentAlteration"), 0);
            }

            Debug.Log("FIRST TIME!");
            PlayerPrefs.SetInt("FirstTime10", 1);
        }
    }

    ////////////////////////////////

    private void OnMouseUpAsButton()
    {
        if (BusMenu.isSlowMotion) busMenu.ExitSlowMotion();

        if (!PlayerPrefs.HasKey("FilmPlayed"))
        {
            StartCoroutine(FilmPlay());
        }
        else
        {
            StartCoroutine(AllObjectsPlayAnimation());
        }
       
    }

    IEnumerator FilmPlay()
    {
        GameObject g = GameObject.FindGameObjectWithTag("tent");

        if (g != null) g.GetComponent<Animation>().Play("darkerTest");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Film");
        //PlayerPrefs.SetInt("FilmPlayed", 1);
    }

    public IEnumerator AllObjectsPlayAnimation()
    {
        play.position = new Vector3(play.position.x, play.position.y + 200f, play.position.z);

        Modes.Play("ShopSwipe1");

        SettingsButton.Play();
        ExitButton.Play();

        lightbackground.Play();

        yield return new WaitForSeconds(0.5f);
        Destroy(bus);
        Destroy(ContainerOfColliders);
    }

}
