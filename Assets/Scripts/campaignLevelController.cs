using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(LevelOrganizer))]
public class campaignLevelController : MonoBehaviour
{
    public LevelOrganizer levelOrganizer;

    [SerializeField] private GameObject[] enemies;

    [Space]

    [SerializeField] private Image lengthOfLevel;
    [SerializeField] private Transform leftPoint, rightPoint;
    [SerializeField] private Animation textOfWaveIsComing;
    private GameObject cameraFollow;

    [Space]

    public bool isSpawningWave;

    public static bool AllowedToSpawnEnemiesOrdinary = true;

    [Space]

    public bool levelended;
    public bool allowedToSpawnOrdinary;

    public float cooldownBeforeWave;
    public float cooldownAfterWave;

    public bool waitedBeforeWave = true;
    public bool waitedAfterWave = true;

    private void CreateEnemy(int num)
    {
        GameObject prefab;

        if (Random.Range(0, 2) == 0 && num != 12) // right
        {
            prefab = Instantiate(enemies[num], rightPoint.position, Quaternion.identity);
            prefab.name = prefab.name + "Right";
            prefab.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
            prefab = Instantiate(enemies[num], leftPoint.position, Quaternion.identity);


        SpriteRenderer[] spr = prefab.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sp in spr)
        {
            sp.gameObject.GetComponent<SpriteRenderer>().sortingOrder += levelOrganizer.overallAmountOfEnemies * 300;
        }

        levelOrganizer.overallAmountOfEnemies++;
    }

    public int EnemiesInCurrentWave;

    private void FixedUpdate()
    {
        //todo
        allowedToSpawnOrdinary = AllowedToSpawnEnemiesOrdinary;
        EnemiesInCurrentWave = levelOrganizer.CurrentLevel.EnemiesInCurrentWave;

        float t = Time.time - startTime;
        secondsPassed = t % 60;

        CheckForSpawningWave();

        ///// todo check win loose
        if (!waitedBeforeWave && !levelended && !PlayerController.Lost && !PlayerController.Won)
            cooldownBeforeWave -= Time.deltaTime;

        if (!waitedAfterWave && !levelended && !PlayerController.Lost && !PlayerController.Won)
            cooldownAfterWave -= Time.deltaTime;

        if (cooldownBeforeWave <= 0.0f && !waitedBeforeWave)
        {
            cooldownBeforeWave = 6f;

            if (!levelended)
            {
                AudioManager.instance.Play("Transition1");
                textOfWaveIsComing.Play();
            }

            waitedBeforeWave = true;
        }

        if (cooldownAfterWave <= 0.0f && !waitedAfterWave)
        {
            cooldownAfterWave = 5f;
            AllowedToSpawnEnemiesOrdinary = true;
            waitedAfterWave = true;
        }

        ///
        if (!levelended)
        {
            if (levelOrganizer.currentAmountOfEnemiesOrdinary == 0)
                lengthOfLevel.fillAmount = 0f;
            else
                lengthOfLevel.fillAmount = (float)levelOrganizer.currentAmountOfEnemiesOrdinary / levelOrganizer.CurrentLevel.requiredAmountOfEnemiesOrdinary;
        }
        else
        {
            lengthOfLevel.fillAmount = 1f;
        }
       
        ///
    }

    private bool firstEnemy;
    private void Start()
    {
        cooldownBeforeWave = 0f;
        cooldownAfterWave = 0f;

        firstEnemy = true;
        waitedAfterWave = true;
        waitedBeforeWave = true;

        AllowedToSpawnEnemiesOrdinary = true;
        levelended = false;
        lengthOfLevel.fillAmount = 0f;
        isSpawningWave = false;
        startTime = Time.time;

        cameraFollow = GameObject.FindGameObjectWithTag("cameraFollow");
        if (cameraFollow == null)
            Debug.LogError("CAN NOT FIND CAMERA FOLLOW! ");

        StartCoroutine(SpawnEnemiesOrdinary());
        StartCoroutine(SpawnWave());

        //PlayerPrefs.SetInt("currentLevel", 1);
        //todo
        //PlayerController.leftHP = 0.0f;
        //PlayerController.rightHP = 0.0f;
        //PlayerPrefs.SetInt("currentLevel", 5);
        //if (PlayerPrefs.GetInt("currentLevel") == 5)
        //{
        //    Debug.Log("LEVEL ENDED!");
        //    levelended = true;
        //    cameraFollow.GetComponent<Destructible2D.D2dFollow>().ifPlayerWon();
        //    PlayerController.Won = true;
        //}
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3f));

            if (!levelended && !PlayerController.Lost && isSpawningWave && waitedBeforeWave)
            {
                if (Random.Range(0, 2) == 0)
                {
                    CreateEnemy(levelOrganizer.CurrentLevel.GetEnemy);
                    levelOrganizer.CurrentLevel.EnemiesInCurrentWave--;
                }
                else
                {
                    CreateEnemy(levelOrganizer.CurrentLevel.GetEnemy);
                    levelOrganizer.CurrentLevel.EnemiesInCurrentWave--;

                    yield return new WaitForSeconds(0.3f);

                    CreateEnemy(levelOrganizer.CurrentLevel.GetEnemy);
                    levelOrganizer.CurrentLevel.EnemiesInCurrentWave--;
                }

                if (levelOrganizer.CurrentLevel.EnemiesInCurrentWave <= 0)
                {
                    isSpawningWave = false;

                    if (levelOrganizer.CurrentLevel.EnabledWave("Last"))
                    {
                        Debug.Log("NOT LAST WAVE");
                        waitedAfterWave = false;
                        cooldownAfterWave = 5f;
                    }
                }
            }
        }
    }

    IEnumerator SpawnEnemiesOrdinary()
    {
        while (true)
        {
            if (firstEnemy)
            {
                firstEnemy = false;
                yield return new WaitForSeconds(1f);
            }
            else
            yield return new WaitForSeconds(0.1f);

            if (!levelended && AllowedToSpawnEnemiesOrdinary && !PlayerController.Lost)
            {
                yield return new WaitForSeconds(levelOrganizer.CurrentLevel.GetSecondsToSpawnOrdinary());

                CreateEnemy(levelOrganizer.CurrentLevel.GetEnemy);

                levelOrganizer.currentAmountOfEnemiesOrdinary++;
            }
        }
    }

    public void CheckForSpawningWave()
    {
        if (levelOrganizer.CurrentLevel.EnabledWave("First"))
        {
            if (levelOrganizer.currentAmountOfEnemiesOrdinary == (int)(levelOrganizer.CurrentLevel.requiredAmountOfEnemiesOrdinary / 3.5f) && !isSpawningWave)
            {
                levelOrganizer.CurrentLevel.EnemiesInCurrentWave = levelOrganizer.CurrentLevel.enemiesInFirstWave;

                AllowedToSpawnEnemiesOrdinary = false;
                isSpawningWave = true;

                waitedBeforeWave = false;
                cooldownBeforeWave = 6f;
            }
        }

        if (levelOrganizer.CurrentLevel.EnabledWave("Middle"))
        {
            if (levelOrganizer.currentAmountOfEnemiesOrdinary == (int)(levelOrganizer.CurrentLevel.requiredAmountOfEnemiesOrdinary * 0.6f) && !isSpawningWave)
            {
                levelOrganizer.CurrentLevel.EnemiesInCurrentWave = levelOrganizer.CurrentLevel.enemiesInMiddleWave;

                AllowedToSpawnEnemiesOrdinary = false;
                isSpawningWave = true;

                waitedBeforeWave = false;
                cooldownBeforeWave = 6f;
            }
        }

        if (levelOrganizer.CurrentLevel.EnabledWave("Last"))
        {
            if (levelOrganizer.currentAmountOfEnemiesOrdinary == levelOrganizer.CurrentLevel.requiredAmountOfEnemiesOrdinary && !isSpawningWave)
            {
                levelOrganizer.CurrentLevel.EnemiesInCurrentWave = levelOrganizer.CurrentLevel.enemiesInLastWave;

                AllowedToSpawnEnemiesOrdinary = false;
                isSpawningWave = true;

                waitedBeforeWave = false;
            }
        }

        if (!levelOrganizer.CurrentLevel.EnabledWave("Last") && !isSpawningWave) //КОНЕЦ
        {

            GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");
            int count = 0;

            foreach (GameObject go in gos)
            {
                if (go.GetComponent<Enemy>().IsDead == false)
                    ++count;
            }

            if (levelended == false && count == 0 && !PlayerController.Lost)
            {
                Debug.Log("LEVEL ENDED!");
                levelended = true;
                cameraFollow.GetComponent<Destructible2D.D2dFollow>().ifPlayerWon();
                PlayerController.Won = true;
            }
        }
    }

    private float startTime;
    private float secondsPassed;
}
