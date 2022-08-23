using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsInGame : MonoBehaviour
{
    public GameObject Tab;
    public Transform continueButton;

    public Collider2D[] colsToManage;

    public static bool enabledToPressSettings;
    public static bool GameIsFreezed;

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
        if (enabledToPressSettings)
        {
            switch (gameObject.name)
            {
                case "SettingsButton":
                    //FREEZE ENEMIES
                    GameIsFreezed = true;
                   
                    ManageCols(false);

                    FindObjectOfType<FireButton>().enabled = false;
                    FindObjectOfType<Player>().Enablejoystick(false);
                    continueButton.GetComponent<Image>().color = Color.white;
                    continueButton.localScale = new Vector3(58.68001f, 58.68001f, 58.68001f);

                    FreezeTheGame();

                    Tab.SetActive(true);
                    Tab.GetComponent<Animator>().SetTrigger("OPEN");

                    break;
                case "Continue":
                    //UNFREEZE ENEMIES
                    GameIsFreezed = false;
                    FindObjectOfType<Player>().Enablejoystick(true);
                    FindObjectOfType<FireButton>().enabled = true;

                    UnFreezeTheGame();

                    ManageCols(true);

                    Tab.SetActive(false);

                    break;
                case "Restart":

                    StartCoroutine(go());
                    break;
                case "Exit":

                    StartCoroutine(goMenu());
                    break;

            }
        } 
    }

    private void Awake()
    {
        GameIsFreezed = false;
        enabledToPressSettings = true;
    }


    IEnumerator go()
    {
        UnFreezeTheGame();

        GameObject g = GameObject.FindGameObjectWithTag("tent");
        g.GetComponent<Animator>().SetTrigger("DARK");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Scene");

        GameIsFreezed = false;

        PlayerController.SetFullHpSchool();
    }

    IEnumerator goMenu()
    {
        UnFreezeTheGame();

        GameObject g = GameObject.FindGameObjectWithTag("tent");
        g.GetComponent<Animator>().SetTrigger("DARK");

        yield return new WaitForSeconds(0.5f);
        GameIsFreezed = false;
        SceneManager.LoadScene("Menu");
    }
}
