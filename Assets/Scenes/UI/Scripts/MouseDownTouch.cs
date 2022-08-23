using UnityEngine;

/// <summary>
//	Allows "OnMouseDown()" events to work on touch devices.
/// </summary>
public class MouseDownTouch : MonoBehaviour
{
    private void Update()
    {
        var hit = new RaycastHit();

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                // Construct a ray from the current touch coordinates.
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);

                if (Physics.Raycast(ray, out hit))
                {
                    hit.transform.gameObject.SendMessage("OnMouseDown");
                }
            }
        }
    }
}