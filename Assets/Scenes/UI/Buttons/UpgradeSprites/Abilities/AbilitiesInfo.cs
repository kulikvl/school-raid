using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitiesInfo : MonoBehaviour
{
    //public enum Abilities
    //{
    //    FIRST,
    //    SECOND,
    //    THIRD
    //}
    //public Abilities ability;

    public GameObject[] sprites;
    public string[] descriptions;

    public TextMeshProUGUI description;

    private void OnStart()
    {
        foreach (GameObject go in sprites) go.SetActive(false);

       
        int index = UpgradeButton.currentBusIndex;
        Debug.Log("Opens: " + index);

        sprites[index].SetActive(true);

        //description.text = descriptions[index];

        description.GetComponent<TextLocaliserUI>().SetValue(descriptions[index]);
    }

    private void OnEnable()
    {
        OnStart();
    }
}
