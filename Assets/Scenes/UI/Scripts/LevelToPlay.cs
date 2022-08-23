using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelToPlay : MonoBehaviour
{
    private int currentLevel;

    private void Start()
    {
        transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = gameObject.name;

        currentLevel = Convert.ToInt32(gameObject.name);
        int[] levelsWithNewEnemies = new int[] { 1, 2, 4, 6, 9, 13, 18, 21, 25, 29, 33, 37, 40 };

        //stars set
        int stars = PlayerPrefs.GetInt("bestStarsOnLevel" + currentLevel.ToString());

        if (stars == 3)
        {
            ManageStar(1, true);
            ManageStar(2, true);
            ManageStar(3, true);
        }
        else if (stars == 2)
        {
            ManageStar(1, true);
            ManageStar(2, true);
            ManageStar(3, false);
        }
        else if (stars == 1)
        {
            ManageStar(1, true);
            ManageStar(2, false);
            ManageStar(3, false);
        }
        else
        {
            if (currentLevel == 1) Debug.Log("yeeeeeeee");
            ManageStar(1, false);
            ManageStar(2, false);
            ManageStar(3, false);
        }
        //stars set

        if (!(PlayerPrefs.GetInt("unlockedLevels") >= currentLevel))
        {
            //GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            Sprite lockSprite = Resources.Load<Sprite>("tabBlocked");
            GetComponent<Image>().sprite = lockSprite;

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);

            int pos = Array.IndexOf(levelsWithNewEnemies, currentLevel);
            if (pos > -1)
            {
                Image[] sprites = transform.GetChild(2).GetChild(1).gameObject.GetComponentsInChildren<Image>();
                foreach (Image sp in sprites) sp.color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
            }
        }  
    }

    private void ManageStar(int num, bool activate)
    {
        Transform starsContainer = transform.GetChild(1);

        if (activate)
        {
            starsContainer.GetChild(num - 1).GetChild(0).gameObject.SetActive(true);
            starsContainer.GetChild(num - 1).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            starsContainer.GetChild(num - 1).GetChild(0).gameObject.SetActive(false);
            starsContainer.GetChild(num - 1).GetChild(1).gameObject.SetActive(true);
        }
    }

    IEnumerator DoActions()
    {
        GameObject go = GameObject.FindGameObjectWithTag("tent");
        go.GetComponent<Animation>().Play();

        PlayerPrefs.SetInt("currentLevel", currentLevel);

        yield return new WaitForSeconds(0.5f);

        PlayerController.SetFullHpSchool();

        SceneManager.LoadScene("Intro");

    }
    private void OnMouseUpAsButton()
    {
        if (Buttons.AbleToClick)
        {
            if ((PlayerPrefs.GetInt("unlockedLevels") >= currentLevel))
            {
                Buttons.AbleToClick = false;
                StartCoroutine(DoActions());
            }
            else
            {
                Transform _parent = transform.parent;
                Transform[] trs = _parent.GetComponentsInChildren<Transform>();

                foreach (Transform ob in trs)
                {
                    int number;
                    bool success = Int32.TryParse(ob.gameObject.name, out number);

                    if (success)
                        if (number == PlayerPrefs.GetInt("unlockedLevels") && ob.gameObject.GetComponent<Animation>() != null)
                        {
                            Debug.Log("TARGET LEVEL: " + ob.gameObject + "   " + ob.transform.GetChild(0).name);
                            ob.gameObject.GetComponent<Animation>().Play("PLAYrelease");
                        }
                }
            }
        }
        
    }
}
