using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesDestroy : MonoBehaviour, IDamageable
{
    private float HP = 0.7f;
    public bool checkCollision;
    public BoxCollider2D nonTriggerCollider;

    public bool CanBeDamaged { get; set; }

    private void Start()
    {
        CanBeDamaged = false;
    }

    public IEnumerator FreezeBox()
    {
        yield return new WaitForSeconds(0.5f);
        nonTriggerCollider.enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("line") && checkCollision)
            transform.parent.gameObject.GetComponent<ConstructionRelease>().DeleteAllBoxes();
        else if (collision.gameObject.name == "MINIGUN")
        {
            Debug.Log("MINIGUN!");
            takeDamage(1f);
        }
            
    }

    public void takeDamage(float amount)
    {
        if (CanBeDamaged)
            HP -= amount;
        else
            transform.parent.gameObject.GetComponent<ConstructionRelease>().DeleteObjectsIfHitted();

        transform.parent.gameObject.GetComponent<ConstructionRelease>().PlayAnimOnGroupOfBoxes(name == "boxR");

        if (HP <= 0)
            DestroyGroup();
    }

    private void DestroyGroup()
    {
        ///// FOR ENEMIES TO STOP DAMAGING /////

        if (TutorialSputnik.instance.isReadyToKill)
            TutorialSputnik.instance.AfterKilling();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("logic");

        if (gos != null)
        {
            foreach (GameObject go in gos)
            {
                Enemy enemy = go.GetComponent<Enemy>();

                if (enemy.IsDamaging && enemy.TargetToDamage != null && enemy.TargetToDamage.name[3] == (gameObject.name[3]))
                    enemy.stopDamaging(); // if damaging box with the same side (L or R)
            }
        }

        transform.parent.gameObject.GetComponent<ConstructionRelease>().DeleteGroupOfBoxes(gameObject.name == "boxR");
    }

}
