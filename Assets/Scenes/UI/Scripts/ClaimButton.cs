using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ClaimButton : MonoBehaviour
{
    /// MissionFirstCompleted = "true" / "false" MissionSecondCompleted = "true" / "false" MissionThirdCompleted = "true" / "false"
    /// MissionFirstReward
    /// MissionFirstTask
    /// MissionFirstStats
    /// MissionFirstValue


    public enum MissionType
    {
        FIRST = 0,
        SECOND = 1,
        THIRD = 2,
    }

    [Header("Mission Type")]
    public MissionType missionType;

    [Header("Button References")]
    public TextMeshProUGUI taskTxt;
    public TextMeshProUGUI rewardAmountTxt;
    public TextMeshProUGUI progressTxt;

    public GameObject claimButton, progressButton;
    public Image fillArea;
    

    public enum ButtonType
    {
        CLAIM = 0,
        PROGRESS = 1,
    }

    private ButtonType _buttonType;
    public ButtonType buttonType
    {
        get
        {
            return _buttonType;
        }
        set
        {
            switch (value)
            {
                case ButtonType.CLAIM:
                    gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    claimButton.SetActive(true);
                    progressButton.SetActive(false);
                    break;
                case ButtonType.PROGRESS:
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    fillArea.fillAmount = 0f;
                    claimButton.SetActive(false);
                    progressButton.SetActive(true);
                    break;
            }

            _buttonType = value;
        }
    }

    private void Awake()
    {
        //check for first time

        if (!PlayerPrefs.HasKey("MISSIONSOPENED1") && missionType == MissionType.FIRST)
        {
            PlayerPrefs.SetString("MISSIONSOPENED1", "TRUE");
            GiveNewMission(missionType);
        }
        if (!PlayerPrefs.HasKey("MISSIONSOPENED2") && missionType == MissionType.SECOND)
        {
            PlayerPrefs.SetString("MISSIONSOPENED2", "TRUE");
            GiveNewMission(missionType);
        }
        if (!PlayerPrefs.HasKey("MISSIONSOPENED3") && missionType == MissionType.THIRD)
        {
            PlayerPrefs.SetString("MISSIONSOPENED3", "TRUE");
            GiveNewMission(missionType);
        }
    }

    private void OnEnable()
    {
        ShowResultsOfMission(missionType);
    }

    private void GiveNewMission(MissionType _missionType)
    {
        buttonType = ButtonType.PROGRESS;

        if (_missionType == MissionType.FIRST)
        {
            int num = UnityEngine.Random.Range(0, 4);
            switch(num)
            {
                case 0:
                    taskTxt.text = "KILL 25 ENEMIES";
                    rewardAmountTxt.text = "100";
                    progressTxt.text = "0/25";
                    PlayerPrefs.SetInt("MissionFirstStats", 25);
                    break;
                case 1:
                    taskTxt.text = "KILL 50 ENEMIES";
                    rewardAmountTxt.text = "200";
                    progressTxt.text = "0/50";
                    PlayerPrefs.SetInt("MissionFirstStats", 50);
                    break;
                case 2:
                    taskTxt.text = "KILL 75 ENEMIES";
                    rewardAmountTxt.text = "300";
                    progressTxt.text = "0/75";
                    PlayerPrefs.SetInt("MissionFirstStats", 75);
                    break;
                case 3:
                    taskTxt.text = "KILL 100 ENEMIES";
                    rewardAmountTxt.text = "400";
                    progressTxt.text = "0/100";
                    PlayerPrefs.SetInt("MissionFirstStats", 100);
                    break;
            }

            PlayerPrefs.SetString("MissionFirstCompleted", "false");

            PlayerPrefs.SetString("MissionFirstTask", taskTxt.text);
            PlayerPrefs.SetInt("MissionFirstReward", Convert.ToInt32(rewardAmountTxt.text));
            
            PlayerPrefs.SetInt("MissionFirstValue", 0);
        }
        else if (_missionType == MissionType.SECOND)
        {
            int num = UnityEngine.Random.Range(0, 4);
            switch (num)
            {
                case 0:
                    taskTxt.text = "SURVIVE 1 WAVE";
                    rewardAmountTxt.text = "100";
                    progressTxt.text = "0/1";
                    PlayerPrefs.SetInt("MissionSecondStats", 1);
                    PlayerPrefs.SetInt("MissionSecondAdd", 1);
                    break;
                case 1:
                    taskTxt.text = "SURVIVE 3 WAVES";
                    rewardAmountTxt.text = "200";
                    progressTxt.text = "0/1";
                    PlayerPrefs.SetInt("MissionSecondStats", 1);
                    PlayerPrefs.SetInt("MissionSecondAdd", 3);
                    break;
                case 2:
                    taskTxt.text = "SURVIVE 5 WAVES";
                    rewardAmountTxt.text = "300";
                    progressTxt.text = "0/1";
                    PlayerPrefs.SetInt("MissionSecondStats", 1);
                    PlayerPrefs.SetInt("MissionSecondAdd", 5);
                    break;
                case 3:
                    taskTxt.text = "SURVIVE 10 WAVES";
                    rewardAmountTxt.text = "500";
                    progressTxt.text = "0/1";
                    PlayerPrefs.SetInt("MissionSecondStats", 1);
                    PlayerPrefs.SetInt("MissionSecondAdd", 10);
                    break;
            }
            
            PlayerPrefs.SetString("MissionSecondCompleted", "false");

            PlayerPrefs.SetString("MissionSecondTask", taskTxt.text);
            PlayerPrefs.SetInt("MissionSecondReward", Convert.ToInt32(rewardAmountTxt.text));
            
            PlayerPrefs.SetInt("MissionSecondValue", 0);

        }
        else if (_missionType == MissionType.THIRD)
        {
            int num = UnityEngine.Random.Range(0, 4);
            switch (num)
            {
                case 0:
                    taskTxt.text = "KEEP TOWNS UNDAMAGED 1 WAVE";
                    rewardAmountTxt.text = "200";
                    progressTxt.text = "0/1";
                    PlayerPrefs.SetInt("MissionThirdStats", 1);
                    PlayerPrefs.SetInt("MissionThirdAdd", 1);
                    break;
                case 1:
                    taskTxt.text = "KEEP TOWNS UNDAMAGED 2 WAVES";
                    rewardAmountTxt.text = "400";
                    progressTxt.text = "0/1";
                    PlayerPrefs.SetInt("MissionThirdStats", 1);
                    PlayerPrefs.SetInt("MissionThirdAdd", 2);
                    break;
                case 2:
                    taskTxt.text = "KEEP TOWNS UNDAMAGED 3 WAVES";
                    rewardAmountTxt.text = "600";
                    progressTxt.text = "0/1";
                    PlayerPrefs.SetInt("MissionThirdStats", 1);
                    PlayerPrefs.SetInt("MissionThirdAdd", 3);
                    break;
                case 3:
                    taskTxt.text = "KEEP TOWNS UNDAMAGED 4 WAVES";
                    rewardAmountTxt.text = "1000";
                    progressTxt.text = "0/1";
                    PlayerPrefs.SetInt("MissionThirdStats", 1);
                    PlayerPrefs.SetInt("MissionThirdAdd", 4);
                    break;
            }

            PlayerPrefs.SetString("MissionThirdCompleted", "false");

            PlayerPrefs.SetString("MissionThirdTask", taskTxt.text);
            PlayerPrefs.SetInt("MissionThirdReward", Convert.ToInt32(rewardAmountTxt.text));
            
            PlayerPrefs.SetInt("MissionThirdValue", 0);


        }
    }

    private void ShowResultsOfMission(MissionType _missionType)
    {
        if (_missionType == MissionType.FIRST)
        {
            taskTxt.text = PlayerPrefs.GetString("MissionFirstTask");
            rewardAmountTxt.text = PlayerPrefs.GetInt("MissionFirstReward").ToString();

            if (PlayerPrefs.GetInt("MissionFirstValue") >= PlayerPrefs.GetInt("MissionFirstStats"))
            {
                PlayerPrefs.SetString("MissionFirstCompleted", "true");
                PlayerPrefs.SetInt("MissionFirstValue", PlayerPrefs.GetInt("MissionFirstStats"));

                buttonType = ButtonType.CLAIM;     
            }
            else
            {
                buttonType = ButtonType.PROGRESS;

                progressTxt.text = PlayerPrefs.GetInt("MissionFirstValue").ToString() + "/" + PlayerPrefs.GetInt("MissionFirstStats").ToString();
                fillArea.fillAmount = (float)PlayerPrefs.GetInt("MissionFirstValue") / (float)PlayerPrefs.GetInt("MissionFirstStats");
            }
        }
        else if (_missionType == MissionType.SECOND)
        {
            taskTxt.text = PlayerPrefs.GetString("MissionSecondTask");
            rewardAmountTxt.text = PlayerPrefs.GetInt("MissionSecondReward").ToString();

            if (PlayerPrefs.GetInt("MissionSecondValue") >= PlayerPrefs.GetInt("MissionSecondStats"))
            {
                PlayerPrefs.SetString("MissionSecondCompleted", "true");
                PlayerPrefs.SetInt("MissionSecondValue", PlayerPrefs.GetInt("MissionSecondStats"));

                buttonType = ButtonType.CLAIM;
            }
            else
            {
                buttonType = ButtonType.PROGRESS;

                progressTxt.text = PlayerPrefs.GetInt("MissionSecondValue").ToString() + "/" + PlayerPrefs.GetInt("MissionSecondStats").ToString();
                fillArea.fillAmount = (float)PlayerPrefs.GetInt("MissionSecondValue") / (float)PlayerPrefs.GetInt("MissionSecondStats");
            }

        }
        else if (_missionType == MissionType.THIRD)
        {
            taskTxt.text = PlayerPrefs.GetString("MissionThirdTask");
            rewardAmountTxt.text = PlayerPrefs.GetInt("MissionThirdReward").ToString();

            if (PlayerPrefs.GetInt("MissionThirdValue") >= PlayerPrefs.GetInt("MissionThirdStats"))
            {
                PlayerPrefs.SetString("MissionThirdCompleted", "true");
                PlayerPrefs.SetInt("MissionThirdValue", PlayerPrefs.GetInt("MissionThirdStats"));

                buttonType = ButtonType.CLAIM;
            }
            else
            {
                buttonType = ButtonType.PROGRESS;

                progressTxt.text = PlayerPrefs.GetInt("MissionThirdValue").ToString() + "/" + PlayerPrefs.GetInt("MissionThirdStats").ToString();
                fillArea.fillAmount = (float)PlayerPrefs.GetInt("MissionThirdValue") / (float)PlayerPrefs.GetInt("MissionThirdStats");
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        if (buttonType == ButtonType.CLAIM)
        {
            string name = "";
            if (missionType == MissionType.FIRST) name = "MissionFirstReward";
            if (missionType == MissionType.SECOND) name = "MissionSecondReward";
            if (missionType == MissionType.THIRD) name = "MissionThirdReward";

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + PlayerPrefs.GetInt(name));
            FindObjectOfType<Coins>().PlayAnimation();

            AudioManager.instance.Play("BtnClaim");

            buttonType = ButtonType.PROGRESS;
            GiveNewMission(missionType);
        }
    }
}
