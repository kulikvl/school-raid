using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entrance : MonoBehaviour
{
    public float X;
    public Transform p;

    private void Start()
    {
        transform.position = new Vector2(p.position.x + X, transform.position.y);
    }
}
