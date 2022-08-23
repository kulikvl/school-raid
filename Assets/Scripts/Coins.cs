using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    private void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coins").ToString();
    }
    public void PlayAnimation()
    {
        if (GetComponent<Animator>() != null)
        GetComponent<Animator>().SetTrigger("PLAY");
    }
}
