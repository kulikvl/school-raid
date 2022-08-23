using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class SnapScrolling : MonoBehaviour
{
    public GameObject[] Models;
    public GameObject OrdinaryChest, VipChest;

    [Range(1, 50)]
    [Header("Controllers")]
    public int panCount;
    [Range(0, 500)]
    public int panOffset;
    [Range(0f, 20f)]
    public float snapSpeed;
    [Range(0f, 10f)]
    public float scaleOffset;
    [Range(1f, 20f)]
    public float scaleSpeed;
    [Header("Other Objects")]
    public GameObject panPrefab;
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

    public bool moveToSelectedButton = false;
    public int selectedButtonID = 0;

    IEnumerator StraightToScene()
    {
        Buttons.AbleToClick = false;

        yield return new WaitForSeconds(1f);

        GameObject g = GameObject.FindGameObjectWithTag("tent");

        if (g != null) g.GetComponent<Animation>().Play("darkerTest");

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Scene");

        PlayerController.SetFullHpSchool();
    }

    IEnumerator ToLevelChoosing()
    {
        PlayCampaign.currentPageIsLevelSelection = true;

        GameObject shop = GameObject.FindGameObjectWithTag("Shop");
        GameObject menuCoins = GameObject.FindGameObjectWithTag("MenuCoins");
        GameObject lvlSel = GameObject.FindGameObjectWithTag("LevelSelection");

        yield return new WaitForSeconds(0.5f);
        
        shop.GetComponent<Animation>().Play("ShopSwipe2");
        menuCoins.GetComponent<Animation>().Play("CoinsSwipe2");
        lvlSel.GetComponent<Animation>().Play("ShopSwipe1");
    }

    public void PlayAnimation()
    {
        if (PlayerPrefs.GetString("currentMode") == "Campaign")
        {
            StartCoroutine(ToLevelChoosing());
        }
        if (PlayerPrefs.GetString("currentMode") == "Deathmatch")
        StartCoroutine(StraightToScene());

        //Invoke("PlayAnimationOnPlayButton", 0.75f);
    }

    public GameObject FindBackgroundOfPan(int ID)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("back");

        foreach (GameObject _go in gos)
        {
            if (_go.transform.parent.gameObject.name == ID.ToString())
            {
                return _go;
            }
        }

        Debug.LogError("NOTHING FOUND!");
        return null;
    }

    public void Start()
    {
        panCount += 2;

        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];

        for (int i = 0; i < panCount; i++)
        {
            if (i == 0)
            {
                instPans[i] = Instantiate(OrdinaryChest, transform, false);
            }
            else if (i == 1)
            {
                instPans[i] = Instantiate(VipChest, transform, false);
            }
            else
            {
                instPans[i] = Instantiate(panPrefab, transform, false); 
            }
           
            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + AdditionX - 300f + panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset,
                instPans[i].transform.localPosition.y);

            if (i > 2)
                instPans[i - 1].name = (i - 3).ToString();

            pansPos[i] = -instPans[i].transform.localPosition;
        }

        instPans[panCount - 1].name = (panCount - 3).ToString();

    }

    private bool WasSetStartPos = false;

    private void FixedUpdate()
    {
        if (Buttons.Pressed == false && Buttons.Up == true && Buttons.AbleToClick == true && AbilitiesButton.IsAllowedToScroll == true)
        {
            scrollRect.horizontal = true;
        }
        else
        {
            scrollRect.horizontal = false;
        }

        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1].x && !isScrolling)
            scrollRect.inertia = false;
        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            pansScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale + 0.2f + AdditionScale, scaleSpeed * Time.fixedDeltaTime);
            pansScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale + 0.2f + AdditionScale, scaleSpeed * Time.fixedDeltaTime);
            instPans[i].transform.localScale = pansScale[i];
        }
        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;

        if (!WasSetStartPos)
        {
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[2].x, snapSpeed * Time.fixedDeltaTime);
        }
        else if (!moveToSelectedButton)
        {
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        }
        else
        {
            contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedButtonID + 2].x, snapSpeed * Time.fixedDeltaTime);
        }
        
        contentRect.anchoredPosition = contentVector;

        if (selectedPanID == 2 && !WasSetStartPos) WasSetStartPos = true;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }
}