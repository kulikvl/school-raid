using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDetect : MonoBehaviour
{
	public GameObject firePrefab;

    private void Start()
    {
		Explode(transform.GetChild(0).transform.position);
        StartCoroutine(DisableCollision());
	}

    IEnumerator DisableCollision()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDieable dieable = collision.gameObject.GetComponent<IDieable>();

        if (dieable != null && !dieable.IsDead && !collision.isTrigger)
        {
            AudioManager.instance.Play("FireDeath");
            dieable.Die(false, true, false);
        }
    }

    public void Explode(Vector3 place)
    {
        AudioManager.instance.Play("FireBurning");

        CreateNewFire(true, place);
        CreateNewFire(false, place);
    }

    private void CreateNewFire(bool isRight, Vector3 place)
    {
        float plusX = (isRight) ? Random.Range(0.2f, 0.7f) : Random.Range(-0.7f, -0.2f);
        float posY = place.y;

        RaycastHit2D[] allHits = Physics2D.RaycastAll(new Vector3(place.x + plusX, place.y + 3f, place.z), Vector2.down);

        for (int i = 0; i < allHits.Length; i++)
        {
            string _name = allHits[i].transform.gameObject.name;

            if (_name.Contains("line"))
            {
                posY = allHits[i].point.y - 0.1f;
                break;
            }
        }

        var prefab = Instantiate(firePrefab, new Vector3(place.x + plusX, posY, place.z), firePrefab.transform.rotation, transform);

        float plusScale = Random.Range(0.05f, 0.25f);
        foreach (Transform t in prefab.transform.GetComponentsInChildren<Transform>())
        {
            t.localScale -= new Vector3(plusScale, plusScale, plusScale);
        }
    }
}
