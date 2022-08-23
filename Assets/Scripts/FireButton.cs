using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool Pressed = false;
    public bool pressed;

    private void Start()
    {
        Pressed = false;
    }

    private void Update()
    {
        pressed = Pressed;
    }

    //private void OnMouseDown()
    //{
    //    if (!SelectItem.isChoosing)
    //    {
    //        Pressed = true;
    //        Debug.Log("FIRE! CLICKED");
    //    }
    //}

    //private void OnMouseUp()
    //{
    //    if (!SelectItem.isChoosing)
    //    {
    //        if (Pressed)
    //        {
    //            Pressed = false;
    //        }
    //    }
    //}


    public void OnPointerDown(PointerEventData eventData)
    {
        if (!SettingsInGame.GameIsFreezed)
        Pressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Pressed && !SettingsInGame.GameIsFreezed)
        {
            Pressed = false;
        }
    }
}
