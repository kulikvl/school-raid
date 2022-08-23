using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerBuses;

    public GameObject getCurrentBus()
    {
        return playerBuses[PlayerPrefs.GetInt("currentModel")];
    }
    public GameObject getBusByIndex(int index)
    {
        return playerBuses[index];
    }

    public static BusManager instance;

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

        DontDestroyOnLoad(gameObject);
    }
}
