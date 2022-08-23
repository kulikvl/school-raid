using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public Animation exitButton, coins, backGround;
    public GameObject[] gosToMove;
    public Transform pointToMove;

    public bool moveUI = false;

    [System.Serializable]
    public struct DeathMatchPrefabs
    {
        public GameObject Container;
        public TextMeshProUGUI coinsRewardTxt;
        public TextMeshProUGUI chestRewardTxt;
        public TextMeshProUGUI greetingsTxt;
    }
    public DeathMatchPrefabs deathmatch;

    [System.Serializable]
    public struct CampaignPrefabs
    {
        public Transform[] stars;
        public GameObject Container;
        public TextMeshProUGUI coinsRewardTxt;
        public TextMeshProUGUI chestRewardTxt;
        public GameObject EpicVictory;
    }
    public CampaignPrefabs campaign;

    [HideInInspector] public bool isPassedLevel = false;

    private bool starsPlayed = false;

    public void SetFinalMenu()
    {
        StartCoroutine(IMenu());
    }
    private void Start()
    {
        FinalMenuIsStarted = false;
    }

    private void OnMouseUpAsButton()
    {
        if (starsPlayed && PlayerPrefs.GetString("currentMode") == "Campaign")
        {
            if (PlayerPrefs.GetInt("currentLevel") == 40)
            {
                StartCoroutine(GoToMenu());
            }
            else
                StartCoroutine(NextLevel());
        }
        else
            StartCoroutine(NextLevel());

    }
    IEnumerator GoToMenu()
    {
        GameObject g = GameObject.FindGameObjectWithTag("tent");
        g.GetComponent<Animator>().SetTrigger("DARK");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        if (moveUI)
        {
            foreach(GameObject go in gosToMove)
            {
                Vector3 vec = new Vector3(go.transform.position.x, pointToMove.position.y);

                go.transform.position = Vector3.MoveTowards(go.transform.position, vec, 2f * Time.deltaTime);
            }
        }
    }

    public static bool FinalMenuIsStarted;

    private IEnumerator IMenu()
    {
        FinalMenuIsStarted = true;

        backGround.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        coins.gameObject.SetActive(true);

        GetComponent<Animation>().Play();
        backGround.Play();
        exitButton.Play();
        coins.Play();

        AudioManager.instance.Play("Victory");

        yield return new WaitForSeconds(0.5f);

        if (PlayerPrefs.GetString("currentMode") == "Campaign")
        {
            campaign.Container.SetActive(true);

            StartCoroutine(SetStars());
            campaign.EpicVictory.SetActive(true);

            campaign.coinsRewardTxt.text = "+ " + "0";

            GameObject rushController = GameObject.FindGameObjectWithTag("rushController");
            //int reward = rushController.GetComponent<LevelOrganizer>().CurrentLevel.coinReward;

            if (isPassedLevel) // chest giving
            {
                if (rushController.GetComponent<LevelOrganizer>().CurrentLevel.chestRewardAvailable)
                {
                    PlayerPrefs.SetInt("OrdinaryChestCount", PlayerPrefs.GetInt("OrdinaryChestCount") + 1);
                    campaign.chestRewardTxt.text = "CHEST X1";
                }
                else campaign.chestRewardTxt.text = "CHEST X0";  
            }
            else
            {
                campaign.chestRewardTxt.text = "CHEST X0";
            }

            //local Achievements
            BusController bus = FindObjectOfType<BusController>();
            if (bus.Health >= bus.MaxHealth) LocalAchievementsManager.instance.ActivateLocalAchievement(LocalAchievementsManager.LocalAchievements.PRISTINE, true);
            if (PlayerController.leftHP >= 1.0f && PlayerController.rightHP >= 1.0f && PlayerController.centerHP >= 1.0f) LocalAchievementsManager.instance.ActivateLocalAchievement(LocalAchievementsManager.LocalAchievements.UNDAMAGED, true);
            if (BusController.TimesShooted == 0) LocalAchievementsManager.instance.ActivateLocalAchievement(LocalAchievementsManager.LocalAchievements.PACIFIST, true);
            if (PlayerPrefs.GetInt("currentLevel") > 1 && SelectAbility.TimesAbilityActivated == 0) LocalAchievementsManager.instance.ActivateLocalAchievement(LocalAchievementsManager.LocalAchievements.HARDCORE, true);
            if (SelectItem.putniksSetted >= 3) LocalAchievementsManager.instance.ActivateLocalAchievement(LocalAchievementsManager.LocalAchievements.COLLECTOR, true);
            if (BusController.MaxMultiKillCount != 0) LocalAchievementsManager.instance.ActivateLocalAchievementMULTIKILL(BusController.MaxMultiKillCount, true);

            //int num = Random.Range(0, 4);
            //if (num == 0) greetingsTxt.text = "YOU WON THIS MATCH!";
            //if (num == 1) greetingsTxt.text = "CONGRATULATIONS!";
            //if (num == 2) greetingsTxt.text = "LEVEL CLEARED!";
            //if (num == 3) greetingsTxt.text = "WINNER! WINNER!";

        }
        else
        {
            deathmatch.Container.SetActive(true);

            int reward = 0;

            if (DeathmatchLevelController.newRecordSet)
            {
                int unlockedLevels = PlayerPrefs.GetInt("unlockedLevels");

                reward = PlayerPrefs.GetInt("maxWavesPassed") * unlockedLevels * 10;

                PlayerPrefs.SetInt("OrdinaryChestCount", PlayerPrefs.GetInt("OrdinaryChestCount") + 1);

                deathmatch.chestRewardTxt.text = "CHEST X1";
                deathmatch.coinsRewardTxt.text = "+ " + reward;

                RankManager.instance.AddXP(100);

                int num = Random.Range(0, 4);
                if (num == 0) deathmatch.greetingsTxt.text = "WOW! NEW RECORD: " + PlayerPrefs.GetInt("maxWavesPassed").ToString();
                if (num == 1) deathmatch.greetingsTxt.text = "YEAH! NEW SCORE: " + PlayerPrefs.GetInt("maxWavesPassed").ToString();
                if (num == 2) deathmatch.greetingsTxt.text = "EXCELLENT! NEW RECORD: " + PlayerPrefs.GetInt("maxWavesPassed").ToString();
                if (num == 3) deathmatch.greetingsTxt.text = "WINNER! NEW SCORE: " + PlayerPrefs.GetInt("maxWavesPassed").ToString();
            }
            else
            {
                RankManager.instance.AddXP(20);

                if (PlayerPrefs.GetInt("unlockedLevels") < 20)
                reward = PlayerController.Wave * 5;
                else
                    reward = PlayerController.Wave * 10;

                deathmatch.chestRewardTxt.text = "CHEST X0";
                deathmatch.coinsRewardTxt.text = "+ " + reward;

                int num = Random.Range(0, 3);
                if (num == 0) deathmatch.greetingsTxt.text = "BETTER LUCK NEXT TIME!";
                if (num == 1) deathmatch.greetingsTxt.text = "THAT'S STILL DECENT!";
                if (num == 2) deathmatch.greetingsTxt.text = "GOOD!";
            }

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + reward);
        }  
    }

    private IEnumerator SetStars()
    {
        yield return new WaitForSeconds(0.5f);

        int currentLevel = PlayerPrefs.GetInt("currentLevel");

        int reward = 0;
        campaign.coinsRewardTxt.text = "+ " + reward;

        Debug.Log("CONTINUEBUTTON: " + PlayerPrefs.GetInt("currentStarsOnLevel" + currentLevel.ToString()));

        for (int i = 1; i <= 3; ++i)
        {
            if (PlayerPrefs.GetInt("currentStarsOnLevel" + currentLevel.ToString()) >= i)
            {
                campaign.stars[i - 1].GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
                yield return new WaitForSeconds(0.1f);
                AudioManager.instance.Play("Star" + i.ToString());
                yield return new WaitForSeconds(0.1f);
                
                campaign.stars[i - 1].GetChild(1).gameObject.SetActive(true);

                if (PlayerPrefs.GetString("gotPriceStar" + i.ToString() + "Level" + currentLevel.ToString()) != "true")
                {
                    reward += 100;
                    RankManager.instance.AddXP(25);
                    PlayerPrefs.SetString("gotPriceStar" + i.ToString() + "Level" + currentLevel.ToString(), "true");
                }

                campaign.coinsRewardTxt.text = "+ " + reward;

                yield return new WaitForSeconds(0.4f);
            }
        }

        if (reward == 0) reward = 25;
        campaign.coinsRewardTxt.text = "+ " + reward;

        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + reward);

        yield return new WaitForSeconds(0.1f);

        starsPlayed = true;
    }


    private IEnumerator NextLevel()
    {
        if (PlayerPrefs.GetString("currentMode") == "Campaign" && PlayerPrefs.GetInt("currentLevel") < 40)
            PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel") + 1);

        AudioManager.instance.StopAllSounds();

        GameObject g = GameObject.FindGameObjectWithTag("tent");
        g.GetComponent<Animator>().SetTrigger("DARK");

        yield return new WaitForSeconds(0.5f);

        PlayerController.SetFullHpSchool();

        if (PlayerPrefs.GetString("currentMode") == "Campaign")
            SceneManager.LoadScene("Intro");
        else
            SceneManager.LoadScene("Scene");
    }
}
