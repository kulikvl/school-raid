using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class miniLevel : MonoBehaviour
{
    private void Start()
    {
        //PlayerPrefs.SetInt("S1", 3);
        //PlayerPrefs.SetInt("S4", 0);
        //PlayerPrefs.SetInt("S17", 1);
        //PlayerPrefs.SetInt("S13", 2);

        foreach (var ob in GetComponentsInChildren<Transform>())
        {
            if (ob.gameObject.name == "leveltext")
            {
                ob.gameObject.GetComponent<TextMeshProUGUI>().text = gameObject.name;
            }
        }

        SetStars();
        SetName("1", "HAPPY HALLOWEEN");
    }

    private void SetName(string level, string Name)
    {
        if (gameObject.name == "1")
        {
            Transform[] trs = GetComponentsInParent<Transform>();

            foreach (var ob in trs)
            {
                if (ob.gameObject.name == level)
                {
                    Transform[] trss = ob.GetComponentsInChildren<Transform>();

                    foreach (var obb in trss)
                    {
                        if (obb.gameObject.name == "Name")
                        {

                            obb.gameObject.GetComponent<TextMeshProUGUI>().text = Name;
                        }
                    }     
                }
            }
        }
    }

    public List<Transform> FrontStars = new List<Transform>();

    private void SetStars()
    {
        Transform[] trs = GetComponentsInChildren<Transform>();

        foreach (var ob in trs)
        {
            if (ob.parent.name == "starsfront")
            {
                FrontStars.Add(ob);
            }
        }

        string nameInData = "S" + gameObject.name;

        if (PlayerPrefs.GetInt(nameInData) == 0)
        {
            FrontStars[0].gameObject.SetActive(false);
            FrontStars[1].gameObject.SetActive(false);
            FrontStars[2].gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt(nameInData) == 1)
        {
            FrontStars[1].gameObject.SetActive(false);
            FrontStars[2].gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt(nameInData) == 2)
        {
            FrontStars[2].gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt(nameInData) == 3)
        {

        }

    }

    IEnumerator gonex()
    {
        GameObject go = GameObject.FindGameObjectWithTag("tent");
        go.GetComponent<Animation>().Play();

        int level = Convert.ToInt32(gameObject.name);

        PlayerPrefs.SetInt("currentLevel", level);
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("SceneLevel"); // temp

    }
    private void OnMouseUpAsButton()
    {

        StartCoroutine(gonex());
        
    }
}
