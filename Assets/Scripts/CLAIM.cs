using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using TMPro;

public class CLAIM : MonoBehaviour
{

    public Slider sl;

    public GameObject stats;
    public GameObject reward;
    public GameObject task;

    public int number;

    public GameObject missionCompleteGO;

    public string ex;

    public static GameObject _missionCompleteGO;
    public GameObject canv;
    public static GameObject _thisParent;
    public static CLAIM _inst;


    public void Awake() //// CHECKING FOR THE FIRST TIME ENTER TO GIVE TASKS ////
    {
        _missionCompleteGO = missionCompleteGO;
        _thisParent = canv;
        _inst = gameObject.GetComponent<CLAIM>();

        //_inst.number = 6;
        //_inst.GivingTask();

        if (PlayerPrefs.GetString("firstTime") != "true")
        {
            PlayerPrefs.SetString("currentMission1", "true");
            number = 1;
            GivingTask();

            PlayerPrefs.SetString("currentMission2", "true");
            number = 2;
            GivingTask();

            PlayerPrefs.SetString("currentMission3", "true");
            number = 3;
            GivingTask();

            PlayerPrefs.SetString("2currentMission1", "true");
            number = 4;
            GivingTask();

            PlayerPrefs.SetString("2currentMission2", "true");
            number = 5;
            GivingTask();

            PlayerPrefs.SetString("2currentMission3", "true");
            number = 6;
            GivingTask();

            PlayerPrefs.SetString("3currentMission1", "true");
            number = 7;
            GivingTask();

            PlayerPrefs.SetString("3currentMission2", "true");
            number = 8;
            GivingTask();

            PlayerPrefs.SetString("3currentMission3", "true");
            number = 9;
            GivingTask();

            PlayerPrefs.SetString("firstTime", "true");
        }

    }

    private void Update()
    {
        ex = PlayerPrefs.GetString("2currentMission1");

        switch (number)
        {
            case 1:
                string a = PlayerPrefs.GetString("stats1").Remove(0, 2);
                int b;
                int.TryParse(a, out b); // находим правое число то есть сколько ВСЕГО нужно игроков уничтожить

                sl.value = PlayerPrefs.GetFloat("C1value") / b; // с1 валью это сколько мы уже уничтожили значит мы делим чтобы получить нужное число для полоски
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("C1value") + PlayerPrefs.GetString("stats1").Remove(0, 1); // конечный текст
                break;
            case 2:

                string a1 = PlayerPrefs.GetString("stats2").Remove(0, 2);
                int b1;
                int.TryParse(a1, out b1);

                sl.value = PlayerPrefs.GetFloat("C2value") / b1;
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("C2value") + PlayerPrefs.GetString("stats2").Remove(0, 1); //
                break;
            case 3:

                string a2 = PlayerPrefs.GetString("stats3").Remove(0, 2);
                int b2;
                int.TryParse(a2, out b2);

                sl.value = PlayerPrefs.GetFloat("C3value") / b2;
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("C3value") + PlayerPrefs.GetString("stats3").Remove(0, 1); //
                break;
            case 4:

                string a3 = PlayerPrefs.GetString("2stats1").Remove(0, 2);
                int b3;
                int.TryParse(a3, out b3);

                sl.value = PlayerPrefs.GetFloat("2C1value") / b3;
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("2C1value") + PlayerPrefs.GetString("2stats1").Remove(0, 1); //
                break;
            case 5:

                string a4 = PlayerPrefs.GetString("2stats2").Remove(0, 2);
                int b4;
                int.TryParse(a4, out b4);

                sl.value = PlayerPrefs.GetFloat("2C2value") / b4;
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("2C2value") + PlayerPrefs.GetString("2stats2").Remove(0, 1); //
                break;
            case 6:

                string a5 = PlayerPrefs.GetString("2stats3").Remove(0, 2);
                int b5;
                int.TryParse(a5, out b5);

                sl.value = PlayerPrefs.GetFloat("2C3value") / b5;
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("2C3value") + PlayerPrefs.GetString("2stats3").Remove(0, 1); //
                break;
                //////////////////////////////////////////////////////
            case 7:

                string a6 = PlayerPrefs.GetString("3stats1").Remove(0, 2);
                int b6;
                int.TryParse(a6, out b6);

                sl.value = PlayerPrefs.GetFloat("3C1value") / b6;
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("3C1value") + PlayerPrefs.GetString("3stats1").Remove(0, 1); //
                break;
            case 8:

                string a7 = PlayerPrefs.GetString("3stats2").Remove(0, 2);
                int b7;
                int.TryParse(a7, out b7);

                sl.value = PlayerPrefs.GetFloat("3C2value") / b7;
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("3C2value") + PlayerPrefs.GetString("3stats2").Remove(0, 1); //
                break;
            case 9:

                string a8 = PlayerPrefs.GetString("3stats3").Remove(0, 2);
                int b8;
                int.TryParse(a8, out b8);

                sl.value = PlayerPrefs.GetFloat("3C3value") / b8;
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetFloat("3C3value") + PlayerPrefs.GetString("3stats3").Remove(0, 1); //
                break;


        }
    }

    public static void GivingPriceAndNew()
    {
        if (PlayerPrefs.GetString("currentMission1") == "true")
        {

            string a = PlayerPrefs.GetString("reward1").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 1;
            _inst.GivingTask();
        }

        if (PlayerPrefs.GetString("currentMission2") == "true")
        {
            GameObject go = GameObject.FindGameObjectWithTag("MC");

            string a = PlayerPrefs.GetString("reward2").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 2;
            _inst.GivingTask();
        }

        if (PlayerPrefs.GetString("currentMission3") == "true")
        {
            GameObject go = GameObject.FindGameObjectWithTag("MC");

            string a = PlayerPrefs.GetString("reward3").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 3;
            _inst.GivingTask();
        }

        if (PlayerPrefs.GetString("2currentMission1") == "true")
        {
            GameObject go = GameObject.FindGameObjectWithTag("MC");

            string a = PlayerPrefs.GetString("2reward1").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 4;
            _inst.GivingTask();
        }

        if (PlayerPrefs.GetString("2currentMission2") == "true")
        {
            GameObject go = GameObject.FindGameObjectWithTag("MC");

            string a = PlayerPrefs.GetString("2reward2").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 5;
            _inst.GivingTask();
        }

        if (PlayerPrefs.GetString("2currentMission3") == "true")
        {
            GameObject go = GameObject.FindGameObjectWithTag("MC");

            string a = PlayerPrefs.GetString("2reward3").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 6;
            _inst.GivingTask();
        }

        //////////////////////////////////////////////

        if (PlayerPrefs.GetString("3currentMission1") == "true")
        {
            GameObject go = GameObject.FindGameObjectWithTag("MC");

            string a = PlayerPrefs.GetString("3reward1").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 7;
            _inst.GivingTask();
        }

        if (PlayerPrefs.GetString("3currentMission2") == "true")
        {
            GameObject go = GameObject.FindGameObjectWithTag("MC");

            string a = PlayerPrefs.GetString("3reward2").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 8;
            _inst.GivingTask();
        }

        if (PlayerPrefs.GetString("3currentMission3") == "true")
        {
            GameObject go = GameObject.FindGameObjectWithTag("MC");

            string a = PlayerPrefs.GetString("3reward3").Remove(0, 2);
            int b;
            int.TryParse(a, out b);

            var prefab = Instantiate(_missionCompleteGO, _thisParent.transform);
            prefab.GetComponent<missionCompleted>().coins = b;
            prefab.GetComponent<missionCompleted>().allowedToPlay = true;

            _inst.number = 9;
            _inst.GivingTask();
        }
    }

    //public void FixedUpdate() //// CHECKING IF PLAYER COMPLETED TASK TO GIVE NEW ////
    //{
       

    //}

    public void GivingTask()
    {
        if (number == 1 && PlayerPrefs.GetString("currentMission1") == "true")
        {
            int ran = Random.Range(0, 3);

            PlayerPrefs.SetInt("currentMission1level", ran);

            switch (ran)
            {
                case 0://
                    
                    task.GetComponent<TextMeshProUGUI>().text = "KILL 5 ENEMIES";
                    stats.GetComponent<TextMeshProUGUI>().text = "0/5";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 25";

                    sl.value = 0f;
                    break;
                case 1://
                    task.GetComponent<TextMeshProUGUI>().text = "KILL 10 ENEMIES";
                    stats.GetComponent<TextMeshProUGUI>().text = "0/10";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 30";


                    sl.value = 0f;
                    break;
                case 2://
                    task.GetComponent<TextMeshProUGUI>().text = "KILL 15 ENEMIES";
                    stats.GetComponent<TextMeshProUGUI>().text = "0/15";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 35";


                    sl.value = 0f;
                    break;
            }

            PlayerPrefs.SetFloat("C1value", 0);
            PlayerPrefs.SetString("currentMission1", "false");

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("stats1", stat);
            PlayerPrefs.SetString("task1", text);
            PlayerPrefs.SetString("reward1", rewardInString);

        }
        if (number == 2 && PlayerPrefs.GetString("currentMission2") == "true")
        {

            int ran = Random.Range(0, 3);

            PlayerPrefs.SetInt("currentMission2level", ran);

            switch (ran)
            {
                case 0: //
                    task.GetComponent<TextMeshProUGUI>().text = "DESTROY 1 TANK";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 50";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/1";

                    sl.value = 0f;
                    break;
                case 1://
                    task.GetComponent<TextMeshProUGUI>().text = "DESTROY 3 TANKS";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 70";
                    stats.GetComponent<TextMeshProUGUI>().text = "0/3";

                    sl.value = 0f;
                    break;
                case 2: //
                    task.GetComponent<TextMeshProUGUI>().text = "DESTROY 5 TANKS";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 100";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/5";

                    sl.value = 0f;
                    break;

            }

            PlayerPrefs.SetFloat("C2value", 0);
            PlayerPrefs.SetString("currentMission2", "false");

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("stats2", stat);
            PlayerPrefs.SetString("task2", text);
            PlayerPrefs.SetString("reward2", rewardInString);
        }
        if (number == 3 && PlayerPrefs.GetString("currentMission3") == "true")
        {

            int ran = Random.Range(0, 3);

            sl.value = 0f;

            PlayerPrefs.SetInt("currentMission3level", ran);

            switch (ran)
            {
                case 0: //
                    task.GetComponent<TextMeshProUGUI>().text = "SURVIVE 1 WAVE";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 100";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/1";

                    break;
                case 1: //
                    task.GetComponent<TextMeshProUGUI>().text = "SURVIVE 2 WAVES";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 150";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/2";

                    break;
                case 2: //
                    task.GetComponent<TextMeshProUGUI>().text = "SURVIVE 3 WAVES";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 200";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/3";

                    break;

            }

            PlayerPrefs.SetFloat("C3value", 0);
            PlayerPrefs.SetString("currentMission3", "false");

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("stats3", stat);

            PlayerPrefs.SetString("task3", text);
            PlayerPrefs.SetString("reward3", rewardInString);
        }
        if (number == 4 && PlayerPrefs.GetString("2currentMission1") == "true")
        {

            int ran = Random.Range(0, 3);

            sl.value = 0f;

            PlayerPrefs.SetInt("2currentMission1level", ran); //

            switch (ran)
            {
                case 0: //
                    task.GetComponent<TextMeshProUGUI>().text = "KEEP TOWER UNDAMAGED";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 100";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/1";

                    break;
                case 1: //
                    task.GetComponent<TextMeshProUGUI>().text = "KEEP TOWER UNDAMAGED";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 150";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/2";

                    break;
                case 2: //
                    task.GetComponent<TextMeshProUGUI>().text = "KEEP TOWER UNDAMAGED";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 200";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/3";

                    break;

            }

            PlayerPrefs.SetFloat("2C1value", 0); //
            PlayerPrefs.SetString("2currentMission1", "false"); //

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("2stats1", stat); //

            PlayerPrefs.SetString("2task1", text); //
            PlayerPrefs.SetString("2reward1", rewardInString); //
        }
        if (number == 5 && PlayerPrefs.GetString("2currentMission2") == "true")
        {

            int ran = Random.Range(0, 3);

            sl.value = 0f;

            PlayerPrefs.SetInt("2currentMission2level", ran); //

            switch (ran)
            {
                case 0: //
                    task.GetComponent<TextMeshProUGUI>().text = "KILL SHKOLOTA!";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 25";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/5";

                    break;
                case 1: //
                    task.GetComponent<TextMeshProUGUI>().text = "KILL SHKOLOTA!";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 35";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/10";

                    break;
                case 2: //
                    task.GetComponent<TextMeshProUGUI>().text = "KILL SHKOLOTA!";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 50";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/15";

                    break;

            }

            PlayerPrefs.SetFloat("2C2value", 0); //
            PlayerPrefs.SetString("2currentMission2", "false"); //

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("2stats2", stat); //

            PlayerPrefs.SetString("2task2", text); //
            PlayerPrefs.SetString("2reward2", rewardInString); //
        }
        if (number == 6 && PlayerPrefs.GetString("2currentMission3") == "true")
        {

            int ran = Random.Range(0, 3);

            sl.value = 0f;

            PlayerPrefs.SetInt("2currentMission3level", ran); //

            switch (ran)
            {
                case 0: //
                    task.GetComponent<TextMeshProUGUI>().text = "THROW THE BOMBS";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 75";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/3";

                    break;
                case 1: //
                    task.GetComponent<TextMeshProUGUI>().text = "THROW THE BOMBS";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 100";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/5";

                    break;
                case 2: //
                    task.GetComponent<TextMeshProUGUI>().text = "THROW THE BOMBS";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 200";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/10";

                    break;

            }

            PlayerPrefs.SetFloat("2C3value", 0); //
            PlayerPrefs.SetString("2currentMission3", "false"); //

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("2stats3", stat); //

            PlayerPrefs.SetString("2task3", text); //
            PlayerPrefs.SetString("2reward3", rewardInString); //
        }
        ////////////////////////////////////////////////

        if (number == 7 && PlayerPrefs.GetString("3currentMission1") == "true")
        {

            int ran = Random.Range(0, 3);

            sl.value = 0f;

            PlayerPrefs.SetInt("3currentMission1level", ran); //

            switch (ran)
            {
                case 0: //
                    task.GetComponent<TextMeshProUGUI>().text = "AVOID TAKING DAMAGE";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 55";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/1";

                    break;
                case 1: //
                    task.GetComponent<TextMeshProUGUI>().text = "AVOID TAKING DAMAGE";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 110";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/2";

                    break;
                case 2: //
                    task.GetComponent<TextMeshProUGUI>().text = "AVOID TAKING DAMAGE";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 175";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/3";

                    break;

            }

            PlayerPrefs.SetFloat("3C1value", 0); //
            PlayerPrefs.SetString("3currentMission1", "false"); //

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("3stats1", stat); //

            PlayerPrefs.SetString("3task1", text); //
            PlayerPrefs.SetString("3reward1", rewardInString); //
        }

        if (number == 8 && PlayerPrefs.GetString("3currentMission2") == "true")
        {

            int ran = Random.Range(0, 3);

            sl.value = 0f;

            PlayerPrefs.SetInt("3currentMission2level", ran); //

            switch (ran)
            {
                case 0: //
                    task.GetComponent<TextMeshProUGUI>().text = "MAKE DOUBLE KILL";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 35";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/1";

                    break;
                case 1: //
                    task.GetComponent<TextMeshProUGUI>().text = "MAKE DOUBLE KILL";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 50";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/2";

                    break;
                case 2: //
                    task.GetComponent<TextMeshProUGUI>().text = "MAKE DOUBLE KILL";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 100";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/5";

                    break;

            }

            PlayerPrefs.SetFloat("3C2value", 0); //
            PlayerPrefs.SetString("3currentMission2", "false"); //

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("3stats2", stat); //

            PlayerPrefs.SetString("3task2", text); //
            PlayerPrefs.SetString("3reward2", rewardInString); //
        }

        if (number == 9 && PlayerPrefs.GetString("3currentMission3") == "true")
        {

            int ran = Random.Range(0, 3);

            sl.value = 0f;

            PlayerPrefs.SetInt("3currentMission3level", ran); //

            switch (ran)
            {
                case 0: //
                    task.GetComponent<TextMeshProUGUI>().text = "TEAR A HEAD OFF";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 999";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/1";

                    break;
                case 1: //
                    task.GetComponent<TextMeshProUGUI>().text = "TEAR A HEAD OFF";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 999";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/2";

                    break;
                case 2: //
                    task.GetComponent<TextMeshProUGUI>().text = "TEAR A HEAD OFF";
                    reward.GetComponent<TextMeshProUGUI>().text = "+ 999";

                    stats.GetComponent<TextMeshProUGUI>().text = "0/3";

                    break;

            }

            PlayerPrefs.SetFloat("3C3value", 0); //
            PlayerPrefs.SetString("3currentMission3", "false"); //

            string text = task.GetComponent<TextMeshProUGUI>().text;
            string rewardInString = reward.GetComponent<TextMeshProUGUI>().text;
            string stat = stats.GetComponent<TextMeshProUGUI>().text;

            PlayerPrefs.SetString("3stats3", stat); //

            PlayerPrefs.SetString("3task3", text); //
            PlayerPrefs.SetString("3reward3", rewardInString); //
        }

    }

    public void Start()
    {
        switch (number) 
        {
            case 1:
                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("task1");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("reward1");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("stats1");

                string a = PlayerPrefs.GetString("stats1").Remove(0, 2);
                int b;
                int.TryParse(a, out b);

                sl.value = PlayerPrefs.GetFloat("C1value") / b;

                break;
            case 2:
                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("task2");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("reward2");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("stats2");

                string a1 = PlayerPrefs.GetString("stats2").Remove(0, 2);
                int b1;
                int.TryParse(a1, out b1);

                sl.value = PlayerPrefs.GetFloat("C2value") / b1;

                break;
            case 3:
                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("task3");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("reward3");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("stats3");

                string a2 = PlayerPrefs.GetString("stats3").Remove(0, 2);
                int b2;
                int.TryParse(a2, out b2);

                sl.value = PlayerPrefs.GetFloat("C3value") / b2;

                break;
            case 4:

                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2task1");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2reward1");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2stats1");

                string a3 = PlayerPrefs.GetString("2stats1").Remove(0, 2);
                int b3;
                int.TryParse(a3, out b3);

                sl.value = PlayerPrefs.GetFloat("2C1value") / b3;

                break;
            case 5:

                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2task2");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2reward2");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2stats2");

                string a4 = PlayerPrefs.GetString("2stats2").Remove(0, 2);
                int b4;
                int.TryParse(a4, out b4);

                sl.value = PlayerPrefs.GetFloat("2C2value") / b4;

                break;
            case 6:

                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2task3");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2reward3");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("2stats3");

                string a5 = PlayerPrefs.GetString("2stats3").Remove(0, 2);
                int b5;
                int.TryParse(a5, out b5);

                sl.value = PlayerPrefs.GetFloat("2C3value") / b5;

                break;
                ////////////////////////////
            case 7:

                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3task1");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3reward1");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3stats1");

                string a6 = PlayerPrefs.GetString("3stats1").Remove(0, 2);
                int b6;
                int.TryParse(a6, out b6);

                sl.value = PlayerPrefs.GetFloat("3C1value") / b6;

                break;
            case 8:

                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3task2");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3reward2");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3stats2");

                string a7 = PlayerPrefs.GetString("3stats2").Remove(0, 2);
                int b7;
                int.TryParse(a7, out b7);

                sl.value = PlayerPrefs.GetFloat("3C2value") / b7;

                break;
            case 9:

                task.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3task3");
                reward.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3reward3");
                stats.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("3stats3");

                string a8 = PlayerPrefs.GetString("3stats3").Remove(0, 2);
                int b8;
                int.TryParse(a8, out b8);

                sl.value = PlayerPrefs.GetFloat("3C3value") / b8;

                break;

        }

        //ex = sl.value;
        //if (sl.value < 1f)
        //{
        //    GetComponent<SpriteRenderer>().sprite = skip;
        //    Adv.GetComponent<SpriteRenderer>().sprite = adv;
        //    claimable = false;
        //}
        //else
        //{

        //    GetComponent<SpriteRenderer>().sprite = claim;
        //    Adv.GetComponent<SpriteRenderer>().sprite = nothing;
        //    claimable = true;
        //    PlayerPrefs.SetString("isClaimable", "true");
        //}

    }

    //private void OnMouseUpAsButton()
    //{
    //    if (claimable == true)
    //    {

    //        PlayerPrefs.SetString("isClaimable", "false");

    //        switch (number) // ADD ANIMATIONS
    //        {
    //            case 1:
    //                string a = PlayerPrefs.GetString("reward1").Remove(0, 2);
    //                int b;
    //                int.TryParse(a, out b);

    //                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + b);

    //                GivingTask();

    //                GetComponent<SpriteRenderer>().sprite = skip;
    //                Adv.GetComponent<SpriteRenderer>().sprite = adv;
    //                claimable = false;

    //                break;
    //            case 2:
    //                string a1 = PlayerPrefs.GetString("reward2").Remove(0, 2);
    //                int b1;
    //                int.TryParse(a1, out b1);

    //                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + b1);

    //                GivingTask();

    //                GetComponent<SpriteRenderer>().sprite = skip;
    //                Adv.GetComponent<SpriteRenderer>().sprite = adv;
    //                claimable = false;

    //                break;
    //            case 3:
    //                string a2 = PlayerPrefs.GetString("reward3").Remove(0, 2);
    //                int b2;
    //                int.TryParse(a2, out b2);

    //                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + b2);

    //                GivingTask();

    //                GetComponent<SpriteRenderer>().sprite = skip;
    //                Adv.GetComponent<SpriteRenderer>().sprite = adv;
    //                claimable = false;

    //                break;

    //        }


    //    }
    //    else
    //    {
    //        if (Application.internetReachability != NetworkReachability.NotReachable)
    //        {
    //            music.GetComponent<AudioSource>().Pause();
    //            //ShowAd();
    //        }
    //    }

    //}

    //private void ShowAd()
    //{
    //    if (Advertisement.IsReady())
    //    {
    //        Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
    //    }
    //}
    //private void HandleAdResult(ShowResult result)
    //{
    //    switch (result)
    //    {
    //        case ShowResult.Finished:
    //            music.GetComponent<AudioSource>().Play();
    //            GivingTask();
    //            break;
    //        case ShowResult.Skipped:

    //            break;
    //        case ShowResult.Failed:
    //            break;
    //    }
    //}

    
}
