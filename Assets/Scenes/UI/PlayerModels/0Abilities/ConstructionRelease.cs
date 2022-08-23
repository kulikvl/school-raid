using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionRelease : MonoBehaviour
{
    public GameObject[] toDisable;
    public GameObject particleWood;
    public GameObject dustExplosion;
    private bool done = false;
    private bool deleted = false;

    //public Vector2 overlapVector = new Vector2(6f,6f);
    // public Collider2D[] COLS;

    //private void Update()
    //{
    //    COLS = Physics2D.OverlapBoxAll(transform.position, overlapVector, 0f);

    //    foreach (Collider2D col in COLS)
    //    {
    //        IDieable dieable = col.gameObject.GetComponent<IDieable>();
    //        if (dieable != null && !dieable.IsDead) // if touches the enemy
    //        {
    //            dieable.Die(true, true, true);

    //            DeleteObjects();
    //            DeleteAllBoxes();
    //            gameObject.GetComponent<BoxCollider2D>().enabled = false;

    //            GameObject bus = GameObject.FindGameObjectWithTag("BUS");

    //            bus.GetComponent<BusController>().Speed += 2f;
    //            BusController.AbleToShoot = true;
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDieable dieable = collision.gameObject.GetComponent<IDieable>();

        if (collision.gameObject.name.Contains("line") && !done) // if touches the ground
        {
            DeleteObjects();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            DropBoxes();

            GameObject bus = GameObject.FindGameObjectWithTag("BUS");

            bus.GetComponent<BusController>().Speed += 1f;
            BusController.AbleToShoot = true;

            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);

            AudioManager.instance.Play("Destroy5");

            if (TutorialSputnik.instance.isReadyToSetAbility)
            {
                TutorialSputnik.instance.AfterSettingAbility(transform.parent.position);
            }
        }

        else if (dieable != null && !dieable.IsDead && !collision.isTrigger && !done) // if touches the enemy
        {
            dieable.Die(true, true, false);

            //PartsOfEnemy parts = collision.gameObject.GetComponent<PartsOfEnemy>();
            //if (parts != null)
            //{
            //    parts.ImpactAdd();
            //    parts.RandomBloodCome();
            //}

            DeleteObjects();
            DeleteAllBoxes();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            GameObject bus = GameObject.FindGameObjectWithTag("BUS");

            bus.GetComponent<BusController>().Speed += 1f;
            BusController.AbleToShoot = true;

            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);

            AudioManager.instance.Play("Destroy5");
        }
    }

    private void DropBoxes()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Boxes");

        foreach (GameObject go in gos)
        {
            go.GetComponent<BoxesDestroy>().CanBeDamaged = true;
            StartCoroutine(go.GetComponent<BoxesDestroy>().FreezeBox());
        }
    }

    public void DeleteObjectsIfHitted()
    {
        DeleteObjects();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        DropBoxes();

        GameObject bus = GameObject.FindGameObjectWithTag("BUS");

        bus.GetComponent<BusController>().Speed += 1f;
        BusController.AbleToShoot = true;

        iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);

        AudioManager.instance.Play("Destroy5");
    }

    public void DeleteObjects()
    {
        foreach (GameObject go in toDisable)
        {
            if (go.name != "rope")
            {
                Instantiate(dustExplosion, go.transform.position, Quaternion.identity);
                Instantiate(particleWood, go.transform.position, go.transform.rotation);
            }

            Destroy(go);

        }

        done = true;
    }

    public void DeleteAllBoxes()
    {
        if (!deleted)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Boxes");

            foreach (GameObject go in gos)
            {
                Instantiate(particleWood, go.transform.position, go.transform.rotation);
                Instantiate(dustExplosion, go.transform.position, Quaternion.identity);
                Destroy(go);
            }

            deleted = true;
        }
    }

    public void DeleteGroupOfBoxes(bool isRight)
    {

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Boxes");

        foreach (GameObject go in gos)
        {
            if (isRight)
            {
                if (go.name == "boxR")
                {
                    Instantiate(dustExplosion, go.transform.position, Quaternion.identity);
                    Instantiate(particleWood, go.transform.position, go.transform.rotation);
                    Destroy(go);
                }
            }

            if (!isRight)
            {
                if (go.name == "boxL")
                {
                    Instantiate(dustExplosion, go.transform.position, Quaternion.identity);
                    Instantiate(particleWood, go.transform.position, go.transform.rotation);
                    Destroy(go);
                }
            }

        }

        AudioManager.instance.Play("Destroy1");
    }

    public void PlayAnimOnGroupOfBoxes(bool isRight)
    {

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Boxes");

        foreach (GameObject go in gos)
        {
            if (isRight)
            {
                if (go.name == "boxR")
                {
                    go.GetComponent<Animation>().Play();
                }
            }

            if (!isRight)
            {
                if (go.name == "boxL")
                {
                    go.GetComponent<Animation>().Play();
                }
            }

        }
    }
}
