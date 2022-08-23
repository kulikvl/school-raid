using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocaliserUI : MonoBehaviour
{
    //TextMeshProUGUI textField;

    //public string key;

    void Awake()
    {
        //textField = GetComponent<TextMeshProUGUI>();
        //string value = LocalisationSystem.GetLocalisedValue(key);
        //textField.text = value;
    }

    public void SetValue(string key)
    {
        string value = LocalisationSystem.GetLocalisedValue(key);
        GetComponent<TextMeshProUGUI>().text = value;
    }

}
