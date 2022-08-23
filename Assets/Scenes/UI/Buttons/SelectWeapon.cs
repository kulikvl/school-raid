using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectWeapon : MonoBehaviour, IPointerDownHandler
{
    public BusController.Weapons weapon;
    public Image selectedSprite;

    private BusController busController;

    private void Start()
    {
        GameObject bus = GameObject.FindGameObjectWithTag("BUS");
        if (bus != null) busController = bus.GetComponent<BusController>();
        else Debug.LogError("BUS WAS NOT FOUND!");

        if (weapon == BusController.Weapons.Bomb)
            selectedSprite.enabled = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {

        disableSelections();
        selectedSprite.enabled = true;
        selectWeapon();

    }

    private void selectWeapon()
    {
        if (busController.CurrentWeapon != weapon)
        {
            busController.CurrentWeapon = weapon;
        }
    }

    private void disableSelections()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("itemWeapon");
        foreach(GameObject go in gos)
        {
            go.GetComponent<SelectWeapon>().selectedSprite.enabled = false;
        }
    }
}
