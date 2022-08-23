using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChanceButton : MonoBehaviour
{
    public static bool agreedToWatchAd;

    private void OnEnable()
    {
        agreedToWatchAd = false;

        StartCoroutine(Timer());

        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        AudioManager.instance.PauseAllSounds(); 
    }

    public Collider2D[] colsToManage;
    public void ManageCols(bool param)
    {
        FindObjectOfType<Player>().freeze = !param;
        FindObjectOfType<FireButton>().enabled = param;
        SettingsInGame.GameIsFreezed = !param;
        FindObjectOfType<Player>().Enablejoystick(param);
        foreach (Collider2D col in colsToManage) col.enabled = param;
    }

    IEnumerator Timer()
    {
        FindObjectOfType<Player>().freeze = true;
        ManageCols(false);
        yield return new WaitForSecondsRealtime(5f);
        if (!agreedToWatchAd)
        {
            agreedToWatchAd = false;

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            AudioManager.instance.UnPauseAllSounds();

            PlayerController.HealedByWatchingAd = true;
            ManageCols(true);

            if (BusController.agreedToWatchAd)
            {
                BusController.agreedToWatchAd = false;
                FindObjectOfType<BusController>().DeathActions();
                PlayerController.Lost = true;
            }
            else // school
            {
                School[] schools = FindObjectsOfType<School>();
                foreach (School sc in schools)
                {
                    if (sc.gameObject.name == "Center")
                    {
                        sc.DestroyCenter();
                    }
                }

            }

            transform.parent.gameObject.SetActive(false);

        }
    }

    private void OnMouseUpAsButton()
    {
        if (gameObject.name.Contains("+"))
        {
            // no thanks
            agreedToWatchAd = false;

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            AudioManager.instance.UnPauseAllSounds();

            PlayerController.HealedByWatchingAd = true;
            ManageCols(true);

            if (BusController.agreedToWatchAd)
            {
                BusController.agreedToWatchAd = false;
                FindObjectOfType<BusController>().DeathActions();
                PlayerController.Lost = true;
            }
            else // school
            {
                School[] schools = FindObjectsOfType<School>();
                foreach(School sc in schools)
                {
                    if (sc.gameObject.name == "Center")
                    {
                        sc.DestroyCenter();
                    }
                }
                
            }

            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            agreedToWatchAd = true;
            AdManager.instance.PlayRewardedVideoAd();

        }
    }
}
