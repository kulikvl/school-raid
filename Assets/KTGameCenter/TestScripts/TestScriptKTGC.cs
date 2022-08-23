using UnityEngine;
using System.Collections;

public class TestScriptKTGC : MonoBehaviour {

	void Start () {
		KTGameCenter.SharedCenter().Authenticate(); //
	}

	void OnEnable () {
		StartCoroutine(RegisterForGameCenter());
	}
	void OnDisable () {
		KTGameCenter.SharedCenter().GCUserAuthenticated -= GCAuthentication;
		KTGameCenter.SharedCenter().GCScoreSubmitted -= ScoreSubmitted;
		KTGameCenter.SharedCenter().GCAchievementSubmitted -= AchievementSubmitted;
		KTGameCenter.SharedCenter().GCAchievementsReset -= AchivementsReset;
		KTGameCenter.SharedCenter().GCMyScoreFetched -= MyScoreFetched;
	}

	IEnumerator RegisterForGameCenter () {
		yield return new WaitForSeconds(0.5f);
		KTGameCenter.SharedCenter().GCUserAuthenticated += GCAuthentication;
		KTGameCenter.SharedCenter().GCScoreSubmitted += ScoreSubmitted;
		KTGameCenter.SharedCenter().GCAchievementSubmitted += AchievementSubmitted;
		KTGameCenter.SharedCenter().GCAchievementsReset += AchivementsReset;
		KTGameCenter.SharedCenter().GCMyScoreFetched += MyScoreFetched;
	}

	void OnGUI () {
		if (!KTGameCenter.SharedCenter().IsGameCenterAuthenticated()) {
			GUI.skin.label.fontSize = 20;
			GUI.Label(new Rect(10,150,200,50), "Authenticating!");
		}
		else {
			GUI.skin.button.fontSize = 20;
			if (GUI.Button(new Rect(10, 150, 300, 60), "Show Leaderboards")) {
			KTGameCenter.SharedCenter().ShowLeaderboard();
			}
			if (GUI.Button(new Rect(10, 250, 250, 60), "Submit Score")) {
				KTGameCenter.SharedCenter().SubmitScore(110,"grp.com.kashiftasneem.thedarkshadow.highestscoresinglerun");
			}
			if (GUI.Button(new Rect(300, 250, 250, 60), "Submit Achievement")) {
				KTGameCenter.SharedCenter().SubmitAchievement(100,"grp.com.kashiftasneem.thedarkshadow.kill1zombie",true);
			}
			if (GUI.Button(new Rect(10, 350, 300, 60), "Reset Achievement")) {
				KTGameCenter.SharedCenter().ResetAchievements();
			}
			if (GUI.Button(new Rect(330, 350, 250, 60), "Submit Float Score")) {
				KTGameCenter.SharedCenter().SubmitFloatScore(110.123f,3,"com.kashiftasneem.flyingbird.testfloat");
			}
			if (GUI.Button(new Rect(10, 450, 250, 60), "Submit Time")) {
				KTGameCenter.SharedCenter().SubmitFloatScore(2459.3f,2,"com.kashiftasneem.flyingbird.testtime");
			}
			if (GUI.Button(new Rect(10, 550, 250, 60), "Fetch my Score")) {
				KTGameCenter.SharedCenter().FetchMyScore("grp.com.kashiftasneem.thedarkshadow.highestscoresinglerun");
			}
		}
	}

	void GCAuthentication (string status) {
		print ("delegate call back status= "+status);
		StartCoroutine(CheckAttributes());
	}
	void ScoreSubmitted (string leaderboardId,string error) {
		print ("score submitted with id "+leaderboardId +" and error= "+error);
	}
	void AchievementSubmitted (string achId,string error) {
		print ("achievement submitted with id "+achId +" and error= "+error);
	}
	void AchivementsReset (string error) {
		print ("Achievment reset with error= "+error);
	}

	void MyScoreFetched (string leaderboardId,int score,string error) {
		print ("My score for leaderboardId= "+leaderboardId+" is "+score+" with error= "+error);
	}

	IEnumerator CheckAttributes () {
		yield return new WaitForSeconds(1.0f);
		print (" alias= "+KTGameCenter.SharedCenter().PlayerAlias+" name= "+
		       KTGameCenter.SharedCenter().PlayerName+" id= "+KTGameCenter.SharedCenter().PlayerId);
	}
}
