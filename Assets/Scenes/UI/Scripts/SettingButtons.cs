using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingButtons : MonoBehaviour
{
    public GameObject Settings;
    public Collider2D[] collidersToManage;
    public BusMenu busMenu;

    public int SecretTimesClicked = 0;

    private void OnMouseUpAsButton()
    {
        switch(gameObject.name)
        {
            case "SettingsButton":

                if (BusMenu.isSlowMotion) busMenu.ExitSlowMotion();

                ManageColliders(false);
                Settings.SetActive(true);
                
                break;

            case "Close":

                StartCoroutine(CloseWindow());

                break;

            /////////////////////////

            case "RestoreButton":

                // IAP RESTORE HERE

                //if (Application.platform != RuntimePlatform.IPhonePlayer || Application.platform != RuntimePlatform.OSXPlayer)
                //{
                //    Debug.Log("ITS NOT IOS");
                //}
                SecretTimesClicked++;

                if (SecretTimesClicked == 20)
                {
                    PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100000);
                    SecretTimesClicked = 0;
                }

                IAPManager.instance.RestorePurchases();

                break;

            case "SoundButton":
                if (PlayerPrefs.GetString("Sound") == "OFF")
                {
                    PlayerPrefs.SetString("Sound", "ON");

                    GetText().text = "SOUND: ON";
                }
                else
                {
                    PlayerPrefs.SetString("Sound", "OFF");
                    GetText().text = "SOUND: OFF";
                }
                break;

            case "VibrationButton":

                if (iOSHapticFeedback.Instance.IsSupported())
                {
                    if (PlayerPrefs.GetString("Vibration") == "OFF")
                    {
                        PlayerPrefs.SetString("Vibration", "ON");
                        GetText().text = "VIBRATION: ON";
                        iOSHapticFeedback.Instance.IsEnabled = true;
                    }
                    else
                    {
                        PlayerPrefs.SetString("Vibration", "OFF");
                        GetText().text = "VIBRATION: OFF";
                        iOSHapticFeedback.Instance.IsEnabled = false;
                    }
                }
                
                break;

            case "MusicButton":
                if (PlayerPrefs.GetString("Music") == "OFF")
                {
                    PlayerPrefs.SetString("Music", "ON");
                    AudioManager.instance.Play("MainTheme");
                    GetText().text = "MUSIC: ON";
                }
                else
                {
                    PlayerPrefs.SetString("Music", "OFF");
                    AudioManager.instance.StopAllMusic();
                    GetText().text = "MUSIC: OFF";
                }
                break;

        }
    }

    private TextMeshProUGUI GetText()
    {
        return transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (gameObject.name != "SettingsButton")
        transform.localScale = new Vector3(58.68001f, 58.68001f, 58.68001f);

        switch (gameObject.name)
        {
            case "SoundButton":
                GetText().text = "SOUND: " + PlayerPrefs.GetString("Sound");
                break;

            case "VibrationButton":
                GetText().text = "VIBRATION: " + PlayerPrefs.GetString("Vibration");
                break;

            case "MusicButton":
                GetText().text = "MUSIC: " + PlayerPrefs.GetString("Music");
                break;
        }
    }

    private void ManageColliders(bool enable)
    {
        foreach (Collider2D col in collidersToManage) col.enabled = enable;
    }

    IEnumerator CloseWindow()
    {
        Settings.GetComponent<Animation>().Play("win2");

        yield return new WaitForSeconds(0.3f);

        ManageColliders(true);
        Settings.SetActive(false);
    }
}
