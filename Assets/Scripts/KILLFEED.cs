using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KILLFEED : MonoBehaviour
{
    public static int KILAST;
    public static int BULLLAST;

    private void Update()
    {
        if (gameObject.name == "KillFeed")
        gameObject.GetComponent<TextMeshProUGUI>().text = KILAST.ToString();
        else
        gameObject.GetComponent<TextMeshProUGUI>().text = BULLLAST.ToString();
    }
}
