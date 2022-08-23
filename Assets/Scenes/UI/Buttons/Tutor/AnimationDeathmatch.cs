using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimationDeathmatch : MonoBehaviour
{
    public Animator characters;
    public GameObject particle;
    public GameObject blood;

    public Transform enemy0, enemy1;

    private int wavecount;
    public TextMeshProUGUI text;

    private void Start()
    {
        wavecount = 1;
        StartCoroutine(go());
    }

    IEnumerator go()
    {
        while (true)
        {
            GetComponent<Animation>().Play();

            yield return new WaitForSeconds(1.4f);
            characters.SetTrigger("IsFiringBomb");

            yield return new WaitForSeconds(0.5f);
            var prefab = Instantiate(particle, enemy0.position, Quaternion.identity, transform);
            prefab.transform.localScale += new Vector3(25f, 25f, 25f);
            Instantiate(blood, enemy0.position, Quaternion.identity, transform);

            yield return new WaitForSeconds(0.5f);
            prefab = Instantiate(particle, enemy1.position, Quaternion.identity, transform);
            prefab.transform.localScale += new Vector3(20f, 20f, 20f);
            Instantiate(blood, enemy1.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(1.5f);


            text.text = "Wave: " + wavecount;
            ++wavecount;
        }
    }
}
