using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallRating : MonoBehaviour
{
    [SerializeField] private Image fillSprite;
    [SerializeField] private StatsOfParameters[] stats;

    private float currentRating;

    private void Update()
    {
        foreach(var i in stats)
        {
            currentRating += (i.getInitialValueParam() + (PlayerPrefs.GetInt(i.getNameParam()) * i.INCREASEVALUE)) / i.MAXVALUE;
        }

        fillSprite.fillAmount = currentRating / 4.0f;

        currentRating = 0;
    }
}
