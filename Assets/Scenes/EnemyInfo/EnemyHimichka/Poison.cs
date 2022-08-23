using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public bool startedFlying = false;
    public float damage;

    [System.NonSerialized] public bool isRight;

    public float radius;
    public GameObject particleEffectOnExplosion;
    private float rotationSpeed;
    private void Start()
    {
        rotationSpeed = Random.Range(15f, 20f);
        StartCoroutine(die());
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }

    private bool isAiming = false;
    private GameObject target;

    private void Update()
    {
        if (startedFlying)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (Collider2D col in colliders)
            {
                if (startedFlying)
                {
                    if (col.gameObject.CompareTag("school"))
                    {
                        //Debug.Log("school!");
                        IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
                        damageable.takeDamage(damage);
                        Explode(col.gameObject);
                        break;
                    }
                    if (col.gameObject.name.Contains("box"))
                    {
                        isAiming = true;
                        target = col.gameObject;
                        //Debug.Log("school!");
                        IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
                        damageable.takeDamage(damage);
                        Explode(col.gameObject);
                        break;
                    }
                }

            }

            transform.Rotate(0, 0, rotationSpeed);
        }

        if (isAiming)
        {
            if (target != null)
            {
                float step = 10f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            }
        }
    }

    private void Explode(GameObject obj)
    {
        AudioManager.instance.Play("Poison");

        Vector2 pos;

        if (isRight)
            pos = new Vector2(transform.position.x - 0.1f, transform.position.y);
        else
            pos = new Vector2(transform.position.x + 0.1f, transform.position.y);

        var prefab = Instantiate(particleEffectOnExplosion, pos, Quaternion.identity);
        prefab.GetComponent<PoisonParticle>().ObjectToAttach = obj;

        Destroy(gameObject);
    }
}
