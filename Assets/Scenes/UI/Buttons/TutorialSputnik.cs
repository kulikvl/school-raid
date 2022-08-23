using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialSputnik : MonoBehaviour
{
    public bool playTutorial;
    public Collider2D[] colsToManage;
    public GameObject TutorLeftTab;
    public GameObject enemySchoolBoy;
    public GameObject forSputnikTutor;

    [HideInInspector] public bool isReadyToPressSputnik;
    [HideInInspector] public bool isReadyToPressAbility;
    [HideInInspector] public bool isReadyToSetAbility;
    [HideInInspector] public bool isReadyToKill;

    public static TutorialSputnik instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void ManageCols(bool param)
    {
        foreach (Collider2D col in colsToManage) col.enabled = param;
    }
    private void FreezeTheGame(bool ToFreeze)
    {
        float value;
        if (ToFreeze) value = 0f;
        else value = 1f;

        Time.timeScale = value;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void DarkerGameObject(Transform go)
    {
        Animator animator = go.gameObject.GetComponentInChildren<Animator>();
        animator.SetTrigger("start");
    }
    private void LighterGameObject(Transform go)
    {
        Animator animator = go.gameObject.GetComponentInChildren<Animator>();
        animator.SetTrigger("IFPRESSED");
    }
    private void DisableAnimatorFromGameObject(Transform go)
    {
        Animator animator = go.gameObject.GetComponentInChildren<Animator>();
        animator.enabled = false;
    }

    private void Start()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        isReadyToPressAbility = false;
        isReadyToPressSputnik = false;
        isReadyToSetAbility = false;
        isReadyToKill = false;

        GameObject LevelLength = GameObject.FindGameObjectWithTag("LevelLength");

        if (PlayerPrefs.GetInt("currentLevel") == 2 && !PlayerPrefs.HasKey("showedIntroSputnik2") && PlayerPrefs.GetString("currentMode") == "Campaign" && playTutorial)
        {
            Debug.Log("TUTOR SPUTNIK STARTED!");

            sputnikCount.count = 6;

            transform.GetChild(2).gameObject.SetActive(true);
            GetComponent<Animator>().SetTrigger("start");
            SettingsInGame.enabledToPressSettings = false;
            SettingsInGame.GameIsFreezed = true;

            TutorLeftTab.SetActive(true);
            TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
            TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("PLAY");

            TutorLeftTab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR0_0");
            if (LocalisationSystem.language == LocalisationSystem.Language.Russian) TutorLeftTab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().fontSize = 52f;
            TutorLeftTab.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR1_0");
            if (LocalisationSystem.language == LocalisationSystem.Language.Russian) TutorLeftTab.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().fontSize = 68f;

            FindObjectOfType<campaignLevelController>().enabled = false;
            FindObjectOfType<FireButton>().enabled = false;

            DarkerGameObject(transform.GetChild(3));
            DarkerGameObject(transform.GetChild(4));
            DarkerGameObject(transform.GetChild(5));

            isReadyToPressSputnik = true;

            LevelLength.transform.GetChild(3).gameObject.SetActive(false);
            LevelLength.transform.GetChild(5).gameObject.SetActive(false);
            LevelLength.transform.GetChild(6).gameObject.SetActive(false);
            LevelLength.transform.GetChild(7).gameObject.SetActive(false);
            LevelLength.transform.GetChild(8).gameObject.SetActive(false);
            LevelLength.transform.GetChild(9).gameObject.SetActive(false);
            LevelLength.transform.GetChild(10).gameObject.SetActive(true);

            ManageCols(false);
            FreezeTheGame(true);
        }
        else if (PlayerPrefs.GetInt("currentLevel") == 2 && PlayerPrefs.GetString("currentMode") == "Campaign" && playTutorial)
        {
            StartCoroutine(goTutor3());
        }
        else if (PlayerPrefs.GetInt("currentLevel") == 3 && PlayerPrefs.GetString("currentMode") == "Campaign" && playTutorial)
        {
            StartCoroutine(goTutor3());
        }
        else
        {
            TutorLeftTab.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            GetComponent<Animator>().enabled = false;
            DisableAnimatorFromGameObject(transform.GetChild(3));
            DisableAnimatorFromGameObject(transform.GetChild(4));
            DisableAnimatorFromGameObject(transform.GetChild(5));
            DisableAnimatorFromGameObject(transform.GetChild(6));
        }
    }

    IEnumerator goTutor3()
    {
        SettingsInGame.enabledToPressSettings = false;
        yield return new WaitForSeconds(1f);
        if (sputnikCount.count > 3)
        {
            transform.GetChild(2).gameObject.SetActive(true);
            GetComponent<Animator>().SetTrigger("start");
            Debug.Log("TUTOR SPUTNIK STARTED 3!");

            SettingsInGame.enabledToPressSettings = false;
            SettingsInGame.GameIsFreezed = true;

            DarkerGameObject(transform.GetChild(3));
            DarkerGameObject(transform.GetChild(4));
            DarkerGameObject(transform.GetChild(5));

            isReadyToPressSputnik = true;

            ManageCols(false);
            FreezeTheGame(true);
        }
        else
        {
            SettingsInGame.enabledToPressSettings = true;
        }
    }

    IEnumerator DisableTutorialDark()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    [HideInInspector] public GameObject firstSputnik;
    [HideInInspector] public bool activateCameraOnSputnik = false;

    public void IfPressedSputnik3()
    {
        isReadyToPressSputnik = false;
        ManageCols(true);

        FindObjectOfType<Player>().Enablejoystick(true);

        FreezeTheGame(false);
        SettingsInGame.enabledToPressSettings = true;
        SettingsInGame.GameIsFreezed = false;

        GetComponent<Animator>().SetTrigger("IFPRESSED");
        StartCoroutine(DisableTutorialDark());
        LighterGameObject(transform.GetChild(3));
        LighterGameObject(transform.GetChild(4));
        LighterGameObject(transform.GetChild(5));
    }

    public void IfPressedSputnik()
    {
        isReadyToPressSputnik = false;

        FindObjectOfType<Player>().Enablejoystick(true);

        FreezeTheGame(false);
        SettingsInGame.enabledToPressSettings = false;
        SettingsInGame.GameIsFreezed = false;

        StartCoroutine(DisableTutorialDark());
        LighterGameObject(transform.GetChild(3));
        LighterGameObject(transform.GetChild(4));
        LighterGameObject(transform.GetChild(5));

        GetComponent<Animator>().SetTrigger("IFPRESSED");
        TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("BACK");
        TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("BACK");

        // CAMERA ACTIONS

        firstSputnik = GameObject.FindGameObjectWithTag("sputnik");

        activateCameraOnSputnik = true;

        StartCoroutine(DisableCameraOnSputnik());
    }

    IEnumerator DisableCameraOnSputnik()
    {
        FindObjectOfType<Player>().Enablejoystick(false);
        yield return new WaitForSeconds(4f);
        activateCameraOnSputnik = false;
        StartCoroutine(ActivateAnimationToCollectCoins());
    }

    IEnumerator ActivateAnimationToCollectCoins()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        SettingsInGame.enabledToPressSettings = false;
        SettingsInGame.GameIsFreezed = true;
        FreezeTheGame(true);

        DarkerGameObject(transform.GetChild(3));
        DarkerGameObject(transform.GetChild(4));
        DarkerGameObject(transform.GetChild(5));
        DarkerGameObject(transform.GetChild(6));

        transform.GetChild(2).gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("start");
        TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
        TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
        TutorLeftTab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR0_1");
        TutorLeftTab.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR1_1");
        if (LocalisationSystem.language == LocalisationSystem.Language.Russian) TutorLeftTab.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().fontSize = 61f;

        yield return new WaitForSecondsRealtime(2f);

        yield return new WaitForSecondsRealtime(0.5f);

        GetComponent<Animator>().SetTrigger("IFPRESSED");

        yield return new WaitForSecondsRealtime(0.5f);

        transform.GetChild(2).gameObject.SetActive(false);
        TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("BACK");
        TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("BACK");
        FindObjectOfType<Player>().Enablejoystick(true);

        FreezeTheGame(false);
        SettingsInGame.enabledToPressSettings = false;
        SettingsInGame.GameIsFreezed = false;

        readyToCountCoins = true;
    }

    [HideInInspector] public bool readyToCountCoins = false;
    private int coinsTaken;
    public int CoinsTaken
    {
        get { return coinsTaken; }
        set
        {
            coinsTaken = value;

            if (coinsTaken == 3)
            {
                StartCoroutine(ActivateTheAbility());
            }
        }
    }

    IEnumerator ActivateTheAbility()
    {
        yield return new WaitForSecondsRealtime(1f);

        colsToManage[0].enabled = true;

        isReadyToPressAbility = true;

        SettingsInGame.enabledToPressSettings = false;
        SettingsInGame.GameIsFreezed = true;
        FreezeTheGame(true);

        LighterGameObject(transform.GetChild(3));

        transform.GetChild(2).gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("start");
        TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
        TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
        TutorLeftTab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR0_2");
        TutorLeftTab.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR1_2");
    }

    public void IfPressedAbility()
    {
        isReadyToPressAbility = false;

        colsToManage[0].enabled = false;

        DarkerGameObject(transform.GetChild(3));

        FreezeTheGame(false);
        SettingsInGame.enabledToPressSettings = false;
        SettingsInGame.GameIsFreezed = false;

        StartCoroutine(DisableTutorialDark());

        isReadyToSetAbility = true;

        GetComponent<Animator>().SetTrigger("IFPRESSED");
        TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("BACK");
        TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("BACK");

        StartCoroutine(ActivateAbilityAnimation());
    }

    IEnumerator ActivateAbilityAnimation()
    {
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        yield return new WaitForSecondsRealtime(1f);

        forSputnikTutor.SetActive(true);
        SettingsInGame.enabledToPressSettings = false;
        SettingsInGame.GameIsFreezed = true;
        FreezeTheGame(true);

        transform.GetChild(2).gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("start");
        TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
        TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
        TutorLeftTab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR0_3");
        TutorLeftTab.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR1_3");

        yield return new WaitForSecondsRealtime(2f);

        yield return new WaitForSecondsRealtime(0.5f);

        GetComponent<Animator>().SetTrigger("IFPRESSED");

        yield return new WaitForSecondsRealtime(0.5f);

        transform.GetChild(2).gameObject.SetActive(false);
        TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("BACK");
        TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("BACK");

        FreezeTheGame(false);
        SettingsInGame.enabledToPressSettings = false;
        SettingsInGame.GameIsFreezed = false;
    }

    private GameObject player;

    public void AfterSettingAbility(Vector3 pos)
    {
        forSputnikTutor.SetActive(false); 
        FindObjectOfType<Player>().Enablejoystick(false);
        FindObjectOfType<Player>().freeze = true;
        FindObjectOfType<Player>().y = FindObjectOfType<Player>().parachute.transform.position.y + 3f;
        player = GameObject.FindGameObjectWithTag("PLAYER");

        StartCoroutine(SpawnEnemies(pos));

        GameObject container = FindObjectOfType<School>().transform.parent.gameObject;
        Collider2D[] cols = container.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in cols) col.enabled = false;

        StartCoroutine(FinalMove());
    }

    IEnumerator SpawnEnemies(Vector3 pos)
    {
        GameObject prefab;

        for (int i = 0; i < 3; ++i)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            if (FindObjectOfType<BusController>().gameObject.transform.position.x > -0.25f) // right
            {
                Debug.Log(pos);
                prefab = Instantiate(enemySchoolBoy, new Vector3(pos.x + 3f, pos.y + 0.0f), Quaternion.identity);
                prefab.name = prefab.name + "Right";
                if (i == 2) prefab.name += "FINISHER";
                prefab.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                prefab = Instantiate(enemySchoolBoy, new Vector3(pos.x - 3f, pos.y + 0.0f), Quaternion.identity);
                if (i == 2) prefab.name += "FINISHER";
            }

            prefab.transform.GetChild(0).gameObject.GetComponent<Enemy>().damage = 0.005f;
        }
        
    }

    IEnumerator FinalMove()
    {
        yield return new WaitForSeconds(3f);

        FindObjectOfType<FireButton>().enabled = true;
        FindObjectOfType<Player>().Enablejoystick(true);
        FindObjectOfType<Player>().freeze = false;

        isReadyToKill = true;

        TutorLeftTab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("PLAY");
        TutorLeftTab.transform.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("PLAY");

        TutorLeftTab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR0_4");
        if (LocalisationSystem.language == LocalisationSystem.Language.Russian) TutorLeftTab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().fontSize = 75f;
        TutorLeftTab.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextLocaliserUI>().SetValue("TUTOR1_4");
    }

    private bool startedProcessing = false;
    public void AfterKilling()
    {
        if (!startedProcessing)
       StartCoroutine(IAfterKilling());
    }
    IEnumerator IAfterKilling()
    {
        startedProcessing = true;
        FindObjectOfType<FireButton>().enabled = false;

        yield return new WaitForSeconds(2f);

        // passed tutor
        PlayerPrefs.SetInt("showedIntroSputnik2", 1);

        FindObjectOfType<Player>().DisableBusControls();
        PlayerPrefs.SetInt("currentLevel", 2);

        AudioManager.instance.StopAllSounds();

        GameObject g = GameObject.FindGameObjectWithTag("tent");
        g.GetComponent<Animator>().SetTrigger("DARK");

        yield return new WaitForSeconds(0.5f);

        PlayerController.SetFullHpSchool();

        SceneManager.LoadScene("Scene");
    }
}
