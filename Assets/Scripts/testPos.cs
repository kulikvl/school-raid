using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPos : MonoBehaviour
{
    private float y;
    // Start is called before the first frame update
    void Start()
    {
        y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
