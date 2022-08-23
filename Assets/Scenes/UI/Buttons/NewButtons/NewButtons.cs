using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewButtons : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        Buttons.AbleToClick = true;
        Buttons.Pressed = false;
        Buttons.Up = true;
    }

    private void OnMouseUpAsButton()
    {
        if (name != "ClaimButton")
        AudioManager.instance.Play("Btn1");
    }

    private void OnMouseDown()
    {
        if (Buttons.AbleToClick == true)
        {
            Buttons.Pressed = true;
            Buttons.Up = false;

            
            animator.SetTrigger("DOWN");


            if (gameObject.name == "AbilitiesButton")
            {
                GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);
                transform.GetChild(0).GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);
            }
            else if (gameObject.name != "ClaimButton" && gameObject.name != "ItemSputnik" && gameObject.transform.parent.name != "Levels")
                GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f);

        }

    }

    private void OnMouseUp()
    {
        Buttons.Up = true;

        if (Buttons.Pressed == true)
        {
            animator.SetTrigger("UP");


            if (gameObject.name == "AbilitiesButton")
            {
                GetComponent<Image>().color = Color.white;
                transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
            else if (gameObject.name != "ClaimButton"  && gameObject.name != "ItemSputnik" && gameObject.transform.parent.name != "Levels")
                GetComponent<Image>().color = Color.white;

           

            Buttons.Pressed = false;
        }
    }
}
