using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tabFollow : MonoBehaviour
{
    public float posY;
    public float addToY = 0.6f;

    public bool first;
    public bool isRight;

    public float posOfEnemyX;
    public float posOfEnemyY;

    private void Update()
    {
        //posY = posOfEnemyY;
        //GetComponent<RectTransform>().localPosition = new Vector3(63f, 215f + (posY * 125f) + addToY, transform.position.z);

        if (isRight)
            GetComponent<RectTransform>().localPosition = new Vector3(-63f, transform.position.y, transform.position.z);
        else
            GetComponent<RectTransform>().localPosition = new Vector3(63f, transform.position.y, transform.position.z);

        transform.position = new Vector3(transform.position.x, posY + addToY, transform.position.z);
    }
}
