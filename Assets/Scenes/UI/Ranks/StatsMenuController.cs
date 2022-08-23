using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsMenuController : MonoBehaviour
{
    public GameObject RankSelectionUpper;
    public GameObject RankSelectionLeft, RankSelectionRight;

    public Image fillImage;

    public TextMeshProUGUI txt1, txt2;

    private void Start()
    {
        GetRankImage(RankSelectionUpper.transform, PlayerPrefs.GetInt("currentRank")).SetActive(true);
        GetRankImage(RankSelectionLeft.transform, PlayerPrefs.GetInt("currentRank")).SetActive(true);

        if (PlayerPrefs.GetInt("currentRank") < 10)
        GetRankImage(RankSelectionRight.transform, PlayerPrefs.GetInt("currentRank") + 1).SetActive(true);

        Debug.Log((float)PlayerPrefs.GetInt("currentXP") + "   " + (float)RankManager.instance.GetNextRankXP());

        fillImage.fillAmount = (float)PlayerPrefs.GetInt("currentXP") / (float)RankManager.instance.GetNextRankXP();

        txt1.text = "TOTAL ENEMIES KILLED: " + PlayerPrefs.GetInt("enemiesKilled");
        txt2.text = "MAX WAVES PASSED: " + PlayerPrefs.GetInt("maxWavesPassed");
    }

    private GameObject GetRankImage(Transform rankSelection, int rank)
    {
        return rankSelection.GetChild(rank - 1).gameObject;
    }
}
