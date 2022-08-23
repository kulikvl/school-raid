using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCheck : MonoBehaviour
{
    public GameObject BUS;

    private void OnMouseUpAsButton()
    {
        BUS.GetComponent<Destructible2D.D2dPlayerSpaceship>().BlastersMenuShoot();
    }
}
