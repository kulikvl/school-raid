using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    public GameObject enemyToFollow;

    private void Update()
    {
        if (enemyToFollow != null) transform.position = enemyToFollow.transform.position;
        else
        {
            GetComponent<ParticleSystem>().Stop();
            transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }
}
