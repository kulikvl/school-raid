using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    public GameObject Tab;
    public GameObject continueButton;
    public Collider2D[] colsToManage;

    public GameObject warning;

    private void ManageCols(bool param)
    {
        foreach (Collider2D col in colsToManage) col.enabled = param;
    }
    private void FreezeTheGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    private void UnFreezeTheGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    private void OnMouseUpAsButton()
    {
        if (gameObject.name == "MissionsButton")
        {
            SettingsInGame.GameIsFreezed = true;

            ManageCols(false);

            FindObjectOfType<Player>().Enablejoystick(false);
            continueButton.GetComponent<Image>().color = Color.white;
            continueButton.transform.localScale = new Vector3(58.68001f, 58.68001f, 58.68001f);

            FreezeTheGame();

            Tab.SetActive(true);
            Tab.GetComponent<Animator>().SetTrigger("OPEN");
        }
        else
        {
            SettingsInGame.GameIsFreezed = false;
            FindObjectOfType<Player>().Enablejoystick(true);

            UnFreezeTheGame();

            ManageCols(true);

            Tab.SetActive(false);
        }
    }

    private void Update()
    {
        if (gameObject.name == "MissionsButton")
        {
            UpdateResults();

            if (atLeastOneIsCompleted) warning.SetActive(true);
            else warning.SetActive(false);
        } 
    }

    public static bool atLeastOneIsCompleted = false;
    private void UpdateResults()
    {
        if (PlayerPrefs.GetInt("MissionFirstValue") >= PlayerPrefs.GetInt("MissionFirstStats")
            || PlayerPrefs.GetInt("MissionSecondValue") >= PlayerPrefs.GetInt("MissionSecondStats")
            || PlayerPrefs.GetInt("MissionThirdValue") >= PlayerPrefs.GetInt("MissionThirdStats"))
        {
            atLeastOneIsCompleted = true;
        }
        else
        {
            atLeastOneIsCompleted = false;
        }

    }
}
