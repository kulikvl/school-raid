using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelOrganizer : MonoBehaviour
{
    [System.Serializable]
    public class LevelInfo
    {
        [Header("Coin Reward For Completing Level")]
        public int coinReward;

        [Header("Will Chest Be Given For Completing Level")]
        public bool chestRewardAvailable;

        [Header("Amount Of Enemies In Waves")]
        public int enemiesInLastWave;
        public int enemiesInMiddleWave;
        public int enemiesInFirstWave;

        private int enemiesInCurrentWave;
        public int EnemiesInCurrentWave
        {
            get
            {
                return enemiesInCurrentWave;
            }

            set
            {
                if (enemiesInCurrentWave == enemiesInFirstWave && enemiesInFirstWave != 0)
                {
                    enemiesInFirstWave = value;
                }
                else if (enemiesInCurrentWave == enemiesInMiddleWave && enemiesInMiddleWave != 0)
                {
                    enemiesInMiddleWave = value;
                }
                else if (enemiesInCurrentWave == enemiesInLastWave && enemiesInLastWave != 0)
                {
                    enemiesInLastWave = value;
                }

                enemiesInCurrentWave = value;
            }
        }

        public bool EnabledWave(string nameOfWave)
        {
            switch(nameOfWave)
            {
                case "Last":
                    return enemiesInLastWave > 0;
                case "Middle":
                    return enemiesInMiddleWave > 0;
                case "First":
                    return enemiesInFirstWave > 0;
                default:
                    Debug.LogError("Incorrect Name Of Wave");
                    return false;
            }
        }

        [Space]

        public int requiredAmountOfEnemiesOrdinary;

        //////////////////////////////////

        public enum EnemiesToPlay
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

        [Header("Enemy Manager")]
        public EnemyInGameInfo[] enemies;

        [System.Serializable]
        public struct EnemyInGameInfo
        {
            public EnemiesToPlay enemy;

            [Range(0, 100)]
            public int chance;
        }

        public int GetEnemy
        {
            get
            {
                EnemyInGameInfo[] SortedEnemies = null;

                var sortedTrs = from i in enemies
                                orderby i.chance ascending
                                select i;

                SortedEnemies = sortedTrs.ToArray();

                int sum = 0;
                foreach(EnemyInGameInfo i in SortedEnemies)
                {
                    sum += i.chance;
                }
                if (sum != 100) Debug.LogError("SUM OF CHANCES IS MORE OR LESS THAN 100! " + " LEVEL: " + PlayerPrefs.GetInt("currentLevel"));

                //int secondValueOfChance = 100;
                //for (int i = 0; i < SortedEnemies.Length; ++i)
                //{
                //    if (Random.Range(0, secondValueOfChance) < SortedEnemies[i].chance) // 0 100 < 25 . 0 75 < 30 . 0 20 < 100
                //    {
                //        return (int)SortedEnemies[i].enemy;
                //    }
                //    else
                //    {
                //        secondValueOfChance -= SortedEnemies[i].chance;
                //    }
                //}

                int result = Random.Range(0, 100); // 20 30 50

                for (int i = 0; i < SortedEnemies.Length; ++i)
                {
                    if (i == 0)
                    {
                        if (result < SortedEnemies[i].chance)
                        {
                            //Debug.Log("DIAPOSON from " + "0" + " to " + (SortedEnemies[i].chance));
                            return (int)SortedEnemies[i].enemy;
                        }
                    }
                    else
                    {
                        int sumOfPreviousChances = 0;
                        for (int j = 0; j < i; ++j)
                        {
                            sumOfPreviousChances += SortedEnemies[j].chance;
                        }

                        if (result >= sumOfPreviousChances && result < (SortedEnemies[i].chance + sumOfPreviousChances))
                        {
                            //Debug.Log("DIAPOSON from " + sumOfPreviousChances + " to " + (SortedEnemies[i].chance + sumOfPreviousChances));
                            return (int)SortedEnemies[i].enemy;
                        } 
                    }
                }

                return -1;
            }
        }

        //////////////////////////////////

        [Header("Seconds Before Spawning Enemy")] 
        public float startSecondsToSpawnOrdinary; 
        public float lastSecondsToSpawnOrdinary; 

        private bool firstEnemyPassed = false;
        public float differenceToAdd;

        public float GetSecondsToSpawnOrdinary()
        {
            if (!firstEnemyPassed)
            {
                differenceToAdd = (startSecondsToSpawnOrdinary - lastSecondsToSpawnOrdinary) / requiredAmountOfEnemiesOrdinary;

                firstEnemyPassed = true;

                return 3f;
            }
            else
            {
                startSecondsToSpawnOrdinary -= differenceToAdd;

                return Random.Range(startSecondsToSpawnOrdinary, startSecondsToSpawnOrdinary + 3f);
            } 
        }
    }

    [Header("Level Manager")]
    public List<LevelInfo> levels = new List<LevelInfo>();

    public int currentAmountOfEnemiesOrdinary;
    public int overallAmountOfEnemies;

    [System.Serializable]
    public struct Flags
    {
        public GameObject First, Middle, Last;
    }

    [Header("Flag Manager")]
    public Flags flagManager;

    private void Awake()
    {
        currentAmountOfEnemiesOrdinary = 0;
        overallAmountOfEnemies = 0;

        if (CurrentLevel.EnabledWave("Last")) flagManager.Last.SetActive(true);
        if (CurrentLevel.EnabledWave("Middle")) flagManager.Middle.SetActive(true);
        if (CurrentLevel.EnabledWave("First")) flagManager.First.SetActive(true);
    }

    public LevelInfo CurrentLevel
    {
        get
        {
            return levels[PlayerPrefs.GetInt("currentLevel") - 1];
        } 
    }
}
