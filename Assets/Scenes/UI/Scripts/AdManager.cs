using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    private const string playStoreID = "3776305";
    private const string appStoreID = "3776304";

    private const string interstitialAd = "video";
    private const string rewardedVideoAd = "rewardedVideo";

    public bool isTargetPlayStore = false;
    public bool isTestMode;

    public static AdManager instance;

    private void Awake()
    {

#if UNITY_ANDROID
        isTargetPlayStore = true;
#endif

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Advertisement.AddListener(this);
        InitializeAdvertisement();
    }

    public void InitializeAdvertisement()
    {
        if (isTargetPlayStore)
        {
            Advertisement.Initialize(playStoreID, isTestMode);
        }
        else
        {
            Advertisement.Initialize(appStoreID, isTestMode);
        }
    }

    public void PlayInterstitialAd()
    {
        Debug.Log("play intterrrrrrr");
        if (Application.internetReachability != NetworkReachability.NotReachable && PlayerPrefs.GetString("ShowAds") != "false")
        {
            if (!Advertisement.IsReady(interstitialAd))
            {
                return;
            }
            else
            {
                Advertisement.Show(interstitialAd);
            }
        }
        
    }

    public void PlayRewardedVideoAd()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (!Advertisement.IsReady(rewardedVideoAd))
            {
                return;
            }
            else
            {
                Advertisement.Show(rewardedVideoAd);
            }
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        //throw new System.NotImplementedException();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        AudioManager.instance.UnPauseAllSounds();

        if (BuyChest.agreedToWatchAd)
        {
            BuyChest.agreedToWatchAd = false;
        }
        else if (SecondChanceButton.agreedToWatchAd)
        {
            BusController.agreedToWatchAd = false;
            BuyChest.agreedToWatchAd = false;
            AudioManager.instance.UnPauseAllInGameMusic();
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        AudioManager.instance.PauseAllSounds();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch(showResult)
        {
            case ShowResult.Failed:
                Debug.Log("FAILED TO WATCH THE AD");
                if (BuyChest.agreedToWatchAd)
                {
                    BuyChest.agreedToWatchAd = false;
                }
                else if (SecondChanceButton.agreedToWatchAd)
                {
                    BusController.agreedToWatchAd = false;
                    BuyChest.agreedToWatchAd = false;
                }

                break;
            case ShowResult.Skipped:
                Debug.Log("SKIPPED THE AD");
                if (BuyChest.agreedToWatchAd)
                {

                    BuyChest.agreedToWatchAd = false;
                }
                else if (SecondChanceButton.agreedToWatchAd)
                {
                    BusController.agreedToWatchAd = false;
                    BuyChest.agreedToWatchAd = false;
                }

                break;
            case ShowResult.Finished:

                if (placementId == rewardedVideoAd)
                {
                    if (BuyChest.agreedToWatchAd)
                    {
                        BuyChest[] buychests = FindObjectsOfType<BuyChest>();
                        foreach (BuyChest a in buychests)
                        {
                            if (a.ChestName == "OrdinaryChestCount")
                            {
                                Debug.Log("AFTER WATCHING MENU AD!!!!!!");
                                a.AfterWatchingAd();
                            }
                        }
                    }
                    else if (SecondChanceButton.agreedToWatchAd)
                    {
                        if (BusController.agreedToWatchAd)
                        {
                            BusController.agreedToWatchAd = false;
                            FindObjectOfType<BusController>().Revive();
                        }
                        else
                        {
                            School[] schools = FindObjectsOfType<School>();
                            foreach (School school in schools) if (school.gameObject.name == "Center") { school.ReviveSchool(); }
                        }

                        AudioManager.instance.UnPauseAllInGameMusic();
                    }
                }
                break;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        AudioManager.instance.UnPauseAllSounds();
    }
}
