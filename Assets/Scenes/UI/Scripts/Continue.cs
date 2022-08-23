using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    //IEnumerator go()
    //{
    //    GameObject g = GameObject.FindGameObjectWithTag("tent");

    //    if (g != null) g.GetComponent<Animation>().Play("darkerTest");

    //    yield return new WaitForSeconds(0.5f);

    //    SceneManager.LoadScene("Scene");

    //    PlayerController.SetFullHpSchool();
    //}

    //IEnumerator goMenu()
    //{
    //    GameObject g = GameObject.FindGameObjectWithTag("tent");
    //    g.GetComponent<Animation>().Play("darkerTest");
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene("Menu");
    //}

    private void OnMouseUpAsButton()
    {
        //if (gameObject.name == "ContinueButton")
        //{
        //    if (PlayerPrefs.GetInt("currentLevel") < 20)
        //    {
        //        if (PlayerPrefs.GetInt("currentLevel") == PlayerPrefs.GetInt("unlockedLevels"))
        //        {
        //            PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel") + 1);
        //            PlayerPrefs.SetInt("unlockedLevels", PlayerPrefs.GetInt("unlockedLevels") + 1);
        //        }
        //        else
        //        {
        //            PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel") + 1);
        //        }

        //        StartCoroutine(go());
        //    }
        //    else
        //    {
        //        StartCoroutine(goMenu());
        //    }
        //}
        //else if (gameObject.name == "ReplayButton")
        //{
        //    StartCoroutine(go());
        //}
        //else if (gameObject.name == "RestartButton")
        //{
        //    StartCoroutine(go());
        //}

        if (gameObject.name == "ExitButton")
        {
            if (UpgradeButton.currentPageIsUpgrade)
            {
                GameObject abilitiesButton = GameObject.FindGameObjectWithTag("AbilitiesButton");
                abilitiesButton.GetComponent<Animation>().Play("AbilitiesExit");

                GameObject upgradeBack = GameObject.FindGameObjectWithTag("UpgradeBack");
                upgradeBack.GetComponent<Animation>().Play("upgradebackswipeLeft");

                GameObject upgradeBackGround = GameObject.FindGameObjectWithTag("UpgradeBackGround");
                upgradeBackGround.GetComponent<Animation>().Play("upgradebackgroundswipeLeft");

                GameObject upgradeStatistics = GameObject.FindGameObjectWithTag("UpgradeStatistics");
                upgradeStatistics.GetComponent<Animation>().Play("upgradeStatsDisappear");

                GameObject upgradePoint = GameObject.FindGameObjectWithTag("UpgradePoint");
                upgradePoint.GetComponent<Animation>().Play("upgradepointBack");

                GameObject shop = GameObject.FindGameObjectWithTag("Shop");
                shop.GetComponent<Animation>().Play();

                UpgradeButton.currentPageIsUpgrade = false;
            }
            else if (PlayCampaign.currentPageIsLevelSelection)
            {
                GameObject shop = GameObject.FindGameObjectWithTag("Shop");
                GameObject menuCoins = GameObject.FindGameObjectWithTag("MenuCoins");
                GameObject lvlSel = GameObject.FindGameObjectWithTag("LevelSelection");

                shop.GetComponent<Animation>().Play("ShopSwipe1");
                menuCoins.GetComponent<Animation>().Play("CoinsSwipe");
                lvlSel.GetComponent<Animation>().Play("ShopSwipe2");

                GameObject Content = GameObject.FindGameObjectWithTag("CONTENT");
                Content.GetComponent<SnapScrolling>().moveToSelectedButton = false;

                GameObject[] gos = GameObject.FindGameObjectsWithTag("BuyButton");
                foreach (GameObject go in gos)
                {
                    if (go.GetComponent<BuySkin>().isChosen == true)
                    {
                        go.GetComponent<BuySkin>().buttonType = BuySkin.ButtonType.CHOOSE;
                        break;
                    }
                }

                PlayCampaign.currentPageIsLevelSelection = false;
            }
            else
                SceneManager.LoadScene("Menu");
        }

        else if (gameObject.name == "SettingsButton")
        {
            GameObject Settings = GameObject.FindGameObjectWithTag("MenuSettings");
            GameObject PlayButton = GameObject.FindGameObjectWithTag("MenuPlay");
            PlayButton.GetComponent<BoxCollider2D>().enabled = false;
            Settings.SetActive(true);
        }

        else if (gameObject.name == "ExitButtonGame")
        {
            StartCoroutine(goMenu());
        }

        else
        {
            SceneManager.LoadScene("Menu");
        }

        IEnumerator goMenu()
        {
            GameObject g = GameObject.FindGameObjectWithTag("tent");
            g.GetComponent<Animator>().SetTrigger("DARK");

            yield return new WaitForSeconds(0.5f);

            SceneManager.LoadScene("Menu");

        }
    }
}
