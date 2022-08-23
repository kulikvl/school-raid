using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBack : MonoBehaviour
{
    private float value;

    private void Start()
    {
        if (gameObject.name == "melnica")
        {
            value = Random.Range(-0.5f, -0.2f);
        }
        else
            value = Random.Range(-2f, -1f);

    }
    private void Update()
    {
        
        transform.Rotate(0, 0, value);
    }
}
