using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToScene : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        StartCoroutine(go());
    }

    IEnumerator go()
    {
        GameObject g = GameObject.FindGameObjectWithTag("tent");

        if (g != null) g.GetComponent<Animation>().Play("darkerTest");
        else Debug.LogError("cant find");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Scene");

        // todo unfreeze

        PlayerController.SetFullHpSchool();
    }
}
