using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterManager : MonoBehaviour
{
    // its already singleton

    private void Start()
    {
#if UNITY_IOS
        KTGameCenter.SharedCenter().Authenticate();
#endif
    }

    public enum LeaderboardID
    {
        MAXWAVESPASSED,
        TOTALENEMIESKILLED,
    }

    public static void PostToLeaderboard(int newScore, LeaderboardID leaderboardID)
    {
#if UNITY_IOS
        if (KTGameCenter.SharedCenter().IsGameCenterAuthenticated())
        {
            if (leaderboardID == LeaderboardID.TOTALENEMIESKILLED)
            {
                KTGameCenter.SharedCenter().SubmitScore(newScore, "totalenemieskilled");
            }
            else if (leaderboardID == LeaderboardID.MAXWAVESPASSED)
            {
                KTGameCenter.SharedCenter().SubmitScore(newScore, "maxwavespassed");
            }
        }
        else
        {
            Debug.Log("NOT AUTHENTICATED!");
        }
#endif
    }

    public enum AchievementID
    {
        WealtyPerson,
        Experienced,
        Legend,
        Killer,
        Beginning,
        Amateur,
        Skilled,
        PRO,
        Surviver1,
        Surviver2,
        Surviver3,
        UltimateSurviver,
        Trader1,
        Trader2,
        UltimateTrader
    }

    public static void UnlockAchievement(AchievementID achievementID)
    {
#if UNITY_IOS
        if (KTGameCenter.SharedCenter().IsGameCenterAuthenticated())
        {
            if (achievementID == AchievementID.WealtyPerson)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "wealtyperson", true);
            }
            else if (achievementID == AchievementID.Experienced)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "experienced", true);
            }
            else if (achievementID == AchievementID.Legend)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "legend", true);
            }
            else if (achievementID == AchievementID.Killer)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "killer", true);
            }
            else if (achievementID == AchievementID.Beginning)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "beginning", true);
            }
            else if (achievementID == AchievementID.Amateur)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "amateur", true);
            }
            else if (achievementID == AchievementID.Skilled)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "skilled", true);
            }
            else if (achievementID == AchievementID.PRO)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "pro", true);
            }
            else if (achievementID == AchievementID.Surviver1)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "surviver1", true);
            }
            else if (achievementID == AchievementID.Surviver2)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "surviver2", true);
            }
            else if (achievementID == AchievementID.Surviver3)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "surviver3", true);
            }
            else if (achievementID == AchievementID.UltimateSurviver)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "ultimatesurviver", true);
            }
            else if (achievementID == AchievementID.Trader1)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "trader1", true);
            }
            else if (achievementID == AchievementID.Trader2)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "trader2", true);
            }
            else if (achievementID == AchievementID.UltimateTrader)
            {
                KTGameCenter.SharedCenter().SubmitAchievement(100, "ultimatetrader", true);
            }
        }
        else
        {
            Debug.Log("NOT AUTHENTICATED!");
        }
#endif
    }
}
