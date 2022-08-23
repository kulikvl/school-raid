using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathmatchSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;

    [Space]

    public GameObject effectParticleOnAppear;

    [Header("stats:")]
    public int overallAmountOfEnemies;
    public float LengthOfLevelInSeconds;

    private int enemiesAllowed;
    public Transform leftPoint, rightPoint;
    private Transform leftMiddlePoint, rightMiddlePoint;

    private void Awake()
    {
        leftMiddlePoint = transform.GetChild(0);
        rightMiddlePoint = transform.GetChild(1);
    }

    public void CreateEnemy(GameObject go, Vector3 leftPos, Vector3 rightPos, bool effect)
    {
        if (!gameObject.GetComponent<DeathmatchLevelController>().levelended && !PlayerController.Lost)
        {
            GameObject prefab;

            if (Random.Range(0, 2) == 0) // right
            {
                prefab = Instantiate(go, rightPos, Quaternion.identity);
                prefab.name = prefab.name + "Right";
                prefab.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
                prefab = Instantiate(go, leftPos, Quaternion.identity);

            if (effect) Instantiate(effectParticleOnAppear, prefab.transform.GetChild(0).gameObject.GetComponent<Enemy>().CenterOfEnemy.position, Quaternion.identity);

            SpriteRenderer[] spr = prefab.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sp in spr)
            {
                sp.gameObject.GetComponent<SpriteRenderer>().sortingOrder += overallAmountOfEnemies * 300;
            }

            overallAmountOfEnemies++;

            FindObjectOfType<DeathmatchLevelController>().EnemiesAlive++;
        } 
    }

    List<GameObject> EasyEnemies = new List<GameObject>();
    List<GameObject> MediumEnemies = new List<GameObject>();
    List<GameObject> HardEnemies = new List<GameObject>();

    private void Start()
    {
        enemiesAllowed = 0;
        int currentCampaignLevel = PlayerPrefs.GetInt("unlockedLevels");

        //if (currentCampaignLevel >= 1) enemiesAllowed++;
        //if (currentCampaignLevel >= 2) enemiesAllowed++;
        //if (currentCampaignLevel >= 4) enemiesAllowed++;
        //if (currentCampaignLevel >= 6) enemiesAllowed++;
        //if (currentCampaignLevel >= 9) enemiesAllowed++;
        //if (currentCampaignLevel >= 13) enemiesAllowed++;
        //if (currentCampaignLevel >= 18) enemiesAllowed++;
        //if (currentCampaignLevel >= 21) enemiesAllowed++;
        //if (currentCampaignLevel >= 25) enemiesAllowed++;
        //if (currentCampaignLevel >= 29) enemiesAllowed++;
        //if (currentCampaignLevel >= 33) enemiesAllowed++;
        //if (currentCampaignLevel >= 37) enemiesAllowed++;
        //if (currentCampaignLevel >= 40) enemiesAllowed++;

        if (currentCampaignLevel >= 1) EasyEnemies.Add(enemies[0]);
        if (currentCampaignLevel >= 2) EasyEnemies.Add(enemies[1]);
        if (currentCampaignLevel >= 4) MediumEnemies.Add(enemies[2]);
        if (currentCampaignLevel >= 6) MediumEnemies.Add(enemies[3]);
        if (currentCampaignLevel >= 9) EasyEnemies.Add(enemies[4]);
        if (currentCampaignLevel >= 13) MediumEnemies.Add(enemies[5]);
        if (currentCampaignLevel >= 18) HardEnemies.Add(enemies[6]);
        if (currentCampaignLevel >= 21) MediumEnemies.Add(enemies[7]);
        if (currentCampaignLevel >= 25) EasyEnemies.Add(enemies[8]);
        if (currentCampaignLevel >= 29) MediumEnemies.Add(enemies[9]);
        if (currentCampaignLevel >= 33) EasyEnemies.Add(enemies[10]);
        if (currentCampaignLevel >= 37) MediumEnemies.Add(enemies[11]);
        if (currentCampaignLevel >= 40) HardEnemies.Add(enemies[12]);


        Debug.Log("campaignLevel = " + currentCampaignLevel + " => allowed enemies = " + enemiesAllowed);
        CalculateLengthOfLevelInSeconds();
    }

    public void CalculateLengthOfLevelInSeconds()
    {
        if (PlayerController.Wave == 1) LengthOfLevelInSeconds = 45f; // 25
        if (PlayerController.Wave == 2) LengthOfLevelInSeconds = 50f; // 50
        if (PlayerController.Wave == 3) LengthOfLevelInSeconds = 55f; // 55
        if (PlayerController.Wave == 4) LengthOfLevelInSeconds = 59f; //60
        if (PlayerController.Wave >= 5) LengthOfLevelInSeconds = 59f; //65
    }

    private GameObject GetEasyEnemy()
    {
        int num = Random.Range(0, EasyEnemies.Count);
        return EasyEnemies[num];
    }
    private GameObject GetMediumEnemy()
    {
        int num = Random.Range(0, MediumEnemies.Count);
        return MediumEnemies[num];
    }
    private GameObject GetHardEnemy()
    {
        int num = Random.Range(0, HardEnemies.Count);
        return HardEnemies[num];
    }



    public void SpawnRandomly()
    {
        int num = Random.Range(0, 4);
        switch(num)
        {
            case 0:

                StartCoroutine(SpawnOrdinary());
                break;

            case 1:

                if (PlayerController.Wave > 1)
                StartCoroutine(SpawnClose());
                else
                StartCoroutine(SpawnOrdinary());

                break;
            case 2:
                if (PlayerController.Wave > 2)
                    StartCoroutine(SpawnGroupOrdinary());
                else
                    StartCoroutine(SpawnOrdinary());
                break;
            case 3:
                if (PlayerController.Wave > 3)
                    StartCoroutine(SpawnGroupClose());
                else if (PlayerController.Wave > 1)
                    StartCoroutine(SpawnClose());
                else
                    StartCoroutine(SpawnOrdinary());
                break;

                //case 0: StartCoroutine(SpawnClose()); break;
                //case 1: StartCoroutine(SpawnClose()); break;
                //case 2: StartCoroutine(SpawnClose()); break;
                //case 3: StartCoroutine(SpawnClose()); break;
        }
    }


    ////////// COROUTINES ////////////
    public IEnumerator SpawnOrdinary()
    {
        Debug.Log("0");
        float time = 1f;
        GameObject enemyToSpawn = null;
        int num;

        #region balanceOrdinary
        if (PlayerController.Wave == 1)
        {
            time = 1.5f;
            enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 2)
        {
            time = 1.3f;
            num = Random.Range(0, 3);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 3)
        {
            time = 1.1f;
            num = Random.Range(0, 2);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 4)
        {
            time = 0.9f;
            num = Random.Range(0, 3);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else enemyToSpawn = GetMediumEnemy();
        }
        if (PlayerController.Wave == 5)
        {
            time = 0.7f;
            num = Random.Range(0, 3);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else if (num == 1 && HardEnemies.Count > 0) enemyToSpawn = GetHardEnemy();
            else enemyToSpawn = GetMediumEnemy();
        }
        if (PlayerController.Wave == 6)
        {
            time = 0.5f;
            num = Random.Range(0, 4);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else if ((num == 2 || num == 1) && HardEnemies.Count > 0) enemyToSpawn = GetHardEnemy();
            else enemyToSpawn = GetMediumEnemy();

        }
        if (PlayerController.Wave == 7)
        {
            time = 0.3f;
            num = Random.Range(0, 5);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else if ((num == 2 || num == 1 || num == 3) && HardEnemies.Count > 0) enemyToSpawn = GetHardEnemy();
            else enemyToSpawn = GetMediumEnemy();

        }
        if (PlayerController.Wave >= 8)
        {
            time = 0.1f;
            num = Random.Range(0, 8);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else if ((num == 2 || num == 1 || num == 3) && HardEnemies.Count > 0) enemyToSpawn = GetHardEnemy();
            else enemyToSpawn = GetMediumEnemy();
        }
        #endregion 

        yield return new WaitForSeconds(time);
        CreateEnemy(enemyToSpawn, leftPoint.position, rightPoint.position, false);
    }

    public IEnumerator SpawnClose()
    {
        Debug.Log("1");
        float time = 1f;
        GameObject enemyToSpawn = null;
        int num;

        #region balanceClose
        if (PlayerController.Wave == 2)
        {
            time = 1.5f;
            enemyToSpawn = GetEasyEnemy();   
        }
        if (PlayerController.Wave == 3)
        {
            time = 1.3f;
            num = Random.Range(0, 4);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();    
        }
        if (PlayerController.Wave == 4)
        {
            time = 1.1f;
            num = Random.Range(0, 3);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 5)
        {
            time = 1f;
            num = Random.Range(0, 2);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else enemyToSpawn = GetMediumEnemy();    
        }
        if (PlayerController.Wave == 6)
        {
            time = 0.9f;
            num = Random.Range(0, 5);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else if (num == 1 && HardEnemies.Count > 0) enemyToSpawn = GetHardEnemy();
            else enemyToSpawn = GetMediumEnemy();
        }
        if (PlayerController.Wave == 7)
        {
            time = 0.7f;
            num = Random.Range(0, 5);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else if ((num == 2 || num == 1) && HardEnemies.Count > 0) enemyToSpawn = GetHardEnemy();
            else enemyToSpawn = GetMediumEnemy();
        }
        if (PlayerController.Wave >= 8)
        {
            time = 0.4f;
            num = Random.Range(0, 5);
            if (num == 0) enemyToSpawn = GetEasyEnemy();
            else if ((num == 2 || num == 1) && HardEnemies.Count > 0) enemyToSpawn = GetHardEnemy();
            else enemyToSpawn = GetMediumEnemy();
        }
        #endregion 

        yield return new WaitForSeconds(time);
        CreateEnemy(enemyToSpawn, leftMiddlePoint.position, rightMiddlePoint.position, true);
    }

    public IEnumerator SpawnGroupOrdinary()
    {
        Debug.Log("2");
        float time = 1f;
        GameObject enemyToSpawn = null;
        int num;

        #region balanceGroupOrdinary
        if (PlayerController.Wave == 3)
        {
            time = 1.6f;
            enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 4)
        {
            time = 1.4f;
            num = Random.Range(0, 5);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 5)
        {
            time = 1.2f;
            num = Random.Range(0, 4);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 6)
        {
            time = 1.1f;
            num = Random.Range(0, 3);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 7)
        {
            time = 1f;
            num = Random.Range(0, 2);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave >= 8)
        {
            time = 0.9f;
            num = Random.Range(0, 2);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        #endregion 

        yield return new WaitForSeconds(time);
        CreateEnemy(enemyToSpawn, leftPoint.position, rightPoint.position, false);
        yield return new WaitForSeconds(0.8f);
        CreateEnemy(enemyToSpawn, leftPoint.position, rightPoint.position, false);
        yield return new WaitForSeconds(1f);
        CreateEnemy(enemyToSpawn, leftPoint.position, rightPoint.position, false);
    }

    public IEnumerator SpawnGroupClose()
    {
        Debug.Log("3");
        float time = 1f;
        GameObject enemyToSpawn = null;
        int num;

        #region balanceGroupClose
        if (PlayerController.Wave == 4)
        {
            time = 1.7f;
            num = Random.Range(0, 6);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 5)
        {
            time = 1.5f;
            num = Random.Range(0, 5);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 6)
        {
            time = 1.3f;
            num = Random.Range(0, 4);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave == 7)
        {
            time = 1.1f;
            num = Random.Range(0, 3);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        if (PlayerController.Wave >= 8)
        {
            time = 0.8f;
            num = Random.Range(0, 3);
            if (num == 0) enemyToSpawn = GetMediumEnemy();
            else enemyToSpawn = GetEasyEnemy();
        }
        #endregion 

        yield return new WaitForSeconds(time);
        CreateEnemy(enemyToSpawn, leftMiddlePoint.position, rightMiddlePoint.position, true);
        yield return new WaitForSeconds(0.8f);
        CreateEnemy(enemyToSpawn, leftMiddlePoint.position, rightMiddlePoint.position, true);
        yield return new WaitForSeconds(1f);
        CreateEnemy(enemyToSpawn, leftMiddlePoint.position, rightMiddlePoint.position, true);
    }
}
