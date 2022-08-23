using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimberDetect : MonoBehaviour
{
    public GameObject[] toDisable;
    public GameObject particleWood;

    private float HP;

    private void Start()
    {
        HP = 100f;
    }

    private void Update()
    {
        //transform.GetChild(0).transform.gameObject.GetComponent<ParticleSystem>().startRotation = -transform.rotation.z;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDieable dieable = collision.gameObject.GetComponent<IDieable>();

        if (dieable != null && !collision.isTrigger && !dieable.IsDead) // if touches the enemy
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);

            dieable.Die(true, true, false);

            HP -= 20f;
            if (HP <= 0f) DeleteObjects();
        }
    }

    public void DeleteObjects()
    {
        foreach (GameObject go in toDisable)
        {
            if (go.name != "rope")
            {
                Vector3 pos = new Vector3(go.transform.position.x, go.transform.position.y - 0.1f);
                Instantiate(particleWood, pos, go.transform.rotation);
            }

            Destroy(go);
        }

        GameObject bus = GameObject.FindGameObjectWithTag("BUS");

        bus.GetComponent<BusController>().Speed += 1f;
        BusController.AbleToShoot = true;

        Destroy(gameObject.transform.parent.gameObject);
    }
}
