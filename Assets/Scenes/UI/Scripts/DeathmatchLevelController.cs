using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathmatchLevelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentWaveText;
    [SerializeField] private Image lengthOfWave;
    [SerializeField] private Animation textOfNumberOfWave;
    [SerializeField] private GameObject bloodOnDeathParticle;
    public int EnemiesAlive;

    [Space]

    public bool isSpawningWave;

    [Space]

    public float cooldownAfterWave;

    public bool levelended;
    public float secondsPassed;

    private GameObject cameraFollow;
    private float startTime;

    private void FixedUpdate()
    {
        float LengthOfLevelInSeconds = GetComponent<DeathmatchSpawner>().LengthOfLevelInSeconds;

        if (isSpawningWave && !PlayerController.Lost && !levelended)
        {
            float t = Time.time - startTime;
            secondsPassed = t % 60;
        }
        else
            startTime = Time.time - secondsPassed;

        if (!levelended)
            lengthOfWave.fillAmount = secondsPassed / LengthOfLevelInSeconds;
        else
            lengthOfWave.fillAmount = 1f;

        if (secondsPassed >= LengthOfLevelInSeconds) // УРОВЕНЬ ЗАКАНЧИВАЕТСЯ ПО ВРЕМЕНИ
        {
            if (!levelended && !PlayerController.Lost)
            {
                Debug.Log("ADDING WAVE!!!!!!!!!!!!!!!");
                PlayerController.Wave++;

                // missions //
                if (PlayerController.leftHP == 1f && PlayerController.rightHP == 1f && PlayerController.centerHP == 1f)
                    timesUndamaged++;
                if (timesUndamaged >= PlayerPrefs.GetInt("MissionThirdAdd"))
                    PlayerPrefs.SetInt("MissionThirdValue", 1);

                timesPassed++;

                if (timesPassed >= PlayerPrefs.GetInt("MissionSecondAdd"))
                    PlayerPrefs.SetInt("MissionSecondValue", 1);
                if (timesPassed > PlayerPrefs.GetInt("maxWavesPassed"))
                {
                    newRecordSet = true;
                    PlayerPrefs.SetInt("maxWavesPassed", timesPassed);

                    PlayGamesManager.PostToLeaderboard(PlayerPrefs.GetInt("maxWavesPassed"), PlayGamesManager.LeaderboardID.MAXWAVESPASSED);
                    GameCenterManager.PostToLeaderboard(PlayerPrefs.GetInt("maxWavesPassed"), GameCenterManager.LeaderboardID.MAXWAVESPASSED);
                }

                if (timesPassed >= 30)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.UltimateSurviver);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.UltimateSurviver);
                }
                else if (timesPassed >= 15)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Surviver3);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Surviver3);
                }
                else if (timesPassed >= 10)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Surviver2);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Surviver2);
                }
                else if (timesPassed >= 5)
                {
                    GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Surviver1);
                    PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Surviver1);
                }

                RankManager.instance.AddXP(20);
                // missions //

                isSpawningWave = false;

                StartCoroutine(AfterWave());
                //cooldownAfterWave = 3f;

                GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");

                if (gos.Length > 0)
                foreach (GameObject go in gos)
                {
                        string nameOfSound = "BloodSplash" + Random.Range(1, 7).ToString();
                        AudioManager.instance.Play(nameOfSound);
                        Instantiate(bloodOnDeathParticle, go.GetComponent<Enemy>().CenterOfEnemy.position, Quaternion.identity);
                    go.transform.parent.gameObject.GetComponent<tabManager>().deleteTab();
                    Destroy(go.transform.parent.gameObject);
                        EnemiesAlive--;
                }

                IsFirstEnemy = true;
                levelended = true;
                
                secondsPassed = 0.0f;

                Debug.Log("WAVE ENDED!");
                Debug.Log("ENEMIES ALIVE: " + EnemiesAlive);
            }
        }

        /////
        //if (!isSpawningWave && !PlayerController.Lost)
        //    cooldownAfterWave -= Time.fixedDeltaTime;

        //if (cooldownAfterWave <= 0.0f && !isSpawningWave)
        //{
        //    cooldownAfterWave = 3f;
        //    isSpawningWave = true;
        //    levelended = false;

        //    Debug.Log("WAVE STARTED!");

        //    PlayerController.IncreaseIfNeededHpSchool(0.1f);

        //    AudioManager.instance.Play("Transition2");
        //    currentWaveText.text = "WAVE " + PlayerController.Wave.ToString();

        //    textOfNumberOfWave.gameObject.GetComponent<TextMeshProUGUI>().text = "WAVE " + PlayerController.Wave.ToString() + "!";
        //    textOfNumberOfWave.Play();
        //}
        /////
    }

    IEnumerator AfterWave()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<DeathmatchSpawner>().CalculateLengthOfLevelInSeconds();
        isSpawningWave = true;
        levelended = false;

        Debug.Log("WAVE STARTED!");

        PlayerController.IncreaseIfNeededHpSchool(0.1f);

        AudioManager.instance.Play("Transition2");
        currentWaveText.text = "WAVE " + PlayerController.Wave.ToString();

        textOfNumberOfWave.gameObject.GetComponent<TextMeshProUGUI>().text = "WAVE " + PlayerController.Wave.ToString() + "!";
        textOfNumberOfWave.Play();
    }

    private void Start()
    {
        PlayerController.Wave = 1;

        EnemiesAlive = 0;
        cooldownAfterWave = 3f;
        lengthOfWave.fillAmount = 0f;
        startTime = Time.time;

        levelended = false;
        isSpawningWave = true;

        newRecordSet = false;

        cameraFollow = GameObject.FindGameObjectWithTag("cameraFollow");
        if (cameraFollow == null) Debug.LogError("CAN NOT FIND CAMERA FOLLOW! ");

        StartCoroutine(SpawnDeathMatch());
    }

    private bool IsFirstEnemy = true;
    IEnumerator SpawnDeathMatch()
    {
        while (true)
        {
            if (IsFirstEnemy)
            {
                yield return new WaitForSeconds(0.05f);
                IsFirstEnemy = false;
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(3.5f, 4f));
            }
            
            if (!levelended && !PlayerController.Lost)
            {
                GetComponent<DeathmatchSpawner>().SpawnRandomly();
            }
        }
    }

    //for missions
    private int timesUndamaged = 0;
    private int timesPassed = 0;

    public static bool newRecordSet = false;
}
