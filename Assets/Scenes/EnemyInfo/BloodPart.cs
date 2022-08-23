using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPart : MonoBehaviour
{
    public bool EnableBlood { private get; set; }
    public bool EnableDetachingBone { private get; set; }

    [SerializeField] private Vector3 Plus;

    [Space]

    [SerializeField] private GameObject Bloodpart;

    [SerializeField] private HingeJoint2D BoneToDetach;

    public enum DetachingRules
    {
        forbidDetaching = 0,
        necessaryDetaching = 1,
        Default = 2
    }

    [SerializeField] private DetachingRules detachingRules = DetachingRules.Default;

    public void Release()
    {
        if (EnableBlood)
        BloodCome();

        if (EnableDetachingBone)
        DetachBone();
    }

    private void BloodCome()
    {
        if (Random.Range(0, 3) == 0) // 30%
        {
            Vector3 finalPos = new Vector3(transform.position.x + Plus.x, transform.position.y + Plus.y, 0f);
            Instantiate(Bloodpart, finalPos, Quaternion.identity, transform.parent);
        }

        EnableBlood = false;
    }

    private void DetachBone()
    {
        if (detachingRules == DetachingRules.Default)
        {
            if (Random.Range(0, 3) == 0) // 30%
            {
                BoneToDetach.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(200f, 500f));
                BoneToDetach.enabled = false;
            }
        }
        else if (detachingRules == DetachingRules.necessaryDetaching)
        {
            BoneToDetach.enabled = false;
        }

        EnableDetachingBone = false;
    }

    //private void CheckForAchievement()
    //{
    //    if (name.Contains("head"))
    //    {
    //        PlayerController.checkAchievement("3currentMission3level", "3currentMission3", "3C3value", "detachedHeads", 1, 2, 3);
    //    }
    //}

}
