using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalAchievementsManager : MonoBehaviour
{
    public static LocalAchievementsManager instance;

    public enum LocalAchievements
    {
        UNDAMAGED = 0,
        PRISTINE = 1,
        PACIFIST = 2,
        MULTIKILL = 3,
        HARDCORE = 4,
        COLLECTOR = 5,
        ALL = 6,
    }

    public bool IsPlaying = false;

    public List<LocalAchievements> queue = new List<LocalAchievements>();
    List<int> multikills = new List<int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        
    }

    private void SetAllFalse()
    {
        for (int i = 0; i < (int)LocalAchievements.ALL; ++i)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
    }
    private void GiveReward(LocalAchievements localAchievement)
    {
        int reward = 0;
        string nameOfAchievement = "";

        switch (localAchievement)
        {
            case LocalAchievements.UNDAMAGED: nameOfAchievement = "UNDAMAGED"; reward = 100; break;
            case LocalAchievements.PRISTINE: nameOfAchievement = "PRISTINE"; reward = 100; break;
            case LocalAchievements.PACIFIST: nameOfAchievement = "PACIFIST"; reward = 200; break;
            case LocalAchievements.HARDCORE: nameOfAchievement = "HARDCORE"; reward = 200; break;
            case LocalAchievements.COLLECTOR: nameOfAchievement = "COLLECTOR"; reward = 50; break;
        }

        int currentLevel = PlayerPrefs.GetInt("currentLevel");
        if (PlayerPrefs.GetString(currentLevel.ToString() + nameOfAchievement) == "DONE")
        {
            reward /= 10;
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + reward);
        }
        else
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + reward);
            PlayerPrefs.SetString(currentLevel.ToString() + nameOfAchievement, "DONE");
        }

        transform.GetChild(0).GetChild((int)localAchievement).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "+" + reward;

    }

    public void ActivateLocalAchievement(LocalAchievements currentAchievement, bool addToQueue)
    {
        if (IsPlaying)
        {
            Debug.Log("PLAYING => ADD TO QUEUE!");
            queue.Add(currentAchievement);
        }
        else
        {
            if (addToQueue)
            queue.Add(currentAchievement);

            Debug.Log("LOCAL!");

            StartCoroutine(PlayAchievement());

            AudioManager.instance.Play("LocalAchievement");

            if (currentAchievement == LocalAchievements.MULTIKILL)
            {
                Debug.LogError("YOU CANNOT STATE MULTIKILL IN THIS METHOD. YOU THE OTHER INSTEAD!");
                return;
            }

            SetAllFalse();

            transform.GetChild(0).GetChild((int)currentAchievement).gameObject.SetActive(true);

            if (!ContinueButton.FinalMenuIsStarted) transform.GetChild(0).localPosition = new Vector3(0f, transform.GetChild(0).localPosition.y);
            else transform.GetChild(0).localPosition = new Vector3(-480f, transform.GetChild(0).localPosition.y);

            transform.GetChild(0).gameObject.GetComponent<Animation>().Play("main");

            StartCoroutine(ParticlePlay());

            GiveReward(currentAchievement);
        } 
    }

    public void ActivateLocalAchievementMULTIKILL(int count, bool addToQueue)
    {
        if (IsPlaying)
        {
            Debug.Log("PLAYING => ADD TO QUEUE!");
            queue.Add(LocalAchievements.MULTIKILL);
            multikills.Add(count);
        }
        else
        {
            if (addToQueue)
            {
                queue.Add(LocalAchievements.MULTIKILL);
                multikills.Add(count);
            }

            StartCoroutine(PlayAchievement());
            Debug.Log("MULTIKILL!");
            StartCoroutine(ParticlePlay());
            AudioManager.instance.Play("LocalAchievement");

            SetAllFalse();
            int reward = 0;

            if (!ContinueButton.FinalMenuIsStarted)
            {
                transform.GetChild(0).localPosition = new Vector3(0f, transform.GetChild(0).localPosition.y);
                reward = count * 5;
            }
            else
            {
                transform.GetChild(0).localPosition = new Vector3(-480f, transform.GetChild(0).localPosition.y);
                reward = count * 20;
            }
            transform.GetChild(0).gameObject.GetComponent<Animation>().Play("main");

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + reward);

            transform.GetChild(0).GetChild((int)LocalAchievements.MULTIKILL).gameObject.SetActive(true);
            transform.GetChild(0).GetChild((int)LocalAchievements.MULTIKILL).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "+" + reward; 
        }
    }

    IEnumerator ParticlePlay()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("PLAYPARTICLE");
        transform.GetChild(0).GetChild((int)LocalAchievements.ALL).gameObject.GetComponent<ParticleSystem>().Play();
    }
    IEnumerator PlayAchievement()
    {
        IsPlaying = true;
        yield return new WaitForSeconds(2.25f);

        IsPlaying = false;

        if (queue[0] == LocalAchievements.MULTIKILL) multikills.RemoveAt(0);
        queue.RemoveAt(0);

        if (queue.Count > 0)
        {
            if (queue[0] == LocalAchievements.MULTIKILL) ActivateLocalAchievementMULTIKILL(multikills[0], false);
            else ActivateLocalAchievement(queue[0], false);
        }
       
    }
}
