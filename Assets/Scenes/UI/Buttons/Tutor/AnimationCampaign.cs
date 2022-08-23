using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCampaign : MonoBehaviour
{
    public Animator characters;
    public GameObject prefab;
    public Transform point;

    public Transform level1, level2, level3;
    public GameObject prefabFirework;

    private void Start()
    {
        StartCoroutine(go());
    }

    IEnumerator go()
    {
        while (true)
        {
            GetComponent<Animation>().Play();

            yield return new WaitForSeconds(0.8f);
            characters.SetTrigger("IsFiringBomb");
            
            yield return new WaitForSeconds(0.5f);
            Instantiate(prefab, point.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(0.5f);
            Instantiate(prefab, point.position, Quaternion.identity, transform);

            var conf1 = Instantiate(prefabFirework, level1.position, Quaternion.identity, transform);

            yield return new WaitForSeconds(0.2f);

            yield return new WaitForSeconds(1.3f);
            characters.SetTrigger("IsFiringBomb");

            yield return new WaitForSeconds(0.5f);
            Instantiate(prefab, point.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(0.5f);
            Instantiate(prefab, point.position, Quaternion.identity, transform);

            var conf2 = Instantiate(prefabFirework, level2.position, Quaternion.identity, transform);

            yield return new WaitForSeconds(0.2f);

            yield return new WaitForSeconds(1.3f);
            characters.SetTrigger("IsFiringBomb");

            yield return new WaitForSeconds(0.5f);
            Instantiate(prefab, point.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(0.5f);
            Instantiate(prefab, point.position, Quaternion.identity, transform);

            var conf3 = Instantiate(prefabFirework, level3.position, Quaternion.identity, transform);

            yield return new WaitForSeconds(0.2f);

            yield return new WaitForSeconds(1f);

            Destroy(conf1);
            Destroy(conf2);
            Destroy(conf3);
        }
        
    }
}
