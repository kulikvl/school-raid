using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sputPress : MonoBehaviour, IPointerDownHandler
{
    public GameObject _sputnik;
    private Sputnik sputnik;

    public void Start()
    {
        sputnik = _sputnik.GetComponent<Sputnik>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (!FreezeEnemies.FREEZED)
        //sputnik.Condition();
    }
}
