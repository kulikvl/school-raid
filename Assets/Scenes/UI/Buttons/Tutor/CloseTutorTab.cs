using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTutorTab : MonoBehaviour
{
    public GameObject tab;
    private const string playedTutorialHowToShoot = "PlayedTutorialHowToShoot";
    private const string playedTutorialNewChallange = "PlayedTutorialNewChallange";
    public Collider2D[] colsToManage;

    private void OnMouseUpAsButton()
    {
        if (transform.parent.gameObject.name.Contains("HowToPlay"))
            PlayerPrefs.SetInt(playedTutorialHowToShoot, 1);
        else
            PlayerPrefs.SetInt(playedTutorialNewChallange, 1);

        UnFreezeTheGame();
        ManageCols(true);
        SettingsInGame.GameIsFreezed = false;
        FindObjectOfType<Player>().Enablejoystick(true);

        tab.SetActive(false);
    }

    private void ManageCols(bool param)
    {
        foreach (Collider2D col in colsToManage) col.enabled = param;
    }
    private void UnFreezeTheGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    private void FreezeTheGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void Awake()
    {
        SettingsInGame.GameIsFreezed = true;
        FindObjectOfType<Player>().Enablejoystick(false);
        ManageCols(false);
        FreezeTheGame();
    }
}
