using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayCampaign : MonoBehaviour
{

    private GameObject shop, modes, menuCoins;

    public static bool currentPageIsLevelSelection;

    private void Start()
    {
        currentPageIsLevelSelection = false;
        modes = GameObject.FindGameObjectWithTag("Mode");
        shop = GameObject.FindGameObjectWithTag("Shop");
        menuCoins = GameObject.FindGameObjectWithTag("MenuCoins");
    }
    private void OnMouseUpAsButton()
    {

        PlayerPrefs.SetString("currentMode", "Campaign");

        modes.GetComponent<Animation>().Play("ShopSwipe2");
        shop.GetComponent<Animation>().Play("ShopSwipe1");
        menuCoins.GetComponent<Animation>().Play();
    }

    //private GameObject modes, lvlSel, shop;

    //private void Start()
    //{
    //    modes = GameObject.FindGameObjectWithTag("Mode");
    //    lvlSel = GameObject.FindGameObjectWithTag("LevelSelection");
    //    shop = GameObject.FindGameObjectWithTag("Shop");
    //}
    //private void OnMouseUpAsButton()
    //{
        
    //    PlayerPrefs.SetString("currentMode", "Campaign");

    //    shop.GetComponent<Animation>().Play("ShopSwipe1");
    //    modes.GetComponent<Animation>().Play("ShopSwipe2");
    //    lvlSel.GetComponent<Animation>().Play("ShopSwipe1");
    //}




    //public void AllObjectsPlayAnimation()
    //{
    //    play.position = new Vector3(play.position.x, play.position.y + 200f, play.position.z);

    //    lightbackground.Play();
    //}

    //public void NextScene()
    //{
    //    StartCoroutine(nextScene());
    //}

    //IEnumerator nextScene()
    //{

    //    yield return new WaitForSeconds(0.5f);

    //    /////////

    //    PlayerController.SetFullHpSchool();

    //    PlayerPrefs.SetInt("currentModel", 0);

    //    /////////

    //    SceneManager.LoadScene("Intro");
    //}
}
