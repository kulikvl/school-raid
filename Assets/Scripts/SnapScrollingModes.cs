using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnapScrollingModes : MonoBehaviour
{
    public GameObject[] Prefabs;

    [Header("Controllers")]
    [Range(1, 10)]
    public int panCount;
    [Range(0, 3000)]
    public int panOffset;
    [Range(0f, 20f)]
    public float snapSpeed;

    [Header("Other Objects")]
    public ScrollRect scrollRect;

    private GameObject[] instPans;
    private Vector2[] pansPos;
    private Vector2[] pansScale;

    private RectTransform contentRect;
    private Vector2 contentVector;

    public int selectedPanID;
    private bool isScrolling;

    private float AdditionX = 0f;
    private float AdditionScale = 0f;

    public bool next = false;
    public int ButtonSelected = 0;

    public GameObject go;

    public GameObject FindPan(int ID)
    {

        GameObject[] gos = GameObject.FindGameObjectsWithTag("back");

        foreach (GameObject g in gos)
        {
            if (g.name == ID.ToString())
            {
                go = g;
            }
        }

        return go;
    }

    //public void RebuildParameters()
    //{
    //    next = false;
    //    ButtonSelected = 0;

    //    gameObject.transform.position = Vector3.zero;

    //    AdditionX = -600f;
    //    AdditionScale = -0.2f;
    //    panCount = 1;
    //    //
    //    snapSpeed = 20;
    //}

    public void Start()
    {
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];

        for (int i = 0; i < panCount; i++)
        {
            instPans[i] = Instantiate(Prefabs[i], transform, false);
            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + AdditionX  + panOffset,
                instPans[i].transform.localPosition.y);

            instPans[i - 1].name = (i - 1).ToString();

            pansPos[i] = -instPans[i].transform.localPosition;
        }

        instPans[panCount - 1].name = (panCount - 1).ToString();
    }

    private void FixedUpdate()
    {
        if (Buttons.Pressed == false && Buttons.Up == true && Buttons.AbleToClick == true)
        {
            scrollRect.horizontal = true;
        }
        else
        {
            scrollRect.horizontal = false;
        }

        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1].x && !isScrolling)
            scrollRect.inertia = false;

        /// finds the nearest Prefab
        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
        }

        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;
        if (next == false)
        {
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        }
        else
        {
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[ButtonSelected].x, snapSpeed * Time.fixedDeltaTime);
        }

        contentRect.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }
}