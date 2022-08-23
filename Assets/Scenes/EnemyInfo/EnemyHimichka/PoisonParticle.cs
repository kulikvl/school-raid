using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonParticle : MonoBehaviour
{
    public GameObject ObjectToAttach;

    private void Update()
    {
        if (ObjectToAttach != null && !ObjectToAttach.active) Destroy(gameObject);
        else if (ObjectToAttach == null) Destroy(gameObject); 
    }
}
