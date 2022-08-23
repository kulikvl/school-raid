using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FilmController : MonoBehaviour
{
    public GameObject[] chimichkaThingsToDisable;
    public ParticleSystem particleBoom;
    public ParticleSystem busSmoke;
    public ParticleSystem schoolSmoke;
    public Animator characters;

    void Start()
    {
        AudioManager.instance.Stop("BackGroundHalloween");
        AudioManager.instance.Stop("MainTheme");

        StartCoroutine(Film());
    }

    private IEnumerator Film()
    {
        AudioManager.instance.Play("BackGroundCity");
        GetComponent<Animation>().Play();


        yield return new WaitForSeconds(2f);

        AudioManager.instance.Play("FILM_Bus");


        yield return new WaitForSeconds(8f);

        AudioManager.instance.Stop("BackGroundCity");
        AudioManager.instance.Play("FILM_Class");

        yield return new WaitForSeconds(8f);


        AudioManager.instance.Play("Explosion11");
        foreach (GameObject go in chimichkaThingsToDisable) go.SetActive(false);
        particleBoom.Play();

        schoolSmoke.Play();

        yield return new WaitForSeconds(3f);
        //21

        AudioManager.instance.Play("FILM_Alarm");

        yield return new WaitForSeconds(2f);

        AudioManager.instance.Play("FILM_Balloon");
        //23

        yield return new WaitForSeconds(1f);

        AudioManager.instance.Play("FILM_Laugh");
        busSmoke.Stop();

        yield return new WaitForSeconds(1f);

        characters.SetBool("emotionfilm", true);

        yield return new WaitForSeconds(2f);

        GameObject g = GameObject.FindGameObjectWithTag("tent");

        if (g != null) g.GetComponent<Animation>().Play("darkerTest");

        yield return new WaitForSeconds(0.5f);

        PlayerPrefs.SetInt("FilmPlayed", 1);

        PlayerPrefs.SetString("currentMode", "Campaign");
        SceneManager.LoadScene("Intro");
    }

}
