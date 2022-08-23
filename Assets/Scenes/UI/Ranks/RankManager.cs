using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    // 10 ranks;
    /*
     1) 0 - 500
     2) 500 - 1500
     3) 1500 - 3000
     4) 3000 - 5000
     5) 5000 - 7500
     6) 7500 - 10000
     7) 10000 - 12500
     8) 12500 - 15000
     9) 15000 - 20000
     10) 20000...
    */

    public static RankManager instance;

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

        DontDestroyOnLoad(gameObject);
    }

    public void AddXP(int amount)
    {
        int previousXP = PlayerPrefs.GetInt("currentXP");

        PlayerPrefs.SetInt("currentXP", PlayerPrefs.GetInt("currentXP") + amount);

        //checking
        int currentXP = PlayerPrefs.GetInt("currentXP");

        if (previousXP < 500 && currentXP >= 500)
        {
            PlayerPrefs.SetInt("currentRank", 2);
        }
        if (previousXP < 1500 && currentXP >= 1500)
        {
            PlayerPrefs.SetInt("currentRank", 3);
        }
        if (previousXP < 3000 && currentXP >= 3000)
        {
            PlayerPrefs.SetInt("currentRank", 4);
        }
        if (previousXP < 5000 && currentXP >= 5000)
        {
            PlayerPrefs.SetInt("currentRank", 5);
        }
        if (previousXP < 7500 && currentXP >= 7500)
        {
            PlayerPrefs.SetInt("currentRank", 6);
        }
        if (previousXP < 10000 && currentXP >= 10000)
        {
            GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Experienced);
            PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Experienced);
            PlayerPrefs.SetInt("currentRank", 7);
        }
        if (previousXP < 12500 && currentXP >= 12500)
        {
            PlayerPrefs.SetInt("currentRank", 8);
        }
        if (previousXP < 15000 && currentXP >= 15000)
        {
            PlayerPrefs.SetInt("currentRank", 9);
        }
        if (previousXP < 20000 && currentXP >= 20000)
        {
            GameCenterManager.UnlockAchievement(GameCenterManager.AchievementID.Legend);
            PlayGamesManager.UnlockAchievement(PlayGamesManager.AchievementID.Legend);
            PlayerPrefs.SetInt("currentRank", 10);
        }
    }

    public int GetNextRankXP()
    {
        int currentRank = PlayerPrefs.GetInt("currentRank");

        switch(currentRank)
        {
            case 1: return 500;
            case 2: return 1500;
            case 3: return 3000;
            case 4: return 5000;
            case 5: return 7500;
            case 6: return 10000;
            case 7: return 12500;
            case 8: return 15000;
            case 9: return 20000;
            case 10: return 20000;
            default:
                {
                    Debug.LogError("ERROR RANK");
                    return -1;
                }
        }
    }
}
