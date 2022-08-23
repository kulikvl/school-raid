using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroController : MonoBehaviour
{
    private enum EnemiesToPlay
    {
        EnemyStandart = 0,
        SchoolBoy = 1,
        EnemyBig = 2,
        EnemyHimichka = 3,
        EnemyKarlson = 4,
        EnemyInformatik = 5,
        EnemyFizik = 6,
        EnemyKachok = 7,
        SchoolBoyFat = 8,
        EnemyVeteran = 9,
        EnemyKrasotka = 10,
        EnemyFizruk = 11,
        EnemyDirector = 12,
        All = 13,
    }

    public GameObject[] enemies;

    public GameObject enemiesThings;

    public TextLocaliserUI Name, Description, Ability;

    public Animation _camera;

    private void Awake()
    {
        //PlayerPrefs.SetInt("currentLevel", 9);

        if (AudioManager.instance.isPlaying("MainTheme"))
            AudioManager.instance.Stop("MainTheme");

        AudioManager.instance.Play("BackGroundHalloween");

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

        if (((float)Screen.width / Screen.height) < 1.5f)
            _camera.Play("IntroIpad");
        else
            _camera.Play("Intro");

        var prefab = Instantiate(enemies[GetCurrentEnemy()], transform.position, Quaternion.identity, gameObject.transform);
        prefab.GetComponent<Animator>().SetTrigger("Intro");

        // turn off unnecessary settings
        prefab.GetComponent<tabManager>().enabled = false;

        Transform[] trs = prefab.GetComponentsInChildren<Transform>();
        foreach (var ob in trs)
        {
            if (ob.gameObject.name == "Logic")
            {
                ob.GetComponent<Enemy>().ableToRun = false;
            }
            if (ob.gameObject.name == "CheckCollisionWithBus")
            {
                ob.GetComponent<CheckCollision>().enabled = false;
            }
            if (ob.gameObject.name == "Hips" && ob.gameObject.GetComponent<ShootEnemy1>() != null)
            {
                ob.gameObject.GetComponent<ShootEnemy1>().enabled = false;
            }
        }

        if (prefab.name.Contains("Fizruk"))
        {
            prefab.GetComponent<PartsOfEnemyFizruk>().logicGameObject.GetComponent<EnemyFizrukController>().enabled = false;
            prefab.GetComponent<Animator>().enabled = false;
            prefab.GetComponent<Animation>().Play();
        }
        if (prefab.name.Contains("Fizik"))
        {
            prefab.transform.GetChild(0).GetComponent<Enemy>().enabled = false;
        }

    }

    private int GetCurrentEnemy()
    {
        int curLevel = PlayerPrefs.GetInt("currentLevel");
        int enemyNumber = -1;

        switch (curLevel)
        {
            case 1:
                enemyNumber = (int)EnemiesToPlay.EnemyStandart; //

                Name.SetValue("EnemyStandartName");
                Description.SetValue("EnemyStandartDescription");
                Ability.SetValue("EnemyStandartAbility");

                break;

            case 2:
                enemyNumber = (int)EnemiesToPlay.SchoolBoy;

                Name.SetValue("SchoolBoyName");
                Description.SetValue("SchoolBoyDescription");
                Ability.SetValue("SchoolBoyAbility");

                break;

            case 4:
                enemyNumber = (int)EnemiesToPlay.EnemyBig;

                Name.SetValue("EnemyBigName");
                Description.SetValue("EnemyBigDescription");
                Ability.SetValue("EnemyBigAbility");

                break;

            case 6:
                enemyNumber = (int)EnemiesToPlay.EnemyHimichka;

                Name.SetValue("EnemyHimichkaName");
                Description.SetValue("EnemyHimichkaDescription");
                Ability.SetValue("EnemyHimichkaAbility");

                break;

            case 9:
                enemyNumber = (int)EnemiesToPlay.EnemyKarlson;

                Name.SetValue("EnemyKarlsonName");
                Description.SetValue("EnemyKarlsonDescription");
                Ability.SetValue("EnemyKarlsonAbility");

                break;

            case 13:
                enemyNumber = (int)EnemiesToPlay.EnemyInformatik;

                Name.SetValue("EnemyInformatikName");
                Description.SetValue("EnemyInformatikDescription");
                Ability.SetValue("EnemyInformatikAbility");

                break;

            case 18:
                enemyNumber = (int)EnemiesToPlay.EnemyFizik; // 

                Name.SetValue("EnemyFizikName");
                Description.SetValue("EnemyFizikDescription");
                Ability.SetValue("EnemyFizikAbility");

                break;

            case 21:
                enemyNumber = (int)EnemiesToPlay.EnemyKachok;

                Name.SetValue("EnemyKachokName");
                Description.SetValue("EnemyKachokDescription");
                Ability.SetValue("EnemyKachokAbility");

                break;

            case 25:
                enemyNumber = (int)EnemiesToPlay.SchoolBoyFat;

                Name.SetValue("SchoolBoyFatName");
                Description.SetValue("SchoolBoyFatDescription");
                Ability.SetValue("SchoolBoyFatAbility");

                break;

            case 29:
                enemyNumber = (int)EnemiesToPlay.EnemyVeteran;

                Name.SetValue("EnemyVeteranName");
                Description.SetValue("EnemyVeteranDescription");
                Ability.SetValue("EnemyVeteranAbility");

                break;

            case 33:
                enemyNumber = (int)EnemiesToPlay.EnemyKrasotka;

                Name.SetValue("EnemyKrasotkaName");
                Description.SetValue("EnemyKrasotkaDescription");
                Ability.SetValue("EnemyKrasotkaAbility");

                break;

            case 37:
                enemyNumber = (int)EnemiesToPlay.EnemyFizruk;

                Name.SetValue("EnemyFizrukName");
                Description.SetValue("EnemyFizrukDescription");
                Ability.SetValue("EnemyFizrukAbility");

                break;

            case 40:
                enemyNumber = (int)EnemiesToPlay.EnemyDirector;

                Name.SetValue("EnemyDirectorName");
                Description.SetValue("EnemyDirectorDescription");
                Ability.SetValue("EnemyDirectorAbility");

                break;

            default:

                SceneManager.LoadScene("Scene");
                break;

        }

        if (enemyNumber != -1) ActivateEnemyThings(enemyNumber);
        else enemyNumber = 0;

        return enemyNumber;
    }

    private void ActivateEnemyThings(int value)
    {
        enemiesThings.transform.GetChild(value).gameObject.SetActive(true);
    }
}
