using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RCONTROL : MonoBehaviour
{
    public enum EnemiesToPlay
    {
        EnemyStandart = 0,
        EnemyBig = 1,
        SchoolBoy = 2,
        SchoolBoyFat = 3,
        All = 4,
    }

    [SerializeField] private EnemiesToPlay enemiesToPlay = EnemiesToPlay.All;

    [Space]
    [Space]

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private int enemyCount = 0;
    [SerializeField] private int tankCount = 0;

    public Transform leftPoint, rightPoint;

    public int _wavetoShow;

    public GameObject DeathOfEnemy_particle;
    public GameObject DeathOfTank_particle;

    [Space]

    public TextMeshProUGUI textOfWaveCount;

    [Range(0f, 1000f)]
    public float lengthOfLevelInSeconds;

    public static bool ALLOWED = true;

    private float startTime;
    public float T; // todo

    private void Update()
    {
        _wavetoShow = PlayerController.Wave;
    }

    IEnumerator GO()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomSecondsToWait());

            if (levelended == false && ALLOWED == true && !PlayerController.Lost)
            {
                if (tankCount == 999)
                {
                    CreateEnemy(4); // 4
                }
                else
                { 
                    CreateEnemy(returnEnemiesToPlay()); // 0 4
                }

                enemyCount++;
            }
        }
    }

    private float randomSecondsToWait()
    {
        if (enemyCount == 0)
        {
            return 1f;
        }
        else
        {
            return Random.Range(3f, 5f); // 3 6 todo 2 4 - just to test
        }  
    }

    private void CreateEnemy(int num)
    {
        if (num != 4)
        {
            tankCount++;
        }
        else
        {
            tankCount = 0;
        }

        checkEnemyToChangeSpawningPoint(num);

        int direction = Random.Range(0, 2);

        if (direction == 0) //left
        {
            var prefab = Instantiate(enemies[num], leftPoint.position, Quaternion.identity);

            Transform[] trs = prefab.GetComponentsInChildren<Transform>();

            foreach(Transform tr in trs)
            {
                if (tr.gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    tr.gameObject.GetComponent<SpriteRenderer>().sortingOrder += enemyCount * 200;
                }
            }
        }
        else // right
        {
            var prefab = Instantiate(enemies[num], rightPoint.position, Quaternion.identity);
            prefab.name = prefab.name + "Right";

            if (num == 4) // IF TANK
            {
                Vector3 newScale = prefab.transform.localScale;
                newScale.x *= -1;
                prefab.transform.localScale = newScale;
            }
            else
            {
                prefab.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            
            Transform[] trs = prefab.GetComponentsInChildren<Transform>();

            foreach (Transform tr in trs)
            {
                if (tr.gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    tr.gameObject.GetComponent<SpriteRenderer>().sortingOrder += enemyCount * 200;
                }
            }
        }
    }

    public bool levelended = false;

    private void FixedUpdate()
    {
        if (ALLOWED == true)
        {
            float t = Time.time - startTime;

            T = t % 60;
        }
        else
        {
            startTime = Time.time - T;
        }
        
        if (T >= lengthOfLevelInSeconds) // УРОВЕНЬ ЗАКАНЧИВАЕТСЯ ПО ВРЕМЕНИ
        {
            if (levelended == false)
            {
                levelended = true;
                GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");

                if (gos != null)
                    foreach (GameObject go in gos)
                    {
                        Instantiate(DeathOfEnemy_particle, go.transform.position, Quaternion.identity);
                        Destroy(go);
                    }

                PlayerController.Wave++;
                PlayerController.Won = true;
            }
        }
    }

    private bool missioncompletedAntiBug = false; // todo
    private bool missioncompletedAntiBug1 = false;
    private bool missioncompletedAntiBug2 = false;

    private void Start()
    {
        ALLOWED = true;

        levelended = false;

        startTime = Time.time;

        missioncompletedAntiBug = false;
        missioncompletedAntiBug1 = false;
        missioncompletedAntiBug2 = false;

        enemyCount = 0;

        textOfWaveCount.text = "WAVE " + PlayerController.Wave.ToString();

        StartCoroutine(GO());

        if (PlayerController.Wave > 1)
        {
           // if (PlayerController.centerHP <= 0f)
//                LVL.AddLevel(0.2f);
          //  else
              //  LVL.AddLevel(0.1f);
        }

        ////////////////////////////

        //if (PlayerController.Wave > 1)
            //PlayerController.checkAchievement("currentMission3level", "currentMission3", "C3value", "wavesSurvived", 1, 2, 3, ref missioncompletedAntiBug);

        ////////////////////////////

        //if (PlayerController.Wave > 1 && PlayerController.centerHP >= 1f)
            //PlayerController.checkAchievement("2currentMission1level", "2currentMission1", "2C1value", "wavesSurvivedUndamaged", 1, 2, 3, ref missioncompletedAntiBug1);

        ////////////////////////////

        //if (PlayerController.Wave > 1 && BusController.HP >= 1f)
           // PlayerController.checkAchievement("3currentMission1level", "3currentMission1", "3C1value", "wavesSurvivedWithoutDamage", 1, 2, 3, ref missioncompletedAntiBug2);
    }

    private int returnEnemiesToPlay()
    {
        if (enemiesToPlay == EnemiesToPlay.All)
        {
            return Random.Range(0, (int)EnemiesToPlay.All);
        }
        else
        {
            return (int)enemiesToPlay;
        }
    }


    private void checkEnemyToChangeSpawningPoint(int number)
    {
        switch (number)
        {
            case (int)EnemiesToPlay.EnemyStandart:
                rightPoint.position = new Vector2(15.77f, -4.1f);
                leftPoint.position = new Vector2(-20.66f, -4.1f);
                break;
            case (int)EnemiesToPlay.EnemyBig:
                rightPoint.position = new Vector2(15.77f, -4.1f);
                leftPoint.position = new Vector2(-20.66f, -4.1f);
                break;
            case (int)EnemiesToPlay.SchoolBoy:
                rightPoint.position = new Vector2(15.77f, -3.9f);
                leftPoint.position = new Vector2(-20.66f, -3.9f);
                break;
            case (int)EnemiesToPlay.SchoolBoyFat:
                rightPoint.position = new Vector2(15.77f, -3.9f);
                leftPoint.position = new Vector2(-20.66f, -3.9f);
                break;
        }
    }
}
