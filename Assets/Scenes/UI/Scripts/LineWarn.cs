using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineWarn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BUS")
        {
            GetComponent<Animation>().Play();
        }
    }
}
