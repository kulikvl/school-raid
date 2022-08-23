using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform parachute;
    private Transform bus;
    private BusController busController;
    private Animator character;
    public static bool Pressed;

    public Joystick joystick;
    public bool isPressed;
    public float velocity;
    
    public void Start()
    {
        Application.targetFrameRate = 60;

        GameObject _parachute = GameObject.FindGameObjectWithTag("parachute");
        GameObject _bus = GameObject.FindGameObjectWithTag("BUS");
        GameObject _character = GameObject.FindGameObjectWithTag("character");

        parachute = _parachute.transform;
        bus = _bus.transform;
        character = _character.GetComponent<Animator>();

        busController = _bus.GetComponent<BusController>();

    }

    public void Enablejoystick(bool param)
    {
        joystick.gameObject.SetActive(param);

        if (param == true)
        {
            joystick.transform.GetChild(0).gameObject.SetActive(false);
            //Image image = joystick.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            //image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
            //image = joystick.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
            //image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
    }

    [HideInInspector] public bool freeze;
    [HideInInspector] public float y;
    //public void FreezeBus(bool _freeze)
    //{
    //    //if (freeze)
    //    //    bus.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    //    //else
    //    //    bus.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

    //    freeze
    //}

    public void DisableBusControls()
    {
        joystick.gameObject.SetActive(false);
        bus.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        character.SetTrigger("IsFinished");

        GameObject[] gunsToRemove = GameObject.FindGameObjectsWithTag("gunToRemove");
        if (gunsToRemove.Length > 0)
        foreach (GameObject go in gunsToRemove) Destroy(go);
    }

    private void FixedUpdate()
    {
        velocity = parachute.GetComponent<Rigidbody2D>().velocity.x;
        isPressed = Pressed;

        if (!freeze)
        {
            if (Pressed == false)
            {
                // set bus to IDLE
                bus.GetComponent<Rigidbody2D>().gravityScale = 1f;
                parachute.GetComponent<Rigidbody2D>().gravityScale = -1f;
                parachute.GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(parachute.GetComponent<Rigidbody2D>().velocity, Vector2.zero, 555f * Time.fixedDeltaTime);
            }
            else
            {
                Vector2 direction = new Vector2(joystick.Horizontal, joystick.Vertical);
                direction = Vector2.ClampMagnitude(direction, 1.0f);

                MoveBus(direction);
            }
        }
        else
        {
            if (parachute.transform.position.y < y)
            {
                Vector2 direction = new Vector2(0f, 1f);
                direction = Vector2.ClampMagnitude(direction, 1.0f);
                MoveBus(direction);
            }
            else
            {
                bus.GetComponent<Rigidbody2D>().gravityScale = 1f;
                parachute.GetComponent<Rigidbody2D>().gravityScale = -1f;
                parachute.GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(parachute.GetComponent<Rigidbody2D>().velocity, Vector2.zero, 555f * Time.fixedDeltaTime);
                Pressed = false;
            }
        }
        
    }

    void MoveBus(Vector2 direction)
    {
        bus.GetComponent<Rigidbody2D>().gravityScale = 0;
        parachute.GetComponent<Rigidbody2D>().gravityScale = 0;

        parachute.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        parachute.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2((50f * busController.Speed) * direction.x, (50f * busController.Speed) * direction.y), direction);
    }
}

//public static class Rigidbody2DExtension
//{
//    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
//    {
//        var dir = (body.transform.position - explosionPosition);
//        float wearoff = 1 - (dir.magnitude / explosionRadius);
//        body.AddForce(dir.normalized * explosionForce * wearoff);
//    }

//    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
//    {
//        var dir = (body.transform.position - explosionPosition);
//        float wearoff = 1 - (dir.magnitude / explosionRadius);
//        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
//        body.AddForce(baseForce);

//        float upliftWearoff = 1 - upliftModifier / explosionRadius;
//        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
//        body.AddForce(upliftForce);
//    }
//}