using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introSputnikEnd : MonoBehaviour
{

    private void Start()
    {
        AudioManager.instance.Stop("BackGroundHalloween");
        AudioManager.instance.Stop("BackGroundHalloween+");
        AudioManager.instance.Stop("BackGroundCity");
        AudioManager.instance.Stop("BackGroundCity+");
        AudioManager.instance.Play("BackGroundCountrySide");
        AudioManager.instance.Stop("Director2");

        StartCoroutine(go());
    }

    IEnumerator go()
    {
        yield return new WaitForSeconds(12f);

        GameObject g = GameObject.FindGameObjectWithTag("tent");
        if (g != null) g.GetComponent<Animation>().Play("darkerTest");

        yield return new WaitForSeconds(0.5f);

        PlayerController.SetFullHpSchool();

        SceneManager.LoadScene("Scene");
    }
}
