using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Mission : MonoBehaviour
{
    public GameObject Text;
    public static string currentMission = "None";
    public GameObject MissionPanel;

    public GameObject m1, m2, m3;

    public bool opened = false;

    private void OnMouseUpAsButton()
    {
        if (name.Contains("Easy"))
        {
            Text.GetComponent<TextMeshProUGUI>().text = "EASY MISSIONS";

            m1.GetComponent<CLAIM>().number = 1;
            m2.GetComponent<CLAIM>().number = 2;
            m3.GetComponent<CLAIM>().number = 3;

            m1.GetComponent<CLAIM>().Start();
            m2.GetComponent<CLAIM>().Start();
            m3.GetComponent<CLAIM>().Start();

            currentMission = "Easy";


        }
        if (name.Contains("Medium"))
        {
            Text.GetComponent<TextMeshProUGUI>().text = "MEDIUM MISSIONS";

            m1.GetComponent<CLAIM>().number = 4;
            m2.GetComponent<CLAIM>().number = 5;
            m3.GetComponent<CLAIM>().number = 6;

            m1.GetComponent<CLAIM>().Start();
            m2.GetComponent<CLAIM>().Start();
            m3.GetComponent<CLAIM>().Start();

            currentMission = "Medium";
        }
        if (name.Contains("Hard"))
        {
            Text.GetComponent<TextMeshProUGUI>().text = "HARD MISSIONS";

            m1.GetComponent<CLAIM>().number = 7;
            m2.GetComponent<CLAIM>().number = 8;
            m3.GetComponent<CLAIM>().number = 9;

            m1.GetComponent<CLAIM>().Start();
            m2.GetComponent<CLAIM>().Start();
            m3.GetComponent<CLAIM>().Start();

            currentMission = "Hard";
        }

        MissionPanel.GetComponent<Animation>().Play();

       // FreezeEnemies();
    }

    //private void FreezeEnemies() // todo
    //{
    //    RCONTROL.ALLOWED = false;

    //    GameObject[] g1 = GameObject.FindGameObjectsWithTag("Enemy");
    //    GameObject[] g2 = GameObject.FindGameObjectsWithTag("MiniEnemy");
    //    GameObject[] g3 = GameObject.FindGameObjectsWithTag("TankEnemy");

    //    GameObject[] gos = g1.Concat(g2).Concat(g3).ToArray();

    //    if (gos != null)
    //        foreach (GameObject go in gos)
    //        {
    //            if (go.GetComponent<FREEZE>() != null)
    //            go.GetComponent<FREEZE>().Freeze(true);
    //        }

    //}
}
