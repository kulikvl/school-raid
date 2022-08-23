using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BusMenu : MonoBehaviour
{
    public Blasters blastersManager;
    public Animator animatorOfCharacters;

    public static bool isSlowMotion = false;

    public static bool allowedToDoSlowMotion;
    public Transform CombinationsOfEnemies;

    private void Awake()
    {
        allowedToDoSlowMotion = true;

        CombinationsOfEnemies.GetChild(Random.Range(0, 3)).gameObject.SetActive(true);
    }

    private void OnMouseUpAsButton()
    {
        if (!isSlowMotion && allowedToDoSlowMotion)
        {
            isSlowMotion = true;
            StartCoroutine(ISlowMotionActions());
            allowedToDoSlowMotion = false;
        }
    }

    IEnumerator ISlowMotionActions()
    {
        animatorOfCharacters.SetTrigger("IsFiringBlasters");
        blastersManager.Shoot();

        yield return new WaitForSeconds(1.3f);
        if (!isSlowMotion) yield break;

        DoSlowMotion();

        yield return new WaitForSeconds(1f);

        isSlowMotion = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public float slowdownFactor = 0.05f;
    private void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void ExitSlowMotion()
    {
        StopCoroutine(ISlowMotionActions());
        isSlowMotion = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

}
