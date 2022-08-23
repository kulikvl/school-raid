using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class placeSputnik : MonoBehaviour
{
    public GameObject sputnikPrefab;
    public bool hasSputnik;


    private void Start()
    {
        hasSputnik = false;
    }

    public void SetSputnik()
    {
        if (!hasSputnik)
        {
            hasSputnik = true;
            gameObject.tag = "Untagged";
            SelectItem.putniksSetted++;
            Instantiate(sputnikPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, gameObject.transform);
            
        }
    }
}
