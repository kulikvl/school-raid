using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsOfEnemyFizruk : PartsOfEnemy
{
    public GameObject parachute;

    override public IEnumerator ToDisappear()
    {
        yield return new WaitForSeconds(secondsToDisappear);
        Destroy(gameObject.transform.parent.gameObject);
    }

    public override void Die()
    {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        parachute.SetActive(false);

        base.Die();
    }
}
