using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : MonoBehaviour
{
    public float ShootDelay = 0.1f;
    public GameObject BulletPrefab;
    public GameObject MuzzleFlashPrefab;

    [SerializeField]
    private float cooldown;

    public bool CanShoot
    {
        get
        {
            return cooldown <= 0.0f;
        }
    }

    public void Shoot()
    {
        if (cooldown <= 0.0f)
        {
            cooldown = ShootDelay;

            AudioManager.instance.Play("Minigun");

            if (BulletPrefab != null)
            {
                Instantiate(BulletPrefab, transform.position, transform.rotation);
            }

            if (MuzzleFlashPrefab != null)
            {
                Instantiate(MuzzleFlashPrefab, transform.position, transform.rotation);
            }
        }
    }

    protected virtual void Update()
    {
        cooldown -= Time.deltaTime;
    }
}
