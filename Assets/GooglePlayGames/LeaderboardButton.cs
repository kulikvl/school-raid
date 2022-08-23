using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardButton : MonoBehaviour
{
    public BusMenu busMenu;

    private void OnMouseUpAsButton()
    {
        if (BusMenu.isSlowMotion) busMenu.ExitSlowMotion();


#if UNITY_ANDROID
        //if (PlayGamesManager.IsAuthenticated())
            PlayGamesManager.ShowLeaderboardUI();
        //else
        //{
        //    Debug.LogError("YOU ARE NOT AUTHENTICATED1111 => authentication! => result: ");
        //    PlayGamesManager.AuthenticateUser();
        //}
#elif UNITY_IOS
		if (!KTGameCenter.SharedCenter().IsGameCenterAuthenticated())
		{
			Debug.LogError("YOU ARE NOT AUTHENTICATED1111 => authentication!");
			KTGameCenter.SharedCenter().Authenticate();
		}
		else
		{
            KTGameCenter.SharedCenter().ShowLeaderboard();
        }
#else
        Debug.Log("UKNOWN SYSTEM!");
#endif
    }
}
