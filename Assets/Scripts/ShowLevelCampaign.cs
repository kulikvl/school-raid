using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowLevelCampaign : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetString("currentMode") == "Deathmatch")
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "WAVE 1";
        }
        else
        gameObject.GetComponent<TextMeshProUGUI>().text = "LEVEL " + PlayerPrefs.GetInt("currentLevel");
    }
}
