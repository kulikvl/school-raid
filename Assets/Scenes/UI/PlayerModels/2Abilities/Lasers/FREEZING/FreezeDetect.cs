using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeDetect : MonoBehaviour
{
    public GameObject freezePrefab;

    private void Start()
    {
        CreateNewFreeze(transform.position);
        StartCoroutine(DisableCollision());
    }

    IEnumerator DisableCollision()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null && !collision.isTrigger && !enemy.IsFreezed)
        {
            AudioManager.instance.Play("FreezeDeath");
            enemy.DecreaseSpeed((enemy.Speed * 0.75f));
        }
    }

    private void CreateNewFreeze(Vector3 place)
    {
        float posY = place.y;

        RaycastHit2D[] allHits = Physics2D.RaycastAll(new Vector3(place.x, place.y + 3f, place.z), Vector2.down);

        for (int i = 0; i < allHits.Length; i++)
        {
            string _name = allHits[i].transform.gameObject.name;

            if (_name.Contains("line"))
            {
                posY = allHits[i].point.y - 0.1f;
                break;
            }
        }

        var prefab = Instantiate(freezePrefab, new Vector3(place.x, posY, place.z), freezePrefab.transform.rotation, transform);

       // float plusScale = Random.Range(-0.25f, 0.25f);

       // prefab.transform.localScale += new Vector3(plusScale, plusScale, plusScale);
    }
}
