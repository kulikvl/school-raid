using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Laser : MonoBehaviour
{
    public GameObject explosionPrefab;
    public Transform point1, point2;
    public GameObject Guns;
    public GameObject Bus;

    public int shoot;

    public GameObject FreezeOnExplosion;
    private bool isFreezing = false;

    public void Shoot()
    {
        StartCoroutine(IShoot());
    }

    public void ShootLonger()
    {
        StartCoroutine(IShootLonger());
    }

    public void ShootFreezing()
    {
        isFreezing = true;
        StartCoroutine(IShoot());
    }

    private void HitGround(Transform pos)
    {
        AudioManager.instance.Play("LaserShot");

        RaycastHit2D[] allHits = Physics2D.RaycastAll(pos.position, Vector2.down);

        for (int i = 0; i < allHits.Length; i++)
        {
            GameObject target = allHits[i].transform.gameObject;
            Collider2D col = allHits[i].collider;

            //if (target.GetComponent<Collider2D>() != null && !target.GetComponent<Collider2D>().isTrigger)

            if (target.name.Contains("School") && !col.isTrigger) // School Enemies
            {
                Transform go = target.transform.GetChild(0);
                if (go.GetComponent<Enemy>() != null)
                {
                    go.GetComponent<Enemy>().Die(false, false, false);

                    Instantiate(explosionPrefab, allHits[i].point, pos.rotation);
                    shoot++;

                    break;
                }
            }
            else if ((target.layer == 8 || target.layer == 15) && !col.isTrigger) // Enemies
            {
                Debug.Log("hitted: " + col.isTrigger);
                Debug.Log("here");
                Transform[] trs = target.GetComponentsInParent<Transform>();

                foreach (Transform t in trs)
                {
                    if (t.gameObject.name.Contains("Enemy"))
                    {
                        if (t.gameObject.transform.GetChild(0).gameObject.GetComponent<Enemy>().IsDead)
                        {
                            Transform[] trs2 = t.GetComponentsInChildren<Transform>();

                            foreach (Transform g in trs2)
                            {
                                if (g.gameObject.name == "Body")
                                {
                                    if (Random.Range(0, 2) == 0)
                                        g.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000f, 3000f));
                                    else
                                        g.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1000f, 3000f));

                                    //Debug.Log("FORCE ADDED!");

                                    break;
                                }
                            }
                        }
                        else
                        {
                            t.gameObject.transform.GetChild(0).gameObject.GetComponent<Enemy>().Die(true, true, true);
                            

                            //Debug.Log("DEATH FROM LASERS!");

                            break;
                        }
                    }
                }

                Instantiate(explosionPrefab, allHits[i].point, pos.rotation);
                shoot++;
               

                break;
            }


            else if (target.name.Contains("line"))
            {
                Instantiate(explosionPrefab, allHits[i].point, pos.rotation);
                shoot++;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(allHits[i].point, Bus.GetComponent<BusController>().BlastRadius);

                foreach (Collider2D nb in colliders)
                {
                    IDieable dieable = nb.gameObject.GetComponent<IDieable>();

                    if (dieable != null && !dieable.IsDead && !nb.isTrigger)
                    {
                        dieable.Die(true, true, true);
                    }
                }

                if (isFreezing)
                {
                    AudioManager.instance.Play("FreezeExplosion");
                    Instantiate(FreezeOnExplosion, allHits[i].point, Quaternion.identity);
                }

                break;
            }
        }
    }

    IEnumerator IShoot()
    {
        yield return new WaitForSeconds(0.5f); // 0.5

        Guns.SetActive(true);
        Animation[] animations = Guns.GetComponentsInChildren<Animation>();
        foreach (Animation anim in animations) anim.Play("LaserAnim");

        HitGround(point1);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);
        HitGround(point1);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);
        HitGround(point1);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);

        yield return new WaitForSeconds(0.5f); // 1.5

        Guns.SetActive(false);
    }

    IEnumerator IShootLonger()
    {
        yield return new WaitForSeconds(0.5f); // 0.5

        Guns.SetActive(true);
        Animation[] animations = Guns.GetComponentsInChildren<Animation>();
        foreach (Animation anim in animations) anim.Play("LaserAnimLonger");

        HitGround(point1);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);
        HitGround(point1);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);
        HitGround(point1);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);
        HitGround(point2);
        yield return new WaitForSeconds(0.16f);

        yield return new WaitForSeconds(0.5f); // 1.5

        Guns.SetActive(false);
    }

    //private void BloodCome(GameObject go)
    //{
    //    BloodPart[] blds = go.GetComponentsInChildren<BloodPart>();
    //    List<BloodPart> list = blds.ToList();
    //    for (int i = 0; i < 10; ++i)
    //    {
    //        int num = Random.Range(0, list.Count);
    //        list[num].Release();
    //        list.RemoveAt(num);
    //    }
    //}

    //private void ImpactAdd(GameObject go)
    //{
    //    Rigidbody2D[] rbs = go.GetComponentsInChildren<Rigidbody2D>();

    //    foreach (Rigidbody2D rb in rbs)
    //    {
    //        if (rb.gameObject.name == "Body")
    //        {
    //            if (Random.Range(0, 2) == 0)
    //            {
    //                rb.AddForce(new Vector2(3000f, 8000f));
    //            }
    //            else
    //            {
    //                rb.AddForce(new Vector2(-3000f, 8000f));
    //            }
    //        }
    //    }
    //}
}
