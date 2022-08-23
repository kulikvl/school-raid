using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGamesManager : MonoBehaviour
{
    public static PlayGamesManager instance;
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

    private void Start()
    {
#if UNITY_ANDROID
        if (PlayerPrefs.HasKey("FilmPlayed"))
        AuthenticateUser();
#endif
    }

    public static bool IsAuthenticated()
    {
        return Social.localUser.authenticated;
    }

    public static void AuthenticateUser()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) =>
        {
            if (success == true)
            {
                Debug.Log("Logged in to Google Play Games Services");
            }
            else
            {
                Debug.LogError("Unable to sign in to Google Play Games Services");
            }
        });
#endif
    }

    public enum LeaderboardID
    {
        MAXWAVESPASSED,
        TOTALENEMIESKILLED,
    }

    public static void PostToLeaderboard(long newScore, LeaderboardID leaderboardID)
    {
#if UNITY_ANDROID
        if (IsAuthenticated())
        {
            if (leaderboardID == LeaderboardID.TOTALENEMIESKILLED)
            {
                Social.ReportScore(newScore, GPGSIds.leaderboard_total_enemies_killed, (bool success) =>
                {
                    if (success)
                    {
                        Debug.Log("Posted new score to leaderboard");
                    }
                    else
                    {
                        Debug.Log("Unable to post new score to leaderboard");
                    }
                });
            }
            else if (leaderboardID == LeaderboardID.MAXWAVESPASSED)
            {
                Social.ReportScore(newScore, GPGSIds.leaderboard_max_waves_survived, (bool success) =>
                {
                    if (success)
                    {
                        Debug.Log("Posted new score to leaderboard");
                    }
                    else
                    {
                        Debug.Log("Unable to post new score to leaderboard");
                    }
                });
            }
        }
        else
        {
            Debug.Log("NOT AUTHENTICATED!");
        }
#endif
    }

    public static void ShowLeaderboardUI()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_total_enemies_killed);
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
#if UNITY_ANDROID
        if (IsAuthenticated())
        {
            if (achievementID == AchievementID.WealtyPerson)
            {
                Social.ReportProgress(GPGSIds.achievement_wealthy_person, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Experienced)
            {
                Social.ReportProgress(GPGSIds.achievement_experienced, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Legend)
            {
                Social.ReportProgress(GPGSIds.achievement_legend, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Killer)
            {
                Social.ReportProgress(GPGSIds.achievement_killer, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Beginning)
            {
                Social.ReportProgress(GPGSIds.achievement_beginning, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Amateur)
            {
                Social.ReportProgress(GPGSIds.achievement_amateur, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Skilled)
            {
                Social.ReportProgress(GPGSIds.achievement_skilled, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.PRO)
            {
                Social.ReportProgress(GPGSIds.achievement_pro, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Surviver1)
            {
                Social.ReportProgress(GPGSIds.achievement_surviver_i, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Surviver2)
            {
                Social.ReportProgress(GPGSIds.achievement_surviver_ii, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Surviver3)
            {
                Social.ReportProgress(GPGSIds.achievement_surviver_iii, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.UltimateSurviver)
            {
                Social.ReportProgress(GPGSIds.achievement_ultimate_surviver, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Trader1)
            {
                Social.ReportProgress(GPGSIds.achievement_trader_i, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.Trader2)
            {
                Social.ReportProgress(GPGSIds.achievement_trader_ii, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
            else if (achievementID == AchievementID.UltimateTrader)
            {
                Social.ReportProgress(GPGSIds.achievement_ultimate_trader, 100, (bool success) =>
                {
                    if (success)
                        Debug.Log("Unlocked new achievement");
                    else
                        Debug.Log("Unable to Unlocke new achievement");
                });
            }
        }
        else
        {
            Debug.Log("NOT AUTHENTICATED!");
        }
#endif
    }
}
