using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

public class KTGameCenter : MonoBehaviour {

#if UNITY_IOS
    [DllImport("__Internal")]
	private static extern void authenticateLocalUser(string gameobjectName);
	[DllImport("__Internal")]
	private static extern void showLeaderboard(string leaderboardId);
	[DllImport("__Internal")]
	private static extern void resetAchievements();
	[DllImport("__Internal")]
	private static extern void submitScore(int score,string leaderboard);
	[DllImport("__Internal")]
	private static extern void submitAchievement(int percantage,string achivement,bool showBanner);
    [DllImport("__Internal")]
    private static extern void submitIncrementalAchievement (float percantage,string achivement,bool showBanner);
	[DllImport("__Internal")]
	private static extern void showAchievements();
	[DllImport("__Internal")]
	private static extern void submitFloatScore(float score,int decimals,string leaderboard);
	[DllImport("__Internal")]
	private static extern void getMyScore(string leaderboardId);
#endif

    private static KTGameCenter _instance = null;

    enum GCStatus {
        kGCAuthenticating = 0,
        kGCAuthenticated
    }

    private GCStatus currentStatus;

    private string playerName;
    private string playerAlias;
    private string playerId;

    public delegate void UserAuthenticatationDelegate (string value);
    public event UserAuthenticatationDelegate GCUserAuthenticated;

    public delegate void ScoreSubmissionDelegate (string leaderboardId,string error);
    public event ScoreSubmissionDelegate GCScoreSubmitted;

    public delegate void AchievementSubmissionDelegate (string achId,string error);
    public event AchievementSubmissionDelegate GCAchievementSubmitted;

    public delegate void ResetAchievementsDelegate (string error);
    public event ResetAchievementsDelegate GCAchievementsReset;

    public delegate void MyScoreDelegate (string leaderboardId,int score,string error);
    public event MyScoreDelegate GCMyScoreFetched;

    void Awake () {
        if (_instance == null) {
            _instance = this;
            // Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(_instance.gameObject);
        }
        else if (_instance != this) {
            // If there's any other object exist of this type delete it
            // as it's breaking our singleton pattern
            Destroy(gameObject);
        }
    }

    void Start () {
        DontDestroyOnLoad(gameObject);
    }

    public static KTGameCenter SharedCenter () {
        if (!_instance) {
            _instance = FindObjectOfType(typeof(KTGameCenter)) as KTGameCenter;

            if (!_instance) {
                var obj = new GameObject("KTGameCenter");
                _instance = obj.AddComponent<KTGameCenter>();
            }
            else {
                _instance.gameObject.name = "KTGameCenter";
            }
        }
        return _instance;
    }

    public void Authenticate () {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer || currentStatus == GCStatus.kGCAuthenticated) {
			return;
		}
		currentStatus = GCStatus.kGCAuthenticating;
		authenticateLocalUser(gameObject.name);
#endif
	}
    public void ShowLeaderboard (string leadboardId = null) {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            return;
        }
        showLeaderboard(leadboardId);
#endif
    }

    public void ShowAchievements () {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            return;
        }
        showAchievements();
#endif
    }

    public void ResetAchievements () {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            return;
        }
        resetAchievements();
#endif
    }
    public void SubmitScore (int score,string leaderboardId) {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            return;
        }
        submitScore(score,leaderboardId);
#endif
    }

    public void SubmitFloatScore (float score,int decimals,string leaderboardId) {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            return;
        }
        submitFloatScore(score,decimals,leaderboardId);
#endif
    }

    public void SubmitAchievement (int percantage,string achivementId,bool showBanner) {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            return;
        }
        submitAchievement(percantage,achivementId,showBanner);
#endif
    }

    public void SubmitIncrementalAchievement (float percantage,string achivementId,bool showBanner) {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            return;
        }
        submitIncrementalAchievement(percantage,achivementId,showBanner);
#endif
    }

	public bool IsGameCenterAuthenticated () {
		if (currentStatus == GCStatus.kGCAuthenticated) {
			return true;
		}
		else {
			return false;
		}
	}
    public void FetchMyScore (string leaderboardId) {
#if UNITY_IOS
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            return;
        }
        getMyScore(leaderboardId);
#endif
    }

	public string PlayerAlias {
		get {
			return playerAlias;
		}
	}
	public string PlayerName {
		get {
			return playerName;
		}
	}
	public string PlayerId {
		get {
			return playerId;
		}
	}

	//native callbacks
	void IsAuthenticated (string error) {
		if (error == "") {
			currentStatus = GCStatus.kGCAuthenticated;
			if (GCUserAuthenticated != null)
				GCUserAuthenticated("Authenticated");
		}
		else {
			if (GCUserAuthenticated != null)
				GCUserAuthenticated(error);
		}
	}

	void GameCenterAvailable (string value) {

	}

	void ProcessGC (string error) {

	}

	void ReloadScoresCompleted (string result) {
//		string[] arr = result.Split('_');
//		string leaderboardId = arr[0];
//		string error = arr[1];
	}

	void ScoreSubmitted (string result) {
		string[] arr = result.Split('_');
		string leaderboardId = arr[0];
		string error = arr[1];
		if (GCScoreSubmitted != null) {
			GCScoreSubmitted(leaderboardId,error);
		}
	}

	void AchievementSubmitted (string result) {
		string[] arr = result.Split('_');
		string achId = arr[0];
		string error = arr[1];
		if (GCAchievementSubmitted != null) {
			GCAchievementSubmitted(achId,error);
		}
	}

	void AchievementReset (string error) {
		if (GCAchievementsReset != null) {
			GCAchievementsReset(error);
		}
	}

	void SetVariables (string val) {
		string[] arr = val.Split('_');
		playerAlias = arr[0];
		playerName = arr[1];
		playerId = arr[2];
	}

	void ScoreFetched (string val) {
		string[] arr = val.Split('_');
		string leaderboardId = arr[0];
		string scoreString = arr[1];
		string errorString = arr[2];
		int score = 0;
		int.TryParse(scoreString,out score);
		if (GCMyScoreFetched != null) {
			GCMyScoreFetched(leaderboardId,score,errorString);
		}
	}
}	
