using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public static bool Pressed;
    public static bool Up;
    public static bool AbleToClick = true;

    private void Start()
    {
        AbleToClick = true;
        Pressed = false;
        Up = true;
    }

    private void OnMouseUpAsButton()
    {
        if (gameObject.name != "UpgradeParameter" && name != "BuyChest" && name != "BuyButton" && !name.Contains("Item"))
        AudioManager.instance.Play("Btn1");
    }

    private void OnMouseDown()
    {
        if (AbleToClick == true)
        {
            Pressed = true;
            Up = false;

            switch (gameObject.name)
            {
                case "ReplayButton":
                    gameObject.GetComponent<Animation>().Play("ReplayDOWN");
                    break;

                case "ContinueButton":
                    gameObject.GetComponent<Animation>().Play("ContinueDOWN");
                    break;

                case "RestartButton":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;

                case "ExitButton":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;
                case "ExitButtonGame":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;
                case "AbilitiesButton":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;
                case "AbilitiesButtonShop":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;

                case "SettingsButton":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;


                case "MissionEasy":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;
                case "MissionMedium":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;
                case "MissionHard":
                    gameObject.GetComponent<Animation>().Play("SideDOWN");
                    break;


                case "Play":
                    gameObject.GetComponent<Animation>().Play("PLAYpress");
                    break;

                case "ItemSputnik":
                    gameObject.GetComponent<Animation>().Play("sputnikPLAYpress");
                    break;
                case "Item0":
                    gameObject.GetComponent<Animation>().Play("sputnikPLAYpress");
                    break;
                case "Item1":
                    gameObject.GetComponent<Animation>().Play("sputnikPLAYpress");
                    break;
                case "Item2":
                    gameObject.GetComponent<Animation>().Play("sputnikPLAYpress");
                    break;

                default:
                    gameObject.GetComponent<Animation>().Play("PLAYpress");
                    break;
            }

            if (gameObject.name == "AbilitiesButton")
            {
                GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);
                transform.GetChild(0).GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);
            }
            else if (gameObject.name != "ItemSputnik" && gameObject.transform.parent.name != "Levels" && gameObject.name != "CloseBuyVipButton") 
                GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);
        }
        
    }

    //private void OnMouseExit()
    //{
    //    if (Pressed == true)
    //    {
    //        switch (gameObject.name)
    //        {
    //            case "ReplayButton":
    //                gameObject.GetComponent<Animation>().Play("ReplayUP");
    //                break;

    //            case "ContinueButton":
    //                gameObject.GetComponent<Animation>().Play("ContinueUP");
    //                break;

    //            case "RestartButton":
    //                gameObject.GetComponent<Animation>().Play("SideUP");
    //                break;

    //            case "ExitButton":
    //                gameObject.GetComponent<Animation>().Play("SideUP");
    //                break;

    //            case "SettingsButton":
    //                gameObject.GetComponent<Animation>().Play("SideUP");
    //                break;

    //            case "PlayButton":
    //                gameObject.GetComponent<Animation>().Play("PLAYrelease");
    //                break;

    //            default:
    //                    gameObject.GetComponent<Animation>().Play("PLAYrelease");
    //                break;

    //        }

    //        GetComponent<Image>().color = Color.white;

    //        Pressed = false;
    //    }
       
    //}

    private void OnMouseUp()
    {
        Up = true;

        if (Pressed == true)
        {
            switch (gameObject.name)
            {
                case "ReplayButton":
                    gameObject.GetComponent<Animation>().Play("ReplayUP");
                    break;

                case "ContinueButton":
                    gameObject.GetComponent<Animation>().Play("ContinueUP");
                    break;

                case "RestartButton":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;

                case "ExitButton":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;
                case "ExitButtonGame":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;
                case "AbilitiesButton":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;
                case "AbilitiesButtonShop":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;

                case "SettingsButton":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;


                case "MissionEasy":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;
                case "MissionMedium":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;
                case "MissionHard":
                    gameObject.GetComponent<Animation>().Play("SideUP");
                    break;


                case "Play":
                    gameObject.GetComponent<Animation>().Play("PLAYrelease");
                    break;
                case "ItemSputnik":
                    
                    break;
                case "Item0":

                    break;
                case "Item1":

                    break;
                case "Item2":

                    break;

                default:
                        gameObject.GetComponent<Animation>().Play("PLAYrelease");
                    break;

            }

            if (gameObject.name == "AbilitiesButton")
            {
                GetComponent<Image>().color = Color.white;
                transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else if (gameObject.name != "ItemSputnik" && gameObject.transform.parent.name != "Levels" && gameObject.name != "CloseBuyVipButton")
                GetComponent<Image>().color = Color.white;

           

            Pressed = false;
            
        }
    }


}
